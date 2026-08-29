from pathlib import Path
from datetime import datetime

FOLDER = Path(r"C:\Fatura")

def write_csv(kind, invoice_no, content):
    FOLDER.mkdir(parents=True, exist_ok=True)
    timestamp = datetime.now().strftime("%Y%m%d%H%M%S%f")[:17]
    final_path = FOLDER / f"{kind}_{invoice_no}_{timestamp}.csv"
    tmp_path = Path(str(final_path) + ".tmp")
    tmp_path.write_text(content, encoding="utf-8", newline="")
    tmp_path.replace(final_path)
    return final_path
