mod writer; fn main()->std::io::Result<()>{println!("{}",writer::write_csv("Pdf","0000000187","pdf;0000000187")?.display());Ok(())}
