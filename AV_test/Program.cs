using AV_test.DAL;
using AV_test.Parsing;
using AV_test.Parsing.PageParsers;
using System;
using System.Diagnostics;
using System.Threading;

namespace AV_test;

public static class Program
{
    private static void Main()
    {
        const long delayBetweenParsings = 10 * 60 * 1000;
        var repo = new WoodDealsRepository(
            @"Server=localhost\MSSQLSERVER01;Database=Test;Trusted_Connection=True;");
        repo.EnsureCreated();
        var queryExecutor = new QueryExecutor();
        var parsingSettings = new ParsingSettings()
        {
            DelayBetweenRequests = 50,
            SampleSize = 500,
        };
        var parser = new WoodDealsPageParser(parsingSettings,repo,queryExecutor);
        var stopwatch = new Stopwatch();
        while (true)
        {
            stopwatch.Restart();
            parser.DoCycle();
            if (stopwatch.ElapsedMilliseconds >= delayBetweenParsings) continue;
            var delay = delayBetweenParsings - stopwatch.ElapsedMilliseconds;
            Thread.Sleep(TimeSpan.FromMilliseconds(delay));// <= delayBetweenParsings

        }
    }
}