package main

import (
 "bytes"
 "crypto/hmac"
 "crypto/rand"
 "crypto/sha256"
 "encoding/hex"
 "fmt"
 "io"
 "net/http"
 "time"
)

const baseURL = "http://127.0.0.1:5077"
const secret = "CHANGE-THIS-BIT-SEF-SECRET"

func nonce() string { b:=make([]byte,16); _,_=rand.Read(b); return hex.EncodeToString(b) }
func post(path string, body []byte, contentType string) (string,error) {
 ts:=fmt.Sprintf("%d", time.Now().Unix()); n:=nonce(); bh:=sha256.Sum256(body)
 canonical:=fmt.Sprintf("POST\\n%s\\n%s\\n%s\\n%s",path,ts,n,hex.EncodeToString(bh[:]))
 mac:=hmac.New(sha256.New,[]byte(secret)); mac.Write([]byte(canonical)); sig:=hex.EncodeToString(mac.Sum(nil))
 req,_:=http.NewRequest("POST",baseURL+path,bytes.NewReader(body)); if contentType!="" { req.Header.Set("Content-Type",contentType) }
 req.Header.Set("X-BIT-Timestamp",ts); req.Header.Set("X-BIT-Nonce",n); req.Header.Set("X-BIT-Signature",sig)
 r,err:=http.DefaultClient.Do(req); if err!=nil{return "",err}; defer r.Body.Close(); out,_:=io.ReadAll(r.Body)
 if r.StatusCode<200||r.StatusCode>=300{return "",fmt.Errorf("HTTP %d: %s",r.StatusCode,string(out))}; return string(out),nil
}
func fiscal(json string)(string,error){return post("/api/bitsef/fiscal",[]byte(json),"application/json")}
func command(t,n string)(string,error){j:=fmt.Sprintf(`{"type":"%s","invoiceNo":"%s"}`,t,n);return post("/api/bitsef/command",[]byte(j),"application/json")}
