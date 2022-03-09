namespace AspNetCoreAngularApp.AspNetCoreAngularApp.Api.ViewModels
{
    public class StockQuoteViewModel
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double LastPrice { get; set; }
        public double Change { get; set; }
        public double ChangePercent { get; set; }
        public string Timestamp { get; set; }
        public double MSDate { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Volume { get; set; }
        public double ChangeYTD { get; set; }
        public double ChangePercentYTD { get; set; }
        public decimal High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
    }
}