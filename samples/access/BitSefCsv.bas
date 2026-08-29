Option Explicit
Function W(t As String,n As String,c As String) As String
Dim fso As Object,f As Object,d As String,p As String,x As String
d="C:\Fatura\":Set fso=CreateObject("Scripting.FileSystemObject"):If Not fso.FolderExists(d) Then fso.CreateFolder d
p=d & t & "_" & n & "_" & Format(Now,"yyyymmddhhnnss") & "000.csv":x=p & ".tmp":Set f=fso.CreateTextFile(x,True,False):f.Write c:f.Close:fso.MoveFile x,p:W=p
End Function
