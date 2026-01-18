Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.Web.Caching
Imports System.Web.Configuration
Imports System.Web.Profile
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Microsoft.VisualBasic

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        'get reference to the source of the exception chain
        Dim ex As Exception = Server.GetLastError().GetBaseException()

        'log the details of the exception and page state to the
        'Windows 2000 Event Log
        Dim msg As String = "Lead Me Marketing "
        msg &= "MESSAGE: " & ex.Message.ToString
        msg &= "\nSOURCE: " & ex.Source
        msg &= "\nFORM: " & Request.Form.ToString()
        msg &= "\nQUERYSTRING: " & Request.QueryString.ToString()
        msg &= "\nTARGETSITE: " & ex.TargetSite.ToString
        msg &= "\nSTACKTRACE: " & ex.StackTrace
        
        Dim strConn As String = ConfigurationManager.AppSettings("cnn")
        Using cnn As New SqlConnection(strConn)
            cnn.Open()
            Using cmdErrorLog As New SqlCommand("Error_RecordNew", cnn)
                cmdErrorLog.CommandType = CommandType.StoredProcedure
                cmdErrorLog.Parameters.Add(New SqlParameter("@ErrorMsg", SqlDbType.NVarChar)).Value = msg
                cmdErrorLog.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
End Class 