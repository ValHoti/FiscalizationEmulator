mod writer; fn main()->std::io::Result<()>{println!("{}",writer::write_csv("Copy","0000000187","copy;0000000187")?.display());Ok(())}
