<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TestPage.aspx.vb" Inherits="Portal_TestPage" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>API Test Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:20px;">
            <asp:Button ID="btnGetCampaigns" runat="server" Text="Get Campaigns (API Test)" />
            <br /><br />
            <asp:Label ID="lblResult" runat="server" Text="" style="display:block;white-space:pre-wrap;font-family:monospace;"></asp:Label>
        </div>
    </form>
</body>
</html> 