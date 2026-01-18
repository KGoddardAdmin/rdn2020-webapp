Imports System.Data
Imports System.Data.SqlClient

Partial Class ClientNew
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetStateDropDown()
            txtCompanyName.Focus()
            txtDomain.Text = "http://www.rdn2020.com"
            txtClick.Text = "P.aspx"
            txtOpen.Text = "ImageTrack.aspx"
            txtCoupon.Text = "CP.aspx"
        End If
    End Sub

    Protected Sub cmdAddNewCompany_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddNewCompany.Click

        Page.Validate("frmClient")
        If Page.IsValid Then
            CreateClient()
        End If
    End Sub

    Private Sub CreateClient()
        Dim MyGuid As Guid = Guid.NewGuid()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateClient As New SqlCommand("Client_Create", cnn)
                cmdCreateClient.CommandType = CommandType.StoredProcedure
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientName", SqlDbType.NVarChar, 50)).Value = Trim(txtCompanyName.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientAddr1", SqlDbType.NVarChar, 50)).Value = Trim(txtAddr1.Text)
                If Len(Trim(txtAddr2.Text)) > 0 Or txtAddr2.Text <> String.Empty Then
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientAddr2", SqlDbType.NVarChar, 50)).Value = Trim(txtAddr2.Text)
                Else
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientAddr2", SqlDbType.NVarChar, 50)).Value = DBNull.Value
                End If
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientCity", SqlDbType.NVarChar, 50)).Value = Trim(txtCity.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientState", SqlDbType.Int)).Value = ddState.SelectedValue
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientZip", SqlDbType.NVarChar, 10)).Value = Trim(txtZip.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientPhone", SqlDbType.NVarChar, 15)).Value = Trim(txtPhone.Text)
                If Len(Trim(txtFax.Text)) > 0 Or txtFax.Text <> String.Empty Then
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientFax", SqlDbType.NVarChar, 18)).Value = Trim(txtFax.Text)
                Else
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientFax", SqlDbType.NVarChar, 18)).Value = DBNull.Value
                End If
                If Len(Trim(txtCompanyWebsite.Text)) > 0 Or txtCompanyWebsite.Text <> String.Empty Then
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientWebsite", SqlDbType.NVarChar, 100)).Value = Trim(txtCompanyWebsite.Text)
                Else
                    cmdCreateClient.Parameters.Add(New SqlParameter("@ClientWebsite", SqlDbType.NVarChar, 100)).Value = DBNull.Value
                End If
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientDomain", SqlDbType.NVarChar, 50)).Value = Trim(txtDomain.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientClick", SqlDbType.NVarChar, 50)).Value = Trim(txtClick.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientOpen", SqlDbType.NVarChar, 50)).Value = Trim(txtOpen.Text)
                cmdCreateClient.Parameters.Add(New SqlParameter("@ClientCoupon", SqlDbType.NVarChar, 50)).Value = Trim(txtCoupon.Text)
                cmdCreateClient.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "Client created."
        lblmsg.Font.Bold = True
        cmdAddNewCompany.Visible = False
    End Sub

    Private Sub GetStateDropDown()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetState As New SqlCommand("State_GetState", cnn)
                cmdGetState.CommandType = CommandType.StoredProcedure
                Using dtrGetState As SqlDataReader = cmdGetState.ExecuteReader
                    ddstate.DataSource = dtrGetState
                    ddstate.DataBind()
                End Using
            End Using
        End Using
    End Sub

    
    
End Class
