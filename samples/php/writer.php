<?php
function w($t,$n,$c){$d='C:\\Fatura\\';if(!is_dir($d))mkdir($d,0777,true);$s=date('YmdHis').sprintf('%03d',(int)((microtime(true)*1000)%1000));$f=$d.$t.'_'.$n.'_'.$s.'.csv';$x=$f.'.tmp';file_put_contents($x,$c,LOCK_EX);rename($x,$f);return $f;}
