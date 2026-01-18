<%@ Application Codebehind="Global.asax.vb" Inherits="Global_asax" Language="VB" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

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
        
        
                                
        '///Insets into Text Log file
        'Dim FILENAME As String = Server.MapPath("ErrorLog.txt")
        'Get a StreamReader class that can be used to read the file
        'Dim objStreamWriter As StreamWriter
        'objStreamWriter = File.AppendText(FILENAME)
        'Append the the end of the string, "A user viewed this demo at: "
        'followed by the current date and time
        'objStreamWriter.WriteLine(msg)    
        'Close the stream
        'objStreamWriter.Close()


        'Insert optional email notification here...
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
                              
</script>