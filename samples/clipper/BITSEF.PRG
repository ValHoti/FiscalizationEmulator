FUNCTION W(t,n,c)
LOCAL d:="C:\Fatura\",p,x,h
IF !ISDIRECTORY(d);MAKEDIR(d);ENDIF
p:=d+t+"_"+n+"_"+DTOS(DATE())+STRTRAN(TIME(),":","")+"000.csv";x:=p+".tmp";h:=FCREATE(x);FWRITE(h,c);FCLOSE(h);FRENAME(x,p)
RETURN p