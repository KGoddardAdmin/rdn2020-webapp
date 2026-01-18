<%@ Page Language="VB" Debug="true"  MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ImpressionCampaignCreate" title="Untitled Page" Codebehind="ImpressionCampaignCreate.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
            <tr>
                <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                    New Impression Campaign
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table>
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="vsic" runat="server" ValidationGroup="frmCampaign" />
                        <asp:RequiredFieldValidator ID="frfIO" runat="server" ControlToValidate="ddIO" Display="None" ErrorMessage="You must select an IO" InitialValue="Select IO" ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtImpressionName"
                            Display="None" ErrorMessage="The campaign name is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvLink" runat="server" ControlToValidate="txtImpressionLink"
                            Display="None" ErrorMessage="The campaign link is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvImgscr" runat="server" ControlToValidate="txtImpressionImageSrc"
                            Display="None" ErrorMessage="The image src is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvTrackLink" runat="server" ControlToValidate="txtImpressionTrackingLink"
                            Display="None" ErrorMessage="Tracking link is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                </tr> 
                <tr>
		            <td style="text-align:right; padding-left: 10px; font-weight:bold; width:10%;">
			            IO:			
		            </td>
		            <td style="text-align: left; padding-left:2px; font-weight:bold;">
			            <asp:DropDownList ID="ddIO" runat="server" AutoPostBack="True" ValidationGroup="frmCampaign">
			            </asp:DropDownList>
		            </td>
	            </tr> 
                <tr>
                    <td style="text-align: right; padding-left: 10px; font-weight:bold;">
	                    Campaign Name:
                    </td>
                    <td style="text-align: left; padding-left:2px; font-weight:bold;">
	                    <asp:TextBox ID="txtImpressionName" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-left: 10px; font-weight:bold;">
	                    Campaign Link:
                    </td>
                    <td style="text-align: left; padding-left:2px; font-weight:bold;">
	                    <asp:TextBox ID="txtImpressionLink" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox></td>
                    </tr>
                <tr>
                    <td style="text-align: right; padding-left: 10px; font-weight:bold;">
	                    Campaign Image Src:
                    </td>
                    <td style="text-align: left; padding-left:2px; font-weight:bold;">
	                    <asp:TextBox ID="txtImpressionImageSrc" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                </td>
                </tr>
                <tr>
                    <td style="text-align: right; padding-left: 10px; font-weight:bold;">
	                    Campaign Tracking Link:
                    </td>
                    <td style="text-align: left; padding-left:2px; font-weight:bold;">
	                        <asp:TextBox ID="txtImpressionTrackingLink" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="cmdCreate" runat="server" Text="Create Campaign" ValidationGroup="frmCampaign" /></td>                        
                </tr>
                <tr>
                    <td style=" vertical-align:top; text-align: right; padding-left: 10px; font-weight:bold; height: 127px;">Campaign Code:</td>
                    <td style="height: 127px">
                        <asp:TextBox ID="txtCreative" runat="server" Height="109px" TextMode="MultiLine" Width="570px"></asp:TextBox></td>
                </tr>    
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>                 
             </table>            
        </div>
    </div>
        
                           
	            
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

