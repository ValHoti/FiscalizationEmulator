const C=require('../bitsef-client'); const c=new C('http://127.0.0.1:5077','CHANGE-THIS-BIT-SEF-SECRET'); c.status('REQUEST_ID_FROM_POST_RESPONSE').then(console.log).catch(console.error);
