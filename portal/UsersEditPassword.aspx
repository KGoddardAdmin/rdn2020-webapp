<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="UsersEditPassword" title="Untitled Page" Codebehind="UsersEditPassword.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
     <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Edit User Password
	            </td>
	        </tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table align="center">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>
                </tr> 
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="vsPassword" runat="server" ValidationGroup="frmpassword" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Password is required." ValidationGroup="frmpassword"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" Display="None" ValidationExpression=".{6,25}" ValidationGroup="frmpassword" ErrorMessage="Password must be 6 characters long"></asp:RegularExpressionValidator>                                        
                        <asp:CompareValidator ID="cvCPassword" runat="server" ControlToCompare="txtPassword"
                            ControlToValidate="txtConfirmPassword" Display="None" ErrorMessage="Passwords do not match."
                            ValidationGroup="frmpassword"></asp:CompareValidator></td>
                </tr>
                <tr>
	                <td align="right" style="padding-right:5px;">
		                Password :
	                </td>
	                <td align="left" style="width: 296px">
		                <asp:TextBox ID="txtPassword" runat="server" Width="300px" MaxLength="25" ValidationGroup="frmpassword" TextMode="Password"></asp:TextBox>
	                </td>
                </tr>
                <tr>
	                <td align="right" style="padding-right:5px;">
		                Confirm Password :
	                </td>
	                <td align="left" style="width: 296px">
		                <asp:TextBox ID="txtConfirmPassword" runat="server" Width="300px" MaxLength="25" ValidationGroup="frmpassword" TextMode="Password"></asp:TextBox>
	                </td>
                </tr>  
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="cmdSubmit" runat="server" Text="Update Password" ValidationGroup="frmpassword" /></td>
                </tr>
            </table>
        </div>
     </div>        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

