<%@ Page Language="VB" AutoEventWireup="false" Inherits="ClientReportByLink" Codebehind="ClientReportByLink.aspx.vb" %>

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
		        <td style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Client Campaign Report By Link
	            </td>
	        </tr>
	        <tr>
	            <td>
                    <asp:GridView ID="gridreport" runat="server" AutoGenerateColumns="False" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Link">
                                 <ItemTemplate>
                                        <asp:Label ID="Link" runat="server" Text='<%# GetLink(Eval("LinkId")) %>'>                                                
								        </asp:Label>
								 </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TotalClicks" HeaderText="Total Clicks" />
                        </Columns>
                        <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />
	                        <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
	                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                    </asp:GridView>
	            
	            </td>	            
	        </tr>	        	        	           	        	        
        </table>        
    </div>
    </form>
</body>
</html>
