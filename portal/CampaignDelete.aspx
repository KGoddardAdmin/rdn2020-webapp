<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampaignDelete" title="Untitled Page" Codebehind="CampaignDelete.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 Delete Campaign
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
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>                    
                </tr> 
                <tr>
                    <td>
                        <asp:Label ID="lbloutput" runat="server"></asp:Label></td>
                    <td>
                        <asp:Button ID="cmdDelete" runat="server" Text="Yes Delete Campaign" />&nbsp;</td>
                </tr>
            </table>
        </div>
    </div>                              
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

