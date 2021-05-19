using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LibTestProject")]

namespace Martius.Domain
{
    internal class DataManager
    {
        protected readonly string ConnectionString;

        protected DataManager(string connectionString)
        {
            ConnectionString = connectionString;
        }

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

        private protected  IDataEntity GetEntityById(
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

        private protected  void AddEntity(IDataEntity entity, string tableDesc)
        {
            var connection = new SqlConnection(ConnectionString);
            var com = tableDesc + $" values ({entity.ToSqlString()})";
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

        private protected  void DeleteEntityById(int id, string table)
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