#pragma once
#include <string>
class BitSefClient { public: BitSefClient(std::string base="http://127.0.0.1:5077",std::string secret="CHANGE-THIS-BIT-SEF-SECRET"); std::string fiscal(const std::string& json); std::string command(const std::string& type,const std::string& invoiceNo); private: std::string post(const std::string&,const std::string&,const std::string&); std::string base_,secret_; };
