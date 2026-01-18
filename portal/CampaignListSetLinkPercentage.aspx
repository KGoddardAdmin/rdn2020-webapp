<%@ Page Language="VB" Debug="true" MasterPageFile="Admin.master" AutoEventWireup="false" CodeFile="CampaignListSetLinkPercentage.aspx.vb" Inherits="CampaignListSetLinkPercentage" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
	        <tr>
		        <td colspan="2" style="text-align: left; padding-left: 10px; font-weight:bold;">
			        Campign Set Link Percentages
	            </td>
	        </tr>
	        <tr><td colspan="2" style=" height:15px;">&nbsp;</td></tr>	        
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <asp:Label ID="lblmsg" runat = "server"></asp:Label>  
            <br />          
            <table width="100%">
                <tr>
                    <td><%=converted %></td>
                    <td style="width:48%; vertical-align:top;">                        
                        <asp:GridView ID="gridLinks" runat="server" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Link Id">
                                     <ItemTemplate>
                                 <asp:CheckBox runat="server" ID="RowLevelCheckBox" Text ='<%# GetId(Eval("LinkId")) %>' />
                             </ItemTemplate>
                             <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Percentage">
                                    <ItemTemplate>
	                                    <asp:TextBox ID="txtPercent" runat="server" Width="25px" MaxLength="3" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Click Count">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtClicks" runat="server" Width="150px" MaxLength="18" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="cmdReset" runat="server" Text="Reset Percents" />
                        <asp:Button ID="cmdConnect" runat="server" Text="Set Percentages" />
                       </td>
                </tr>
        </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

