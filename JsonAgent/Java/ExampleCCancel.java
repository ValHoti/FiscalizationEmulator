public class ExampleCCancel { public static void main(String[] a)throws Exception { BitSefClient c=new BitSefClient("http://127.0.0.1:5077","CHANGE-THIS-BIT-SEF-SECRET"); String json="{" + "\n" +
   "  \"type\": \"C\"," + "\n" +
   "  \"invoiceNo\": \"0000000187\"," + "\n" +
   "  \"invoiceDate\": \"2026-08-29T11:30:00\"," + "\n" +
   "  \"clientId\": \"0\"," + "\n" +
   "  \"clientName\": \"Bleres Qytetar\"," + "\n" +
   "  \"workerId\": \"1\"," + "\n" +
   "  \"workerName\": \"admin\"," + "\n" +
   "  \"reason\": \"Test reason\"," + "\n" +
   "  \"referenceDateTime\": \"2026-08-29T10:00:00\"," + "\n" +
   "  \"referenceNo\": \"0000000100\"," + "\n" +
   "  \"items\": [" + "\n" +
   "    {" + "\n" +
   "      \"orderId\": 1," + "\n" +
   "      \"barcode\": \"8008423400539\"," + "\n" +
   "      \"article\": \"Aceton\"," + "\n" +
   "      \"price\": 1.500000," + "\n" +
   "      \"amount\": 5.000000," + "\n" +
   "      \"mass\": \"Cope\"," + "\n" +
   "      \"vat\": \"E\"," + "\n" +
   "      \"discountPercentage\": 0.000000," + "\n" +
   "      \"discountEuro\": 0.000000," + "\n" +
   "      \"totalArticle\": 7.500000," + "\n" +
   "      \"vatArticle\": 1.144068," + "\n" +
   "      \"articleType\": \"TT\"" + "\n" +
   "    }" + "\n" +
   "  ]," + "\n" +
   "  \"totals\": {" + "\n" +
   "    \"countOrders\": 1," + "\n" +
   "    \"total\": 7.500000," + "\n" +
   "    \"totalNoVat\": 6.355932," + "\n" +
   "    \"discountOnTotal\": 0.000000," + "\n" +
   "    \"totalWithVat0\": 0.000000," + "\n" +
   "    \"totalWithVat8\": 0.000000," + "\n" +
   "    \"onlyVat8\": 0.000000," + "\n" +
   "    \"totalWithVat18\": 7.500000," + "\n" +
   "    \"onlyVat18\": 1.144068," + "\n" +
   "    \"typeOfPayment\": 1," + "\n" +
   "    \"discountOnPercentage\": 0.000000," + "\n" +
   "    \"discountEuro\": 0.000000" + "\n" +
   "  }" + "\n" +
   "}"; System.out.println(c.fiscalJson(json)); } }
