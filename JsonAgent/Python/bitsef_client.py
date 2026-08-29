import hashlib, hmac, json, time, uuid, urllib.request, urllib.error

class BitSefClient:
    def __init__(self, base_url="http://127.0.0.1:5077", secret="CHANGE-THIS-BIT-SEF-SECRET"):
        self.base_url = base_url.rstrip("/")
        self.secret = secret.encode("utf-8")

    def _post(self, path, body=b"", content_type=None):
        ts = str(int(time.time()))
        nonce = uuid.uuid4().hex
        body_hash = hashlib.sha256(body).hexdigest()
        canonical = f"POST\\n{path}\\n{ts}\\n{nonce}\\n{body_hash}".encode("utf-8")
        signature = hmac.new(self.secret, canonical, hashlib.sha256).hexdigest()
        headers = {"X-BIT-Timestamp": ts, "X-BIT-Nonce": nonce, "X-BIT-Signature": signature}
        if content_type: headers["Content-Type"] = content_type
        req = urllib.request.Request(self.base_url + path, data=body, headers=headers, method="POST")
        with urllib.request.urlopen(req) as r:
            return r.read().decode("utf-8")

    def fiscal(self, payload):
        body = json.dumps(payload, separators=(",", ":"), ensure_ascii=False).encode("utf-8")
        return self._post("/api/bitsef/fiscal", body, "application/json")

    def command(self, command_type, invoice_no):
        payload = {"type": command_type, "invoiceNo": invoice_no}
        body = json.dumps(payload, separators=(",", ":")).encode("utf-8")
        return self._post("/api/bitsef/command", body, "application/json")
