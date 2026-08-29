Imports System
Imports System.Threading.Tasks
Module Example_Status
 Async Function Main() As Task
  Using c As New BitSefClient("http://127.0.0.1:5077","CHANGE-THIS-BIT-SEF-SECRET")
   Console.WriteLine(Await c.StatusAsync("REQUEST_ID_FROM_POST_RESPONSE"))
  End Using
 End Function
End Module
