<%@ Page Language="VB" ValidateRequest="false" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampiagnListSeed" title="Untitled Page" Codebehind="CampaignListSeed.aspx.vb" %>
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
                    <td style="padding-top:5px; padding-right:2px; height: 24px;">
                        Unsubscribe Link Settings :
                    </td>
                    <td style="height: 24px">
                        <asp:DropDownList ID="ddAlign" runat="server">
                            <asp:ListItem Value="0">Left</asp:ListItem>
                            <asp:ListItem Value="1">Center</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddsize" runat="server">
                            <asp:ListItem Value="0">X-Small</asp:ListItem>
                            <asp:ListItem Value="1">Small</asp:ListItem>
                            <asp:ListItem Value="2">Medium</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddColor" runat="server">
                            <asp:ListItem Value="0">Gray</asp:ListItem>
                            <asp:ListItem Value="1">Black</asp:ListItem>
                        </asp:DropDownList>
                         &nbsp;&nbsp;&nbsp;&nbsp;                         
                    </td>                                       
                </tr> 
                <tr>
	                <td style="text-align:right; padding-right:2px;">
		                Use :
	                </td>
	                    <td style="text-align:left; padding-left:2px;">
		                    <asp:RadioButton ID="radUnConverted" runat="server" GroupName="CreativeType" Text="UnConverted" AutoPostBack="True" />
                                &nbsp;
                            <asp:RadioButton ID="radConverted" runat="server" GroupName="CreativeType" Text="Converted " AutoPostBack="True" />
                                Creative in seed.
	                </td>
                </tr>
                <%If SeedType = "Coupon" Then%>
                <tr>
                    <td style="text-align:right; padding-right:2px;">
                        Coupon Variable :
                    </td>
                    <td style="text-align:left; padding-left:2px;">
                        <asp:TextBox ID="txtCouponVariable" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
	                <td style="text-align:right; padding-right:2px;">
		                Coupon Value :
	                </td>
	                <td style="text-align:left; padding-left:2px;">
		                <asp:TextBox ID="txtCouponValue" runat="server"></asp:TextBox>
	                </td>
                </tr>
                <%End If%>               
	            <tr>
	                <td colspan="2" align="left">
                        <asp:ValidationSummary ID="vsSeed" runat="server" ValidationGroup="frmSeed" />
                        <asp:RequiredFieldValidator ID="rfvTo" runat="server" ControlToValidate="txtTo" Display="None" ErrorMessage="Who the seed is to is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvFrom" runat="server" ControlToValidate="txtFrom" Display="None" ErrorMessage="From address is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" Display="None" ErrorMessage="Suject is required." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvCreative" runat="server" ControlToValidate="txtCreative" Display="None" ErrorMessage="You must enclude a creative for the seed." ValidationGroup="frmSeed"></asp:RequiredFieldValidator>&nbsp;&nbsp;
                    </td>
	            </tr> 
	            <tr id="trmsg" runat="server" visible = "false" >
	                <td colspan="2" align="left" style="height: 231px">
                        &nbsp;<asp:TextBox ID="txtMsg" runat="server" Height="219px" Width="402px" TextMode="MultiLine"></asp:TextBox></td>
	            </tr>                               
	            <tr>
		            <td  align="right" valign="top"  style="height: 249px"><br />To:</td>
		            <td align="left" style="width: 505px; height: 249px;">
		                 <span style="font-size:xx-small;">To send to multiple address, place each address on a new line</span><br />
			            <asp:TextBox ID="txtTo" runat="server" Width="491px" ValidationGroup="frmSeed" Height="225px" TextMode="MultiLine"></asp:TextBox>
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

