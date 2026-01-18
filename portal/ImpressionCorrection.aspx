<%@ Page Language="VB" AutoEventWireup="false" Debug="True" Inherits="ImpressionCorrection" Codebehind="ImpressionCorrection.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
        <table style="width: 70%">
            <tr><td colspan="2">
                <asp:Label ID="lblmsg" runat="server"></asp:Label></td></tr>
            <tr id="trOldId" runat = "server">
                <td align="right" style="padding-right:10px;">Campaign Id you want to copy:</td>
                <td align="left">
                    <asp:TextBox ID="txtOLdId" runat="server"></asp:TextBox>
                    <asp:Button ID="cmdGetCount" runat="server" Text="Get Couut" /></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td align="right" style="padding-right:10px;">Maxiumn Number Of Records That You Can Add:</td>
                <td align="left">
                    <asp:Label ID="lblCount" runat="server"></asp:Label></td>
            </tr>
            <%If ShowForm = True Then%>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr>
                <td align="right" style="padding-right:10px;">Number Of Records to Create:</td>
                <td align="left" >
                    <asp:TextBox ID="txtCount" runat="server"></asp:TextBox></td>
            </tr>
            <tr><td colspan="2">&nbsp;</td></tr>
            <tr id="trNewId">
                <td  align="right" style="padding-right:10px;">Enter Id Of Campaign To Create:</td>
                <td align="left">
                    <asp:TextBox ID="txtId" runat="server"></asp:TextBox>
                    <asp:Button ID="cmdClone" runat="server" Text="Create Records" /></td>
            </tr>
            <%End If%>
        </table>    
    </div>
    </form>
</body>
</html>
