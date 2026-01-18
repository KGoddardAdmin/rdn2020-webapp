<%@ Page Language="VB" AutoEventWireup="false" Inherits="Unsubscribe" Codebehind="Unsubscribe.aspx.vb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
  <head runat="server">
    <title>Untitled Page</title>
  </head>
  <body>
    <form id="form1" runat="server">
      <div>
        <table>
            <tr>
                <td><asp:Label ID="lblmsg" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:ValidationSummary ID="vsUnsubscribe" runat="server" ValidationGroup="frmUnsubscribe" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="You must submit an email address to be removed." ValidationGroup="frmUnsubscribe"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="The email you submitted it not in the proper formmat." ValidationGroup="frmUnsubscribe" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                    To no longer receive emails from us.<br />
                    Please enter your email address below and submit.
                </td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtEmail" runat="server" Width="479px" ValidationGroup="frmUnsubscribe"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Button ID="cmdSubmit" runat="server" Text="Please Remove Me" ValidationGroup="frmUnsubscribe" /></td>
            </tr>        
            </table>
        </div>
    </form>
  </body>
</html>
