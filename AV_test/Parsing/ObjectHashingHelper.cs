using System;
using System.Security.Cryptography;
using System.Text;

namespace AV_test;

public static class ObjectHashingHelper
{
    public static string ComputeSha256Hash(object obj)
    {
        // Convert object to JSON string
        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
    
        // Compute hash
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(jsonString));
        
        // Convert hash to base64 string
        var base64String = Convert.ToBase64String(bytes);
        
        return base64String;
    }
}