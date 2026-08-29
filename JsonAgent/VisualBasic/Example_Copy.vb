Imports System
Imports System.Threading.Tasks
Module Example_Copy
 Async Function Main() As Task
  Using c As New BitSefClient("http://127.0.0.1:5077","CHANGE-THIS-BIT-SEF-SECRET")
   Dim json As String = "{""type"":""Copy"",""invoiceNo"":""0000000187""}"
   Console.WriteLine(Await c.CommandJsonAsync(json))
  End Using
 End Function
End Module
