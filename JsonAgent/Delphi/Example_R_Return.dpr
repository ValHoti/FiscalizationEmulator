program Example_R_Return; {$APPTYPE CONSOLE} uses System.SysUtils, BitSefClient; var C:TBitSefClient;J:string; begin C:=TBitSefClient.Create('http://127.0.0.1:5077','CHANGE-THIS-BIT-SEF-SECRET'); try J:='{' + sLineBreak +
 '  "type": "R",' + sLineBreak +
 '  "invoiceNo": "0000000187",' + sLineBreak +
 '  "invoiceDate": "2026-08-29T11:30:00",' + sLineBreak +
 '  "clientId": "0",' + sLineBreak +
 '  "clientName": "Bleres Qytetar",' + sLineBreak +
 '  "workerId": "1",' + sLineBreak +
 '  "workerName": "admin",' + sLineBreak +
 '  "reason": "Test reason",' + sLineBreak +
 '  "referenceDateTime": "2026-08-29T10:00:00",' + sLineBreak +
 '  "referenceNo": "0000000100",' + sLineBreak +
 '  "items": [' + sLineBreak +
 '    {' + sLineBreak +
 '      "orderId": 1,' + sLineBreak +
 '      "barcode": "8008423400539",' + sLineBreak +
 '      "article": "Aceton",' + sLineBreak +
 '      "price": 1.500000,' + sLineBreak +
 '      "amount": 5.000000,' + sLineBreak +
 '      "mass": "Cope",' + sLineBreak +
 '      "vat": "E",' + sLineBreak +
 '      "discountPercentage": 0.000000,' + sLineBreak +
 '      "discountEuro": 0.000000,' + sLineBreak +
 '      "totalArticle": 7.500000,' + sLineBreak +
 '      "vatArticle": 1.144068,' + sLineBreak +
 '      "articleType": "TT"' + sLineBreak +
 '    }' + sLineBreak +
 '  ],' + sLineBreak +
 '  "totals": {' + sLineBreak +
 '    "countOrders": 1,' + sLineBreak +
 '    "total": 7.500000,' + sLineBreak +
 '    "totalNoVat": 6.355932,' + sLineBreak +
 '    "discountOnTotal": 0.000000,' + sLineBreak +
 '    "totalWithVat0": 0.000000,' + sLineBreak +
 '    "totalWithVat8": 0.000000,' + sLineBreak +
 '    "onlyVat8": 0.000000,' + sLineBreak +
 '    "totalWithVat18": 7.500000,' + sLineBreak +
 '    "onlyVat18": 1.144068,' + sLineBreak +
 '    "typeOfPayment": 1,' + sLineBreak +
 '    "discountOnPercentage": 0.000000,' + sLineBreak +
 '    "discountEuro": 0.000000' + sLineBreak +
 '  }' + sLineBreak +
 '}'; Writeln(C.FiscalJson(J)); finally C.Free;end;Readln;end.
