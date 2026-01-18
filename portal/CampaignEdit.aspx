<%@ Page Language="VB" Debug="true" ValidateRequest="false"   MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="CampaignEdit" title="Untitled Page" Codebehind="CampaignEdit.aspx.vb" %>

<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker"
    TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <div style="text-align: left; padding-left: 10px; font-weight:bold; padding-bottom:10px;">
        Edit Campaign
    </div>						
	<table width="90%" align="center" cellpadding="0" cellspacing="0" style="border:solid 1px #424F99;">	
	<tr>
		<td>
			<table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
			    <tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>	    				
				<tr>
					<td colspan="2" style="height: 16px">
						<asp:Label ID="lblHeading" runat="server"></asp:Label>
					</td>                
				</tr>
				<tr>
					<td align="right" style="width:40%; padding-right:5px; height: 24px;">
						From Active IO/Campaigns Created Within Last :
					</td>
					<td align="left" style="height: 24px" >
						<asp:DropDownList ID="ddTime" runat="server" TabIndex="1" Width="175px" AutoPostBack="True">
							<asp:ListItem Value="0">Last Week</asp:ListItem>
							<asp:ListItem Value="1">Last 30 Days</asp:ListItem>
							<asp:ListItem Value="2">Current Month</asp:ListItem>
							<asp:ListItem Value="3">Last Month</asp:ListItem>
							<asp:ListItem Value="4">Custom Dates</asp:ListItem>
						</asp:DropDownList>
					</td>			
				</tr>
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
				<tr id="trDate" runat="server" visible = "false" style="padding-left:50px;">
					<td align="right">
					Start Date: &nbsp;&nbsp;
						<cc1:DatePicker ID="DatePicker1" runat="server" />
					&nbsp;&nbsp;
					</td>
					<td align="left" style="margin-left:0;">
					End Date:&nbsp;&nbsp;                             
						<cc1:DatePicker ID="DatePicker2" runat="server" />
					</td>
				</tr>
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
				<tr>
					<td align="right" style="width:40%; padding-right:5px; height: 24px;">
						Client :			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold; height: 22px;">
						<asp:DropDownList ID="ddClient" runat="server" AutoPostBack="True" ValidationGroup="frmCampaign" DataTextField="ClientName" DataValueField="ClientUId" Width="175px">
						</asp:DropDownList>
					</td>
				</tr>      
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>  	
				<tr>
					<td style="text-align:right; padding-left: 10px;  width:15%;">
						IO:			
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
						<asp:DropDownList ID="ddIO" runat="server" AutoPostBack="True" ValidationGroup="frmCampaign" DataTextField="IoName" DataValueField="IoUId">
						</asp:DropDownList>
					</td>
				</tr>
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
				<tr>
					<td align="right" style="width:15%; padding-right:2px; height: 24px;">
						Campaign To Edit :
					</td>
					<td align="left" style="height: 24px">
						<asp:DropDownList ID="ddCampaign" runat="server" AutoPostBack="True" DataTextField="CampaignName"
							DataValueField="CampaignId" Width="177px">
						</asp:DropDownList>
						<asp:RadioButton ID="radActive" runat="server" GroupName="CampaignStatus" Text="Active" AutoPostBack="True" />
						&nbsp;
						<asp:RadioButton ID="radNon" runat="server" GroupName="CampaignStatus" Text="Non Active" AutoPostBack="True" /></td>
				</tr> 
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>
				<tr>
					<td align="right" style="width:15%; padding-right:2px;">
						Campaign Id:
					</td>
					<td style="text-align: left; padding-left:2px; font-weight:bold;">
						<asp:TextBox ID="txtCampaignId" runat="server" Width="103px" ValidationGroup="frmCampaign"></asp:TextBox>
						&nbsp;&nbsp;
						<asp:Button ID="cmdSearch" runat="server" Text="Find Campaign" /></td>
				</tr>  
				<tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>          
			</table>			
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
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>
                </tr>
			    <tr>
	                <td colspan="2" style="height: 47px">
                        <asp:ValidationSummary ID="vsCreative" runat="server" ValidationGroup="frmCampaign" />
                            &nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtCampaignName"
                            Display="None" ErrorMessage="Campiagn name is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvQuanity" runat="server" ControlToValidate="txtEmailQuanity" Display="None" ErrorMessage="You must enter the number of emails ordered." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddStatus"
                            Display="None" ErrorMessage="You must select the status" InitialValue="Select Status "
                            ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvCoupon" runat="server" ControlToValidate="txtCouponVariable"
				            Display="None" Enabled="False" ErrorMessage="You must enter a coupon variable."
				            ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvFullfull" runat="server" ControlToValidate="txtFullFill"
                            Display="None" ErrorMessage="A fullfillment date is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                            Display="None" ErrorMessage="Subject line is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>&nbsp;
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ControlToValidate="txtETitle"
                            Display="None" ErrorMessage="Ad title is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtEDiscription"
                            Display="None" ErrorMessage="Ad description is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvdisplay" runat="server" ControlToValidate="txtEDisplay"
                            Display="None" ErrorMessage="Ad display url is required." ValidationGroup="frmCampaign"></asp:RequiredFieldValidator></td>
	            </tr>	                             		             	            	           	            	        
	            <tr>
	                <td valign="top">
	                    <table width="100%" height="100%" style="border:solid 1px #424F99;">
	                        <tr>
	                            <td>
	                                <table width="100%" height="100%">
	                                    <tr>
					                        <td style="text-align:right; font-weight:bold; width:40%;">
						                        Campaign Type :	                    
					                        </td>
					                        <td style="text-align:left; font-weight:bold;">						                        
						                        <asp:RadioButton ID="radRegYes" runat="server" GroupName="CampaignType" Text="Regular" AutoPostBack="True" />
						                        &nbsp;&nbsp;&nbsp;
						                        <asp:RadioButton ID="radRegNo" runat="server" GroupName="CampaignType" Text="Coupon " AutoPostBack="True" />
                                                <asp:HiddenField ID="hfType" runat="server" />
					                        </td>		
				                        </tr>
				                        <tr id="trcvar" runat = "server">
					                        <td style="text-align:right; font-weight:bold;">
						                        Coupon Variable : 
					                        </td>
					                        <td style="text-align: left; padding-left:2px; font-weight:bold;">					                            
						                        <asp:TextBox ID="txtCouponVariable" runat="server" Width="300px" ValidationGroup="frmCampaign"></asp:TextBox>                        	
					                        </td>	                		
				                        </tr>  
	                                    <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
			                                    Campaign Name:
		                                    </td>
		                                    <td style="text-align: left; padding-left:2px; font-weight:bold;">
                                                <asp:TextBox ID="txtCampaignName" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
                                            </td>	                                        
	                                    </tr>
	                                    <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Full Fill By:
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
		                                        <asp:TextBox ID="txtFullFill" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                                        </td>
                                        </tr>
	                                    <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Email Quanity:
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
		                                        <asp:TextBox ID="txtEmailQuanity" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
		                                    </td>
                                        </tr>
                                        <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Clicks:
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
		                                        <asp:TextBox ID="txtClicks" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                                        </td>
                                        </tr>
                                        <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Impressions:
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
		                                        <asp:TextBox ID="txtImpressions" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                                        </td>
                                        </tr>
                                        <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Subject Line:
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
		                                        <asp:TextBox ID="txtSubject" runat="server" Width="320px" ValidationGroup="frmCampaign"></asp:TextBox>
	                                        </td>
                                        </tr>
                                        <tr>
	                                        <td style="text-align:right; font-weight:bold; width:40%;">
		                                        Use Internal Open Link :	                    
	                                        </td>
	                                        <td colspan="2" style="text-align:left; font-weight:bold;">		                                        
		                                        <asp:RadioButton ID="radInternalYes" runat="server" GroupName="Opentype" Text="Yes" AutoPostBack="True"  />
		                                        &nbsp;&nbsp;&nbsp;
		                                        <asp:RadioButton ID="radInternalNo" runat="server" GroupName="Opentype" Text="No" AutoPostBack="True" />
	                                        </td>		
                                        </tr>	
                                        <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Is Active :
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
                                            <asp:CheckBox ID="ckIsActive" runat="server" /></td>
                                        </tr>
                                        <tr>
	                                        <td style="text-align: right; padding-left: 10px; font-weight:bold;">
		                                        Status :
	                                        </td>
	                                        <td style="text-align: left; padding-left:2px; font-weight:bold;">
                                                <asp:DropDownList ID="ddStatus" runat="server" DataTextField="CampaignStatus" DataValueField="CampaignStatusId">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>  
	                                </table>	                                
	                            </td>
	                        </tr>
	                    </table>
	                </td>
	                <td>
	                    <table width="96%" style="border:solid 1px #424F99;">
	                        <tr>
	                            <td>
	                                <table width="100%" height="100px">
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
                                                <asp:TextBox ID="txtEDiscription" runat="server" Height="79px" TextMode="MultiLine"
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
                                    <asp:HiddenField ID="hfHasAd" runat="server" />
	                            </td>
	                        </tr>
	                    </table>	        
	                    
	                               	                    
	                </td>		            
	            </tr>
	             <tr>
	             
	                <td colspan="2">
	                    
	                    
	                    <table width="96%" style="border:solid 1px #424F99;">
	                        <tr>
	                            <td>
	                                <table width="100%">
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
			                                    <asp:TextBox ID="txtOpenLink1" runat="server" Width="600px" AutoPostBack="True"></asp:TextBox>
			                                </td>
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
		                                    <td style="text-align:right; padding-right:2px; font-weight:bold; height: 17px;">
			                                    Open Link 3:
		                                    </td>
		                                    <td style="height: 17px">
			                                    <asp:TextBox ID="txtOpenLink3" runat="server" Width="600px" AutoPostBack="True" Enabled="False"></asp:TextBox>
			                                </td>
	                                    </tr>
	                                </table>
	                            </td>
	                        </tr>
	                    </table>	                    
	            
	                	                
	                </td>	
	            </tr>            	            	            	            	                                                                        
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Enter Creative" Font-Bold="True"></asp:Label></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 183px">
                        <asp:TextBox ID="txtHtml" runat="server" Height="300px" TextMode="MultiLine" Width="98%" ValidationGroup="frmCampaign"></asp:TextBox>
                    </td>
                </tr> 
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
                <tr>
                    <td style=" padding-left:15px;">
                        <asp:Button ID="cmdUpdate" runat="server" Text="Update Campaign" />
                    </td>
                    <td>
                        <asp:Button ID="cmdDeltet" runat="server" Text="Delete Campaign" /></td>
                </tr>
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
                <tr>
	                <td>
		                <asp:Label ID="Label1" runat="server" Text="Converted Creative" Font-Bold="True"></asp:Label></td>
	                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtConverted" runat="server" Width="98%" Height="300px" TextMode="MultiLine"></asp:TextBox></td>
                </tr>
                <tr><td colspan="2" style=" height:10px;">&nbsp;</td></tr>
                 
		    </table>
	    </div>
    </div>                                 	           
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

