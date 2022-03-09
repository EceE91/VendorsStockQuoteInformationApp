using System;
using System.IO;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Models;
using Newtonsoft.Json;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Data.ImportStrategies
{
    public class JsonStrategy: IImportStrategy
    {
        public StockQuote ImportStockQuote(string filePath)
        {
            try
            {
                StreamReader streamReader = new StreamReader(filePath);
                string jsonString = streamReader.ReadToEnd();
                StockQuote result = JsonConvert.DeserializeObject<StockQuote>(jsonString);
                return result;
            }
            catch (JsonSerializationException e)
            {
                Console.WriteLine(e);
                throw;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}