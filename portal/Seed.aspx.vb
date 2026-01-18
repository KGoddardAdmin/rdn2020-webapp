Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO

Partial Class Seed
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private WebPath As String ' = "http://www.leadmemarketing.com"
    Private Footer As String
    Private SeedSent As Boolean = True
    Private CName As String
    Private CAddr As String
    Private CAddr2 As String
    Private CCity As String
    Private CState As String
    Private CZip As String
    'Mail Sending Variables    
    Private mailserver As String = "relay-hosting.secureserver.net"
    'Private FromAddress As String = "customerservice@leadmemarketing.com"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load        
        If Not IsPostBack Then
            txtFrom.Text = "customerservice@mrmtrack.com"
        End If
    End Sub

    Protected Sub cmdSendMail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSendMail.Click
        Page.Validate("frmSeed")
        If Page.IsValid Then
            GetCoInfo()
            FormatFooter()
            SendEmail()

            If SeedSent = True Then
                lblmsg.Text = "Seed Sent."
            Else
                lblmsg.Text = "Seed Not Sent."
            End If
        End If
    End Sub

    Private Sub SendEmail()
        Dim Body As String = txtCreative.Text & Footer
        Dim smtpClient As New SmtpClient(mailserver)
        'smtpClient.Port = 25
        Dim Mail As MailMessage
        Mail = New MailMessage        
        If Trim(Len(txtFriendlyTo.Text)) > 0 Or Trim(txtFriendlyTo.Text) <> String.Empty Then
            Mail.To.Add(New MailAddress(txtTo.Text, txtFriendlyTo.Text))
        Else
            Mail.To.Add(New MailAddress(txtTo.Text))
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
    End Sub

    Private Sub GetCoInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCoInfo As New SqlCommand("Company_GetInfo", cnn)
                Using dtrGetCoInfo As SqlDataReader = cmdGetCoInfo.ExecuteReader
                    While dtrGetCoInfo.Read

                        If Not IsDBNull(dtrGetCoInfo("Name")) Then
                            CName = dtrGetCoInfo("Name")
                        Else
                            CName = String.Empty
                        End If


                        If Not IsDBNull(dtrGetCoInfo("Name")) Then
                            CName = dtrGetCoInfo("Name")
                        Else
                            CName = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Addr1")) Then
                            CAddr = dtrGetCoInfo("Addr1")
                        Else
                            CAddr = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Addr2")) Then
                            CAddr2 = dtrGetCoInfo("Addr2")
                        Else
                            CAddr2 = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("City")) Then
                            CCity = dtrGetCoInfo("City")
                        Else
                            CCity = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("State")) Then
                            CState = dtrGetCoInfo("State")
                        Else
                            CState = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("Zip")) Then
                            CZip = dtrGetCoInfo("Zip")
                        Else
                            CZip = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCoInfo("WebPath")) Then
                            WebPath = Microsoft.VisualBasic.Left(dtrGetCoInfo("WebPath"), Len(dtrGetCoInfo("WebPath")) - 1)
                        Else
                            WebPath = "http://www.rdn2020.com"
                        End If

                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub FormatFooter()

        'Dim Footer As String
        Dim FILENAME As String = Server.MapPath("Text/Footer2.txt")

        'Get a StreamReader class that can be used to read the file
        Dim objStreamReader As StreamReader
        objStreamReader = File.OpenText(FILENAME)

        'Now, read the entire file into a string
        Dim contents As String = objStreamReader.ReadToEnd()
        Footer = contents
        Footer = Footer.Replace("@WebPath", WebPath)
        Footer = Footer.Replace("@CompanyName", CName)
        Footer = Footer.Replace("@CompanyAddress1", CAddr)
        Footer = Footer.Replace("@CompanyAddress2", CAddr2)
        Footer = Footer.Replace("@CompanyCity", CCity)
        Footer = Footer.Replace("@CompanyState", CState)
        Footer = Footer.Replace("@CompanyZip", CZip)
    End Sub
End Class
