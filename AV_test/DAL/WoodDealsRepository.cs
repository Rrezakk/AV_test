using AV_test.DAL.Interfaces;
using AV_test.Parsing.Deserialization.WoodDeals;
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
            entity.object_hash = Domain.Helpers.ObjectHashingHelper.ComputeSha256Hash(entity);
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
        return Get(entity.SellerInn,entity.BuyerInn,entity.DealNumber);
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
            var report = new ReportWoodDeal();
            report.SellerName = reader.GetString(0);
            report.SellerInn = reader.GetString(1);
            report.BuyerName = reader.GetString(2);
            report.BuyerInn = reader.GetString(3);
            report.WoodVolumeBuyer = (float)reader.GetDouble(4);
            report.WoodVolumeSeller = (float)reader.GetDouble(5);
            report.DealDate = reader.GetString(6);
            report.DealNumber = reader.GetString(7);
            report.object_hash = reader.GetString(8);
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
        entity.object_hash = Domain.Helpers.ObjectHashingHelper.ComputeSha256Hash(entity);
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        // Construct the SQL command with parameters for the composite primary key and updated fields
        var sql = "UPDATE ReportWoodDeal " +
                  $"SET SellerName = '{entity.SellerName}', BuyerName = '{entity.BuyerName}', WoodVolumeBuyer = '{entity.WoodVolumeBuyer}', " +
                  $"WoodVolumeSeller = '{entity.WoodVolumeSeller}', DealDate = '{entity.DealDate}', object_hash = '{entity.object_hash}' " +
                  $"WHERE SellerInn = '{entity.SellerInn}' AND BuyerInn = '{entity.BuyerInn}' AND DealNumber = '{entity.DealNumber}'";
        using var command = new SqlCommand(sql, connection);
        // Execute the SQL command and check the number of rows affected
        var rowsAffected = command.ExecuteNonQuery();
        return rowsAffected != 0;
    }
}