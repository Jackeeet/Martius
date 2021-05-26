using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

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

        private protected List<IDataEntity> GetEntities(string table, Func<SqlDataReader, IDataEntity> entityBuilder,
            string filter = null, string join = null)
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
                            FillEntityList(reader, result, entityBuilder);
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
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
                    Console.WriteLine(e.Message);
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
                    Console.WriteLine(e.Message);
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

        private protected void DeleteById(int id, string table)
        {
            var com = $"delete from {table} where id = {id}";
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
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void FillEntityList(
            SqlDataReader reader, List<IDataEntity> result, Func<SqlDataReader, IDataEntity> entityBuilder)
        {
            while (reader.Read())
            {
                var item = entityBuilder(reader);
                result.Add(item);
            }
        }
    }
}