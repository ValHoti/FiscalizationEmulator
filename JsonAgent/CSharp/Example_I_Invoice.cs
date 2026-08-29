using System;
using System.Threading.Tasks;
class Example_I_Invoice
{
 static async Task Main()
 {
  using (var c = new BitSefClient("http://127.0.0.1:5077", "CHANGE-THIS-BIT-SEF-SECRET"))
  {
   string json = @"{
  ""type"": ""I"",
  ""invoiceNo"": ""0000000187"",
  ""invoiceDate"": ""2026-08-29T11:30:00"",
  ""clientId"": ""0"",
  ""clientName"": ""Bleres Qytetar"",
  ""workerId"": ""1"",
  ""workerName"": ""admin"",
  ""items"": [
    {
      ""orderId"": 1,
      ""barcode"": ""8008423400539"",
      ""article"": ""Aceton"",
      ""price"": 1.500000,
      ""amount"": 5.000000,
      ""mass"": ""Cope"",
      ""vat"": ""E"",
      ""discountPercentage"": 0.000000,
      ""discountEuro"": 0.000000,
      ""totalArticle"": 7.500000,
      ""vatArticle"": 1.144068,
      ""articleType"": ""TT""
    }
  ],
  ""totals"": {
    ""countOrders"": 1,
    ""total"": 7.500000,
    ""totalNoVat"": 6.355932,
    ""discountOnTotal"": 0.000000,
    ""totalWithVat0"": 0.000000,
    ""totalWithVat8"": 0.000000,
    ""onlyVat8"": 0.000000,
    ""totalWithVat18"": 7.500000,
    ""onlyVat18"": 1.144068,
    ""typeOfPayment"": 1,
    ""discountOnPercentage"": 0.000000,
    ""discountEuro"": 0.000000
  }
}";
   Console.WriteLine(await c.FiscalJsonAsync(json));
  }
 }
}
