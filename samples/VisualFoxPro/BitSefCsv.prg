FUNCTION W(t,n,c)
d="C:\Fatura\"
IF !DIRECTORY(d)
MD (d)
ENDIF
p=d+t+"_"+n+"_"+STRTRAN(STRTRAN(STRTRAN(TTOC(DATETIME(),1),"-",""),":","")," ","")+"000.csv"
x=p+".tmp"
STRTOFILE(c,x,0)
RENAME (x) TO (p)
RETURN p
ENDFUNC