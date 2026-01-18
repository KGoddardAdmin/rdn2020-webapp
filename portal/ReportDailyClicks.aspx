<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ReportDailClick" title="Untitled Page" Codebehind="ReportDailyClicks.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                Daily Click Report
           </td>
        </tr>
        <tr><td  style="height:5px;">&nbsp;</td></tr>
        <tr>
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
                <asp:Button ID="cmdGetReport" runat="server" Text="Get Report" />
            </td>            
        </tr>
        <tr><td  style="height:5px;">&nbsp;</td></tr>
        <tr>
	        <td style="text-align:left;  padding-left: 2px; font-weight:bold;">                   
		        Click Total:&nbsp;&nbsp;<asp:Label ID="lblTotal" runat="server"></asp:Label>
	        </td>
	       
        </tr> 
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr><td style=" height:5px;">&nbsp;</td></tr>
                <tr>
                    <td>                          	            &nbsp;</td>
                </tr>
                <tr><td style=" height:5px;">&nbsp;</td></tr>
            </table>                     
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

