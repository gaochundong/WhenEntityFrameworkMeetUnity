using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace WhenEntityFrameworkMeetUnity.BulkExtensions
{
  internal class BulkOperationProvider
  {
    private DbContext _context;
    private string _connectionString;

    public BulkOperationProvider(DbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("context");

      _context = context;

      ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[context.GetType().Name];
      _connectionString = cs.ConnectionString;
    }

    public void Insert<T>(IEnumerable<T> entities, int batchSize)
    {
      using (var dbConnection = new SqlConnection(_connectionString))
      {
        dbConnection.Open();

        using (var transaction = dbConnection.BeginTransaction())
        {
          try
          {
            Insert(entities, transaction, SqlBulkCopyOptions.Default, batchSize);
            transaction.Commit();
          }
          catch (Exception)
          {
            if (transaction.Connection != null)
            {
              transaction.Rollback();
            }
            throw;
          }
        }
      }
    }

    private void Insert<T>(IEnumerable<T> entities, SqlTransaction transaction, SqlBulkCopyOptions options, int batchSize)
    {
      TableMapping tableMapping = DbMapper.GetDbMapping(_context)[typeof(T)];
      using (DataTable dataTable = CreateDataTable(tableMapping, entities))
      {
        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(transaction.Connection, options, transaction))
        {
          sqlBulkCopy.BatchSize = batchSize;
          sqlBulkCopy.DestinationTableName = dataTable.TableName;

          foreach (DataColumn dataColumn in dataTable.Columns)
          {
            sqlBulkCopy.ColumnMappings.Add(dataColumn.ColumnName, dataColumn.ColumnName);
          }

          sqlBulkCopy.WriteToServer(dataTable);
        }
      }
    }

    private static DataTable CreateDataTable<T>(TableMapping tableMapping, IEnumerable<T> entities)
    {
      var dataTable = BuildDataTable<T>(tableMapping);

      foreach (var entity in entities)
      {
        DataRow row = dataTable.NewRow();

        foreach (var columnMapping in tableMapping.Columns)
        {
          var @value = entity.GetPropertyValue(columnMapping.PropertyName);

          if (columnMapping.IsIdentity) continue;

          if (@value == null)
          {
            row[columnMapping.ColumnName] = DBNull.Value;
          }
          else
          {
            row[columnMapping.ColumnName] = @value;
          }
        }

        dataTable.Rows.Add(row);
      }

      return dataTable;
    }

    private static DataTable BuildDataTable<T>(TableMapping tableMapping)
    {
      var entityType = typeof(T);
      string tableName = string.Join(@".", tableMapping.SchemaName, tableMapping.TableName);

      var dataTable = new DataTable(tableName);
      var primaryKeys = new List<DataColumn>();

      foreach (var columnMapping in tableMapping.Columns)
      {
        var propertyInfo = entityType.GetProperty(columnMapping.PropertyName, '.');
        columnMapping.Type = propertyInfo.PropertyType;

        var dataColumn = new DataColumn(columnMapping.ColumnName);

        Type dataType;
        if (propertyInfo.PropertyType.IsNullable(out dataType))
        {
          dataColumn.DataType = dataType;
          dataColumn.AllowDBNull = true;
        }
        else
        {
          dataColumn.DataType = propertyInfo.PropertyType;
          dataColumn.AllowDBNull = columnMapping.Nullable;
        }

        if (columnMapping.IsIdentity)
        {
          dataColumn.Unique = true;
          if (propertyInfo.PropertyType == typeof(int)
            || propertyInfo.PropertyType == typeof(long))
          {
            dataColumn.AutoIncrement = true;
          }
          else continue;
        }
        else
        {
          dataColumn.DefaultValue = columnMapping.DefaultValue;
        }

        if (propertyInfo.PropertyType == typeof(string))
        {
          dataColumn.MaxLength = columnMapping.MaxLength;
        }

        if (columnMapping.IsPk)
        {
          primaryKeys.Add(dataColumn);
        }

        dataTable.Columns.Add(dataColumn);
      }

      dataTable.PrimaryKey = primaryKeys.ToArray();

      return dataTable;
    }
  }
}
