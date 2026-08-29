mod bitsef_client; fn main()->Result<(),Box<dyn std::error::Error>>{println!("{}",bitsef_client::command("Copy","0000000187")?);Ok(())}
