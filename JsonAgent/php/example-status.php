<?php require __DIR__.'/../BitSefClient.php'; $c=new BitSefClient('http://127.0.0.1:5077','CHANGE-THIS-BIT-SEF-SECRET'); echo $c->status('REQUEST_ID_FROM_POST_RESPONSE');
