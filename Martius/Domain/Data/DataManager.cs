using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Martius.Infrastructure;

namespace Martius.Domain
{
    internal abstract class DataManager
    {
        protected readonly string ConnectionString;

        protected DataManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected abstract IDataEntity BuildEntity(SqlDataReader reader);

        private protected List<IDataEntity> GetEntities(string table, string filter = null, string join = null)
        {
            var result = new List<IDataEntity>();
            var connection = new SqlConnection(ConnectionString);
            if (filter != null && !filter.StartsWith("where "))
                filter = "where " + filter;
            var comString = $"select * from {table} {join} {filter}";
            
            using (connection)
            {
                var command = new SqlCommand(comString, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.HasRows)
                            FillEntityList(reader, result);
                    }
                }
                catch (SqlException e)
                {
                    throw new DbAccessException(e.Message, e);
                }
            }

            return result;
        }

        private protected void AddEntity(IDataEntity entity, string table, string columns)
        {
            var connection = new SqlConnection(ConnectionString);
            var com = $"insert into {table}({columns}) values ({entity.ToSqlString()})";
            using (connection)
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    throw new DbAccessException(e.Message, e);
                }
            }
        }

        private protected void UpdateEntity(IDataEntity entity, string table, string columns)
        {
            var com = $"update {table} " +
                      GetUpdateString(entity, columns) +
                      $"where id = {entity.Id}";
            var connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    throw new DbAccessException(e.Message, e);
                }
            }
        }

        private static string GetUpdateString(IDataEntity entity, string columns)
        {
            var separator = new[] {", "};
            var columnNames = columns.Split(separator, StringSplitOptions.None);
            var values = entity.ToSqlString().Split(separator, StringSplitOptions.None);

            var sb = new StringBuilder("set ");
            var length = columnNames.Length;
            for (int i = 0; i < length; i++)
                sb.Append($"{columnNames[i]} = {values[i]}, ");
            sb.Remove(sb.Length - 2, 2);
            return sb.ToString();
        }


        private void FillEntityList(SqlDataReader reader, List<IDataEntity> result)
        {
            while (reader.Read())
            {
                result.Add(BuildEntity(reader));
            }
        }
    }
}