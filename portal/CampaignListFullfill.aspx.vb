Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Imports System.Net.Mime
Imports System.IO


Partial Class CampaignListFullfill
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private CampaignType As String
    Private FName As String
    Private LName As String
    Private Email As String
    Private EmailBody As String
    Private CampaignName As String
    Private Quanity As Integer
    Private subject As String
    Private CouponVariable As String
    Private OrginalCreative As String
    Private Creative As String    
    Private DirectoryPath As String
    Private CampaignLink As String
    Private CouponLink As String
    Private LinkId As Integer = 0
    Private CouponPath As String
    Private OpenLink As String
    Private ImpressionPath As String
    Private NewLink As String = "href={Replace}"
    Private rcreative As String
    Private replacestring As String
    Private ConvertLink As String
    Private Domain As String
    Private DefaultDomain As String = "http://www.rdn2020.com"
    Private ClickPage As String
    Private DefaultClickPage As String = "P.aspx."
    Private OpenPage As String
    Private DefaultOpenPage As String = "ImageTrack.aspx"
    Private CouponPage As String
    Private DefaultCouponPage As String = "CP.aspx"
    'Private ImageOpenLink As String
    'SessionVariables To PreLoad
    Private CampaignId As Integer
    Private Status As Integer
    'Mail Sending Variables  
    Private mailserver As String = "relay.justonevision.com"
    Private FromAddress As String = "testing@justonevision.com"
    Private MailUser As String = "trackingstats@relay.leadmehosting.com"
    Private MailPass As String = "trackingpass!1"
    'Private mailserver As String = "64.244.63.33"
    'Private FromAddress As String = "lga4040@relay.leadmemarketing.com"
    Private EmailSubject As String
    'Private MailUser As String = "lga4040@relay.leadmemarketing.com"
    'Private MailPass As String = "Password!1"
    Private MailPort As Integer = 25


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CheckForCreativeDirectory()        
        CheckSessionVariable()
        GetCampaignInfo()        
        GetPaths()
        GetEmailSubject()
        If CouponVariable <> String.Empty Then            
            FormatCouponbody()
            txtLink.Text = CouponLink
            ConvertSeedCampaign()
        Else
            CampaignType = "Reg"
            Formatbody()
            txtLink.Text = CampaignLink
            convert()
        End If
        lblmsg.Text = String.Empty
        lblmsg.Font.Bold = False
        lblmsg.ForeColor = Drawing.Color.Black

        If Not IsPostBack Then        
            txtEmail.Text = Email
        End If       

    End Sub

    Protected Sub cmdSend_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSend.Click
        Page.Validate("frmsend")
        If Page.IsValid Then            
            SendEmails()
        End If
    End Sub

    Private Sub SendEmails()
        Dim sent As Boolean
        sent = SendEmail()
        If sent = True Then
            Dim sendagain As Boolean
            sendagain = SendMail()
            If sendagain = True Then
                UpdateStatus(CampaignId)
                lblmsg.Text = "Fullfillment info sent."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Green
            Else                
                lblmsg.Text = "There was a problem sending the fullfillment info."
                lblmsg.Font.Bold = True
                lblmsg.ForeColor = Drawing.Color.Red
            End If
        Else            
            lblmsg.Text = "There was a problem sending the fullfillment info."
            lblmsg.Font.Bold = True
            lblmsg.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Private Sub CheckSessionVariable()
        If Session("CampaignFullfill") IsNot Nothing Then
            Dim arrseed() As String = New String() {}
            arrseed = Session("CampaignFullfill").Split("~")
            CampaignId = arrseed(0)
            Status = arrseed(1)
            If arrseed.Length = 3 Then
                CouponVariable = arrseed(2)
            End If
        End If

        If Session("AcceptedUser") IsNot Nothing Then
            Dim arruser() As String = New String() {}
            arruser = Session("AcceptedUser").Split("~")
            FName = arruser(0)
            LName = arruser(1)
            Email = arruser(3)
        End If        
    End Sub

    Private Sub GetEmailSubject()
        Dim ClientName As String = String.Empty
        Dim CampaignName As String = String.Empty
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetEmailSubject As New SqlCommand("Client_GetClientNameByCampaignId", cnn)
                cmdGetEmailSubject.CommandType = CommandType.StoredProcedure
                cmdGetEmailSubject.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetEmailSubject As SqlDataReader = cmdGetEmailSubject.ExecuteReader
                    While dtrGetEmailSubject.Read

                        If Not IsDBNull(dtrGetEmailSubject("ClientName")) Then
                            ClientName = Trim(dtrGetEmailSubject("ClientName"))
                        Else
                            ClientName = String.Empty
                        End If

                        If Not IsDBNull(dtrGetEmailSubject("CampaignName")) Then
                            CampaignName = Trim(dtrGetEmailSubject("CampaignName"))
                        Else
                            CampaignName = String.Empty
                        End If                        
                    End While
                End Using
            End Using
        End Using
        EmailSubject = CampaignId & "/" & ClientName & "/" & CampaignName
    End Sub

    Private Sub GetCampaignInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignInfo As New SqlCommand("CampaignAdCopy_GetByCampaignId", cnn)
                cmdGetCampaignInfo.CommandType = CommandType.StoredProcedure
                cmdGetCampaignInfo.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetCampaignInfo As SqlDataReader = cmdGetCampaignInfo.ExecuteReader
                    While dtrGetCampaignInfo.Read

                        If Not IsDBNull(dtrGetCampaignInfo("CampaignName")) Then
                            CampaignName = Trim(dtrGetCampaignInfo("CampaignName"))
                        Else
                            CampaignName = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignInfo("EmailsOrdered")) Then
                            Quanity = dtrGetCampaignInfo("EmailsOrdered")
                        Else
                            Quanity = 0
                        End If

                        If Not IsDBNull(dtrGetCampaignInfo("SubjectLine")) Then
                            subject = Trim(dtrGetCampaignInfo("SubjectLine"))
                        Else
                            subject = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignInfo("CampaignCreative")) Then
                            OrginalCreative = Trim(dtrGetCampaignInfo("CampaignCreative"))
                        Else
                            OrginalCreative = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCampaignInfo("ConvertedCreative")) Then
                            Creative = Trim(dtrGetCampaignInfo("ConvertedCreative"))
                        Else
                            Creative = String.Empty
                        End If
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub FormatCouponbody()
        'Dim Footer As String
        Dim FILENAME As String = Server.MapPath("Text/Fullfillwithcoupon.txt")
        'Get a StreamReader class that can be used to read the file
        Dim objStreamReader As StreamReader
        objStreamReader = File.OpenText(FILENAME)
        'Now, read the entire file into a string
        Dim contents As String = objStreamReader.ReadToEnd()
        EmailBody = contents
        EmailBody = EmailBody.Replace("@CampaignId", CampaignId)
        EmailBody = EmailBody.Replace("@CampaignName", CampaignName)
        EmailBody = EmailBody.Replace("@CampaignLink", Trim(txtLink.Text))
        EmailBody = EmailBody.Replace("@Subject", subject)
        EmailBody = EmailBody.Replace("@Quanity", Quanity)
        EmailBody = EmailBody.Replace("@CouponVriable", CouponVariable)
        objStreamReader.Close()
        objStreamReader.Dispose()

    End Sub

    Private Sub Formatbody()
        'Dim Footer As String
        Dim FILENAME As String = Server.MapPath("Text/FullFill.txt")
        'Get a StreamReader class that can be used to read the file
        Dim objStreamReader As StreamReader
        objStreamReader = File.OpenText(FILENAME)
        'Now, read the entire file into a string
        Dim contents As String = objStreamReader.ReadToEnd()
        EmailBody = contents
        EmailBody = EmailBody.Replace("@CampaignId", CampaignId)
        EmailBody = EmailBody.Replace("@CampaignName", CampaignName)
        EmailBody = EmailBody.Replace("@CampaignLink", Trim(txtLink.Text))
        EmailBody = EmailBody.Replace("@Subject", subject)
        EmailBody = EmailBody.Replace("@Quanity", Quanity)
        objStreamReader.Close()
        objStreamReader.Dispose()
    End Sub

    Private Function SendEmail() As Boolean  'SendEmail()       
        Dim rtn As Boolean
        Try
            Dim smtpClient As New SmtpClient(mailserver)
            smtpClient.UseDefaultCredentials = False
            smtpClient.Port = MailPort
            smtpClient.Credentials = New Net.NetworkCredential(MailUser, MailPass)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim Mail As MailMessage
            Mail = New MailMessage
            Mail.To.Add(New MailAddress(Trim(txtEmail.Text)))
            Mail.From = New MailAddress(FromAddress)
            Mail.Subject = Trim(EmailSubject)
            Mail.Body = Trim(EmailBody)
            Dim contentType As New Net.Mime.ContentType()
            Dim convertedattachment As Attachment
            Using memoryStream As New MemoryStream()
                Dim contentAsBytes As Byte() = Encoding.UTF8.GetBytes(OrginalCreative)
                memoryStream.Write(contentAsBytes, 0, contentAsBytes.Length)
                ' Set the position to the beginning of the stream. 
                memoryStream.Seek(0, SeekOrigin.Begin)
                ' Create attachment 
                'Dim contentType As New Net.Mime.ContentType()
                contentType.MediaType = MediaTypeNames.Application.Octet
                contentType.Name = CampaignId & "-" & CampaignName & "-SemiConveted.htm"
                convertedattachment = New Attachment(memoryStream, contentType)
                ' Add the attachment 
                Mail.Attachments.Add(convertedattachment)
                Mail.IsBodyHtml = True
                ' Send Mail via SmtpClient 
                smtpClient.Send(Mail)
                convertedattachment.Dispose()
            End Using
            rtn = True
            'lblmsg.Text = "Fullfillment info sent."
            'lblmsg.Font.Bold = True
            'lblmsg.ForeColor = Drawing.Color.Green
            'UpdateStatus(CampaignId)
        Catch ex As Exception
            rtn = False
            'Response.Write(ex.ToString)
            'lblmsg.Text = "There was a problem sending the fullfillment info."
            'lblmsg.Font.Bold = True
            'lblmsg.ForeColor = Drawing.Color.Red
        End Try
        Return rtn
    End Function

    Private Function SendMail() As Boolean  'SendEmail() 
        Dim rtn As Boolean
        Dim subject As String = EmailSubject & "-WITH LINKS"
        Try
            Dim smtpClient As New SmtpClient(mailserver)
            smtpClient.UseDefaultCredentials = False
            smtpClient.Port = MailPort
            smtpClient.Credentials = New Net.NetworkCredential(MailUser, MailPass)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim Mail As MailMessage
            Mail = New MailMessage
            Mail.To.Add(New MailAddress(Trim(txtEmail.Text)))
            Mail.From = New MailAddress(FromAddress)
            Mail.Subject = Trim(subject)
            Mail.Body = Trim(EmailBody)
            Dim contentType As New Net.Mime.ContentType()
            Dim convertedattachment As Attachment
            Using memoryStream As New MemoryStream()
                Dim contentAsBytes As Byte() = Encoding.UTF8.GetBytes(Creative)
                memoryStream.Write(contentAsBytes, 0, contentAsBytes.Length)
                ' Set the position to the beginning of the stream. 
                memoryStream.Seek(0, SeekOrigin.Begin)
                ' Create attachment 
                'Dim contentType As New Net.Mime.ContentType()
                contentType.MediaType = MediaTypeNames.Application.Octet
                contentType.Name = CampaignId & "-" & CampaignName & "-Conveted.htm"
                convertedattachment = New Attachment(memoryStream, contentType)
                ' Add the attachment 
                Mail.Attachments.Add(convertedattachment)
                Mail.IsBodyHtml = True
                ' Send Mail via SmtpClient 
                smtpClient.Send(Mail)
                convertedattachment.Dispose()
            End Using
            rtn = True
            'lblmsg.Text = "Fullfillment info sent."
            'lblmsg.Font.Bold = True
            'lblmsg.ForeColor = Drawing.Color.Green
            'UpdateStatus(CampaignId)
        Catch ex As Exception
            rtn = False
            'Response.Write(ex.ToString)
            'lblmsg.Text = "There was a problem sending the fullfillment info."
            'lblmsg.Font.Bold = True
            'lblmsg.ForeColor = Drawing.Color.Red
        End Try
        Return rtn
    End Function


    Private Sub UpdateStatus(ByVal Id As Integer)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdUpdateStatus As New SqlCommand("CampaignAdCopy_UpdateStatus", cnn)
                cmdUpdateStatus.CommandType = CommandType.StoredProcedure
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int)).Value = 6
                cmdUpdateStatus.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = Id
                cmdUpdateStatus.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub GetPaths()

        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetPaths As New SqlCommand("Client_GetByCampaignId", cnn)
                cmdGetPaths.CommandType = CommandType.StoredProcedure
                cmdGetPaths.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int)).Value = CampaignId
                Using dtrGetPaths As SqlDataReader = cmdGetPaths.ExecuteReader
                    While dtrGetPaths.Read
                        If Not IsDBNull(dtrGetPaths("ClientDomain")) Then
                            Domain = dtrGetPaths("ClientDomain")
                        Else
                            Domain = DefaultDomain
                        End If

                        If Not IsDBNull(dtrGetPaths("ClientClick")) Then
                            ClickPage = dtrGetPaths("ClientClick")
                        Else
                            ClickPage = DefaultClickPage
                        End If

                        If Not IsDBNull(dtrGetPaths("ClientOpen")) Then
                            OpenPage = dtrGetPaths("ClientOpen")
                        Else
                            OpenPage = DefaultOpenPage
                        End If

                        If Not IsDBNull(dtrGetPaths("ClientCoupon")) Then
                            CouponPage = dtrGetPaths("ClientCoupon")
                        Else
                            CouponPage = DefaultCouponPage
                        End If
                        ConvertLink = Domain & "/" & ClickPage & "?c=@CampaignId" '~@LinkId"
                        CampaignLink = Domain & "/" & ClickPage & "?c=@CampaignId" '~@LinkId"
                        CampaignLink = Replace(CampaignLink, "@CampaignId", CampaignId)
                        'CampaignLink = Replace(CampaignLink, "~@LinkId", "")
                        CouponPath = Domain & "/" & CouponPage & "?c=@CampaignId" '~@LinkId"
                        CouponLink = Domain & "/" & CouponPage & "?c=@CampaignId" '~@LinkId"
                        CouponLink = Replace(CouponLink, "@CampaignId", CampaignId)
                        'CouponLink = Replace(CouponLink, "~@LinkId", "")
                        'CouponLink = CouponLink & "~" & CouponVariable

                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetCompanyInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCompanyInfo As New SqlCommand("Company_GetInfo", cnn)
                cmdGetCompanyInfo.CommandType = CommandType.StoredProcedure
                Using dtrGetCompanyInfo As SqlDataReader = cmdGetCompanyInfo.ExecuteReader
                    While dtrGetCompanyInfo.Read
                        If Not IsDBNull(dtrGetCompanyInfo("CreativeRedirectPath")) Then
                            ConvertLink = dtrGetCompanyInfo("CreativeRedirectPath")
                            CampaignLink = dtrGetCompanyInfo("CreativeRedirectPath")
                            CampaignLink = Replace(CampaignLink, "@CampaignId", CampaignId)
                            CampaignLink = Replace(CampaignLink, "~@LinkId", "")                            
                        Else
                            CampaignLink = String.Empty
                        End If

                        If Not IsDBNull(dtrGetCompanyInfo("CouponPath")) Then
                            CouponPath = dtrGetCompanyInfo("CouponPath")
                            'CouponPath = Domain & "/" & CouponPage & "?c=@CampaignId~@LinkId"
                            CouponLink = dtrGetCompanyInfo("CouponPath")
                            CouponLink = Replace(CouponLink, "@CampaignId", CampaignId)
                            CouponLink = Replace(CouponLink, "~@LinkId", "")                            
                            'CouponLink = CouponLink & "~" & CouponVariable
                        Else
                            CouponLink = String.Empty
                        End If

                    End While                    
                End Using
            End Using
        End Using
    End Sub

#Region "Covert"


    Public Sub convert()
        ImpressionPath = Domain & "/" & OpenPage & "?c=" & CampaignId        
        OpenLink &= "<img src=""" & ImpressionPath & """ alt="""" width=""0"" height=""0""/>"
        OrginalCreative = Replace(OrginalCreative, "href = ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href= ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF = ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF= ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF =", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href =", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href=", NewLink, 1, -1, CompareMethod.Text)
        Dim StartOfLink As Integer = 1
        Dim endoflink As Integer = 1

        Do Until StartOfLink = 0
            StartOfLink = InStr(OrginalCreative, NewLink)
            If StartOfLink <> 0 Then
                rcreative = Right(OrginalCreative, Len(OrginalCreative) - StartOfLink - 14)
                endoflink = InStr(rcreative, """")
                replacestring = Left(rcreative, endoflink - 1)
                Transform(replacestring, StartOfLink, Len(replacestring))
            End If
        Loop
        OrginalCreative = Replace(OrginalCreative, "href={Replaced}", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = OrginalCreative & OpenLink

    End Sub

    Public Sub ConvertSeedCampaign()

        'OrginalCreative = Replace(OrginalCreative, CouponVariable, "{CPLink}", 1, -1, CompareMethod.Text)
        ImpressionPath = Domain & "/" & OpenPage & "?c=" & CampaignId
        OpenLink &= "<img src=""" & ImpressionPath & """ alt="""" width=""0"" height=""0""/>"
        OrginalCreative = Replace(OrginalCreative, "href = ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href= ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF = ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF= ", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "HREF =", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href =", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = Replace(OrginalCreative, "href=", NewLink, 1, -1, CompareMethod.Text)
        Dim StartOfLink As Integer = 1
        Dim endoflink As Integer = 1
        Do Until StartOfLink = 0
            StartOfLink = InStr(OrginalCreative, NewLink)
            If StartOfLink <> 0 Then
                rcreative = Right(OrginalCreative, Len(OrginalCreative) - StartOfLink - 14)
                endoflink = InStr(rcreative, """")
                replacestring = Left(rcreative, endoflink - 1)
                TransformSeed(replacestring, StartOfLink, Len(replacestring))
            End If
        Loop
        OrginalCreative = Replace(OrginalCreative, "href={Replaced}", "href=", 1, -1, CompareMethod.Text)
        OrginalCreative = OrginalCreative & OpenLink
       
    End Sub

    Private Sub TransformSeed(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
        LinkId = LinkId + 1
        Dim nstr As String = Trim(CouponPath)
        nstr = Replace(nstr, "@CampaignId", Trim(CampaignId))
        'nstr = Replace(nstr, "~@LinkId", "")
        'nstr = nstr & "~" & CouponVariable
        nstr = Trim(nstr)
        nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)        
        OrginalCreative = OrginalCreative.Remove(startpos + 14, length)
        OrginalCreative = OrginalCreative.Insert(startpos + 14, nstr)
        OrginalCreative = OrginalCreative.Insert(startpos + 12, "d")        
    End Sub

    Private Sub Transform(ByVal link As String, ByVal startpos As Integer, ByVal length As Integer)
        LinkId = LinkId + 1
        Dim nstr As String = Trim(ConvertLink)
        nstr = Replace(nstr, "@CampaignId", CampaignId)
        'nstr = Replace(nstr, "~@LinkId", "")
        nstr = Trim(nstr)
        nstr = Replace(nstr, Chr(9), "", 1, -1, CompareMethod.Text)
        OrginalCreative = OrginalCreative.Remove(startpos + 14, length)
        OrginalCreative = OrginalCreative.Insert(startpos + 14, Trim(nstr))
        OrginalCreative = OrginalCreative.Insert(startpos + 12, "d")

    End Sub
#End Region

End Class
