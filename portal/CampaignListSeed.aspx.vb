Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO

Partial Class CampiagnListSeed
    Inherits Page

    Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private _webPath As String
    Private _footer As String
    Private _seedSent As Boolean = True
    Private _cName As String
    Private _cAddr As String
    Private _cAddr2 As String
    Private _cCity As String
    Private _cState As String
    Private _cZip As String
    Public SeedType As String
    'SessionVariables To PreLoad
    Private _campaignId As Integer
    Private _status As Integer
    Private _couponVariable As String = String.Empty
    Private _fName As String
    Private _lName As String
    Private _email As String
    'Mail Sending Variables        
    Private _mailServer As String = "127.0.0.1"
    '"Smtpout.secureserver.net"
    Private _fromAddress As String = "testing@goddardent.com"
    '"tom@leadmemarketing.com"
    Private _mailUser As String = "trackingstats@relay.goddardent.com"
    Private _mailPass As String = "trackingpass!1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        trmsg.Visible = False
        txtMsg.Text = String.Empty
        txtMsg.Font.Bold = False
        txtMsg.ForeColor = Drawing.Color.Black
        CheckSessionVariable()
        'GetFrom()
        If Not IsPostBack Then
            radUnConverted.Checked = True
            txtFrom.Text = _fromAddress
            ddColor.SelectedValue = 0
            ddsize.SelectedValue = 0
            ddAlign.SelectedValue = 0
            GetCampaign()
        End If
    End Sub

    Protected Sub radUnConverted_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radUnConverted.CheckedChanged
        GetCampaign()
    End Sub

    Protected Sub radConverted_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radConverted.CheckedChanged
        GetCampaign()
    End Sub

    Private Sub CheckSessionVariable()
        If Session("CampaignSeed") IsNot Nothing Then
            Dim arrseed() As String
            arrseed = Session("CampaignSeed").Split("~")
            _campaignId = arrseed(0)
            _status = arrseed(1)
            If arrseed.Length = 3 Then
                _couponVariable = arrseed(2)
            End If
            If _couponVariable <> String.Empty Then
                SeedType = "Coupon" 
            Else
                SeedType = "Reg"
            End If
        End If

        If Session("AcceptedUser") IsNot Nothing Then
            Dim arruser() As String = New String() {}
            arruser = Session("AcceptedUser").Split("~")
            _fName = arruser(0)
            _lName = arruser(1)
            _email = arruser(3)
            'FromAddress = Email
        End If

    End Sub


    Private Sub GetCampaign()
        Using cnn As New SqlConnection(_strConn)
            cnn.Open()
            Using cmdGetCampaign As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetCampaign.CommandType = CommandType.StoredProcedure
                cmdGetCampaign.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = _campaignId
                Using dtrGetCampaign As SqlDataReader = cmdGetCampaign.ExecuteReader
                    While dtrGetCampaign.Read
                        If Not IsDBNull(dtrGetCampaign("CouponVariable")) Then
                            txtCouponVariable.Text = dtrGetCampaign("CouponVariable")
                        Else
                            txtCouponVariable.Text = String.Empty
                        End If

                        If radUnConverted.Checked = True Then
                            txtCreative.Text = dtrGetCampaign("CampaignCreative")
                        Else
                            txtCreative.Text = (dtrGetCampaign("ConvertedCreative"))
                        End If

                        If Not IsDBNull(dtrGetCampaign("SubjectLine")) Then
                            txtSubject.Text = dtrGetCampaign("SubjectLine")
                        Else
                            txtSubject.Text = String.Empty
                        End If

                    End While
                End Using
            End Using
        End Using
    End Sub

    Protected Sub cmdSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendMail.Click

        If SeedType = "Coupon" Then
            If Len(Trim(txtCouponValue.Text)) = 0 Then
                trmsg.Visible = True
                txtMsg.Text = "* You must enter a coupon value."
                txtMsg.Font.Bold = True
                txtMsg.ForeColor = Drawing.Color.Red
                Exit Sub
            End If
        End If

        Page.Validate("frmSeed")
        If Page.IsValid Then
            GetCoInfo()
            FormatFooter()
            Dim arrEmail() As String = New String() {}
            arrEmail = txtTo.Text.Split(vbCrLf)
            For i As Integer = 0 To arrEmail.GetUpperBound(0)
                arrEmail(i) = Replace(arrEmail(i), Chr(10), vbNullString)
                arrEmail(i) = Replace(arrEmail(i), Chr(13), vbNullString)
                SendEmail(Trim(arrEmail(i)))
            Next
            UpdateStatus(_campaignId)
            If Session("CampaignSeed") IsNot Nothing Then
                Session.Remove("CampaignSeed")
            End If
        End If
    End Sub


    Private Sub SendEmail(ByVal ToAddress As String) 'SendEmail()
        Dim Var As String = Trim(txtCouponVariable.Text)
        Dim Value As String = Trim(txtCouponValue.Text)
        Dim Body As String = txtCreative.Text & _footer
        trmsg.Visible = True

        If SeedType = "Coupon" Then
            If InStr(txtCreative.Text, txtCouponVariable.Text) = 0 Then
                trmsg.Visible = True
                txtMsg.Text = "There is no coupon variable in the creative, please reenter the coupon variable.   "
                _seedSent = False
                Exit Sub
            End If
            Body = Body.Replace(txtCouponVariable.Text, txtCouponValue.Text)
        End If

        Try
            Dim smtpClient As New SmtpClient(_mailServer)
            smtpClient.UseDefaultCredentials = False
            smtpClient.Port = 80 'Test 25 Live 80
            'smtpClient.Credentials = New Net.NetworkCredential(MailUser, MailPass)
            'smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim Mail As MailMessage
            Mail = New MailMessage
            If Trim(Len(txtFriendlyTo.Text)) > 0 Or Trim(txtFriendlyTo.Text) <> String.Empty Then
                Mail.To.Add(New MailAddress(ToAddress, txtFriendlyTo.Text))
            Else
                Mail.To.Add(New MailAddress(ToAddress))
            End If
            If Trim(Len(txtFriendlyFrom.Text)) > 0 Or Trim(txtFriendlyFrom.Text) <> String.Empty Then
                Mail.From = New MailAddress(txtFrom.Text, txtFriendlyFrom.Text)
            Else
                Mail.From = New MailAddress(txtFrom.Text)
            End If
            Mail.Subject = txtSubject.Text
            Mail.Body = Body
            Mail.IsBodyHtml = True
            smtpClient.Send(Mail)
            txtMsg.Text &= "Seed Sent To: " & ToAddress & vbCrLf
            txtMsg.Font.Bold = True
            txtMsg.ForeColor = Drawing.Color.Green
        Catch ex As Exception
            txtMsg.Text &= "Seed Not Sent To: " & ToAddress & vbCrLf
            txtMsg.Font.Bold = True
            txtMsg.ForeColor = Drawing.Color.Red
        End Try


    End Sub

    Private Sub UpdateStatus(ByVal Id As Integer)
        If _status = 1 Then
            Using cnn As New SqlConnection(_strConn)
                cnn.Open()
                Using cmdUpdateStatus As New SqlCommand("CampaignAdCopy_UpdateStatus", cnn)
                    cmdUpdateStatus.CommandType = CommandType.StoredProcedure
                    cmdUpdateStatus.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int)).Value = 2
                    cmdUpdateStatus.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                    cmdUpdateStatus.ExecuteNonQuery()
                End Using
            End Using
        End If
    End Sub

    Private Sub GetCoInfo()
        Using cnn As New SqlConnection(_strConn)
            cnn.Open()
            Using cmdGetCoInfo As New SqlCommand("Company_GetInfo", cnn)
                Using dtrGetCoInfo As SqlDataReader = cmdGetCoInfo.ExecuteReader
                    While dtrGetCoInfo.Read

                        If Not IsDBNull(dtrGetCoInfo("Name")) Then
                            _cName = dtrGetCoInfo("Name")
                        Else
                            _cName = String.Empty
                        End If


                        If Not IsDBNull(dtrGetCoInfo("Name")) Then
                            _cName = dtrGetCoInfo("Name")
                        Else
                            _cName = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Addr1")) Then
                            _cAddr = dtrGetCoInfo("Addr1")
                        Else
                            _cAddr = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Addr2")) Then
                            _cAddr2 = dtrGetCoInfo("Addr2")
                        Else
                            _cAddr2 = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("City")) Then
                            _cCity = dtrGetCoInfo("City")
                        Else
                            _cCity = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("State")) Then
                            _cState = dtrGetCoInfo("State")
                        Else
                            _cState = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Zip")) Then
                            _cZip = dtrGetCoInfo("Zip")
                        Else
                            _cZip = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("WebPath")) Then
                            _webPath = Microsoft.VisualBasic.Left(dtrGetCoInfo("WebPath"), Len(dtrGetCoInfo("WebPath")) - 1)
                        Else
                            _webPath = "http://www.leadmemarketing.com"
                        End If

                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub FormatFooter()
        Dim Align As String = String.Empty
        Dim Color As String = String.Empty
        Dim size As String = String.Empty
        If ddAlign.SelectedValue = 0 Then
            Align = "Left"
        Else
            Align = "Center"
        End If

        Select Case ddsize.SelectedValue
            Case 0
                size = "8px"
            Case 1
                size = "10px"
            Case 2
                size = "12px"

        End Select

        If ddColor.SelectedValue = 0 Then
            Color = "Gray"
        Else
            Color = "Black"
        End If

        'Dim Footer As String
        Dim filename As String = Server.MapPath("Text/Footer.txt")

        'Get a StreamReader class that can be used to read the file
        Dim objStreamReader As StreamReader
        objStreamReader = File.OpenText(filename)

        'Now, read the entire file into a string
        Dim contents As String = objStreamReader.ReadToEnd()
        _footer = contents
        _footer = _footer.Replace("@UnSubAlign", Align)
        _footer = _footer.Replace("@UnSubFontSize", size)
        _footer = _footer.Replace("@UnSubColor", Color)
        _footer = _footer.Replace("@WebPath", _webPath)
        _footer = _footer.Replace("@CompanyName", _cName)
        _footer = _footer.Replace("@CompanyAddress1", _cAddr)
        _footer = _footer.Replace("@CompanyAddress2", _cAddr2)
        _footer = _footer.Replace("@CompanyCity", _cCity)
        _footer = _footer.Replace("@CompanyState", _cState)
        _footer = _footer.Replace("@CompanyZip", _cZip)
    End Sub

End Class
