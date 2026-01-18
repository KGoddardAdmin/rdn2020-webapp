<%@ Page Language="VB" Debug="true"  ValidateRequest="false" AutoEventWireup="false" Inherits="ConvertCreative" Codebehind="ConvertCreative.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Customize Creative Creation</title>
<link rel="stylesheet" type="text/css" href="Styles/admin.css" />            

</head>
<body>
    <form id="form1" runat="server">
    <div class="mainform">
        <fieldset style="height: 757px">
            <div class="button">            
                <asp:Label ID="lblhead" runat="server" Font-Bold="True" Font-Names="Verdana"
                    Font-Size="Large" Text="rdn2020 Convert Creative" Width="483px"></asp:Label>
                <a href="ManageErrors.aspx" runat="Server">Go To Manage Errors</a>
            </div>
              <br />
            <asp:Label ID="Label1" runat="server" Text="Enter Creative" Width="191px"></asp:Label><br />
            <asp:TextBox ID="txtHtml" runat="server" Height="379px" TextMode="MultiLine" Width="100%"></asp:TextBox>                        
            <div class="button">
                <div class="spacer">&nbsp;</div>
                <asp:Button ID="cmdConvert" runat="server" Text="Convert Creative" /><br /><br />
            </div>
            <asp:Label ID="Label2" runat="server" Text="Converted Creative"></asp:Label><br />
            <asp:TextBox ID="txtConverted" runat="server" Height="255px" TextMode="MultiLine"
                Width="100%"></asp:TextBox>
        </fieldset>
    
    </div>
    </form>
</body>
</html>
