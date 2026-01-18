<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MyAdCampaignsPullReview.aspx.vb" Inherits="MyAdCampaignsPullReview" MasterPageFile="~/portal/Admin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" runat="server">
    <title>Review and Import MyAdCampaigns</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" runat="server">
    <asp:Button ID="btnView" runat="server" Text="View Campaigns/Offers" OnClick="btnView_Click" />
    <asp:Button ID="btnSubmit" runat="server" Text="Submit to Database" OnClick="btnSubmit_Click" Enabled="false" />
    <br /><br />
    <asp:GridView ID="gridPreview" runat="server" AutoGenerateColumns="True" />
    <asp:Label ID="lblStatus" runat="server" />
</asp:Content> 