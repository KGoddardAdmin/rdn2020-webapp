<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampaignListViewCreative" title="Untitled Page" Codebehind="CampaignListViewCreative.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td style="text-align: left; padding-left: 10px; font-weight:bold;">
			        View Campaign Creative
	            </td>
	        </tr>	
	        <tr><td style=" height:20px;">&nbsp;</td></tr>                
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" style="border:solid 1px rgb(66,79,153);" >
                            <tr>
                                <td>
                                    <%=Creative %>
                                </td>
                            </tr>
                        </table>
                        
                    </td>
                </tr>
            </table>
        </div>
    </div>
        
            
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

