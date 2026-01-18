<%@ Page Language="VB" Debug="true" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampaignListFullfill" title="Untitled Page" Codebehind="CampaignListFullfill.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Campign Fullfillment
	            </td>
	        </tr>
	        <tr><td colspan="2" style=" height:15px;">&nbsp;</td></tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">        
            <table width="100%">
                <tr><td colspan="2">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label></td></tr>
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="vssend" runat="server" ValidationGroup="frmsend" />
                        <asp:RequiredFieldValidator ID="rfvemail" runat="server" ControlToValidate="txtEmail"
                            Display="None" ErrorMessage="You must enter an email address." ValidationGroup="frmsend"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
	                <td style="text-align:right; padding-right: 2px; font-weight:bold;">
		                Campaign Link:
	                </td>
	                <td style="text-align: left; padding-left: 2px; font-weight:bold;">
		                <asp:TextBox ID="txtLink" runat="server" Width="729px" ValidationGroup="frmsend" ReadOnly="True"></asp:TextBox>
	                </td>                    
                </tr>                       
                <tr>
                    <td style="text-align:right; padding-right: 2px; font-weight:bold;">
                        Send To:
                    </td>
                    <td style="text-align: left; padding-left: 2px; font-weight:bold;">
                        <asp:TextBox ID="txtEmail" runat="server" Width="729px" ValidationGroup="frmsend"></asp:TextBox>
                    </td>                    
                </tr>                
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="cmdSend" runat="server" Text="Send" ValidationGroup="frmsend" />
                    </td>
                </tr>
                 <tr><td colspan="2" style=" height:15px;">&nbsp;</td></tr>
            </table>        
    </div>                     
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

