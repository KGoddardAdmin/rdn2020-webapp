<%@ Page Language="VB" Debug="True" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ReportClicksByCampaign" title="Untitled Page" Codebehind="ReportClicksByCampaign.aspx.vb" %>

<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" >
	<tr>
		<td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			Campiagn Click Report
	   </td>
	</tr>
	<tr>
	    <td colspan="2">
            <asp:Label ID="lblMsg" runat="server"></asp:Label></td>
	</tr>	
	<tr><td colspan="2" style="height:5px;">&nbsp;<asp:ValidationSummary ID="vsReport" runat="server" ValidationGroup="frmReport" />
        <asp:RequiredFieldValidator ID="rfvClient" runat="server" ControlToValidate="ddClient"
            Display="None" ErrorMessage="Client is Required" InitialValue="Select Client"
            ValidationGroup="frmReport"></asp:RequiredFieldValidator></td></tr>	
	 <tr>
	        <td align="right" style="width:40%; padding-right:5px;">
		        From Campaigns Created Within Last :
	        </td>
		    <td align="left" style="height: 24px; width: 552px;" >
			    <asp:DropDownList ID="ddTime" runat="server" TabIndex="1" Width="175px" AutoPostBack="True" ValidationGroup="frmReport">
                    <asp:ListItem Value="0">Yesterday</asp:ListItem>
                    <asp:ListItem Value="1">Today</asp:ListItem>
                    <asp:ListItem Value="2">Last 7 Days</asp:ListItem>
                    <asp:ListItem Value="3">Last 10 Days</asp:ListItem>
                    <asp:ListItem Value="4">Last 30 Days</asp:ListItem>
                    <asp:ListItem Value="5">Custom Dates</asp:ListItem>
			    </asp:DropDownList>
			</td>			
        </tr>
        <tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
        <tr id="trDate" runat="server" visible ="false">
	        <td align="right">
	        Start Date:&nbsp;&nbsp;
                <cc1:DatePicker ID="DatePicker1" runat="server" ValidationGroup="frmReport" />                                             
            </td>
            <td align="left" style="margin-left:0; width: 552px;">
            &nbsp;&nbsp;
            End Date:&nbsp;<cc1:DatePicker ID="DatePicker2" runat="server" ValidationGroup="frmReport" />
                &nbsp;&nbsp;&nbsp;
                        
                <asp:Button ID="cmdViewDD" runat="server" Text="Get Choices" /></td>
        </tr>
        <tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>				
	<tr>
		<td style="text-align:right; padding-right:5px; font-weight:bold;">
			Client:
		</td>
		<td style="width: 552px">
            &nbsp;<asp:DropDownList ID="ddClient" runat="server" DataTextField="ClientName" DataValueField="ClientUId" ValidationGroup="frmReport">
            </asp:DropDownList></td>
	</tr>
	<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>		
	<tr>
		<td style="text-align:right; padding-right:5px; font-weight:bold; height: 20px;">
		    Report Type:
		</td>
		<td style="width: 552px; height: 20px;">
		    <asp:RadioButton ID="radUniqueYes" runat="server" GroupName="ReportType" Text="Unique Clicks"
				ValidationGroup="Yes" />&nbsp;
			<asp:RadioButton ID="radUniqueNo" runat="server" GroupName="ReportType" Text="Total Clicks" />
			
		</td>
	</tr>
	<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
	<tr>
		<td>&nbsp;</td>		
		<td style="width: 552px">
		    <asp:Button ID="cmdGetReport" runat="server" Text="Get Report" ValidationGroup="frmReport" />
		</td>
	</tr>	
</table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr><td style=" height:5px;">&nbsp;</td></tr>                
                <tr id="trgrid" runat="server" visible="false">
                    <td>
                        <asp:Label id="lblPageCount" runat="server" /><br>
                        <asp:label id="RecordCount" runat="server" />
                        <asp:DataGrid ID="dgCampaigns" runat="server" OnItemCommand ="dgCampaigns_ItemCommand" AutoGenerateColumns="False" Width="100%" PageSize="5" AllowPaging="True" DataKeyField="CampaignId">
                            <Columns>
                                <asp:BoundColumn DataField="CampaignId" HeaderText="Campaign Id"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Campaign Name">
                                    <ItemTemplate>
                                        <asp:Label ID="Name" runat="server" Text='<%# GetCampaignName(Eval("CampaignId")) %>'>                                                
								        </asp:Label>
								    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="List Size">
                                    <ItemTemplate>
                                    	<asp:Label ID="Quanity" runat="server" Text='<%# GetQuanityOrdered(Eval("CampaignId")) %>'>                                                
	                                    </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total Opens">
                                    <ItemTemplate>
                                        <asp:Label ID="Opens" runat="server" Text='<%# GetOpens(Eval("CampaignId")) %>'>                                                
								        </asp:Label>
								 </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="TotalClicks" HeaderText="Total Clicks"></asp:BoundColumn>
                                <asp:ButtonColumn HeaderText="Link Report" CommandName="GetId" Text="View"></asp:ButtonColumn>
                                <asp:TemplateColumn HeaderText="Open Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="Otr" runat="server" Text='<%# GetOpenThroughRate(Eval("CampaignId")) %>'>                                                
								        </asp:Label>
								 </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Click Through Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="Ctr" runat="server" Text='<%# GetClickThroughs(Eval("TotalClicks"), Eval("CampaignId")) %>'>                                                
								        </asp:Label>
								 </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                             <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />	                        
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle BackColor="#DDDDDD" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                        </asp:DataGrid>
                        <asp:linkbutton id="Firstbutton" Text="<< 1st Page" CommandArgument="0" runat="server" onClick="PagerButtonClick"/>
                        <asp:linkbutton id="Prevbutton" Text= "" CommandArgument="prev" runat="server" onClick="PagerButtonClick"/>
                            &nbsp;&nbsp;
                        <asp:linkbutton id="Nextbutton" Text= "" CommandArgument="next" runat="server" onClick="PagerButtonClick"/>
                        <asp:linkbutton id="Lastbutton" Text="Last Page >>" CommandArgument="last" runat="server" onClick="PagerButtonClick"/>
                    </td>
                </tr>
            </table>                     
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

