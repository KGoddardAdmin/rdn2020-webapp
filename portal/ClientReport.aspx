<%@ Page Language="VB" Debug="true" AutoEventWireup="false" Inherits="ClientReport" Codebehind="ClientReport.aspx.vb" %>

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
			        Client Report
	            </td>
	        </tr>	        	        	           	        	        
        </table>        
    </div>
     <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">                  
                <tr>
                    <td>
                        <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td>
                </tr> 
                   <tr><td style=" height:10px;">&nbsp;</td></tr> 
                <tr>
                    <td>
                        <asp:GridView ID="gridClientIO" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="IOId" HeaderText="ID" />
                                <asp:BoundField DataField="IOName" HeaderText="Name" />
                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" />
                                <asp:BoundField DataField="ContactName" HeaderText="Contact Name" />
                                <asp:BoundField DataField="SalesRepName" HeaderText="Sales Rep Name" />
                                <asp:BoundField DataField="IOStatusName" HeaderText="IO Status" />
                                <asp:BoundField DataField="IOTypeName" HeaderText="IO Type" />
                                <asp:BoundField DataField="IOUnitCost" HeaderText="Unit Cost" />
                                <asp:BoundField DataField="IOQuanity" HeaderText="Quanity" />
                                <asp:HyperLinkField DataNavigateUrlFields="ClientName,IOUId,StartDate,IOId,IOTypeName" DataNavigateUrlFormatString="ClientReportByIO.aspx?Client={0}&amp;IO={1}&amp;Start={2}&amp;IOId={3}&amp;TypeName={4}"
                                    HeaderText="IO Report" NavigateUrl="~/ClientReportByIO.aspx" Text="View Report" />
                            </Columns>
                            <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />
	                        <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
	                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>                                            
                    </td>
                </tr>  
                <tr><td style=" height:10px;">&nbsp;</td></tr>  
            </table>
        </div>
    </div>                                         
    </form>
</body>
</html>
