# BIT-SEF Local Agent API

## 1. Purpose
BIT-SEF Local Agent is a standalone Windows Service installed on the client PC. Third-party software sends HTTP requests to `127.0.0.1`; the Agent creates BIT-SEF-compatible CSV command files in the configured output folder.

Default base URL:

```text
http://127.0.0.1:5077
```

Default output folder:

```text
C:\Fatura\
```

The paths are configurable in `appsettings.json`.

## 2. File naming standard
All created files use:

```text
{TYPE}_{INVOICE_NO}_{yyyyMMddHHmmssfff}.csv
```

Examples:

```text
I_0000000187_20260828150054194.csv
C_0000000187_20260828150054194.csv
R_0000000187_20260828150054194.csv
Copy_0000000187_20260828150054194.csv
Pdf_0000000187_20260828150054194.csv
```

The filename identifies the request. The CSV content is authoritative and must follow BIT-SEF standards.

## 3. Decimal precision - IMPORTANT
All calculated numeric values must use **six decimal places**.

Required representation:

```text
0.000000
```

Examples:

```text
1.500000
7.500000
6.355932
1.144068
0.000000
```

Do not round intermediate calculations to 2 or 3 decimals. Incorrect precision can create differences between taxable base, VAT and total and can lead to fiscalization/calculation errors.

Example for a total including 18% VAT:

```text
Total = 7.500000
Base  = 7.500000 / 1.18 = 6.355932
VAT   = 7.500000 - 6.355932 = 1.144068
```

The JSON endpoint always writes decimal fields to CSV using `0.000000`. Third-party systems must still calculate their values with 6-decimal precision before sending them.

The Direct CSV endpoint rejects fiscal `S` and `T` rows whose numeric fields do not contain exactly 6 decimals.

## 4. Security - BIT Local Security v1
Every `POST` request requires:

```text
X-BIT-Timestamp
X-BIT-Nonce
X-BIT-Signature
```

`X-BIT-Timestamp` is Unix time in seconds (UTC).

`X-BIT-Nonce` is a unique random value for each request.

Canonical string:

```text
METHOD\n
PATH\n
TIMESTAMP\n
NONCE\n
SHA256(BODY)
```

Signature:

```text
HMAC-SHA256(sharedSecret, canonicalString)
```

The digest and signature are lowercase hexadecimal strings.

Example canonical request:

```text
POST
/api/bitsef/fiscal
1788000000
0f6fcbf2a4a44d9789941fa74f41a0ca
<sha256-body>
```

Default request lifetime is 60 seconds. A nonce cannot be reused.

`GET` status endpoints do not require HMAC.

## 5. JSON API
### Health/status

```http
GET /api/bitsef/status
```

### Fiscal I / C / R

```http
POST /api/bitsef/fiscal
Content-Type: application/json
```

`type` must be `I`, `C`, or `R`.

### Copy / PDF as JSON commands

```http
POST /api/bitsef/command
Content-Type: application/json
```

Copy body:

```json
{"type":"Copy","invoiceNo":"0000000187"}
```

PDF body:

```json
{"type":"Pdf","invoiceNo":"0000000187"}
```

Convenience routes are also available:

```http
POST /api/bitsef/copy/{invoiceNo}
POST /api/bitsef/pdf/{invoiceNo}
```

Copy creates a file whose content is:

```text
copy;0000000187
```

PDF creates a file whose content is:

```text
pdf;0000000187
```

## 6. Direct CSV integration - filesystem only

Direct CSV does not use HTTP, WebAPI or HMAC. Create the file directly in `C:\Fatura\`. Use `.tmp` then rename to `.csv` where possible. Filename: `{TYPE}_{INVOICE_NO}_{yyyyMMddHHmmssfff}.csv`. All calculated fiscal decimal values must use exactly `0.000000`.

## 7. BIT-SEF fiscal CSV rows
### I header

```text
I;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;ReferenceNo
```

### C / R header

```text
C;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;Reason;ReferenceDateTime;ReferenceNo
R;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;Reason;ReferenceDateTime;ReferenceNo
```

### S item

```text
S;OrderId;Barcode;Article;Price;Amount;Mass;Vat;DiscountPercentage;DiscountEUR;TotalArticle;VatArticle;ArticleType
```

Decimal S columns:

```text
Price, Amount, DiscountPercentage, DiscountEUR, TotalArticle, VatArticle
```

All use `0.000000`.

### T totals

```text
T;CountOrders;Total;TotalNoVat;DiscountOnTotal;TotalWithVat0;TotalWithVat8;OnlyVat8;TotalWithVat18;OnlyVat18;TypeOfPayment;DiscountOnPercentage;DiscountEUR
```

All monetary/percentage values use `0.000000`; `CountOrders` and `TypeOfPayment` are integers.

## 8. Accepted response
On a successful command submission:

```json
{
  "success": true,
  "requestId": "f2aa0e8cb42d49a19a05889d54ec06ab",
  "invoiceNo": "0000000187",
  "fileName": "I_0000000187_20260828150054194.csv",
  "status": "Pending"
}
```

The request being accepted means the Agent created the command file. It does **not** yet mean BIT-SEF completed fiscalization/printing.

## 9. Feedback/status
Poll:

```http
GET /api/bitsef/status/{requestId}
```

The Agent checks for successful processing using the exact generated filename plus `.bak`:

```text
I_0000000187_20260828150054194.csv.bak
```

If found:

```text
Completed
```

The Agent detects an error by finding a filename beginning with:

```text
error.{INVOICE_NO}
```

Therefore both are detected:

```text
error.0000000187.csv
error.0000000187.csv.bak
```

If no success/error file is found:

```text
Pending
```

After the configured timeout:

```text
Timeout
```

`Timeout` is informational and is not a confirmed failure; BIT-SEF may still be processing/printing.

Recommended polling interval for web integrations: 1-2 seconds.

## 10. Configuration
`appsettings.json`:

```json
{
  "Urls": "http://127.0.0.1:5077",
  "BitSef": {
    "OutputPath": "C:\\temp\\fatura\\",
    "ArchivePath": "C:\\temp\\fatura\\archive\\",
    "PendingTimeoutSeconds": 60
  },
  "Security": {
    "SharedSecret": "CHANGE-THIS-BIT-SEF-SECRET",
    "MaxAgeSeconds": 60
  }
}
```

The shared secret in the third-party application must exactly match the Agent configuration.

## 11. Encoding
Files are written as UTF-8 without BOM. CSV uses semicolon (`;`) delimiters.
