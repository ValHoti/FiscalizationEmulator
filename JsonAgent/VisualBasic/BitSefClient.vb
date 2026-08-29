Imports System
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Public Class BitSefClient
 Implements IDisposable
 Private ReadOnly _baseUrl As String, _secret As String
 Private ReadOnly _http As New HttpClient()
 Public Sub New(baseUrl As String, sharedSecret As String) : _baseUrl=baseUrl.TrimEnd("/"c) : _secret=sharedSecret : End Sub
 Public Function FiscalJsonAsync(json As String) As Task(Of String) : Return PostAsync("/api/bitsef/fiscal",json,"application/json") : End Function
 Public Function CommandJsonAsync(json As String) As Task(Of String) : Return PostAsync("/api/bitsef/command",json,"application/json") : End Function
 Public Function CopyAsync(no As String) As Task(Of String) : Return PostAsync("/api/bitsef/copy/" & no,"",Nothing) : End Function
 Public Function PdfAsync(no As String) As Task(Of String) : Return PostAsync("/api/bitsef/pdf/" & no,"",Nothing) : End Function
 Public Function DirectCsvAsync(type As String,no As String,csv As String) As Task(Of String) : Return PostAsync("/api/bitsef/csv/" & type & "/" & no,csv,"text/plain") : End Function
 Public Async Function StatusAsync(id As String) As Task(Of String) : Dim r=Await _http.GetAsync(_baseUrl & "/api/bitsef/status/" & id) : Dim t=Await r.Content.ReadAsStringAsync() : r.EnsureSuccessStatusCode() : Return t : End Function
 Private Async Function PostAsync(path As String,body As String,mediaType As String) As Task(Of String)
  Dim ts=DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), nonce=Guid.NewGuid().ToString("N")
  Dim canonical="POST" & vbLf & path & vbLf & ts & vbLf & nonce & vbLf & Sha256Hex(If(body,""))
  Using req As New HttpRequestMessage(HttpMethod.Post,_baseUrl & path)
   req.Headers.Add("X-BIT-Timestamp",ts) : req.Headers.Add("X-BIT-Nonce",nonce) : req.Headers.Add("X-BIT-Signature",HmacHex(canonical,_secret))
   If mediaType IsNot Nothing Then req.Content=New StringContent(If(body,""),Encoding.UTF8,mediaType)
   Using res=Await _http.SendAsync(req) : Dim text=Await res.Content.ReadAsStringAsync() : If Not res.IsSuccessStatusCode Then Throw New Exception("BIT-SEF HTTP " & CInt(res.StatusCode) & ": " & text) : Return text : End Using
  End Using
 End Function
 Private Shared Function Sha256Hex(s As String) As String : Using h=SHA256.Create() : Return Hex(h.ComputeHash(Encoding.UTF8.GetBytes(s))) : End Using : End Function
 Private Shared Function HmacHex(s As String,k As String) As String : Using h=New HMACSHA256(Encoding.UTF8.GetBytes(k)) : Return Hex(h.ComputeHash(Encoding.UTF8.GetBytes(s))) : End Using : End Function
 Private Shared Function Hex(b As Byte()) As String : Dim sb As New StringBuilder() : For Each x In b : sb.Append(x.ToString("x2")) : Next : Return sb.ToString() : End Function
 Public Sub Dispose() Implements IDisposable.Dispose : _http.Dispose() : End Sub
End Class
