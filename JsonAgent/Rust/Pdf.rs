mod bitsef_client; fn main()->Result<(),Box<dyn std::error::Error>>{println!("{}",bitsef_client::command("Pdf","0000000187")?);Ok(())}
