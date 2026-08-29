<?php
class BitSefClient {
 private string $baseUrl; private string $secret;
 function __construct(string $u,string $s){$this->baseUrl=rtrim($u,'/');$this->secret=$s;}
 function fiscalJson(string $j):string{return $this->post('/api/bitsef/fiscal',$j,'application/json');}
 function commandJson(string $j):string{return $this->post('/api/bitsef/command',$j,'application/json');}
 function copy(string $n):string{return $this->post('/api/bitsef/copy/'.$n,'',null);}
 function pdf(string $n):string{return $this->post('/api/bitsef/pdf/'.$n,'',null);}
 function directCsv(string $t,string $n,string $c):string{return $this->post('/api/bitsef/csv/'.$t.'/'.$n,$c,'text/plain; charset=utf-8');}
 function status(string $id):string{return file_get_contents($this->baseUrl.'/api/bitsef/status/'.$id);}
 private function post(string $p,string $b,?string $ct):string{$ts=(string)time();$nonce=bin2hex(random_bytes(16));$canonical="POST\n$p\n$ts\n$nonce\n".hash('sha256',$b);$sig=hash_hmac('sha256',$canonical,$this->secret);$h=["X-BIT-Timestamp: $ts","X-BIT-Nonce: $nonce","X-BIT-Signature: $sig"];if($ct)$h[]="Content-Type: $ct";$ch=curl_init($this->baseUrl.$p);curl_setopt_array($ch,[CURLOPT_POST=>true,CURLOPT_RETURNTRANSFER=>true,CURLOPT_HTTPHEADER=>$h]);if($ct)curl_setopt($ch,CURLOPT_POSTFIELDS,$b);$r=curl_exec($ch);$code=curl_getinfo($ch,CURLINFO_HTTP_CODE);if($r===false)throw new Exception(curl_error($ch));curl_close($ch);if($code<200||$code>=300)throw new Exception("BIT-SEF HTTP $code: $r");return $r;}
}
