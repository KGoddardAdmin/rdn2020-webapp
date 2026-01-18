<%@ Page Language="VB" MasterPageFile="~/Portal/Admin.master" AutoEventWireup="false" Inherits="ClientContactEdit" title="Untitled Page" Codebehind="ClientContactEdit.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 Edit Client Contact
           </td>
        </tr>
        <tr><td style="height:10px;">&nbsp;</td></tr>
        <tr>
            <td style="padding-left:50px;">
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
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                </tr> 
                <tr>
	                <td align="right">
		                Contact:
	                </td>
	                <td align="left" >
		                <asp:DropDownList ID="ddContact" runat="server" DataTextField="ContactName" DataValueField="ContactUId" TabIndex="2" AutoPostBack="True">
		                </asp:DropDownList>
	                </td>
                </tr>
                 <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
                <tr>
	                <td align="right">
		                Password:
	                </td>
	                <td align="left" style="width: 204px">
		                &nbsp;<asp:HyperLink ID="hyplPassword" runat="server">Edit Password</asp:HyperLink></td>
                    </tr>
                    <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
            </table>
        </div>
    </div>
        
        
            
            
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

