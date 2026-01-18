<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ClientNew" title="Untitled Page" Codebehind="ClientNew.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 New Client
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
                    <asp:ValidationSummary ID="vsClient" runat="server" ValidationGroup="frmClient" />
                    <asp:RequiredFieldValidator ID="rfvCompany" runat="server" ControlToValidate="txtCompanyName"
                        Display="None" ErrorMessage="Company name is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddr1"
                        Display="None" ErrorMessage="Address is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                        Display="None" ErrorMessage="City is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" ControlToValidate="ddState"
                        Display="None" ErrorMessage="State is required." InitialValue="0" ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvZip" runat="server" ControlToValidate="txtZip"
                        Display="None" ErrorMessage="Zip code is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>    
                    <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                        Display="None" ErrorMessage="Phone is required" ValidationGroup="frmClient"></asp:RequiredFieldValidator>                                            
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                        Display="None" ErrorMessage="Phone is not in proper format." ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}"
                        ValidationGroup="frmClient"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revZip" runat="server" ControlToValidate="txtZip"
                        Display="None" ErrorMessage="Zip code is not in proper format." ValidationExpression="\d{5}(-\d{4})?"
                        ValidationGroup="frmClient"></asp:RegularExpressionValidator>                                                                                                                                            
                    <asp:RegularExpressionValidator ID="revWebsite" runat="server" ControlToValidate="txtCompanyWebsite"
                        Display="None" ErrorMessage="Website is not in proper format." ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
                        ValidationGroup="frmClient"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvDomain" runat="server" ControlToValidate="txtDomain"
                        Display="None" ErrorMessage="Domain is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvClick" runat="server" ControlToValidate="txtClick"
                        Display="None" ErrorMessage="Click Page is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvOpen" runat="server" ControlToValidate="txtOpen"
                        Display="None" ErrorMessage="Open Page is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvCoupon" runat="server" ControlToValidate="txtCoupon"
                        Display="None" ErrorMessage="Coupon Page is required." ValidationGroup="frmClient"></asp:RequiredFieldValidator></td>
                        
            </tr>
            <tr>
                <td align="right">
                    Company Name:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtCompanyName" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="50" TabIndex="1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Address:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtAddr1" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="50" TabIndex="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Address 2:
                </td>
                <td align="left" >
                    <asp:TextBox ID="txtAddr2" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="50" TabIndex="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    City:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCity" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="50" TabIndex="4"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    State:
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddState" runat="server" DataTextField="StateName" DataValueField="StateId" ValidationGroup="frmClient" TabIndex="5">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Zip:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtZip" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="10" TabIndex="6"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Phone:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPhone" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="15" TabIndex="7"></asp:TextBox>
                    e.g.&nbsp;&nbsp;xxx-xxx-xxxx
                </td>
            </tr>
            <tr>
                <td align="right">
                    Fax:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFax" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="18" TabIndex="8"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Website:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCompanyWebsite" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="100" TabIndex="9"></asp:TextBox>
                </td>
            </tr>
             <tr>
	            <td align="right">
		            Domain:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtDomain" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="100" TabIndex="9"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Click Page:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtClick" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="100" TabIndex="9"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Open Page:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtOpen" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="100" TabIndex="9"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Coupon Page:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtCoupon" runat="server" Width="225px" ValidationGroup="frmClient" MaxLength="100" TabIndex="9"></asp:TextBox>
	            </td>
            </tr>
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="cmdAddNewCompany" runat="server" Text="Add New Company" ValidationGroup="frmClient" />
                </td>
            </tr> 
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>   
        </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

