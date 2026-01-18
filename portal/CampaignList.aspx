<%@ Page Language="VB" Debug="true"  MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampaignList" title="Untitled Page" Codebehind="CampaignList.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
     <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
	                Campaign List
	            </td>
	        </tr>
	        <tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        
            <asp:GridView ID="gridCampaigns" OnRowCommand ="ManageList" OnRowDataBound="SetgridCampaignBGColor" runat="server" AutoGenerateColumns="False" Width="100%" Font-Names="Verdana" Font-Size="Medium" DataSourceID="odsCampaigns" DataKeyNames="CampaignId,CouponVariable,Status">
                 <Columns>
                    <asp:ButtonField ButtonType="Button" CommandName="ManageList" HeaderText="Manage" Text="Button" />
                    <asp:ButtonField HeaderText="Seed" CommandName="SendSeed" Text="Send" ButtonType="Button" />
                    <asp:HyperLinkField DataNavigateUrlFields="CampaignId" DataNavigateUrlFormatString="CampaignEdit.aspx?CampaignId={0}"
                        DataTextField="CampaignId" HeaderText=" Id" NavigateUrl="CampaignEdit.aspx" />
                    <asp:BoundField DataField="CampaignName" HeaderText="Campaign Name" />
                    <asp:BoundField DataField="EmailsOrdered" HeaderText="Quanity" />
                    <asp:TemplateField HeaderText="Created On">
                        <ItemTemplate>
                            <asp:Label ID="StartDate" runat="server" Text='<%# FormatStartDate(Eval("CreatedOn")) %>'>                                                
							</asp:Label>
						</ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Subjectline" HeaderText="Subject" />
                    <asp:BoundField DataField="ClientName" HeaderText="Client" />
                    <asp:BoundField DataField="IOName" HeaderText="IO" />
                    <asp:HyperLinkField DataNavigateUrlFields="CampaignId" DataNavigateUrlFormatString="CampaignListViewCreative.aspx?CampaignId={0}"
                        HeaderText="View " NavigateUrl="CampaignListViewCreative.aspx" Target="_blank" Text="View" />
                </Columns>
                <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />
	                        <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
	                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
            </asp:GridView>
        <asp:ObjectDataSource ID="odsCampaigns" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="CampaignsTableAdapters.CampaignAdCopy_CampaignListGetAllCampaignsTableAdapter">
        </asp:ObjectDataSource>
            
        
    </div>
        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

