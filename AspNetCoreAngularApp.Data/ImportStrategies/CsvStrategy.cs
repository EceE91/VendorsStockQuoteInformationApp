using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Data.ImportStrategies
{
    public class CsvStrategy: IImportStrategy
    {
        public StockQuote ImportStockQuote(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                List<StockQuote> result = new List<StockQuote>();
                for (int i = 1; i < lines.Length; i++)
                {
                    string l = lines[i];
                    string[] parsedLine = l.Split(',');
                    result.Add(
                        new StockQuote
                        {
                            Name = parsedLine[0],
                            Symbol = parsedLine[1],
                            LastPrice = double.Parse(parsedLine[2]),
                            Change = double.Parse(parsedLine[3]),
                            ChangePercent = double.Parse(parsedLine[4]),
                            Timestamp = parsedLine[5],
                            MSDate = double.Parse(parsedLine[6]),
                            MarketCap = decimal.Parse(parsedLine[7]),
                            Volume = decimal.Parse(parsedLine[8]),
                            ChangeYTD = double.Parse(parsedLine[9]),
                            ChangePercentYTD = double.Parse(parsedLine[10]),
                            High = decimal.Parse(parsedLine[11]),
                            Low = double.Parse(parsedLine[12]),
                            Open = double.Parse(parsedLine[13]),
                        }
                    );
                }

                return result.FirstOrDefault();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}