#include "DirectCsvWriter.h"
#include <filesystem>
#include <fstream>
#include <chrono>
#include <iomanip>
#include <sstream>
std::string WriteBitSefCsv(const std::string&k,const std::string&n,const std::string&c){namespace fs=std::filesystem;fs::path d=R"(C:\Fatura)";fs::create_directories(d);auto now=std::chrono::system_clock::now();auto tt=std::chrono::system_clock::to_time_t(now);std::tm tm{};
#ifdef _WIN32
localtime_s(&tm,&tt);
#else
localtime_r(&tt,&tm);
#endif
auto ms=std::chrono::duration_cast<std::chrono::milliseconds>(now.time_since_epoch())%1000;std::ostringstream s;s<<std::put_time(&tm,"%Y%m%d%H%M%S")<<std::setw(3)<<std::setfill('0')<<ms.count();fs::path f=d/(k+"_"+n+"_"+s.str()+".csv"),t=f;t+=".tmp";{std::ofstream o(t,std::ios::binary);o<<c;}fs::rename(t,f);return f.string();}
