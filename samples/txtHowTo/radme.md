# Fiscal CSV Export Format Documentation

This document describes the structure of the three CSV export formats used for fiscal reporting: **invoice.csv**, **cancel.csv**, and **return.csv**.

## General Rules

- **Field delimiter:** semicolon (`;`)
- **Line delimiter:** `CRLF` (`\r\n`)
- Each file is composed of **three record types**, identified by the first field of each line:
  - `I` / `C` — Header record (one per invoice/cancellation/return)
  - `S` — Line item record (one per sold/cancelled/returned article; repeated per order line)
  - `T` — Totals record (one per invoice, summarizing all `S` lines)
- Fields marked **(optional)** may be left empty depending on configuration.

---

## 1. invoice.csv — Invoice

### Header Record — `I`

Raw format:
```
I;invoiceno;invoicedatetime;buyersid;buyersname;sellersdbid;sellername;referenceno
```

| # | Field (raw)     | Mapped Name         | Description                          |
|---|------------------|---------------------|---------------------------------------|
| 1 | `I`              | RecordType          | Record identifier — always `I`        |
| 2 | invoiceno        | InvoiceNo           | Invoice number                        |
| 3 | invoicedatetime  | InvoiceDatetime     | Date and time of the invoice          |
| 4 | buyersid         | ClientId            | Buyer's identifier                    |
| 5 | buyersname       | ClientName          | Buyer's name                          |
| 6 | sellersdbid      | WorkerId            | Seller/operator identifier            |
| 7 | sellername       | WorkerName           | Seller/operator name                  |
| 8 | referenceno      | ReferenceNo         | Reference number                      |

### Line Item Record — `S`

Raw format:
```
S;OrderNo;PLUNumberOnDB;Item;Price;Amount;Mass;Vat;Discount Percentage;Discount Euro;TotalCalcOfRow;TotalVatOfRow
```

| #  | Field (raw)          | Mapped Name              | Description                              |
|----|-----------------------|---------------------------|--------------------------------------------|
| 1  | `S`                  | RecordType                | Record identifier — always `S`             |
| 2  | OrderNo              | OrderId                   | Order line number                          |
| 3  | PLUNumberOnDB        | Barcode                   | PLU / barcode of the article               |
| 4  | Item                 | ArticleForSale            | Article/item name                          |
| 5  | Price                | Price                     | Unit price                                 |
| 6  | Amount               | Amount                    | Quantity                                   |
| 7  | Mass                 | Mass                      | Mass/weight (if applicable)                |
| 8  | Vat                  | Vat                        | VAT rate applied                           |
| 9  | Discount Percentage  | DiscountPercentage        | Discount applied as a percentage           |
| 10 | Discount Euro        | DiscountEUR               | Discount applied in currency (EUR)         |
| 11 | TotalCalcOfRow       | TotalArtikulli *(optional)*| Total calculated value of the row          |
| 12 | TotalVatOfRow        | TvshArtikulli *(optional)* | Total VAT value of the row                 |

### Totals Record — `T`

Raw format:
```
T;CountOfOrders;Total;TotalNoVat;Discount;TotalWithVat0;TotalWithVat8;OnlyVat8;TotalWithVat18;OnlyVat8;TypeOfPayment
```

| #  | Field (raw)      | Mapped Name        | Description                                  |
|----|-------------------|----------------------|-----------------------------------------------|
| 1  | `T`              | RecordType           | Record identifier — always `T`                |
| 2  | CountOfOrders    | CountOrders          | Total number of order lines                   |
| 3  | Total            | Total                | Grand total                                    |
| 4  | TotalNoVat       | TotalNoVat           | Total excluding VAT                            |
| 5  | Discount         | DiscountOnTotal      | Total discount applied                         |
| 6  | TotalWithVat0    | TotalWithVat0        | Total for items with 0% VAT                    |
| 7  | TotalWithVat8    | TotalWithVat8        | Total for items with 8% VAT                    |
| 8  | OnlyVat8         | OnlyVat8             | VAT amount only, at 8%                         |
| 9  | TotalWithVat18   | TotalWithVat18       | Total for items with 18% VAT                   |
| 10 | OnlyVat8         | OnlyVat18            | VAT amount only, at 18%                        |
| 11 | TypeOfPayment    | TypeOfPayment        | Payment method used                            |

---

## 2. cancel.csv — Invoice Cancellation

### Header Record — `C`

Raw format:
```
C;invoiceno;invoicedatetime;buyersid;buyersname;sellersdbid;sellername;referenceno;Reason of cancelation
```

| # | Field (raw)             | Mapped Name         | Description                          |
|---|--------------------------|----------------------|----------------------------------------|
| 1 | `C`                     | RecordType           | Record identifier — always `C`         |
| 2 | invoiceno               | InvoiceNo            | Invoice number being cancelled         |
| 3 | invoicedatetime         | InvoiceDatetime      | Date and time of the invoice           |
| 4 | buyersid                | ClientId             | Buyer's identifier                     |
| 5 | buyersname              | ClientName           | Buyer's name                           |
| 6 | sellersdbid             | WorkerId             | Seller/operator identifier             |
| 7 | sellername              | WorkerName            | Seller/operator name                   |
| 8 | referenceno             | ReferenceNo          | Reference number                       |
| 9 | Reason of cancelation   | ReasonOfCancelation  | Reason for the cancellation            |

### Line Item Record — `S`

Same structure as in [invoice.csv — Line Item Record](#line-item-record--s).

### Totals Record — `T`

Same structure as in [invoice.csv — Totals Record](#totals-record--t).

---

## 3. return.csv — Invoice Return

> **Note:** the header line in the source spec uses `C` as the record identifier (same as cancellation), even though the file is named `return.csv`.

### Header Record — `C`

Raw format:
```
C;invoiceno;invoicedatetime;buyersid;buyersname;sellersdbid;sellername;referenceno;Reason of cancelation
```

| # | Field (raw)             | Mapped Name         | Description                          |
|---|--------------------------|----------------------|----------------------------------------|
| 1 | `C`                     | RecordType           | Record identifier — always `C`         |
| 2 | invoiceno               | InvoiceNo            | Original invoice number being returned |
| 3 | invoicedatetime         | InvoiceDatetime      | Date and time of the invoice           |
| 4 | buyersid                | ClientId             | Buyer's identifier                     |
| 5 | buyersname              | ClientName           | Buyer's name                           |
| 6 | sellersdbid             | WorkerId             | Seller/operator identifier             |
| 7 | sellername              | WorkerName            | Seller/operator name                   |
| 8 | referenceno             | ReferenceNo          | Reference number                       |
| 9 | Reason of cancelation   | ReasonOfCancelation  | Reason for the return                  |

### Line Item Record — `S`

Same structure as in [invoice.csv — Line Item Record](#line-item-record--s).

### Totals Record — `T`

Same structure as in [invoice.csv — Totals Record](#totals-record--t).

---

## Summary Table — Record Types by File

| File          | Purpose               | Header Record | Line Record | Totals Record |
|---------------|------------------------|:--------------:|:-------------:|:----------------:|
| invoice.csv   | Invoice                | `I`            | `S`           | `T`              |
| cancel.csv    | Invoice Cancellation   | `C`            | `S`           | `T`              |
| return.csv    | Invoice Return         | `C`            | `S`           | `T`              |
