using AV_test.DAL;
using AV_test.Parsing;
using AV_test.Parsing.PageParsers;
using System;

namespace AV_test
{
    public static class Program
    {
        private static void Main()
        {
             var repo = new WoodDealsRepository(
                 @"Server=localhost\MSSQLSERVER01;Database=Test;Trusted_Connection=True;");
             var queryExecutor = new QueryExecutor();
             var parser = new WoodDealsPageParser(repo,queryExecutor);
            parser.DoCycle();
            Console.ReadLine();
        }
    }
}
