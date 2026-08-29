package main
import("fmt";"os";"path/filepath";"time")
func writeCSV(kind,no,content string)(string,error){d:=`C:\Fatura`;if err:=os.MkdirAll(d,0755);err!=nil{return "",err};ts:=time.Now().Format("20060102150405")+fmt.Sprintf("%03d",time.Now().Nanosecond()/1e6);f:=filepath.Join(d,fmt.Sprintf("%s_%s_%s.csv",kind,no,ts));t:=f+".tmp";if err:=os.WriteFile(t,[]byte(content),0644);err!=nil{return "",err};if err:=os.Rename(t,f);err!=nil{return "",err};return f,nil}
