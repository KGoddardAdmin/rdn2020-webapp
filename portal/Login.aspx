<%@ Page Language="VB" AutoEventWireup="false" Inherits="Login" Codebehind="Login.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>rdn2020</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style=" text-align:center;">
            <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
	            <tr>
		            <td align="center" colspan="5" style="padding:15px; font-size:2em; font-weight: bold; color: white; background-color: rgb(66,79,153);">
		                rdn2020 
		            </td>
	            </tr>
	            <tr>
		            <td align="center" colspan="5" style="padding:15px; font-weight: bold; color: white; background-color: rgb(66,79,153); ">
                        <asp:ValidationSummary ID="vsLogin" runat="server" ValidationGroup="Login1" ForeColor="#FF6412" />		  
		            </td>
	            </tr>			
	            <tr>
		            <td align="center" rowspan="3" style="padding:15px; font-weight: bold; color: white; background-color: rgb(66,79,153); ">
		                Log In
		            </td>
		            <td colspan="3">
                        <asp:Label ID="lblmsg" runat="server" Width="100%"></asp:Label>
		            </td>
		            <td align="center" rowspan="3" style="padding:15px; background-color: rgb(66,79,153); ">
			            <asp:Button ID="cmdSubmit" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" TabIndex="5" />
		            </td>
	            </tr>
	            <tr>
		            <td style="width:8px;">&nbsp;</td>
		            <td align="right">
		                <asp:Label Font-Bold="True" ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Login:</asp:Label>
		            </td>
		            <td align="Left" style="width: 170px">
		                <asp:TextBox ID="UserName" runat="server" TabIndex="1"></asp:TextBox>
		                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
				            ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1" Display="None">*</asp:RequiredFieldValidator>
		            </td>
	            </tr>
	            <tr>
		            <td style="width:8px;">&nbsp;</td>
		            <td align="right">
			            <asp:Label Font-Bold="True" ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
		            </td>
		            <td align="Left" style="width: 170px">
			            <asp:TextBox ID="Password" runat="server" TextMode="Password" TabIndex="2" Width="149px"></asp:TextBox>
			            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
				            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1" Display="None">*</asp:RequiredFieldValidator>
		            </td>
	            </tr>
	            <tr>
	                <td align="center" colspan="5" style="padding:15px; font-weight: bold; color: white; background-color: rgb(66,79,153); ">
	                    &nbsp;
	                </td>
                </tr>		
            </table>            
    </div>
    </form>
</body>
</html>
