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

        private protected List<IDataEntity> GetAllEntities(
            string table, Func<SqlDataReader, IDataEntity> entityBuilder)
        {
            var result = new List<IDataEntity>();
            var connection = new SqlConnection(ConnectionString);
            var comString = $"select * from {table}";
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

        private protected List<IDataEntity> GetFilteredEntities(
            string table, string filter, Func<SqlDataReader, IDataEntity> entityBuilder, string join = null)
        {
            var result = new List<IDataEntity>();
            var connection = new SqlConnection(ConnectionString);
            var comString = $"select * from {table}{join} where {filter}";
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

        private protected IDataEntity GetEntityById(
            int id, string table, Func<SqlDataReader, IDataEntity> entityBuilder)
        {
            IDataEntity entity = null;
            var cmd = $"select * from {table} where id = {id}";
            var connection = new SqlConnection(ConnectionString);
            using (connection)
            {
                var command = new SqlCommand(cmd, connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    using (reader)
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            entity = entityBuilder(reader);
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return entity;
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

        private protected static void FillEntityList(
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