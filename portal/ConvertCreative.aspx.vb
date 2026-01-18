Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Net



Partial Class ConvertCreative
    Inherits System.Web.UI.Page


    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private InValidLoginRedirectURL As String = ConfigurationSettings.AppSettings("InvalidLoginURL")
    Private Creative As String
    Private NewLink As String = "href={Replace}"
    Private ConvertLink  '= "http://www.rdn2020.com/process.aspx?c=@CampaingId~@LinkId"
    Private rcreative As String
    Private replacestring As String
    Private LinkId As Integer = 0
    Private CampaignId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckLogin()
        'http://([\w-]+\.)+[\w-]+(/[\w- ./]*)+\.(?:gif|jpg|jpeg|png|bmp|GIF|JPEG|JPG|PNG|BMP|Gif|Jpg|Jpeg|Png|Bmp)$
    End Sub

    Protected Sub cmdConvert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdConvert.Click       
        convert()
    End Sub

    Private Sub CheckLogin()
        If Session("AcceptedUser") Is Nothing Then
            LoginFailure()
        End If
    End Sub

    Public Sub convert()
        Creative = txtHtml.Text

        If Trim(Len(Creative)) > 0 Or Creative <> String.Empty Then
            GetCampaignId()
            GetCompanyInfo()
            Insertcreative()
            Creative = Replace(Creative, "href = ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href= ", "href=", 1, -1, CompareMethod.Text)
            Creative = Replace(Creative, "href =", "href=", 1, -1, CompareMethod.Text)
            If InStr(Creative, "href=") = 0 Then
                txtHtml.Text = "Creative Has No Click Link In It!!"
                txtHtml.Font.Bold = True
                txtHtml.ForeColor = Drawing.Color.Red
                txtHtml.Font.Size = 24
                Exit Sub
            End If
            Creative = Replace(Creative, "href=", NewLink, 1, -1, CompareMethod.Text)
            Dim StartOfLink As Integer = 1
            Dim endoflink As Integer = 1
            Do Until StartOfLink = 0
                StartOfLink = InStr(Creative, NewLink)
                If StartOfLink <> 0 Then
                    rcreative = Right(Creative, Len(Creative) - StartOfLink - 14)
                    endoflink = InStr(rcreative, """")
                    replacestring = Left(rcreative, endoflink - 1)
                    TransformLink(replacestring)
                End If
            Loop
        Else
            txtHtml.Text = "You need to enter a creative"
            txtHtml.Font.Bold = True
            txtHtml.ForeColor = Drawing.Color.Red
            txtHtml.Font.Size = 24
        End If

        txtConverted.Text = Creative

    End Sub

    Private Sub TransformLink(ByVal link As String)                   
        LinkId = LinkId + 1
        Dim nstr As String = ConvertLink
        nstr = Replace(nstr, "@CampaingId", CampaignId)
        nstr = Replace(nstr, "@LinkId", LinkId)
        Creative = Replace(Creative, link, nstr, 1, -1, CompareMethod.Text)
        Creative = Replace(Creative, "href={Replace}""http://www.rdn2020.com", "href=""http://www.rdn2020.com", 1, -1, CompareMethod.Text)
        InsertCampaign(LinkId, link)
    End Sub

   

    Private Function CheckLink(ByVal link As String) As Boolean
        Dim ValidLink As Boolean = True

        ' Static method: 
        If Not Regex.IsMatch(link, "^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$") Then
            ' Name does not match schema 
            ValidLink = False
        End If

        Return ValidLink

    End Function

    Private Sub GetCampaignId()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignId As New SqlCommand("Campaign_CampaignId_GetNext", cnn)
                cmdGetCampaignId.CommandType = Data.CommandType.StoredProcedure
                CampaignId = cmdGetCampaignId.ExecuteScalar
            End Using
        End Using
    End Sub

    Private Sub InsertCampaign(ByVal linkId As Integer, ByVal Link As String)
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertCampaign As New SqlCommand("Campaign_CreateNew", cnn)
                cmdInsertCampaign.CommandType = Data.CommandType.StoredProcedure
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@LinkId", Data.SqlDbType.Int)).Value = linkId
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@Link", Data.SqlDbType.NVarChar, 150)).Value = Link
                cmdInsertCampaign.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub GetCompanyInfo()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCompanyInfo As New SqlCommand("Company_GetInfo", cnn)
                cmdGetCompanyInfo.CommandType = Data.CommandType.StoredProcedure
                Using dtrGetCompanyInfo As SqlDataReader = cmdGetCompanyInfo.ExecuteReader
                    While dtrGetCompanyInfo.Read
                        If Not IsDBNull(dtrGetCompanyInfo("CreativeRedirectPath")) Then
                            ConvertLink = dtrGetCompanyInfo("CreativeRedirectPath")
                        Else
                            txtHtml.Text = "There is a problem with the db, the redirect link cannot be found!!"
                            txtHtml.Font.Bold = True
                        End If
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub LoginFailure()
        Response.Redirect(InValidLoginRedirectURL)
    End Sub

    Private Sub Insertcreative()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertcreative As New SqlCommand("CampaignCreative_InsertNew", cnn)
                cmdInsertcreative.CommandType = Data.CommandType.StoredProcedure
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignId", Data.SqlDbType.Int)).Value = CampaignId
                cmdInsertcreative.Parameters.Add(New SqlParameter("@CampaignCreative", Data.SqlDbType.NVarChar)).Value = Trim(txtHtml.Text)
                cmdInsertcreative.ExecuteNonQuery()
            End Using
        End Using


    End Sub
End Class
