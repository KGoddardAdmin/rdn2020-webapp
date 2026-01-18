<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" CodeFile="MyAdCampaigns.aspx.vb" Inherits="Portal_MyAdCampaigns" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <h2>MyAdCampaigns API Integration</h2>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div style="margin:20px;">
        <cc1:DatePicker ID="dpStartDate" runat="server" />
        <cc1:DatePicker ID="dpEndDate" runat="server" />
        <asp:DropDownList ID="ddlIsActive" runat="server">
            <asp:ListItem Text="All" Value="" />
            <asp:ListItem Text="Enabled" Value="true" />
            <asp:ListItem Text="Disabled" Value="false" />
        </asp:DropDownList>
        <asp:DropDownList ID="ddlPricingModel" runat="server">
            <asp:ListItem Text="All" Value="" />
            <asp:ListItem Text="CPC" Value="CPC" />
            <asp:ListItem Text="CPM" Value="CPM" />
        </asp:DropDownList>
        <asp:Button ID="btnPullApiCampaigns" runat="server" Text="Pull API Campaigns" />
        <br /><br />
        <asp:GridView ID="gridApiCampaigns" runat="server" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:BoundField DataField="id" HeaderText="ID" />
                <asp:BoundField DataField="name" HeaderText="Campaign Name" />
                <asp:BoundField DataField="pricing_model" HeaderText="Pricing Model" />
                <asp:BoundField DataField="description" HeaderText="Description" />
                <asp:BoundField DataField="start_date" HeaderText="Start Date" />
                <asp:BoundField DataField="end_date" HeaderText="End Date" />
                <asp:BoundField DataField="is_active" HeaderText="Enabled" />
                <asp:BoundField DataField="budget_total" HeaderText="Total Budget" />
                <asp:BoundField DataField="cost_total" HeaderText="Total Cost" />
                <asp:BoundField DataField="budget_daily" HeaderText="Daily Budget" />
                <asp:BoundField DataField="budget_limiter_type" HeaderText="Budget Type" />
                <asp:BoundField DataField="cost_today" HeaderText="Today's Cost" />
                <asp:BoundField DataField="clicks_daily" HeaderText="Clicks/Day" />
                <asp:BoundField DataField="conversions_daily" HeaderText="Conversions/Day" />
                <asp:BoundField DataField="clicks_today" HeaderText="Today's Clicks" />
                <asp:BoundField DataField="impressions_today" HeaderText="Today's Impressions" />
                <asp:BoundField DataField="clicks_per_ip" HeaderText="Clicks per IP" />
                <asp:BoundField DataField="impressions_per_ip" HeaderText="Impressions per IP" />
                <asp:BoundField DataField="impressions_per_ip_requests_only" HeaderText="Impr/IP Requests Only" />
                <asp:BoundField DataField="feedlist_mode" HeaderText="Feeds List Mode" />
                <asp:BoundField DataField="feedlist" HeaderText="Feeds List" />
                <asp:BoundField DataField="siteid_list_mode" HeaderText="Site Id List Mode" />
                <asp:BoundField DataField="siteid_list" HeaderText="Site Id List" />
                <asp:BoundField DataField="appid_list_mode" HeaderText="App Id List Mode" />
                <asp:BoundField DataField="appid_list" HeaderText="App Id List" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content> 