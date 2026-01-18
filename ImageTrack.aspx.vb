Imports System.Data
Imports System.Data.SqlClient

Partial Class ImageTrack
    Inherits System.Web.UI.Page

    Private Shared _imgbytes As Byte() = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private Ip As String = Context.Request.ServerVariables.Item("REMOTE_ADDR")
    Private CampaignId As Integer

    Protected Overloads Overrides Sub OnInit(ByVal e As EventArgs)
        AddHandler Me.Load, AddressOf Me.Page_Load
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)        
        CampaignId = Request.QueryString("c")
        InsertView()
        Response.ContentType = "image/gif"
        Response.AppendHeader("Content-Length", _imgbytes.Length.ToString())
        Response.Cache.SetLastModified(DateTime.Now)
        Response.Cache.SetCacheability(HttpCacheability.[Public])
        Response.BinaryWrite(_imgbytes)       
    End Sub

    Private Sub InsertView()

        Dim intMonth As Integer
        Dim spToUse As String
        intMonth = DatePart(DateInterval.Month, Date.Now)
        spToUse = "Impression_ProcessView_" & intMonth

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertView As New SqlCommand(spToUse, cnn)
                cmdInsertView.CommandType = CommandType.StoredProcedure
                cmdInsertView.Parameters.Add(New SqlParameter("@ImpressionCampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertView.Parameters.Add(New SqlParameter("@Ip", Data.SqlDbType.NVarChar, 15)).Value = Ip
                cmdInsertView.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Function useCached(ByVal req As HttpRequest) As Boolean
        Dim ifmod As String = req.Headers("If-Modified-Since")
        Return IIf(ifmod Is Nothing, False, DateTime.Parse(ifmod).AddHours(24) >= DateTime.Now)
    End Function
End Class
