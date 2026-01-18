<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MyAdCampaignsPull.aspx.vb" Inherits="MyAdCampaignsPull" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Pull MyAdCampaigns Data</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btnPull" runat="server" Text="Pull MyAdCampaigns Data" OnClick="btnPull_Click" />
        <asp:Label ID="lblStatus" runat="server" />
    </form>
</body>
</html> 