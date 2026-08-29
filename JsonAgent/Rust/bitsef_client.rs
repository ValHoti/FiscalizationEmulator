use hmac::{Hmac,Mac}; use sha2::{Digest,Sha256}; use uuid::Uuid;
const BASE:&str="http://127.0.0.1:5077"; const SECRET:&str="CHANGE-THIS-BIT-SEF-SECRET";
pub fn post(path:&str, body:&str, ct:Option<&str>)->Result<String,Box<dyn std::error::Error>>{
 let ts=std::time::SystemTime::now().duration_since(std::time::UNIX_EPOCH)?.as_secs().to_string(); let nonce=Uuid::new_v4().simple().to_string();
 let bh=hex::encode(Sha256::digest(body.as_bytes())); let canonical=format!("POST\n{}\n{}\n{}\n{}",path,ts,nonce,bh);
 let mut mac=Hmac::<Sha256>::new_from_slice(SECRET.as_bytes())?; mac.update(canonical.as_bytes()); let sig=hex::encode(mac.finalize().into_bytes());
 let c=reqwest::blocking::Client::new(); let mut r=c.post(format!("{}{}",BASE,path)).header("X-BIT-Timestamp",ts).header("X-BIT-Nonce",nonce).header("X-BIT-Signature",sig);
 if let Some(x)=ct{r=r.header("Content-Type",x);} let res=r.body(body.to_owned()).send()?; let status=res.status(); let text=res.text()?; if !status.is_success(){return Err(format!("HTTP {}: {}",status,text).into())} Ok(text)
}
pub fn fiscal(j:&str)->Result<String,Box<dyn std::error::Error>>{post("/api/bitsef/fiscal",j,Some("application/json"))}
pub fn command(t:&str,n:&str)->Result<String,Box<dyn std::error::Error>>{let j=format!(r#"{{"type":"{}","invoiceNo":"{}"}}"#,t,n);post("/api/bitsef/command",&j,Some("application/json"))}
