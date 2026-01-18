<%@ Page Language="VB" AutoEventWireup="false" Inherits="V2P" Codebehind="V2P.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
	<meta http-equiv="X-UA-Compatible" content="IE=7" />
    <script language="JavaScript" type="text/javascript">
<!--
window.onload = function()
{   
    if (navigator.appName == "Microsoft Internet Explorer")
    { document.getElementById( 'targetlink' ).click(); } 
    else
    { 
        var evt = document.createEvent("MouseEvents");
        evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        var cb = document.getElementById("targetlink"); 
        var canceled = !cb.dispatchEvent(evt);
     }
}
//-->
    </script>
</head>
<body>
<a href="<%= ClickLink %>" style="color:White;" onclick="location.href=this.href" id="targetlink">Continue</a> 
</body>
</html>
