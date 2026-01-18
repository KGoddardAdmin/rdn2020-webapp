<%@ Page Language="VB" AutoEventWireup="false" Inherits="PPage2" Codebehind="PPage2.aspx.vb" %>
<%@ Register Src="Controls/CheckJS.ascx" TagName="CheckJS" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script type="text/javascript">
        window.onload = myfunction;
        function myfunction() {
            var qrStr = window.location.search;
            qrStr = qrStr.substring(3, qrStr.length);
            //window.location.href = "testing1.aspx?boo=" + qrStr; ;

            window.location.href = unescape(qrStr);
        }
    </script>
</head>
<body>
</body>
</html>
