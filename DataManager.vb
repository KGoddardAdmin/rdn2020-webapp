Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration

Namespace lga4040
    Public Class DataManager
        Private ReadOnly _strConn As String = ConfigurationSettings.AppSettings("cnn")
        Private _sqlConn As SqlConnection
        Private _sqlCommand As SqlCommand
        Private _sqlDataAdapter As SqlDataAdapter
        Private _errorMsg As String = ""

        Private Property ErrorMsg() As String
            Get
                Return _errorMsg
            End Get

            Set(ByVal value As String)
                _errorMsg = value
            End Set
        End Property

        Public Function GetCampaigns() As DataSet
            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "CampaignAdCopy_CampaignListGetAllCampaigns"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlDataAdapter.Fill(dsCampaign)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsCampaign.Dispose()
            End Try

            Return dsCampaign
        End Function

        Public Function GetCampaignById(ByVal campaignId As Int32) As DataSet

            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "Campaign_GetByCampaignId"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int, 20)).Value = CampaignId
                _sqlDataAdapter.Fill(dsCampaign)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
            End Try

            Return dsCampaign
        End Function

        Public Function GetCampaignByCampaignUId(ByVal campaignUId As Guid) As DataSet

            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "CheckUserSecurityLevel"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@CampaignUId", SqlDbType.UniqueIdentifier)).Value = CampaignUId
                _sqlDataAdapter.Fill(dsCampaign)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsCampaign.Dispose()
            End Try

            Return dsCampaign
        End Function

        Public Function GetCampaignByIOUId(ByVal IOUId As Guid) As DataSet

            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "CheckUserSecurityLevel"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@IOUId", SqlDbType.UniqueIdentifier)).Value = IOUId
                _sqlDataAdapter.Fill(dsCampaign)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsCampaign.Dispose()
            End Try

            Return dsCampaign
        End Function

        Public Function GetCampaignsByStatus(ByVal status As Int32) As DataSet

            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "CampaignAdCopy_CampaignListByStatus"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int, 10)).Value = Status
                _sqlDataAdapter.Fill(dsCampaign)
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsCampaign.Dispose()
            End Try

            Return dsCampaign
        End Function

        Public Function UpdateCampaignStatus(ByVal campaignId As Int32) As DataSet

            Dim dsCampaign As New DataSet
            'Dim isAdmin As Boolean = False
            _sqlConn = New SqlConnection(_strConn)
            _sqlCommand = New SqlCommand()
            _sqlCommand.CommandType = CommandType.StoredProcedure
            _sqlCommand.CommandText = "CampaignAdCopy_UpdateStatus"
            _sqlDataAdapter = New SqlDataAdapter(_sqlCommand)

            Try
                If _sqlConn.State = ConnectionState.Closed Then _sqlConn.Open()
                _sqlCommand.Connection = _sqlConn
                _sqlCommand.Parameters.Add(New SqlParameter("@Status", SqlDbType.Int, 10)).Value = 5
                _sqlCommand.Parameters.Add(New SqlParameter("@CampaignId", SqlDbType.Int, 10)).Value = campaignId
                _sqlCommand.ExecuteNonQuery()
            Catch ex As Exception
                ErrorMsg = "An error has occured while retrieving the Campaign list. &nbsp; Please contact your system administrator"
            Finally
                _sqlConn.Close()
                _sqlCommand.Dispose()
                dsCampaign.Dispose()
            End Try

            Return dsCampaign
        End Function
    End Class
End Namespace