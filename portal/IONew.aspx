<%@ Page Language="VB" Debug="true"  MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="IONew" title="Untitled Page" Codebehind="IONew.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                New IO
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
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>
                </tr>  
                <tr>
                    <td colspan="2">
                        <asp:ValidationSummary ID="vsIO" runat="server" ValidationGroup="frmIO" />                        
                        <asp:RequiredFieldValidator ID="rfvClient" runat ="server" ControlToValidate="ddClient" Display="None" ErrorMessage="You must select a client" InitialValue="Select Client" ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvContact" runat="server" ControlToValidate="ddContact" Display="None" ErrorMessage="You must select a contact." InitialValue="Select Contact" ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvSalesRep" runat="server" ControlToValidate="ddSalesRep"
                            Display="None" ErrorMessage="You must select a sales rep." InitialValue="Select Account Rep"
                            ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvId" runat="server" ControlToValidate="txtIOId" Display="None" ErrorMessage="IO Id is required." ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtIOName" Display="None" ErrorMessage="IO name is required." ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvType" runat="server" ControlToValidate="ddType" Display="None" ErrorMessage="You must select an IO type." InitialValue="0" ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddStatus" Display="None" ErrorMessage="You must select an IO status." InitialValue="0" ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvClick" runat="server" ControlToValidate="txtUnitCost" Display="None" ErrorMessage="Click cost is requirded." ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvQuanity" runat="server" ControlToValidate="txtQuanity" Display="None" ErrorMessage="Quanity is required." ValidationGroup="frmIO"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount" Display="None" ValidationGroup="frmIO"></asp:RequiredFieldValidator>&nbsp;
                        <asp:RegularExpressionValidator ID="revAmount" runat ="server" ControlToValidate="txtAmount" Display="None" ErrorMessage="Amount is not in corrct format." ValidationExpression="^\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$" ValidationGroup="frmIO"></asp:RegularExpressionValidator>                        
                    </td>
                </tr>                                              
                <tr>
                    <td align="right">
                        Client:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:DropDownList ID="ddClient" runat="server" AutoPostBack="True" DataTextField="ClientName"
                            DataValueField="ClientUId" ValidationGroup="frmIO">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td align="right">
                        Contact:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:DropDownList ID="ddContact" runat="server" AutoPostBack="True" ValidationGroup="frmIO">
                        </asp:DropDownList></td>
                </tr> 
                <tr>
                    <td align="right">
                        Sales Rep:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:DropDownList ID="ddSalesRep" runat="server" AutoPostBack="True" ValidationGroup="frmIO">
                        </asp:DropDownList></td>
                </tr> 
                <tr>
                    <td align="right">
                        Id:
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtIOId" runat="server" Width="100px" MaxLength="25" ValidationGroup="frmIO"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td align="right">
                        Name:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:TextBox ID="txtIOName" runat="server" Width="400px" MaxLength="50" ValidationGroup="frmIO"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td align="right">
                        IO Type:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:DropDownList ID="ddType" runat="server" DataTextField="IOTypeName" DataValueField="IOTypeId" ValidationGroup="frmIO">
                        </asp:DropDownList>
                    </td>
                </tr> 
                <tr>
                    <td align="right" style="height: 26px">
                        IO Status:
                    </td>
                    <td align="left" style="width: 407px; height: 26px;">
                        <asp:DropDownList ID="ddStatus" runat="server" DataTextField="IOStatusName" DataValueField="IOStatusId" ValidationGroup="frmIO">
                        </asp:DropDownList></td>
                </tr> 
                <tr>
                    <td align="right">
                        Unit Cost:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:TextBox ID="txtUnitCost" runat="server" Width="100px" MaxLength="50" ValidationGroup="frmIO"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td align="right">
                        Quanity:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:TextBox ID="txtQuanity" runat="server" Width="100px" MaxLength="50" ValidationGroup="frmIO"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td align="right">
                        Amount:
                    </td>
                    <td align="left" style="width: 407px">
                        <asp:TextBox ID="txtAmount" runat="server" Width="100px" MaxLength="50" ValidationGroup="frmIO"></asp:TextBox>
                    </td>
                </tr> 
                <tr>
                    <td align="right" style="height: 127px">
                        Notes:
                    </td>
                    <td align="left" style="width: 407px; height: 127px;">
                        <asp:TextBox ID="txtNote" runat="server" Width="378px" MaxLength="50" ValidationGroup="frmIO" Height="94px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>                 
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="cmdAddIO" runat="server" Text="Add IO" ValidationGroup="frmIO" />
                   </td>
                </tr>
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>              
            </table>            
        </div>
    </div>        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

