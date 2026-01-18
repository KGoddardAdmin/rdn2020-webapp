<%@ Page Title="" Language="VB" MasterPageFile="~/portal/Admin.master" AutoEventWireup="false" CodeFile="ClientCampaignNames.aspx.vb" Inherits="portal_ClientCampaignNames" %>
<%@ Register Assembly="EclipseWebSolutions.DatePicker" Namespace="EclipseWebSolutions.DatePicker" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
     <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
            <tr><td style="height:10px;">&nbsp;</td></tr>
	        <tr>
		        <td style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Client's Campaign Names                   
	            </td>
	        </tr>
            <tr><td style="height:10px;">&nbsp;</td></tr>
            <tr>
		        <td style="text-align: left; padding-left: 10px; font-weight:bold;">			        
	                Clients : &nbsp;&nbsp;<asp:DropDownList 
                        ID="ddClient" runat="server" DataTextField="ClientName" 
                        DataValueField="ClientUId">
                    </asp:DropDownList>
	            </td>
	        </tr>
            <tr><td style="height:10px;">&nbsp;</td></tr>
            <tr>
                <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                     Date: <cc1:DatePicker ID="DatePicker1" runat="server" />
                     <br /><br />
                     <asp:Button ID="cmdGetReport" runat="server" Text="Get Names" />                     
                </td>
            </tr>	        
            <tr><td style="height:10px;">&nbsp;</td></tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr><td style=" height:5px;">&nbsp;</td></tr>  
                <tr><td>
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                    </td></tr>  
                    
                <tr id="trgrid" runat="server" visible="true">
                    <td>                        
                        <asp:label id="RecordCount" runat="server" />
                        <asp:DataGrid ID="dgCampaigns" runat="server" AutoGenerateColumns="False" 
                            Width="100%" PageSize="500" DataKeyField="CampaignId">
                            <Columns>
                                <asp:BoundColumn DataField="CampaignId" HeaderText="Campaign Id"></asp:BoundColumn>
                                <asp:BoundColumn DataField="CampaignName" HeaderText="Campaign Name"></asp:BoundColumn>                                
                                <asp:BoundColumn DataField="BroadCastDate" HeaderText="Broadcast Date"></asp:BoundColumn>
                            </Columns>
                             <HeaderStyle BackColor="#424FA3" Font-Bold="True" ForeColor="White" />	                        
	                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
	                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                            <ItemStyle BackColor="#DDDDDD" ForeColor="#333333" />
                            <AlternatingItemStyle BackColor="White" ForeColor="#284775" />
                        </asp:DataGrid>                        
                    </td>
                </tr>
            </table>                     
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

