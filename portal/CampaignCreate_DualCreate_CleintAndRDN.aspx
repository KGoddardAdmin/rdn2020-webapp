<%@ Page Title="" Language="VB" ValidateRequest ="false"   MasterPageFile="~/portal/Admin.master" AutoEventWireup="false" Inherits="portal_CampaignCreateForClientEmailReportSite" Codebehind="CampaignCreate_DualCreate_CleintAndRDN.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        New Email Report Site Campign
	            </td>
	        </tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
	<tr>
		<td colspan="2">
			<asp:Label ID="lblhmsg" runat="server"></asp:Label></td>
	</tr>
	<tr>
		<td colspan="2" style="height: 47px">
			<asp:ValidationSummary ID="vsCreative" runat="server" ValidationGroup="frmCampaign" />            
			<asp:RequiredFieldValidator ID="rfvCoupon" runat="server" ControlToValidate="txtCouponVariable"
				Display="None" Enabled="False" ErrorMessage="You must enter a coupon variable."
				ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="frvClient" runat="server" ControlToValidate="ddClient" Display="None" ErrorMessage="You must select a client" InitialValue="Select Client " ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvIO" runat="server" ControlToValidate="ddIO" Display="None" ErrorMessage="You must select an IO" InitialValue="Select IO" ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>			
			<asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtCampaignName"
				Display="None" ErrorMessage="Campiagn name is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvQuanity" runat="server" ControlToValidate="txtEmailQuanity" Display="None" ErrorMessage="You must enter the number of emails ordered." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvhtml" runat="server" ControlToValidate="txtHtml"
				Display="None" ErrorMessage="You must enter a creative." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="revCampaignName" runat="server" ControlToValidate="txtCampaignName"
				Display="None" ErrorMessage="Campiagnn Name must be alphanumeric." ValidationExpression="^[0-9a-zA-Z ]*$"
				ValidationGroup="frmCampaign"></asp:RegularExpressionValidator>
			<asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
				Display="None" ErrorMessage="Subject is required" ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>               
            </td>
	</tr>
	<tr>
		<td colspan="2">
			<table width="60%" align="center" style="border:solid 1px #424F99;">
				<tr>
					<td style="text-align:right; font-weight:bold; width:40%;">
						Campaign Type :	                    
					</td>
					<td colspan="2" style="text-align:left; font-weight:bold;">
						&nbsp;&nbsp;
						<asp:RadioButton ID="radRegYes" runat="server" GroupName="CampaignType" Text="Regular" AutoPostBack="True" />
						&nbsp;&nbsp;&nbsp;
						<asp:RadioButton ID="radRegNo" runat="server" GroupName="CampaignType" Text="Coupon " AutoPostBack="True" />
					</td>		
				</tr>
				<tr id="trcvar" runat = "server">
					<td style="text-align:right; font-weight:bold;">
						Coupon Variable : 
					</td>
					<td colspan="2" style="text-align: left; padding-left:2px; font-weight:bold;">
					    &nbsp;&nbsp;
						<asp:TextBox ID="txtCouponVariable" runat="server" Width="300px" ValidationGroup="frmCampaign"></asp:TextBox>                        	
					</td>	                		
				</tr>  
				<tr>
	                <td style="text-align:right; font-weight:bold;">
		                Days To Fullfill:
	                </td>
	                <td align="left">
	                    &nbsp;&nbsp;
		                <asp:DropDownList ID="ddfullfill" runat="server" AutoPostBack="True" 
			                ValidationGroup="frmIO">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem></asp:ListItem>
		                </asp:DropDownList>
	                </td>
                </tr>					
				<tr>
	                <td style="text-align:right; font-weight:bold; width:40%;">
		                Use Internal Open Link :	                    
	                </td>
	                <td colspan="2" style="text-align:left; font-weight:bold;">
		                &nbsp;&nbsp;
		                <asp:RadioButton ID="radInternalYes" runat="server" GroupName="Opentype" Text="Yes" AutoPostBack="True" />
		                &nbsp;&nbsp;&nbsp;
		                <asp:RadioButton ID="radInternalNo" runat="server" GroupName="Opentype" Text="No" AutoPostBack="True" />
	                </td>		
                </tr>	
                <tr>
					<td style="text-align:right; font-weight:bold;">
						rdn2020 Client:
					</td>
					<td  style="text-align: left; padding-left:2px;">
					    &nbsp;&nbsp;
						<asp:DropDownList ID="ddClient" runat="server" AutoPostBack="True" 
                            ValidationGroup="frmIO">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-left: 10px; font-weight:bold; width:15%;">
						rdn2020 IO:			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
					    &nbsp;&nbsp;
						<asp:DropDownList ID="ddIO" runat="server" AutoPostBack="True" ValidationGroup="frmCampaign">
						</asp:DropDownList>
					</td>
				</tr>				
                <tr><td colspan="2">&nbsp;</td></tr>
                <!------------------------------------>
                <tr>
					<td style="text-align:right; font-weight:bold;">
						rdn2020 Client's Client:
					</td>
					<td  style="text-align: left; padding-left:2px;">
					    &nbsp;&nbsp;
						<asp:DropDownList ID="ddClientsClient" runat="server" AutoPostBack="True" DataTextField="ClientName"
							DataValueField="ClientUId" ValidationGroup="frmIO">
						</asp:DropDownList>
					</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-left: 10px; font-weight:bold; width:15%;">
						rdn2020 Client's Client IO:			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
					    &nbsp;&nbsp;
						<asp:DropDownList ID="ddClientsClientIO" runat="server" AutoPostBack="True" 
                            ValidationGroup="frmCampaign">
						</asp:DropDownList>
					</td>
				</tr>                
                <tr>
					<td style="text-align:right; padding-left: 10px; font-weight:bold; width:15%;">
						rdn2020 Client's Friendly From:			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
					    &nbsp;&nbsp;						
					    <asp:TextBox ID="txtClientsFriendlyFrom" Width="300px" ValidationGroup="frmCampaign" runat="server"></asp:TextBox>						
					</td>
				</tr>
                 <tr>
					<td style="text-align:right; padding-left: 10px; font-weight:bold; width:15%;">
						rdn2020 Client's Notes:			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
					    &nbsp;&nbsp;						
					   <asp:TextBox ID="txtNote" runat="server" Height="73px" Width="300px" MaxLength="300" TextMode="MultiLine"></asp:TextBox> 
					</td>
				</tr>
                								
                <!------------------------------------>				
                <tr><td colspan="2">&nbsp;</td></tr>
			</table>
		</td>
	</tr>	 
	<tr>
		<td>
			<table width="96%" style="border:solid 1px #424F99;">
				<tr>
		            <td style="text-align: right; padding-left: 10px; font-weight:bold;">
			            Campaign Name:
		            </td>
		            <td style="text-align: left; padding-left:2px; font-weight:bold;">
			            <asp:TextBox ID="txtCampaignName" runat="server" Width="300px" ValidationGroup="frmCampaign"></asp:TextBox>                        
			                &nbsp;&nbsp;0-9 a-z only			
		            </td>
	            </tr>
	            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
	            <tr>
		            <td style="text-align: right; padding-left: 10px; font-weight:bold; height: 26px;">
			            Email Quanity:
		            </td>
		            <td style="text-align: left; padding-left:2px; font-weight:bold; height: 26px;">
			            <asp:TextBox ID="txtEmailQuanity" runat="server" Width="100px" ValidationGroup="frmCampaign"></asp:TextBox>
		            </td>
	            </tr>
	            
	            <tr>   
	                <td colspan="2" style=" height:10px;">&nbsp;</td>	                
	            </tr> 
                <tr>
	                <td style="text-align: right; padding-left: 10px; font-weight:bold; height: 26px;">
		                Clicks:
	                </td>
	                <td style="text-align: left; padding-left:2px; font-weight:bold; height: 26px;">
		                <asp:TextBox ID="txtClicks" runat="server" Width="100px" ValidationGroup="frmCampaign"></asp:TextBox>
	                </td>
				</tr>
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr> 
                <tr>
	                <td style="text-align: right; padding-left: 10px; font-weight:bold; height: 26px;">
		                Impressions:
	                </td>
	                <td style="text-align: left; padding-left:2px; font-weight:bold; height: 26px;">
		                <asp:TextBox ID="txtImpressions" runat="server" Width="100px" ValidationGroup="frmCampaign"></asp:TextBox>
	                </td>
                </tr>	            	            	            
	            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
	            <tr>
		            <td style="text-align: right; padding-left: 10px; font-weight:bold;">
			            Subject Line:
		            </td>
		            <td style="text-align: left; padding-left:2px; font-weight:bold;">
			            <asp:TextBox ID="txtSubject" runat="server" Width="300px" ValidationGroup="frmCampaign"></asp:TextBox>
		            </td>
	            </tr>
	            <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
	            </table>						
		</td>
		<td>
			<table width="96%" style="border:solid 1px #424F99;" height="100%">
				<tr>
					<td colspan="2" style="text-align:center; font-weight:bold;">Ad Info</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold;">
						Title:
					</td>
					<td>
						<asp:TextBox ID="txtETitle" runat="server" Width="300px"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold; height: 95px;">
						Description:
					</td>
					<td style="height: 95px">
						<asp:TextBox ID="txtEDescription" runat="server" Height="79px" TextMode="MultiLine"
							Width="300px"></asp:TextBox></td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold;">
						Display URL:
					</td>
					<td>
						<asp:TextBox ID="txtEDisplay" runat="server" Width="300px"></asp:TextBox>
					</td>
				</tr>
			</table>	
		</td>
	</tr>
	<tr>
		<td colspan="2">
			<table width="90%" align="center" style="border:solid 1px #424F99;">
				<tr>
					<td colspan="2" style="text-align:center; font-weight:bold;">
						Impression Info
					</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold;">
						Open Link 1:
					</td>
					<td>
						<asp:TextBox ID="txtOpenLink1" runat="server" Width="600px" AutoPostBack="True"></asp:TextBox></td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold; height: 95px;">
						Open Link 2:
					</td>
					<td style="height: 95px">
						<asp:TextBox ID="txtOpenLink2" runat="server" Width="600px" AutoPostBack="True" Enabled="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td style="text-align:right; padding-right:2px; font-weight:bold;">
						Open Link 3:
					</td>
					<td>
						<asp:TextBox ID="txtOpenLink3" runat="server" Width="600px" AutoPostBack="True" Enabled="False"></asp:TextBox>
				   </td>
				</tr>				
			</table>			
		</td>
	</tr>
	
	<tr>
		<td colspan="2">
            &nbsp;</td>                
	</tr>                 
	<tr>
		<td style="height: 18px">
			<asp:Label ID="Label2" runat="server" Text="Enter Creative" Font-Bold="True"></asp:Label></td>
		<td colspan="2" style="height: 18px">&nbsp;</td>
	</tr>
	<tr>
		<td colspan="2" style="height: 183px">
			<asp:TextBox ID="txtHtml" runat="server" Height="300px" TextMode="MultiLine" Width="98%" ValidationGroup="frmCampaign"></asp:TextBox>
		</td>
	</tr> 
	<tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
	<tr>
		<td colspan="2" style=" height:10px; padding-left:15px;">
			<asp:Button ID="cmdConvert" runat="server" Text="Convert Creative" />
		</td>
	</tr>
	<tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
	<tr>
		<td colspan="2">
			<asp:TextBox ID="txtConverted" runat="server" Width="98%" Height="300px" TextMode="MultiLine"></asp:TextBox>
		</td>
	</tr>
	<tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
	
	
</table>
	     </div>
    </div>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

