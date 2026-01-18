<%@ Page Language="VB" Debug="true" AutoEventWireup="false" Inherits="ManageContactErrors" Codebehind="ManageErrors.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>        
        <fieldset style="height: 453px">
            <asp:TextBox ID="txtmsg" runat="server" Height="354px" Width="100%" TextMode="MultiLine"></asp:TextBox>
            <br />
            <asp:Button ID="cmdClear" runat="server" Text="Next Error" />&nbsp;
            <asp:HiddenField ID="ErrorId" runat="server" />
        </fieldset>    
    </div>
    </form>
</body>
</html>
