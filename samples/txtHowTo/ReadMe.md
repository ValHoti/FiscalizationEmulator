# Fiscal CSV File Format Specification

This document defines the required structure and meaning of the fiscal CSV files:

- `invoice.csv`
- `cancel.csv`
- `return.csv`

The purpose of this specification is to describe:

- how each file must be generated,
- which record types it must contain,
- the exact field order,
- the required decimal precision,
- and the meaning of every value.

---

# 1. General File Rules

All fiscal CSV files must follow these rules:

- **Encoding:** UTF-8
- **Field delimiter:** semicolon (`;`)
- **Line delimiter:** CRLF (`\r\n`)
- **Decimal separator:** dot (`.`)
- **Decimal precision:** exactly **6 digits after the decimal point**
- No thousands separators may be used.
- Empty optional fields may be left empty.
- Each file must contain:
  - one header record,
  - zero or more item records,
  - one totals record.

Example decimal values:

```text
3.900000
1.000000
0.400000
16.042000
13.595000
2.447000
```

Incorrect examples:

```text
3.9
1
0.4
16,042
13.595
```

Correct fixed format:

```text
3.900000
1.000000
0.400000
16.042000
13.595000
```

---

# 2. Record Types

The first field of every line identifies the record type.

| Record Type | Meaning |
|---|---|
| `I` | Invoice header |
| `C` | Cancellation header |
| `R` | Return header |
| `S` | Item / article line |
| `T` | Totals record |

---

# 3. invoice.csv

An invoice file contains:

```text
I
S
S
...
T
```

## 3.1 Header Record — `I`

### Format

```text
I;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;ReferenceNo
```

### Fields

| # | Field | Meaning |
|---:|---|---|
| 1 | `I` | Identifies the line as an invoice header |
| 2 | InvoiceNo | Number of the current invoice |
| 3 | InvoiceDatetime | Date and time when the invoice was created |
| 4 | ClientId | Customer/buyer identifier |
| 5 | ClientName | Customer/buyer name |
| 6 | WorkerId | Seller/operator identifier |
| 7 | WorkerName | Seller/operator name |
| 8 | ReferenceNo | Reference document number; normally `0` for a normal invoice |

### Example

```text
I;0000000145;20260827133225;000000000;Bleres Qytetar;1;admin;0
```

---

# 4. cancel.csv

A cancellation file contains:

```text
C
S
S
...
T
```

## 4.1 Header Record — `C`

### Format

```text
C;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;ReferenceNo;ReasonOfCancel;ReferenceDateTime
```

### Fields

| # | Field | Meaning |
|---:|---|---|
| 1 | `C` | Identifies the line as a cancellation header |
| 2 | InvoiceNo | Number of the new cancellation document |
| 3 | InvoiceDatetime | Date and time of the cancellation |
| 4 | ClientId | Customer/buyer identifier |
| 5 | ClientName | Customer/buyer name |
| 6 | WorkerId | Seller/operator identifier |
| 7 | WorkerName | Seller/operator name |
| 8 | ReferenceNo | Number of the original invoice being cancelled |
| 9 | ReasonOfCancel | Reason for the cancellation |
| 10 | ReferenceDateTime | Date and time of the original referenced invoice |

### Important

`InvoiceNo` and `ReferenceNo` represent different documents:

```text
InvoiceNo   = new cancellation document
ReferenceNo = original invoice being cancelled
```

### Example

```text
C;0000000150;20260827141025;000000000;Bleres Qytetar;1;admin;0000000145;Gabim ne fature;20260827133225
```

---

# 5. return.csv

A return file contains:

```text
R
S
S
...
T
```

## 5.1 Header Record — `R`

### Format

```text
R;InvoiceNo;InvoiceDatetime;ClientId;ClientName;WorkerId;WorkerName;ReferenceNo;ReasonOfReturn;ReferenceDateTime
```

### Fields

| # | Field | Meaning |
|---:|---|---|
| 1 | `R` | Identifies the line as a return header |
| 2 | InvoiceNo | Number of the new return document |
| 3 | InvoiceDatetime | Date and time of the return |
| 4 | ClientId | Customer/buyer identifier |
| 5 | ClientName | Customer/buyer name |
| 6 | WorkerId | Seller/operator identifier |
| 7 | WorkerName | Seller/operator name |
| 8 | ReferenceNo | Number of the original invoice related to the return |
| 9 | ReasonOfReturn | Reason for the return |
| 10 | ReferenceDateTime | Date and time of the original referenced invoice |

### Example

```text
R;0000000151;20260827142510;000000000;Bleres Qytetar;1;admin;0000000145;Kthim artikulli;20260827133225
```

---

# 6. Item Record — `S`

The `S` record format is the same for invoice, cancellation, and return files.

## 6.1 Format

```text
S;OrderId;Barcode;ArticleForSale;OriginalPrice;Amount;Mass;Vat;DiscountPercentage;DiscountEUR;FinalPrice;FinalTotal;VatAmount;ArticleType
```

## 6.2 Fields

| # | Field | Meaning |
|---:|---|---|
| 1 | `S` | Identifies the line as an item/article record |
| 2 | OrderId | Sequential line number inside the document |
| 3 | Barcode | Barcode, PLU, serial number, or article identifier |
| 4 | ArticleForSale | Article/item description |
| 5 | OriginalPrice | Original unit price before discounts |
| 6 | Amount | Quantity |
| 7 | Mass | Unit of measure, for example `Cope`, `Kg`, `L`, `M` |
| 8 | Vat | VAT category code |
| 9 | DiscountPercentage | Discount percentage applied to the item |
| 10 | DiscountEUR | Discount amount applied to the item |
| 11 | FinalPrice | Final unit price after all applicable calculations |
| 12 | FinalTotal | Final total value of the item row |
| 13 | VatAmount | VAT amount corresponding to the item row |
| 14 | ArticleType | Fiscal/article type code |

---

# 7. Meaning of OriginalPrice and FinalPrice

These two fields must be treated as different values.

## OriginalPrice

`OriginalPrice` represents the original unit price before discounts.

Example:

```text
OriginalPrice = 3.900000
```

This value is useful for:

- showing the original price,
- displaying discounts,
- reporting,
- audit purposes.

## FinalPrice

`FinalPrice` represents the effective final unit price after all applicable calculations.

Example:

```text
FinalPrice = 3.070000
```

When `FinalPrice` is present, it must be treated as the authoritative final unit price.

It should not be reconstructed from:

```text
OriginalPrice - DiscountEUR
```

or:

```text
OriginalPrice - DiscountPercentage
```

because the final value may also include:

- total-level discounts,
- distributed discounts,
- rounding adjustments,
- other calculation rules.

Conceptually:

```text
FinalPrice = FinalTotal / Amount
```

---

# 8. Meaning of FinalTotal

`FinalTotal` represents the final gross value of the full item row.

Conceptually:

```text
FinalTotal = FinalPrice × Amount
```

Example:

```text
FinalPrice = 3.070000
Amount     = 1.000000
FinalTotal = 3.070000
```

For quantities greater than 1:

```text
FinalPrice = 2.345678
Amount     = 3.000000
FinalTotal = 7.037034
```

The value written in `FinalTotal` should be preserved with 6-decimal precision.

---

# 9. Meaning of VatAmount

`VatAmount` represents the VAT amount associated with the item row.

Example:

```text
FinalTotal = 3.070000
VatAmount  = 0.468305
```

The row value can be used for:

- detailed reporting,
- audit,
- item-level VAT information.

However, document-level VAT totals should be calculated from the final gross totals grouped by VAT category, because summing rounded row VAT values can cause differences of `0.001`.

---

# 10. VAT Codes

The supported VAT categories are:

| Code | VAT Rate |
|---|---:|
| `C` | 0% |
| `D` | 8% |
| `E` | 18% |

---

# 11. Example Item Record

```text
S;2;3616303440527;Adidas deo ultra prot.;3.900000;1.000000;Cope;E;0.000000;0.400000;3.070000;3.070000;0.468305;TT
```

Meaning:

```text
OrderId            = 2
Barcode            = 3616303440527
ArticleForSale     = Adidas deo ultra prot.
OriginalPrice      = 3.900000
Amount             = 1.000000
Mass               = Cope
Vat                = E
DiscountPercentage = 0.000000
DiscountEUR        = 0.400000
FinalPrice         = 3.070000
FinalTotal         = 3.070000
VatAmount          = 0.468305
ArticleType        = TT
```

---

# 12. Totals Record — `T`

The `T` record contains the final document totals.

## 12.1 Format

```text
T;CountOrders;Total;TotalNoVat;DiscountOnTotal;TotalWithVat0;TotalWithVat8;OnlyVat8;TotalWithVat18;OnlyVat18;TypeOfPayment;DiscountPercentage;DiscountE
```

## 12.2 Fields

| # | Field | Meaning |
|---:|---|---|
| 1 | `T` | Identifies the line as the totals record |
| 2 | CountOrders | Number of `S` item records |
| 3 | Total | Final document total including VAT |
| 4 | TotalNoVat | Final document total excluding VAT |
| 5 | DiscountOnTotal | Total discount amount applied to the document |
| 6 | TotalWithVat0 | Taxable/base amount for VAT code `C` |
| 7 | TotalWithVat8 | Taxable/base amount for VAT code `D` |
| 8 | OnlyVat8 | VAT amount for VAT code `D` / 8% |
| 9 | TotalWithVat18 | Taxable/base amount for VAT code `E` |
| 10 | OnlyVat18 | VAT amount for VAT code `E` / 18% |
| 11 | TypeOfPayment | Payment method/type identifier |
| 12 | DiscountPercentage | Total-level percentage discount |
| 13 | DiscountE | Total-level discount amount in currency |

---

# 13. Important Meaning of VAT Total Fields

The historical field names:

```text
TotalWithVat8
TotalWithVat18
```

represent the taxable/base amount, not the gross amount including VAT.

For example:

```text
Total          = 16.042000
TotalNoVat     = 13.595000
TotalWithVat18 = 13.595000
OnlyVat18      = 2.447000
```

and:

```text
13.595000 + 2.447000 = 16.042000
```

---

# 14. VAT Group Calculation

VAT must be calculated from the final gross totals grouped by VAT category.

## VAT C — 0%

```text
GrossC   = SUM(FinalTotal where Vat = C)
TaxableC = GrossC
VatC     = 0.000000
```

## VAT D — 8%

```text
GrossD = SUM(FinalTotal where Vat = D)

TaxableD = Round(GrossD / 1.08, 3)
VatD     = Round(GrossD - TaxableD, 3)
```

## VAT E — 18%

```text
GrossE = SUM(FinalTotal where Vat = E)

TaxableE = Round(GrossE / 1.18, 3)
VatE     = Round(GrossE - TaxableE, 3)
```

Recommended fiscal rounding:

```csharp
Math.Round(
    value,
    3,
    MidpointRounding.AwayFromZero
);
```

Even when the fiscal value is rounded to 3 decimals, it should still be written in the CSV with 6 decimal places.

Example:

```text
13.595000
2.447000
```

---

# 15. Why Fixed 6-Decimal Precision Is Required

Using fewer decimals at item level may introduce small differences when VAT totals are calculated.

Example:

```text
Gross E = 16.042000
```

Exact taxable base:

```text
16.042000 / 1.18
= 13.594915254...
```

Rounded fiscal base:

```text
13.595000
```

VAT:

```text
16.042000 - 13.595000
= 2.447000
```

If each item VAT/base is rounded too early and those rounded values are summed, the result may become:

```text
13.596000
2.446000
```

instead of:

```text
13.595000
2.447000
```

The correct approach is:

```text
Item calculations
    ↓
keep 6-decimal precision
    ↓
sum FinalTotal by VAT category
    ↓
calculate VAT base and VAT
    ↓
round final fiscal totals
```

---

# 16. Decimal Formatting Rules

All decimal values must use this fixed format:

```text
0.000000
```

Examples:

```text
0.000000
1.000000
0.100000
3.900000
16.042000
```

Recommended C# formatting:

```csharp
decimalValue.ToString(
    "0.000000",
    CultureInfo.InvariantCulture
);
```

---

# 17. Date and Time Format

Recommended date/time format:

```text
yyyyMMddHHmmss
```

Example:

```text
20260827133225
```

Meaning:

```text
2026-08-27 13:32:25
```

The same format should be used for:

- `InvoiceDatetime`
- `ReferenceDateTime`

---

# 18. Complete Invoice Example

```text
I;0000000145;20260827133225;000000000;Bleres Qytetar;1;admin;0
S;1;8938507378303;Abo - Pasion Fruit;1.190000;1.000000;Cope;E;0.000000;0.000000;1.044000;1.044000;0.159254;TT
S;2;3616303440527;Adidas deo ultra prot.;3.900000;1.000000;Cope;E;0.000000;0.400000;3.070000;3.070000;0.468305;TT
S;3;194252156926;Adapter iphone 17pro max;12.000000;1.000000;Cope;E;25.000000;0.000000;7.894000;7.894000;1.204169;TT
S;4;42070047;Airwaves;0.600000;1.000000;Cope;E;0.000000;0.000000;0.526000;0.526000;0.080237;TT
S;5;6972573332786;adapter kerri;4.000000;1.000000;Cope;E;0.000000;0.000000;3.508000;3.508000;0.535119;TT
T;5;16.042000;13.595000;5.648000;0.000000;0.000000;0.000000;13.595000;2.447000;1;12.290000;0.000000
```

---

# 19. Complete Cancellation Example

```text
C;0000000150;20260827141025;000000000;Bleres Qytetar;1;admin;0000000145;Gabim ne fature;20260827133225
S;1;8938507378303;Abo - Pasion Fruit;1.190000;1.000000;Cope;E;0.000000;0.000000;1.044000;1.044000;0.159254;TT
T;1;1.044000;0.885000;0.146000;0.000000;0.000000;0.000000;0.885000;0.159000;1;0.000000;0.000000
```

---

# 20. Complete Return Example

```text
R;0000000151;20260827142510;000000000;Bleres Qytetar;1;admin;0000000145;Kthim artikulli;20260827133225
S;1;3616303440527;Adidas deo ultra prot.;3.900000;1.000000;Cope;E;0.000000;0.400000;3.070000;3.070000;0.468305;TT
T;1;3.070000;2.602000;0.830000;0.000000;0.000000;0.000000;2.602000;0.468000;1;0.000000;0.000000
```

---

# 21. Summary

| File | Purpose | Header | Item | Totals |
|---|---|:---:|:---:|:---:|
| `invoice.csv` | Invoice / Sale | `I` | `S` | `T` |
| `cancel.csv` | Cancellation | `C` | `S` | `T` |
| `return.csv` | Return | `R` | `S` | `T` |

---

# 22. Final Format Rule

```text
Header
    ↓
S item rows
    ↓
T totals row
```

For item values:

```text
OriginalPrice -> original commercial unit price
FinalPrice    -> final effective unit price
FinalTotal    -> final gross row total
VatAmount     -> VAT amount of the row
```

For precision:

```text
All calculation-related decimal values
    ↓
exactly 6 decimal places
    ↓
decimal separator "."
    ↓
no thousands separator
```

For VAT totals:

```text
FinalTotal grouped by VAT category
    ↓
calculate taxable base
    ↓
calculate VAT amount
    ↓
round fiscal result
    ↓
write result using 6 decimal places
```
