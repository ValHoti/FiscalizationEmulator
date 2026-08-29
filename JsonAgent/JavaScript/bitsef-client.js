const crypto=require('crypto');
class BitSefClient{
 constructor(baseUrl,secret){this.baseUrl=baseUrl.replace(/\/$/,'');this.secret=secret;}
 sha(s){return crypto.createHash('sha256').update(s,'utf8').digest('hex');}
 hmac(s){return crypto.createHmac('sha256',this.secret).update(s,'utf8').digest('hex');}
 async post(path,body='',contentType=null){const ts=Math.floor(Date.now()/1000).toString(),nonce=crypto.randomUUID().replace(/-/g,'');const canonical=['POST',path,ts,nonce,this.sha(body)].join('\n');const headers={'X-BIT-Timestamp':ts,'X-BIT-Nonce':nonce,'X-BIT-Signature':this.hmac(canonical)};if(contentType)headers['Content-Type']=contentType;const r=await fetch(this.baseUrl+path,{method:'POST',headers,body:contentType?body:undefined});const t=await r.text();if(!r.ok)throw new Error(`BIT-SEF HTTP ${r.status}: ${t}`);return t;}
 fiscalJson(j){return this.post('/api/bitsef/fiscal',j,'application/json');} commandJson(j){return this.post('/api/bitsef/command',j,'application/json');} copy(n){return this.post('/api/bitsef/copy/'+n);} pdf(n){return this.post('/api/bitsef/pdf/'+n);} directCsv(t,n,c){return this.post(`/api/bitsef/csv/${t}/${n}`,c,'text/plain; charset=utf-8');} async status(id){return await (await fetch(this.baseUrl+'/api/bitsef/status/'+id)).text();}
}
module.exports=BitSefClient;
