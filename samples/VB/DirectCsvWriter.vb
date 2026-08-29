Imports System.IO
Module DirectCsvWriter
 Function W(t As String,n As String,c As String) As String
 Dim d="C:\Fatura\":Directory.CreateDirectory(d):Dim f=Path.Combine(d,t & "_" & n & "_" & Now.ToString("yyyyMMddHHmmssfff") & ".csv"):Dim x=f & ".tmp":File.WriteAllText(x,c):File.Move(x,f):Return f
 End Function
End Module