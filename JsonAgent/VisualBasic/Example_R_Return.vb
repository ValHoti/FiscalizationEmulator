Imports System
Imports System.Threading.Tasks
Module Example_R_Return
 Async Function Main() As Task
  Using c As New BitSefClient("http://127.0.0.1:5077","CHANGE-THIS-BIT-SEF-SECRET")
   Dim json As String = "{" & vbCrLf & _
            "  ""type"": ""R""," & vbCrLf & _
            "  ""invoiceNo"": ""0000000187""," & vbCrLf & _
            "  ""invoiceDate"": ""2026-08-29T11:30:00""," & vbCrLf & _
            "  ""clientId"": ""0""," & vbCrLf & _
            "  ""clientName"": ""Bleres Qytetar""," & vbCrLf & _
            "  ""workerId"": ""1""," & vbCrLf & _
            "  ""workerName"": ""admin""," & vbCrLf & _
            "  ""reason"": ""Test reason""," & vbCrLf & _
            "  ""referenceDateTime"": ""2026-08-29T10:00:00""," & vbCrLf & _
            "  ""referenceNo"": ""0000000100""," & vbCrLf & _
            "  ""items"": [" & vbCrLf & _
            "    {" & vbCrLf & _
            "      ""orderId"": 1," & vbCrLf & _
            "      ""barcode"": ""8008423400539""," & vbCrLf & _
            "      ""article"": ""Aceton""," & vbCrLf & _
            "      ""price"": 1.500000," & vbCrLf & _
            "      ""amount"": 5.000000," & vbCrLf & _
            "      ""mass"": ""Cope""," & vbCrLf & _
            "      ""vat"": ""E""," & vbCrLf & _
            "      ""discountPercentage"": 0.000000," & vbCrLf & _
            "      ""discountEuro"": 0.000000," & vbCrLf & _
            "      ""totalArticle"": 7.500000," & vbCrLf & _
            "      ""vatArticle"": 1.144068," & vbCrLf & _
            "      ""articleType"": ""TT""" & vbCrLf & _
            "    }" & vbCrLf & _
            "  ]," & vbCrLf & _
            "  ""totals"": {" & vbCrLf & _
            "    ""countOrders"": 1," & vbCrLf & _
            "    ""total"": 7.500000," & vbCrLf & _
            "    ""totalNoVat"": 6.355932," & vbCrLf & _
            "    ""discountOnTotal"": 0.000000," & vbCrLf & _
            "    ""totalWithVat0"": 0.000000," & vbCrLf & _
            "    ""totalWithVat8"": 0.000000," & vbCrLf & _
            "    ""onlyVat8"": 0.000000," & vbCrLf & _
            "    ""totalWithVat18"": 7.500000," & vbCrLf & _
            "    ""onlyVat18"": 1.144068," & vbCrLf & _
            "    ""typeOfPayment"": 1," & vbCrLf & _
            "    ""discountOnPercentage"": 0.000000," & vbCrLf & _
            "    ""discountEuro"": 0.000000" & vbCrLf & _
            "  }" & vbCrLf & _
            "}"
   Console.WriteLine(Await c.FiscalJsonAsync(json))
  End Using
 End Function
End Module
