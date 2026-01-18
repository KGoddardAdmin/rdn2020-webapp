<%@ Page Language="VB" Debug="true" AutoEventWireup="false" MasterPageFile="Admin.master" CodeFile="CampaignUpdateStatus.aspx.vb" Inherits="portal_CampaignUpdateStatus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="mstMain" runat="server">
    <div style="border: 1px solid #000; padding: 20px; width: 540px; margin: 0 auto; background-color: rgb(66,79,153);">
        <asp:Label ID="updateMsg" Font-Size="Small" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Label ID="lblCampaignIdList" Font-Size="Large" Font-Bold="true" runat="server">Enter Campaign ID's For Status Update:</asp:Label>
        <br />
        <br />
        <asp:TextBox ID="campaignIdList" Height="400px" Width="100%" TextMode="MultiLine" runat="server"></asp:TextBox>
        <br />
        <asp:Button ID="cmdUpdate" runat="server" Text="Update Status" />
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
        <asp:CheckBox ID="clearApproved" runat="server" Font-Size="Small" Text="Clear Approved and Ready To Go (All Greens)" />
    </div>
</asp:Content>
