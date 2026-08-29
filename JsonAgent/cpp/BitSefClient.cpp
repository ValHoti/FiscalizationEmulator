// Requires libcurl + OpenSSL.
#include "BitSefClient.h"
#include <curl/curl.h>
#include <openssl/hmac.h>
#include <openssl/sha.h>
#include <chrono>
#include <iomanip>
#include <random>
#include <sstream>
#include <stdexcept>
static std::string hex(const unsigned char* p,size_t n){std::ostringstream s;for(size_t i=0;i<n;i++)s<<std::hex<<std::setw(2)<<std::setfill('0')<<(int)p[i];return s.str();}
static std::string sha256(const std::string&s){unsigned char o[SHA256_DIGEST_LENGTH];SHA256((const unsigned char*)s.data(),s.size(),o);return hex(o,sizeof(o));}
static std::string nonce(){std::random_device r;std::ostringstream s;for(int i=0;i<16;i++)s<<std::hex<<std::setw(2)<<std::setfill('0')<<(r()&255);return s.str();}
static size_t wr(char*p,size_t a,size_t b,void*u){((std::string*)u)->append(p,a*b);return a*b;}
BitSefClient::BitSefClient(std::string b,std::string s):base_(std::move(b)),secret_(std::move(s)){}
std::string BitSefClient::post(const std::string&path,const std::string&body,const std::string&ct){auto ts=std::to_string(std::chrono::duration_cast<std::chrono::seconds>(std::chrono::system_clock::now().time_since_epoch()).count());auto n=nonce();auto can="POST\n"+path+"\n"+ts+"\n"+n+"\n"+sha256(body);unsigned int len=0;unsigned char out[EVP_MAX_MD_SIZE];HMAC(EVP_sha256(),secret_.data(),(int)secret_.size(),(const unsigned char*)can.data(),can.size(),out,&len);auto sig=hex(out,len);CURL*c=curl_easy_init();if(!c)throw std::runtime_error("curl init");std::string result;curl_slist*h=nullptr;h=curl_slist_append(h,("X-BIT-Timestamp: "+ts).c_str());h=curl_slist_append(h,("X-BIT-Nonce: "+n).c_str());h=curl_slist_append(h,("X-BIT-Signature: "+sig).c_str());if(!ct.empty())h=curl_slist_append(h,("Content-Type: "+ct).c_str());curl_easy_setopt(c,CURLOPT_URL,(base_+path).c_str());curl_easy_setopt(c,CURLOPT_HTTPHEADER,h);curl_easy_setopt(c,CURLOPT_POST,1L);curl_easy_setopt(c,CURLOPT_POSTFIELDS,body.c_str());curl_easy_setopt(c,CURLOPT_POSTFIELDSIZE,(long)body.size());curl_easy_setopt(c,CURLOPT_WRITEFUNCTION,wr);curl_easy_setopt(c,CURLOPT_WRITEDATA,&result);auto rc=curl_easy_perform(c);long code=0;curl_easy_getinfo(c,CURLINFO_RESPONSE_CODE,&code);curl_slist_free_all(h);curl_easy_cleanup(c);if(rc!=CURLE_OK||code<200||code>=300)throw std::runtime_error("BIT-SEF HTTP error: "+result);return result;}
std::string BitSefClient::fiscal(const std::string&j){return post("/api/bitsef/fiscal",j,"application/json");}
std::string BitSefClient::command(const std::string&t,const std::string&n){return post("/api/bitsef/command","{\"type\":\""+t+"\",\"invoiceNo\":\""+n+"\"}","application/json");}
