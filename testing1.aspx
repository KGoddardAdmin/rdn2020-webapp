<%@ Page Language="VB" AutoEventWireup="false" Inherits="testing1" Codebehind="testing1.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        window.onload = myfunction;
        function myfunction() {
            var qrStr = window.location.search;
            //qrStr = qrStr.substring(0, qrStr.length);

            //window.location.href = "testing.aspx?boo=" + qrStr;
            //alert(qrStr);
            window.location.href = unescape(qrStr);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
