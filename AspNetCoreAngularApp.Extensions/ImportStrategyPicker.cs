using System;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Core.Interfaces;
using AspNetCoreAngularApp.AspNetCoreAngularApp.Data.ImportStrategies;

namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Extensions
{
    public static class ImportStrategyPicker
    {
        public static IImportStrategy Select(string fileType)
        {
            return fileType switch
            {
                ".csv" => new CsvStrategy(),
                ".json" => new JsonStrategy(),
                _ => throw new ApplicationException("No strategy found for file type: " + fileType)
            };
        }
    }
}