<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" CodeFile="ShortedCampaigns.aspx.vb" Inherits="Portal_ShortedCampaigns" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table cellpadding="0" cellspacing="0" border="0" width="800">
            <tr>
                <td align="right" style="height: 23px">
                    <asp:Button ID="cmdGetReport" runat="server" Text="Get Report" />&nbsp;&nbsp;
                    Select A Client:
                    <asp:DropDownList ID="ddlClient" runat="server"></asp:DropDownList>
                    &nbsp;Select A Month:
                    <asp:DropDownList ID="ddlMonth" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr>
                    <td style="height: 10px;"></td>
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cvError" runat="server" ErrorMessage=""></asp:CustomValidator>
                        <asp:GridView ID="gridShortedCampaigns" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="100" AllowSorting="True" OnSorting="gridShortedCampaigns_Sorting">
                            <Columns>
                                <asp:BoundField DataField="CampaignId" HeaderText="Campaign ID" SortExpression="CampaignId" />
                                <asp:TemplateField HeaderText="Campaign Name" SortExpression="CampaignName">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkCampaignName" runat="server"
                                            Text='<%# Eval("CampaignName") %>'
                                            NavigateUrl='<%# "ReportByDomainAdjustment.aspx?Name=" & Eval("CampaignName") & _
                                                "&DomainID=" & Eval("CampaignId") & _
                                                "&Domain=" & Eval("Domain") & _
                                                "&Opens=" & Eval("Opens") & _
                                                "&Clicks=" & Eval("Clicks") %>'
                                            Target="_blank" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BroadcastDate" HeaderText="Broadcast Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="BroadcastDate" />
                                <asp:BoundField DataField="EmailsOrdered" HeaderText="Emails Ordered" SortExpression="EmailsOrdered" />
                                <asp:BoundField DataField="Opens" HeaderText="Opens" SortExpression="Opens" />
                                <asp:BoundField DataField="Clicks" HeaderText="Clicks" SortExpression="Clicks" />
                                <asp:BoundField DataField="OpensLeft" HeaderText="Opens Left" SortExpression="OpensLeft" />
                                <asp:BoundField DataField="ClicksLeft" HeaderText="Clicks Left" SortExpression="ClicksLeft" />
                                <asp:BoundField DataField="RdnCampaignId" HeaderText="RDN Campaign ID" SortExpression="RdnCampaignId" />
                            </Columns>
                            <HeaderStyle BackColor="#6D7171" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content> 