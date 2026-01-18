<%@ Page Language="VB" Debug="true"  MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ContactNew" title="Untitled Page" Codebehind="ContactNew.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 New Client Contact
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
                <td colspan="2">
                    <asp:ValidationSummary ID="vsContact" runat="server" ValidationGroup="frmContact" />
                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="ddClient"
                        Display="None" ErrorMessage="You must select a company." InitialValue="Select Client"
                        ValidationGroup="frmContact"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvSale" runat="server" ControlToValidate="ddSaleRep"
                        Display="None" ErrorMessage="You must select a sales rep." InitialValue="Select Account Rep"
                        ValidationGroup="frmContact"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvFName" ValidationGroup="frmContact" ControlToValidate ="txtFName"
                        display="none" ErrorMessage="First name is required." runat ="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvLName" ValidationGroup="frmContact" ControlToValidate="txtLName"
                        display="none" ErrorMessage ="Last name is required." runat ="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat ="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="Email is required." ValidationGroup="frmContact"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone" Display="None" ErrorMessage="Phone is required." ValidationGroup="frmContact"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="None" ErrorMessage="Password is required." ValidationGroup="frmContact"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revPhone" runat ="server" ControlToValidate="txtPhone" Display="None" ErrorMessage="Phone is not in proper format." ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ValidationGroup="frmContact"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat ="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="Email is not in proper format." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="frmContact"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" Display="None" ValidationExpression=".{6,25}" ValidationGroup="frmContact" ErrorMessage="Password must be 6 characters long"></asp:RegularExpressionValidator>
                    
                </td>
            </tr> 
            <tr>
                <td align="right">
                    Company:
                </td>
                <td align="left" >
                     <asp:DropDownList ID="ddClient" runat="server" DataTextField="ClientName" DataValueField="ClientUId" TabIndex="1">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Sales Rep:
                </td>
                <td align="left" >
                     <asp:DropDownList ID="ddSaleRep" runat="server" DataTextField="ClientName" DataValueField="ClientUId" TabIndex="2">
                    </asp:DropDownList>
                </td>
            
            </tr>
            <tr>
                <td align="right">
                    First Name:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtFName" runat="server" Width="225px" MaxLength="50" ValidationGroup="frmContact" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    MI:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtMI" runat="server" Width="25px" MaxLength="1" ValidationGroup="frmContact" TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Last Name:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtLName" runat="server" Width="225px" MaxLength="50" ValidationGroup="frmContact" TabIndex="5"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Email:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtEmail" runat="server" Width="225px" MaxLength="100" ValidationGroup="frmContact" TabIndex="6"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Phone:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtPhone" runat="server" Width="225px" MaxLength="15" ValidationGroup="frmContact" TabIndex="7"></asp:TextBox>
                    e.g.&nbsp;&nbsp;xxx-xxx-xxxx
                    </td>
            </tr>
            <tr>
                <td align="right">
                    Cell:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtCell" runat="server" Width="225px" MaxLength="15" ValidationGroup="frmContact" TabIndex="8"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Fax:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtFax" runat="server" Width="225px" MaxLength="15" ValidationGroup="frmContact" TabIndex="9"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="right">
                    Password:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtPassword" runat="server" Width="225px" MaxLength="25" ValidationGroup="frmContact" TextMode="Password" TabIndex="10"></asp:TextBox></td>
            </tr>
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
            <tr>
                <td>&nbsp;</td>
                <td >
                    <asp:Button ID="cmdAddContact" runat="server" Text="Add Contact" /></td>
            </tr>
           <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
        </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

