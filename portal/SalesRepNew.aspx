<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="SalesRepNew" title="Untitled Page" Codebehind="SalesRepNew.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                New Sales Representive
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
            <tr><td colspan="2" style=" height:10px;">
                    <asp:ValidationSummary ID="vsSales" runat="server" ValidationGroup="frmSalesRep" />
                
                    <asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="txtFName"
                        Display="None" ErrorMessage="First name is required." ValidationGroup="frmSalesRep"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="txtLName"
                        Display="None" ErrorMessage="Last name is required." ValidationGroup="frmSalesRep"></asp:RequiredFieldValidator> 
                    <asp:RequiredFieldValidator ID="rfvBdate" runat="server" ControlToValidate="txtBDate" Display="None" ErrorMessage="Birth date is required" ValidationGroup="frmSalesRep"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator id="rfvSS" runat="server" ControlToValidate="txtTaxId" Display="None" ErrorMessage="Social security number is required." ValidationGroup="frmSalesRep"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revSS" runat = "server" ControlToValidate="txtTaxId" Display="None" ErrorMessage="Social security number not in proper format." ValidationExpression="\d{3}-\d{2}-\d{4}" ValidationGroup="frmSalesRep"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvAddr" ControlToValidate ="txtAddr1" ValidationGroup ="frmSalesRep" ErrorMessage="Address is required."
                        display="none" runat="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity"
                        Display="None" ErrorMessage="City is required." ValidationGroup="frmSalesRep"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvState" ControlToValidate="ddState" InitialValue ="0" ValidationGroup="frmSalesRep"
                        errormessage="You must select a state" Display="none" runat="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvZip" Display="none" ValidationGroup="frmSalesRep" ControlToValidate="txtZip"
                        errormessage="Zip code is required." runat="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvPhone" Display="none" ControlToValidate ="txtPhone" ValidationGroup="frmSalesRep"
                        errormessage="Phone is required." runat="server"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvEmail" Display="none" ControlToValidate="txtEmail" ValidationGroup="frmSalesRep"
                        errormessage="Email is required." runat="server"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revBday" runat="server" ControlToValidate="txtBDate" Display="None" ErrorMessage="Birth date not in proper format." ValidationExpression="(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d" ValidationGroup="frmSalesRep"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revZip" runat="server" Display="None" ValidationExpression="\d{5}(-\d{4})?" ValidationGroup="frmSalesRep" ControlToValidate="txtZip" ErrorMessage="Zip code is not in proper format."></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" Display="None" ErrorMessage="Phone is not in proper format" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ValidationGroup="frmSalesRep"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revCell" runat ="server" ControlToValidate="txtCell" Display="None" ErrorMessage="Cell phone is not in proper format." ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ValidationGroup="frmSalesRep"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="None" ErrorMessage="Email is not in proper format." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="frmSalesRep"></asp:RegularExpressionValidator>                                       
                </td>
            </tr>
            <tr>
	            <td align="right">
		            First Name:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtFName" runat="server" Width="223px" MaxLength="50" ValidationGroup="frmSalesRep" TabIndex="1"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            MI:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtMI" runat="server" Width="22px" MaxLength="1" ValidationGroup="frmSalesRep" TabIndex="2"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Last Name:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtLName" runat="server" Width="223px" MaxLength="50" ValidationGroup="frmSalesRep" TabIndex="3"></asp:TextBox>
	            </td>
            </tr>
            <tr>
                <td align="right" >
                    Birth Date:
                </td>
                <td >
                    <asp:TextBox ID="txtBDate" runat="server" Width="223px" MaxLength="10" ValidationGroup="frmSalesRep" TabIndex="4"></asp:TextBox>
                    e.g.&nbsp;&nbsp;mm/dd/yyyy                                 
                </td>
            </tr>
            <tr>
	            <td align="right" >
		            Social Security #:
	            </td>
	            <td style="height: 21px">
		            <asp:TextBox ID="txtTaxId" runat="server" Width="223px" MaxLength="11" ValidationGroup="frmSalesRep" TabIndex="5"></asp:TextBox>                                
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Address:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtAddr1" runat="server" Width="223px" MaxLength="50" ValidationGroup="frmSalesRep" TabIndex="6"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Address 2:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtAddr2" runat="server" Width="223px" MaxLength="50" ValidationGroup="frmSalesRep" TabIndex="7"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            City:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtCity" runat="server" Width="223px" MaxLength="50" ValidationGroup="frmSalesRep" TabIndex="8"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            State:
	            </td>
	            <td align="left" >
                    &nbsp;<asp:DropDownList ID="ddState" runat="server" DataTextField="StateName" DataValueField="StateId" TabIndex="9">
                    </asp:DropDownList></td>
            </tr>
            <tr>
	            <td align="right">
		            Zip:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtZip" runat="server" Width="223px" MaxLength="10" ValidationGroup="frmSalesRep" TabIndex="10"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Phone:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtPhone" runat="server" Width="223px" MaxLength="15" ValidationGroup="frmSalesRep" TabIndex="11"></asp:TextBox>
		            e.g.&nbsp;&nbsp;xxx-xxx-xxxx
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Cell:
	            </td>
	            <td align="left">
		            <asp:TextBox ID="txtCell" runat="server" Width="223px" MaxLength="15" ValidationGroup="frmSalesRep" TabIndex="12"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Fax:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtFax" runat="server" Width="223px" MaxLength="18" ValidationGroup="frmSalesRep" TabIndex="13"></asp:TextBox>
	            </td>
            </tr>
            <tr>
	            <td align="right">
		            Email:
	            </td>
	            <td align="left" >
		            <asp:TextBox ID="txtEmail" runat="server" Width="223px" MaxLength="100" ValidationGroup="frmSalesRep" TabIndex="14"></asp:TextBox>
	            </td>
            </tr>
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
            <tr>
	            <td>&nbsp;</td>		           
		        <td>
                    <asp:Button ID="cmdAddSalesRep" runat="server" Text="Add Sales Rep" /></td>
            </tr> 
            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
        </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

