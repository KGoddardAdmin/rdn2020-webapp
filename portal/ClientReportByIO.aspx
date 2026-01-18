<%@ Page Language="VB" Debug="true" AutoEventWireup="false" Inherits="ClientReportByIO" Codebehind="ClientReportByIO.aspx.vb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        IO Client Report
	            </td>
	        </tr>
	        <tr>
                    <td colspan="2">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></td>
                </tr>              
                <tr>
	                <td style="text-align: right; width:12%; padding-right: 2px; font-weight:bold;">                    
		                Client:
	                </td>
	                <td style="text-align: Left; padding-left: 2px;">
		                <asp:Label ID="lblClient" runat="server"></asp:Label>
	                </td>
                </tr> 
                <tr>
	                <td style="text-align: right; padding-right: 2px; font-weight:bold;">                   
		                IO:
	                </td>
	                <td style="text-align: Left; padding-left: 2px;">
		                <asp:Label ID="lblIO" runat="server"></asp:Label>
	                </td>
                </tr> 
	        <tr>
	        <td>&nbsp;</td>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                <asp:DropDownList ID="ddMonth" runat="server">
                    <asp:ListItem Value="1">January</asp:ListItem>
                    <asp:ListItem Value="2">Feburary</asp:ListItem>
                    <asp:ListItem Value="3">March</asp:ListItem>
                    <asp:ListItem Value="4">April</asp:ListItem>
                    <asp:ListItem Value="5">May</asp:ListItem>
                    <asp:ListItem Value="5">June</asp:ListItem>
                    <asp:ListItem Value="7">July</asp:ListItem>
                    <asp:ListItem Value="8">August</asp:ListItem>
                    <asp:ListItem Value="9">September</asp:ListItem>
                    <asp:ListItem Value="10">October</asp:ListItem>
                    <asp:ListItem Value="11">November</asp:ListItem>
                    <asp:ListItem Value="12">December</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Button ID="Button1" runat="server" Text="Get Report" />
            </td>            
        </tr>
         <tr>
	                <td style="text-align: right; padding-right: 2px; font-weight:bold;">                   
                        <asp:Label ID="lblTotal" runat="server"></asp:Label></td>
	                <td style="text-align: Left; padding-left: 2px;">
		                <asp:Label ID="txtTotal" runat="server"></asp:Label>
	                </td>
                </tr> 
	    <tr><td colspan="2" style="height:5px;">&nbsp;</td></tr>	   
                <tr id="trclick" runat="server">
                    <td colspan="2">
                        <asp:GridView ID="gridReport" OnRowCommand ="GetReport" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:ButtonField  CommandName="GetId"  HeaderText="Link Report" Text="View" />
                                <asp:BoundField DataField="CampaignName" HeaderText="Campaign Name" />
                                <asp:BoundField DataField="CampaignId" HeaderText="Campaign Id" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ClickCount" HeaderText="Total Clicks" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="TotalClicks" runat="server" Text='<%# GetTotalClicks(Eval("ClickCount")) %>'>                                                
								        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                            <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />
	                        <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
	                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>                        
                    </td>
                </tr>               
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gridImpressions" runat="server" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="ImpressionCampaignName" HeaderText="Campaign Name" />
                                <asp:BoundField DataField="ImpressionCampaignId" HeaderText="Campaign Id" />
                                <asp:BoundField DataField="Opens" HeaderText="Total Opens" />
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
	                                    <asp:Label ID="TotalOpens" runat="server" Text='<%# GetTotalOpens(Eval("Opens")) %>'>                                                
	                                        </asp:Label>
                                        </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                             <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />
	                        <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
	                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                    </td>
                </tr>	   
<tr id="trimpression" runat="server">
	<td colspan="2">
        &nbsp;</td>
</tr>
<tr><td style=" height:5px;">&nbsp;</td></tr> 	  	        	             	        	        
        </table>
    </div>
    </form>
</body>
</html>
