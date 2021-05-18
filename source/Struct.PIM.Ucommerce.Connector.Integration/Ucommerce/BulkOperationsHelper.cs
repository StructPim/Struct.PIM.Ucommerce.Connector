using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Struct.PIM.Ucommerce.Connector.Integration.Ucommerce
{
    public static class BulkOperationsHelper
    {
        public static void InsertIntoTempTable<T>(SqlConnection connection, string schemaName, string tempTableName,
            Dictionary<string, string> columns, IEnumerable<T> items)
        {
            //Create temp table
            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = BuildCreateTempTable(columns, tempTableName);
            command.ExecuteNonQuery();

            //Insert into temp table
            var hashSet = new HashSet<string>();
            foreach (var column in columns)
            {
                hashSet.Add(column.Key);
            }

            var dataTable = CreateDataTable<T>(hashSet);
            var dt = ConvertListToDataTable(dataTable, items, hashSet);
            InsertToTmpTable(connection, tempTableName, dt);

            //If single column, Create index on temp table
            if (hashSet.Count == 1)
            {
                SqlCommand createIndexCommand = connection.CreateCommand();
                createIndexCommand.Connection = connection;
                createIndexCommand.CommandText =
                    $"CREATE NONCLUSTERED INDEX ix_{tempTableName.Replace("#", "")} ON {tempTableName} ({hashSet.Single()})";
                createIndexCommand.ExecuteNonQuery();
            }
        }

        internal static void InsertToTmpTable(SqlConnection connection, string tempTableName, DataTable dt)
        {
            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(connection))
            {
                bulkcopy.DestinationTableName = tempTableName;
                bulkcopy.WriteToServer(dt);
            }
        }

        internal static DataTable CreateDataTable<T>(HashSet<string> columns)
        {
            if (columns == null)
                return null;

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var column in columns.ToList())
            {
                dataTable.Columns.Add(column);
            }

            AssignTypes(props, columns, dataTable);

            return dataTable;
        }

        private static void AssignTypes(PropertyInfo[] props, HashSet<string> columns, DataTable dataTable)
        {
            int count = 0;

            foreach (var column in columns.ToList())
            {
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].Name == column)
                    {
                        dataTable.Columns[count].DataType =
                            Nullable.GetUnderlyingType(props[i].PropertyType) ?? props[i].PropertyType;
                    }
                }

                count++;
            }
        }

        public static DataTable ConvertListToDataTable<T>(DataTable dataTable, IEnumerable<T> list, HashSet<string> columns)
        {
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            int counter = 0;

            foreach (T item in list)
            {
                var values = new List<object>();

                foreach (var column in columns.ToList())
                {
                    for (int i = 0; i < props.Length; i++)
                    {
                        if (props[i].Name == column && item != null
                                                    && CheckForValidDataType(props[i].PropertyType,
                                                        throwIfInvalid: true))
                            values.Add(props[i].GetValue(item, null));


                    }
                }

                counter++;
                dataTable.Rows.Add(values.ToArray());
            }

            return dataTable;
        }

        private static bool CheckForValidDataType(Type type, bool throwIfInvalid = false)
        {
            if (type.IsValueType ||
                type == typeof(string) ||
                type == typeof(byte[]) ||
                type == typeof(char[]) ||
                type == typeof(SqlXml)
            )
                return true;

            if (throwIfInvalid)
                throw new ArgumentException("Only value, string, char[], byte[] " +
                                            "and SqlXml types can be used with SqlBulkTools. " +
                                            "Refer to https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx for more details.");

            return false;
        }

        private static string BuildCreateTempTable(IEnumerable<KeyValuePair<string, string>> columns, string tempTableName)
        {
            StringBuilder command = new StringBuilder();
            command.Append("CREATE TABLE " + tempTableName + "(");

            List<string> collateColumns = new List<string> {"varchar", "nvarchar", "char", "binary", "varbinary", "nchar"};
            List<string> paramList = new List<string>();

            foreach (var column in columns)
            {
                string columnType = column.Value;
                string columnTypeNoLength = columnType.Contains("(")
                    ? columnType.ToLower().Substring(0, columnType.IndexOf("("))
                    : columnType;

                if (collateColumns.Contains(columnTypeNoLength) && !columnType.ToLower().Contains("collate"))
                {
                    columnType += " COLLATE SQL_Latin1_General_CP1_CI_AS";
                }

                paramList.Add("[" + column.Key + "]" + " " + columnType);
            }

            string paramListConcatenated = string.Join(", ", paramList);

            command.Append(paramListConcatenated);
            command.Append(");");

            return command.ToString();
        }
    }
}
