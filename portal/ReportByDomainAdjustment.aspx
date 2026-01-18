<%@ Page Language="VB" AutoEventWireup="false" Inherits="portal_ReportByDomainAdjustment" CodeFile="ReportByDomainAdjustment.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .tdLeft {
            text-align: right;
            width: 50%;
            padding-right: 15px;
        }

        .tdRight {
            text-align: left;
            padding-left: 15px;
        }

        .tdAll {
            text-align: center;
        }

        .outer {
            text-align: center;
            border: 3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <br />
            <table width="100%" border="2px">
                <tr>
                    <td>
                        <table width="100%" cellpadding="0" cellspacing="0" style="margin-left: auto; margin-right: auto; text-align: left">
                            <tr>
                                <td class="tdLeft">
                                    <asp:Label ID="Label1" runat="server" Text="Campaign Name"></asp:Label>
                                </td>
                                <td class="tdRight">
                                    <asp:Label ID="lblCampiagnName" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLeft">
                                    <asp:Label ID="Label3" runat="server" Text="Domain"></asp:Label>
                                </td>
                                <td class="tdRight">
                                    <asp:Label ID="lblDomain" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdAll" colspan="2">
                                    <asp:Label ID="lblCount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdAll" colspan="2">
                                    <asp:Label ID="lblOpenmsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLeft">
                                    <asp:Label ID="lblOpens" runat="server"></asp:Label>
                                </td>
                                <td class="tdRight">
                                    <asp:TextBox ID="txtOpens" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdAll" colspan="2">
                                    <asp:Label ID="lblClickmsg" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLeft">
                                    <asp:Label ID="lblClicks" runat="server"></asp:Label>
                                </td>
                                <td class="tdRight">
                                    <asp:TextBox ID="txtClicks" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdAll" colspan="2">
                                    <asp:Button ID="cmdAdd" runat="server" Text="Add Selected Counts" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
