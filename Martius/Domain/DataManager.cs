using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Martius.Data;
using Martius.Infrastructure;

namespace Martius.Domain
{
    public static class DataManager
    {
        private static readonly string connectionString = ConnectionConfig.ConnectionString;

        public static List<RealProperty> GetAllRealProperties()
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

            if (result.Count == 0)
                throw new EmptyDbTableException();
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
            var property = new RealProperty(id, address, roomCount, area, res, furn, park);
            return property;
        }

        public static List<Tenant> GetAllTenants()
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

            if (result.Count == 0)
                throw new EmptyDbTableException();
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

        public static List<Lease> GetAllLeases()
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

            if (result.Count == 0)
                throw new EmptyDbTableException();
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

        public static RealProperty GetPropertyById(int propId)
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

        public static Tenant GetTenantById(int tenantId)
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

        public static Lease GetLeaseById(int leaseId)
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
            var building = reader["building"].ToString();
            var aptNumber = (int?) reader["apt_number"];
            return new Address(city, street, building, aptNumber);
        }

        private static Person GetPerson(SqlDataReader reader)
        {
            var surname = reader["surname"].ToString();
            var name = reader["name"].ToString();
            var patronym = reader["patronym"].ToString();
            var dobString = reader["dob"].ToString();
            return new Person(surname, name, patronym, DateTime.Parse(dobString));
        }
    }
}