using System.Collections.Generic;
using System.Linq;

namespace WhenEntityFrameworkMeetUnity.BulkExtensions
{
  internal class TableMapping
  {
    private readonly Dictionary<string, ColumnMapping> _columnMappings = new Dictionary<string, ColumnMapping>();

    public TableMapping(string tableTypeFullName, string schemaName, string tableName)
    {
      TableTypeFullName = tableTypeFullName;
      SchemaName = schemaName;
      TableName = tableName;
    }

    public string TableTypeFullName { get; private set; }
    public string SchemaName { get; private set; }
    public string TableName { get; private set; }

    public ColumnMapping[] Columns
    {
      get { return _columnMappings.Values.ToArray(); }
    }

    public ColumnMapping this[string property]
    {
      get { return _columnMappings[property]; }
    }

    public ColumnMapping AddColumn(string property, string columnName)
    {
      var cmap = new ColumnMapping(property, columnName);
      _columnMappings.Add(property, cmap);

      return cmap;
    }
  }
}
