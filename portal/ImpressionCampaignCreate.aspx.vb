Imports System.Data
Imports System.Data.SqlClient

Partial Class ImpressionCampaignCreate
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")
    Private ImpressionPath As String
    Private Creative As String
    Private CreativeId As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetIOs()
        End If
    End Sub

    Protected Sub cmdCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreate.Click               
        Page.Validate("frmCampaign")
        If Page.IsValid Then
            GetCampaignId()
            GetCompanyInfo()
            InsertCampaign()
            CreateCreative()
        End If
    End Sub

    Private Sub InsertCampaign()
        Dim MyGuid As Guid = Guid.NewGuid()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdInsertCampaign As New SqlCommand("ImpressionCreative_CreateNewCampaign", cnn)
                cmdInsertCampaign.CommandType = CommandType.StoredProcedure
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignId", SqlDbType.Int)).Value = CreativeId
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddIO.SelectedValue)
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignName", SqlDbType.NVarChar, 50)).Value = Trim(txtImpressionName.Text)
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignLink", SqlDbType.NVarChar, 250)).Value = Trim(txtImpressionLink.Text)
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignTrackingLink", SqlDbType.NVarChar, 250)).Value = Trim(txtImpressionTrackingLink.Text)
                cmdInsertCampaign.Parameters.Add(New SqlParameter("@ImpressionCampaignImageSrc", SqlDbType.NVarChar, 250)).Value = Trim(txtImpressionImageSrc.Text)
                cmdInsertCampaign.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub CreateCreative()
        ImpressionPath = Replace(ImpressionPath, "@CampaingId", CreativeId)
        Creative = "<a href=" & txtImpressionLink.Text & " target=""_blank"">"
        Creative &= "<img src=" & txtImpressionImageSrc.Text & " border=""0"">"
        Creative &= "<img src=" & txtImpressionTrackingLink.Text & " border=""0"" height=""1"" width=""1"" alt="""">"
        Creative &= "<img src=" & ImpressionPath & " alt="""" width=""0"" height=""0""/></a>"
        txtCreative.Text = Creative
    End Sub

    Private Sub GetCampaignId()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetCampaignId As New SqlCommand("Impression_ImpressionCampaignId_GetNext", cnn)
                cmdGetCampaignId.CommandType = CommandType.StoredProcedure
                CreativeId = cmdGetCampaignId.ExecuteScalar
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
                        ImpressionPath = dtrGetCompanyInfo("ImpressionPath")
                    End While
                End Using
            End Using
        End Using
    End Sub

    Private Sub GetIOs()
        Dim dadGetIOs As New SqlDataAdapter
        Dim dstGetIOs As New DataSet
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetIOs As New SqlCommand("IO_Get_ForImpressionCreation", cnn)
                cmdGetIOs.CommandType = CommandType.StoredProcedure
                dadGetIOs.SelectCommand = cmdGetIOs
                dadGetIOs.Fill(dstGetIOs, "IOs")
                If dstGetIOs.Tables("IOs").Rows.Count = 0 Then
                    lblmsg.Text = "There are no active IO's, Please create an IO for this campaign."

                    lblmsg.Font.Bold = True
                Else
                    Dim Dyncolumn As New DataColumn
                    With Dyncolumn
                        .ColumnName = "Name"
                        .DataType = System.Type.GetType("System.String")
                        .Expression = "IOId+'   '+IOName"
                    End With
                    dstGetIOs.Tables("IOs").Columns.Add(Dyncolumn)
                    ddIO.DataTextField = "Name"
                    ddIO.DataValueField = "IOUId"
                    ddIO.DataSource = dstGetIOs.Tables("IOs").DefaultView
                    ddIO.DataBind()
                End If
            End Using
        End Using
        ddIO.Items.Insert(0, "Select IO")
        dadGetIOs.Dispose()
        dstGetIOs.Dispose()
    End Sub
End Class
