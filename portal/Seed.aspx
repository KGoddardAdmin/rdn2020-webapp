<%@ Page Language="VB" ValidateRequest ="false" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="Seed" title="Untitled Page" Codebehind="Seed.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Send Seed
	            </td>
	        </tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
     <div class="mainform">
        <div class="mainformcontent">
            <table>
	            <tr>
	                <td colspan="2" align="left">
                        <asp:ValidationSummary ID="vsSeed" runat="server" ValidationGroup="frmSeed" />
                        <asp:RequiredFieldValidator ID="rfvTo" runat="server" ControlToValidate="txtTo" Display="None" ErrorMessage="Who the seed is to is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvFrom" runat="server" ControlToValidate="txtFrom" Display="None" ErrorMessage="From address is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" Display="None" ErrorMessage="Suject is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revTo" runat="server" ControlToValidate="txtTo" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="frmSeed"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvCreative" runat="server" ControlToValidate="txtCreative" Display="None" ErrorMessage="You must enclude a creative for the seed." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>	            
	                </td>
	            </tr> 
	            <tr>
	                <td colspan="2" align="left">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>
	            </tr>                               
	            <tr>
		            <td  align="right">To:</td>
		            <td align="left" style="width: 505px">
			            <asp:TextBox ID="txtTo" runat="server" Width="491px" ValidationGroup="frmSeed"></asp:TextBox>
			        </td>
	            </tr>  
	            <tr>
	                <td  align="right">Friendly To:</td>
	                <td align="left" style="width: 505px">
		                <asp:TextBox ID="txtFriendlyTo" runat="server" Width="491px" ValidationGroup="frmSeed"></asp:TextBox>
	                </td>
                </tr>                    
	            <tr>
		            <td  align="right">From:</td>
		            <td align="left" style="width: 505px">
			            <asp:TextBox ID="txtFrom" runat="server" Width="488px" ValidationGroup="frmSeed"></asp:TextBox>
			        </td>
	            </tr> 
	            <tr>
	                <td  align="right">Friendly From:</td>
	                <td align="left" style="width: 505px">
		                <asp:TextBox ID="txtFriendlyFrom" runat="server" Width="488px" ValidationGroup="frmSeed"></asp:TextBox>
	                </td>
                </tr>                      
	            <tr>
		            <td  align="right">Subject:</td>
		            <td align="left" style="width: 505px">
			            <asp:TextBox ID="txtSubject" runat="server" Width="489px" ValidationGroup="frmSeed"></asp:TextBox>
			        </td>
	            </tr>                                     	
	            <tr>
		            <td colspan="2" style="height: 396px">
			            <asp:TextBox ID="txtCreative" runat="server" Height="383px" TextMode="MultiLine"
				            Width="747px" ValidationGroup="frmSeed"></asp:TextBox>
		            </td>
	            </tr> 
	            <tr>
	                <td>&nbsp;</td>
	                <td align="left">
                        <asp:Button ID="cmdSendMail" runat="server" Text="Send Seed" />
                    </td>
	            </tr>
            </table>                                     
        </div>
     </div>                       
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

