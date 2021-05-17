using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using Martius.Data;
using Martius.Infrastructure;
using static Martius.Infrastructure.CastUtils;

[assembly: InternalsVisibleTo("LibTestProject")]

namespace Martius.Domain
{
    internal static class DataManager
    {
        private static readonly string connectionString = ConnectionConfig.ConnectionString;
        //private static readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\\..\\..\\..\\Martius\\Data\\MartiusDB.mdf;Integrated Security=True";

        internal static List<RealProperty> GetAllRealProperties()
        {
            var result = new List<RealProperty>();
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand("select * from property", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                        FillPropertyList(reader, result);

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return result;
        }

        private static void FillPropertyList(SqlDataReader reader, List<RealProperty> result)
        {
            while (reader.Read())
            {
                var property = BuildProperty(reader);
                result.Add(property);
            }
        }

        private static RealProperty BuildProperty(SqlDataReader reader)
        {
            var id = (int) reader["id"];
            var address = GetAddress(reader);
            var roomCount = (int) reader["room_count"];
            var area = (double) reader["area"];
            var res = (bool) reader["residential"];
            var furn = (bool) reader["furnished"];
            var park = (bool) reader["has_parking"];
            var price = (decimal) reader["monthly_price"];
            var property = new RealProperty(id, address, roomCount, area, res, furn, park, price);
            return property;
        }

        internal static List<Tenant> GetAllTenants()
        {
            var result = new List<Tenant>();
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand("select * from tenant", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                        FillTenantList(reader, result);

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return result;
        }

        private static void FillTenantList(SqlDataReader reader, List<Tenant> result)
        {
            while (reader.Read())
            {
                var tenant = BuildTenant(reader);
                result.Add(tenant);
            }
        }

        private static Tenant BuildTenant(SqlDataReader reader)
        {
            var id = (int) reader["id"];
            var person = GetPerson(reader);
            var phone = reader["phone"].ToString();
            var passport = reader["passport"].ToString();
            var tenant = new Tenant(id, person, phone, passport);
            return tenant;
        }

        internal static List<Lease> GetAllLeases()
        {
            var result = new List<Lease>();
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand("select * from lease", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                        FillLeaseList(reader, result);

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return result;
        }

        private static void FillLeaseList(SqlDataReader reader, List<Lease> result)
        {
            while (reader.Read())
            {
                var lease = BuildLease(reader);
                result.Add(lease);
            }
        }

        private static Lease BuildLease(SqlDataReader reader)
        {
            var id = (int) reader["id"];
            var propId = (int) reader["property_id"];
            var prop = GetPropertyById(propId);
            var tenantId = (int) reader["tenant_id"];
            var tenant = GetTenantById(tenantId);
            var monthlyPrice = (decimal) reader["monthly_price"];
            var startDate = DateTime.Parse(reader["start_date"].ToString());
            var endDate = DateTime.Parse(reader["end_date"].ToString());
            var lease = new Lease(id, prop, tenant, monthlyPrice, startDate, endDate);
            return lease;
        }

        internal static RealProperty GetPropertyById(int propId)
        {
            RealProperty property = null;
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand($"select * from property where id = {propId}", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        property = BuildProperty(reader);
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return property;
        }

        internal static Tenant GetTenantById(int tenantId)
        {
            Tenant tenant = null;
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand($"select * from tenant where id = {tenantId}", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tenant = BuildTenant(reader);
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return tenant;
        }

        internal static Lease GetLeaseById(int leaseId)
        {
            Lease lease = null;
            var connection = new SqlConnection(connectionString);
            using (connection)
            {
                var command = new SqlCommand($"select * from lease where id = {leaseId}", connection);
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        lease = BuildLease(reader);
                    }

                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return lease;
        }

        // todo implement a single method that finds data by id and specified table/datatype

        private static Address GetAddress(SqlDataReader reader)
        {
            var city = reader["city"].ToString();
            var street = reader["street"].ToString();
            var building = (int) reader["building"];
            var buildExtra = reader["building_extra"].ToString();
            var aptNumber = ToNullableInt(reader["apt_number"].ToString());
            return new Address(city, street, building, aptNumber, buildExtra);
        }

        private static Person GetPerson(SqlDataReader reader)
        {
            var surname = reader["surname"].ToString();
            var name = reader["name"].ToString();
            var patronym = reader["patronym"].ToString();
            var dobString = reader["dob"].ToString();
            return new Person(surname, name, patronym, DateTime.Parse(dobString));
        }

        internal static void AddProperty(RealProperty prop)
        {
            var connection = new SqlConnection(connectionString);
            var com =
                "insert into property(city, street, building, building_extra, apt_number, room_count, area, residential, furnished, has_parking, monthly_price)" +
                $" values ({prop.ToSqlString()})";
            using (connection)
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        internal static void AddTenant(Tenant tenant)
        {
            var connection = new SqlConnection(connectionString);
            var com = "insert into tenant(surname, name, patronym, dob, phone, passport)" +
                      $" values ({tenant.ToSqlString()})";
            using (connection)
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        internal static void AddLease(Lease lease)
        {
            var connection = new SqlConnection(connectionString);
            var com = "insert into lease(property_id, tenant_id, monthly_price, start_date, end_date)" +
                      $" values ({lease.ToSqlString()})";
            {
                var command = new SqlCommand(com, connection);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}