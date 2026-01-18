<%@ Page Language="VB" Debug="true"  MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="UsersCreateNew" title="Untitled Page" Codebehind="UsersCreateNew.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                New User
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
                <td colspan="2" style=" height:10px;">
                    <asp:ValidationSummary ID="vsUser" runat="server" ValidationGroup="frmUsers" />
                    <asp:RequiredFieldValidator ID="rfvLogin" runat="server" ControlToValidate="txtLogin"
                        Display="None" ErrorMessage="Login is required." ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvemail" runat="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="Email is required." ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvEmail" OnServerValidate ="EmailIsUnique" runat="server" ControlToValidate="txtEmail" Display="None"
                        ErrorMessage="We are sorry but that email is already in our system." ValidationGroup="frmUsers"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Password is required." ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="txtFName" Display="None" ErrorMessage="First name is required." ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="txtLName" Display="None" ErrorMessage="Last name is required." ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddRole" Display="None" ErrorMessage="You must select a role." InitialValue="0" ValidationGroup="frmUsers"></asp:RequiredFieldValidator>
                </td>
            </tr> 
            <tr>
	            <td align="right">
		            Login:
	            </td>
	            <td align="left" style="width: 204px">
		            <asp:TextBox ID="txtLogin" runat="server" Width="225px" ValidationGroup="frmUsers" MaxLength="50"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Email:
	            </td>
	            <td align="left" style="width: 204px">
		            <asp:TextBox ID="txtEmail" runat="server" Width="225px" ValidationGroup="frmUsers" MaxLength="50"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Password:
	            </td>
	            <td align="left" style="width: 204px">
		            <asp:TextBox ID="txtPassword" runat="server" Width="225px" ValidationGroup="frmUsers" MaxLength="50" TextMode="Password"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right" style="height: 26px">
		            First Name:
	            </td>
	            <td align="left" style="width: 204px; height: 26px;">
		            <asp:TextBox ID="txtFName" runat="server" Width="225px" ValidationGroup="frmUsers" MaxLength="50"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Last Name:
	            </td>
	            <td align="left" style="width: 204px">
		            <asp:TextBox ID="txtLName" runat="server" Width="225px" ValidationGroup="frmUsers" MaxLength="50"></asp:TextBox>
	            </td>
            </tr>
            
            <tr>
	            <td align="right">
		            Role:
	            </td>
	            <td align="left" style="width: 204px">
                    &nbsp;<asp:DropDownList ID="ddRole" runat="server" DataTextField="RoleName" DataValueField="RoleId">
                    </asp:DropDownList></td>
            </tr>
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="cmdAddUser" runat="server" Text="Add User" ValidationGroup="frmUsers" /></td>
            </tr> 
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>       
         </table>
         </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

