using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace AV_test.Domain.Helpers
{
    public class ObjectHashingHelper
    {
        public static string GetObjectHash(object o)
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, o);
            var bytes = stream.ToArray();
            using (HashAlgorithm hashAlg = SHA256.Create())
            {
                var hash = hashAlg.ComputeHash(bytes);
                return BitConverter.ToString(hash);
            }
        }
    }
}
