using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace WhenEntityFrameworkMeetUnity.BulkExtensions
{
  internal class DbMapping
  {
    private readonly DbContext _context;
    private readonly MetadataWorkspace _metadataWorkspace;
    private readonly EntityContainer _codeFirstEntityContainer;
    private readonly Dictionary<string, TableMapping> _tableMappings = new Dictionary<string, TableMapping>();
    private readonly Dictionary<string, List<EdmMember>> _tableColumnEdmMembers = new Dictionary<string, List<EdmMember>>();
    private readonly Dictionary<string, List<string>> _primaryKeysMapping = new Dictionary<string, List<string>>();

    public DbMapping(DbContext context)
    {
      _context = context;

      var objectContext = ((IObjectContextAdapter)context).ObjectContext;
      _metadataWorkspace = objectContext.MetadataWorkspace;

      _codeFirstEntityContainer = _metadataWorkspace.GetEntityContainer("CodeFirstDatabase", DataSpace.SSpace);

      MapDb();
    }

    public TableMapping this[Type tableType]
    {
      get { return this[tableType.FullName]; }
    }

    public TableMapping this[string tableTypeFullName]
    {
      get { return _tableMappings[tableTypeFullName]; }
    }

    private void MapDb()
    {
      ExtractTableColumnEdmMembers();

      List<EntityType> tables =
        _metadataWorkspace
          .GetItems(DataSpace.OCSpace)
          .Select(x => x.GetPrivateFieldValue("EdmItem") as EntityType)
          .Where(x => x != null)
          .ToList();

      foreach (var table in tables)
      {
        MapTable(table);
      }
    }

    private void ExtractTableColumnEdmMembers()
    {
      IEnumerable<object> entitySetMaps =
        (IEnumerable<object>)_metadataWorkspace
          .GetItemCollection(DataSpace.CSSpace)[0]
          .GetPrivateFieldValue("EntitySetMaps");

      foreach (var entitySetMap in entitySetMaps)
      {
        IEnumerable<object> typeMappings = (IEnumerable<object>)entitySetMap.GetPrivateFieldValue("TypeMappings");
        foreach (var typeMapping in typeMappings)
        {
          IEnumerable<EdmType> types = (IEnumerable<EdmType>)typeMapping.GetPrivateFieldValue("Types");
          foreach (EntityType type in types)
          {
            string identity = type.FullName;

            List<EdmMember> properties = new List<EdmMember>(type.Properties);
            properties.AddRange(type.NavigationProperties);

            _tableColumnEdmMembers[identity] = properties;
          }
        }
      }
    }

    private void MapTable(EntityType tableEdmType)
    {
      string identity = tableEdmType.FullName;
      EdmType baseEdmType = tableEdmType;
      EntitySet storageEntitySet = null;

      while (!_codeFirstEntityContainer.TryGetEntitySetByName(baseEdmType.Name, false, out storageEntitySet))
      {
        if (baseEdmType.BaseType == null) break;
        baseEdmType = baseEdmType.BaseType;
      }
      if (storageEntitySet == null) return;

      var tableName = (string)storageEntitySet.MetadataProperties["Table"].Value;
      var schemaName = (string)storageEntitySet.MetadataProperties["Schema"].Value;

      var tableMapping = new TableMapping(identity, schemaName, tableName);
      _tableMappings.Add(identity, tableMapping);
      _primaryKeysMapping.Add(identity, storageEntitySet.ElementType.KeyMembers.Select(x => x.Name).ToList());

      foreach (var prop in storageEntitySet.ElementType.Properties)
      {
        MapColumn(identity, _tableMappings[identity], prop);
      }
    }

    private void MapColumn(string identity, TableMapping tableMapping, EdmProperty property)
    {
      var columnName = property.Name;
      EdmMember edmMember = _tableColumnEdmMembers[identity].Single(c => c.Name == columnName);
      string propertyName = edmMember.Name;

      ColumnMapping columnMapping = tableMapping.AddColumn(propertyName, columnName);
      BuildColumnMapping(identity, property, propertyName, columnMapping);
    }

    private void BuildColumnMapping(string identity, EdmProperty property, string propertyName, ColumnMapping columnMapping)
    {
      if (_primaryKeysMapping[identity].Contains(propertyName))
      {
        columnMapping.IsPk = true;
      }

      foreach (var facet in property.TypeUsage.Facets)
      {
        switch (facet.Name)
        {
          case "Nullable":
            columnMapping.Nullable = (bool)facet.Value;
            break;
          case "DefaultValue":
            columnMapping.DefaultValue = facet.Value;
            break;
          case "StoreGeneratedPattern":
            columnMapping.IsIdentity = (StoreGeneratedPattern)facet.Value == StoreGeneratedPattern.Identity;
            columnMapping.Computed = (StoreGeneratedPattern)facet.Value == StoreGeneratedPattern.Computed;
            break;
          case "MaxLength":
            columnMapping.MaxLength = (int)facet.Value;
            break;
        }
      }
    }
  }
}
