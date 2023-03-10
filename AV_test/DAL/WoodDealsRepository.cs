using AV_test.DAL.Interfaces;
using AV_test.Parsing.Deserialization;
using System;
using System.Data.SqlClient;
using System.Globalization;

namespace AV_test.DAL;

public class WoodDealsRepository:IWoodDealsRepository
{
    private readonly string _connectionString;
    public WoodDealsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public bool Create(ReportWoodDeal entity)
    {
        try
        {
            entity.object_hash = ObjectHashingHelper.ComputeSha256Hash(entity);
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = $@"
            INSERT INTO ReportWoodDeal (SellerName, SellerInn, BuyerName, BuyerInn, WoodVolumeBuyer, WoodVolumeSeller, DealDate, DealNumber, object_hash)
            VALUES ('{entity.SellerName}', '{entity.SellerInn}', '{entity.BuyerName}', '{entity.BuyerInn}',
             {entity.WoodVolumeBuyer.ToString(CultureInfo.InvariantCulture).Replace(',', '.')},
              {entity.WoodVolumeSeller.ToString(CultureInfo.InvariantCulture).Replace(',', '.')},
                    '{entity.DealDate}', '{entity.DealNumber}', '{entity.object_hash}')";
            command.ExecuteNonQuery();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception creating deal: {e.Message}");
            return false;
        }
    }
    public ReportWoodDeal? Get(ReportWoodDeal entity)
    {
        if (!string.IsNullOrWhiteSpace(entity.SellerInn) && 
            !string.IsNullOrWhiteSpace(entity.BuyerInn) &&
            !string.IsNullOrWhiteSpace(entity.DealNumber))
            return Get(entity.SellerInn!, entity.BuyerInn!, entity.DealNumber!);
        Console.WriteLine("Error: primary key is not valid");
        return null;
    }
    public ReportWoodDeal? Get(string sellerInn, string buyerInn, string dealNumber)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText =
                "SELECT sellerName, sellerInn, buyerName, buyerInn, woodVolumeBuyer, woodVolumeSeller, dealDate, dealNumber, object_hash " +
                $"FROM ReportWoodDeal WHERE sellerInn = '{sellerInn}' AND buyerInn = '{buyerInn}' AND dealNumber = '{dealNumber}'";
            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;
            var report = new ReportWoodDeal
            {
                SellerName = reader.GetString(0),
                SellerInn = reader.GetString(1),
                BuyerName = reader.GetString(2),
                BuyerInn = reader.GetString(3),
                WoodVolumeBuyer = (float)reader.GetDouble(4),
                WoodVolumeSeller = (float)reader.GetDouble(5),
                DealDate = reader.GetString(6),
                DealNumber = reader.GetString(7),
                object_hash = reader.GetString(8)
            };
            return report;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception getting deal: {e.Message}");
            return null;
        }
    }
    public bool Edit(ReportWoodDeal entity)
    {
        try
        {
            entity.object_hash = ObjectHashingHelper.ComputeSha256Hash(entity);
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Construct the SQL command with parameters for the composite primary key and updated fields
            var sql = "UPDATE ReportWoodDeal " +
                      $"SET SellerName = '{entity.SellerName}', BuyerName = '{entity.BuyerName}', " +
                      $"WoodVolumeBuyer = '{entity.WoodVolumeBuyer.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}', " +
                      $"WoodVolumeSeller = '{entity.WoodVolumeSeller.ToString(CultureInfo.InvariantCulture).Replace(',', '.')}', " +
                      $"DealDate = '{entity.DealDate}', object_hash = '{entity.object_hash}' " +
                      $"WHERE SellerInn = '{entity.SellerInn}' AND BuyerInn = '{entity.BuyerInn}' AND DealNumber = '{entity.DealNumber}'";
            using var command = new SqlCommand(sql, connection);
            // Execute the SQL command and check the number of rows affected
            var rowsAffected = command.ExecuteNonQuery();
            return rowsAffected != 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception editing deal: {e.Message}");
            return false;
        }
        
    }
    public bool EnsureCreated()
    {
        try
        {
            CreateReportWoodDealTable();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception during table creation: {e.Message}");
            return false;
        }
    }
    private void CreateReportWoodDealTable()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReportWoodDeal]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE ReportWoodDeal (
                        SellerName NVARCHAR(255) NOT NULL,
                        SellerInn CHAR(12) NOT NULL,
                        BuyerName NVARCHAR(255) NOT NULL,
                        BuyerInn CHAR(12) NOT NULL,
                        WoodVolumeBuyer FLOAT NOT NULL,
                        WoodVolumeSeller FLOAT NOT NULL,
                        DealDate CHAR(10) NOT NULL,
                        DealNumber CHAR(28) NOT NULL,
                        Object_hash CHAR(44) NOT NULL,
                        CONSTRAINT PK_ReportWoodDeal PRIMARY KEY (SellerInn, BuyerInn, DealNumber) 
                    )
                END";
        command.ExecuteNonQuery();
        connection.Close();
    }
}