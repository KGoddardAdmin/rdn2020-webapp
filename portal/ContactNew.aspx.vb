Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Security.Cryptography

Partial Class ContactNew
    Inherits System.Web.UI.Page

    Private strConn As String = ConfigurationSettings.AppSettings("cnn")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            GetClientList()
            GetSalesRep()
            ddClient.Focus()
        End If

    End Sub

    Protected Sub cmdAddContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddContact.Click
       
        Page.Validate("frmContact")
        If Page.IsValid Then
            CreateContact()
        End If

    End Sub

    Private Sub CreateContact()

        'Encode the Password NO Salt used       
        Dim md5Hasher As New MD5CryptoServiceProvider()
        Dim hashedBytes As Byte()
        Dim encoder As New UTF8Encoding()
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(txtPassword.Text))

        Dim MyGuid As Guid = Guid.NewGuid()
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdCreateContact As New SqlCommand("ClientContact_Create", cnn)
                cmdCreateContact.CommandType = CommandType.StoredProcedure
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactUId", SqlDbType.UniqueIdentifier)).Value = MyGuid
                cmdCreateContact.Parameters.Add(New SqlParameter("@ClientUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddClient.SelectedValue)
                cmdCreateContact.Parameters.Add(New SqlParameter("@SalesRepUId", SqlDbType.UniqueIdentifier)).Value = SqlTypes.SqlGuid.Parse(ddSaleRep.SelectedValue)
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactFName", SqlDbType.NVarChar, 50)).Value = Trim(txtFName.Text)
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactLName", SqlDbType.NVarChar, 50)).Value = Trim(txtLName.Text)
                If Len(Trim(txtMI.Text)) > 0 Or Trim(txtMI.Text) <> String.Empty Then
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactMI", SqlDbType.Char)).Value = Trim(txtMI.Text)
                Else
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactMI", SqlDbType.Char)).Value = DBNull.Value
                End If
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactEmail", SqlDbType.NVarChar, 100)).Value = Trim(txtEmail.Text)
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactPhone", SqlDbType.NVarChar, 15)).Value = Trim(txtPhone.Text)
                If Len(Trim(txtCell.Text)) > 0 Or Trim(txtCell.Text) <> String.Empty Then
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactCell", SqlDbType.NVarChar, 15)).Value = Trim(txtCell.Text)
                Else
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactCell", SqlDbType.NVarChar, 15)).Value = DBNull.Value
                End If
                If Len(Trim(txtFax.Text)) > 0 Or Trim(txtFax.Text) <> String.Empty Then
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactFax", SqlDbType.NVarChar, 18)).Value = Trim(txtFax.Text)
                Else
                    cmdCreateContact.Parameters.Add(New SqlParameter("@ContactFax", SqlDbType.NVarChar, 18)).Value = DBNull.Value
                End If
                cmdCreateContact.Parameters.Add(New SqlParameter("@ContactPassword", SqlDbType.Binary, 16)).Value = hashedBytes
                cmdCreateContact.ExecuteNonQuery()
            End Using
        End Using
        lblmsg.Text = "Contact Created."
        lblmsg.Font.Bold = True
        cmdAddContact.Enabled = False
        cmdAddContact.Visible = False
    End Sub

    Private Sub GetSalesRep()
        Dim dstGetSalesRep As DataSet
        Dim dadGetSalesRep As SqlDataAdapter
        Dim cnn As New SqlConnection(strConn)
        dstGetSalesRep = New DataSet
        dadGetSalesRep = New SqlDataAdapter("SalesRep_Get", cnn)
        dadGetSalesRep.Fill(dstGetSalesRep, "AccountReps")
        If dstGetSalesRep.Tables("AccountReps").Rows.Count = 0 Then            
            lblmsg.Text = "Need to add sales rep."
            lblmsg.Font.Bold = True
            cmdAddContact.Enabled = False
        Else
            Dim Dyncolumn As New DataColumn
            With Dyncolumn
                .ColumnName = "Name"
                .DataType = System.Type.GetType("System.String")
                .Expression = "SalesRepFName+'  '+SalesRepLName"
            End With
            dstGetSalesRep.Tables("AccountReps").Columns.Add(Dyncolumn)
            ddSaleRep.DataTextField = "Name"
            ddSaleRep.DataValueField = "SalesRepUId"
            ddSaleRep.DataSource = dstGetSalesRep.Tables("AccountReps").DefaultView
            ddSaleRep.DataBind()
        End If
        ddSaleRep.Items.Insert(0, "Select Account Rep")
        dadGetSalesRep.Dispose()
        dstGetSalesRep.Dispose()
        If cnn.State = ConnectionState.Open Then
            cnn.Close()
            cnn.Dispose()
        End If


    End Sub

    Private Sub GetClientList()
        ddClient.Items.Add(New ListItem("Select Client", 0))
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdGetClientList As New SqlCommand("Client_Get", cnn)
                Using dtrGetClientList As SqlDataReader = cmdGetClientList.ExecuteReader
                    ddClient.DataSource = dtrGetClientList
                    ddClient.DataBind()
                End Using
            End Using
        End Using
        ddClient.Items.Insert(0, "Select Client")

    End Sub

End Class

