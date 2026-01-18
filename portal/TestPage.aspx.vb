Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Partial Class Portal_TestPage
    Inherits System.Web.UI.Page

    Private Function GetApiToken(username As String, password As String) As String
        Dim loginUrl As String = "https://login.myadcampaigns.com/advertiser/auth?login=" & username & "&password=" & password
        Dim request As HttpWebRequest = CType(WebRequest.Create(loginUrl), HttpWebRequest)
        request.Method = "GET"
        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Using reader As New StreamReader(response.GetResponseStream())
                Dim json As String = reader.ReadToEnd()
                Dim serializer As New JavaScriptSerializer()
                Try
                    ' Try to parse as JSON object
                    Dim authResponse = serializer.DeserializeObject(json)
                    If TypeOf authResponse Is System.Collections.Generic.Dictionary(Of String, Object) AndAlso DirectCast(authResponse, System.Collections.Generic.Dictionary(Of String, Object)).ContainsKey("token") Then
                        Return authResponse("token").ToString()
                    End If
                Catch
                    ' If parsing fails, treat as plain string token
                    If Not String.IsNullOrEmpty(json) Then
                        Return json.Trim(""""c) ' Remove quotes if present
                    End If
                End Try
                Throw New Exception("Token not found in authentication response.")
            End Using
        End Using
    End Function

    Protected Sub btnGetCampaigns_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetCampaigns.Click
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) ' TLS 1.2
        Dim username As String = "goddardent"
        Dim password As String = "Porsche2023!1"
        Try
            Dim userToken As String = GetApiToken(username, password)
            Dim url As String = "https://login.myadcampaigns.com/advertiser/api/Campaign/?userToken=" & userToken
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim json As String = reader.ReadToEnd()
                    lblResult.Text = Server.HtmlEncode(json)
                End Using
            End Using
        Catch ex As Exception
            lblResult.Text = "Error: " & Server.HtmlEncode(ex.Message)
        End Try
    End Sub
End Class 