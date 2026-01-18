Imports System.Data.SqlClient
Imports System.Data
Imports System.Collections.Generic

Partial Class CProcessPage
    Inherits System.Web.UI.Page

    Public Link As String
    Public ShowTable As Boolean = False
    Private fullOrigionalpath As String
    Private WebPath As String = ConfigurationSettings.AppSettings("WebPath")
    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private CampaignId As Integer
    Private LinkId As Integer
    Private Table_Name As String
    Private CouponCode As String
    Private CouponGuid As String
    Private CouponProvided As Boolean = False
    Private CouponLink As String
    Private strURL As String
    Private arURL As New ArrayList()    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strURL = HttpContext.Current.Request.Url.AbsoluteUri       
        If Len(Trim(Request.QueryString.ToString)) > 0 Then
            fullOrigionalpath = Request.QueryString("c")            
            SetCookie(fullOrigionalpath)
            fullOrigionalpath = Server.UrlDecode(fullOrigionalpath)
            'Declare the array and fill with string elements       
            Dim arrfullOrigionalpath() As String = New String() {}
            arrfullOrigionalpath = fullOrigionalpath.Split("~")
            CampaignId = CInt(arrfullOrigionalpath(0))
            LinkId = CInt(arrfullOrigionalpath(1))
            If arrfullOrigionalpath.Length = 2 Then
                Table_Name = "Coupon_" & arrfullOrigionalpath(2)
            End If
            If arrfullOrigionalpath.Length = 3 Then
                CouponCode = arrfullOrigionalpath(2)
                CouponProvided = True
            End If
            ProcessClick(CampaignId, LinkId)
        Else
            Link = WebPath & "InactiveCampaign.aspx"
            'Response.Redirect(Link)
        End If
    End Sub

    Private Sub ProcessClick(ByVal CampaignId As Integer, ByVal LinkId As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdProcessClick As New SqlCommand("campaign_getlink", cnn)
                cmdProcessClick.CommandType = Data.CommandType.StoredProcedure
                cmdProcessClick.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdProcessClick.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = LinkId

                If Not IsDBNull(cmdProcessClick.ExecuteScalar) Then
                    Link = cmdProcessClick.ExecuteScalar
                    Link = Replace(Link, "{CPLink}", CouponCode, 1, -1, CompareMethod.Text)

                    GetUrlParameters()
                    Dim counter As Integer = 0
                    Dim kvl As KeyValuePair(Of String, String)
                    For Each kvl In arURL
                        If counter > 0 Then
                            Link = Link & "&" & kvl.Key & "=" & kvl.Value
                        End If
                        counter += 1
                    Next                    
                Else
                    Link = WebPath & "InactiveCampaign.aspx"
                    'Response.Redirect(Link)
                    Exit Sub
                End If

                If Len(Link) = 0 Or Link = String.Empty Then
                    Link = WebPath & "InactiveCampaign.aspx"
                    'Response.Redirect(Link)
                    Exit Sub
                End If

            End Using
        End Using
    End Sub

    Private Sub SetCookie(ByVal campaign As String)

        Dim objCookie As HttpCookie
        Try
            objCookie = New HttpCookie("Cookie", campaign)
            objCookie.Expires = DateTime.Now.AddMinutes(30)
            Response.Cookies.Add(objCookie)
        Catch ex As Exception
            Link = WebPath & "InactiveCampaign.aspx"
            'Response.Redirect(Link)
        End Try

    End Sub

    Private Sub GetCouponId()


        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCouponId As New SqlCommand("Coupon_GetCouponCode", cnn)
                cmdGetCouponId.CommandType = CommandType.StoredProcedure
                cmdGetCouponId.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = Table_Name
                Using dtrGetCouponId As SqlDataReader = cmdGetCouponId.ExecuteReader
                    While dtrGetCouponId.Read
                        CouponCode = dtrGetCouponId("CouponCode")
                        CouponGuid = dtrGetCouponId("CouponGuid").ToString
                    End While
                End Using
            End Using
        End Using
        UpdateCoupon()

    End Sub

    Private Sub UpdateCoupon()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateCoupon As New SqlCommand("Coupon_MarkAsUsed", cnn)
                cmdUpdateCoupon.CommandType = CommandType.StoredProcedure
                cmdUpdateCoupon.Parameters.Add(New SqlParameter("@Table_Name", SqlDbType.NVarChar)).Value = Table_Name
                cmdUpdateCoupon.Parameters.Add(New SqlParameter("@CouponGuid", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(CouponGuid)                
                cmdUpdateCoupon.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub GetUrlParameters()
        Dim url As String = strURL
        ' consider only the querystring, that's the part after the ? char
        Dim qsStart As Integer = url.IndexOf("?")
        If qsStart > -1 Then url = url.Substring(qsStart + 1)
        ' split the querystring with the & char
        Dim params() As String = url.Split(New Char() {"&"c})
        Dim param As String
        ' for each param extract the param name (the part before the =) and the 
        ' value
        'Dim arlist As New ArrayList()
        For Each param In params
            Dim i As Integer = param.IndexOf("="c)
            If i > -1 Then
                Dim kv As New KeyValuePair(Of String, String)(param.Substring(0, i), param.Substring(i + 1))
                arURL.Add(kv)
            End If
        Next
    End Sub

End Class
