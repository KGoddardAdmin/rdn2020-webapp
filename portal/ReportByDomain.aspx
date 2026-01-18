<%@ Page Language="VB" Debug="true" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="Portal_ReportByDomain" title="Untitled Page" CodeFile="ReportByDomain.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table cellpadding="0" cellspacing="0" border="0" width="800">
	<tr>
		<td align="right" style="height: 23px">
            <asp:Button ID="cmdGetReport" runat="server" Text="Get Report" />&nbsp;&nbsp;
                Select A Client:<asp:DropDownList ID="ddlClient" runat="server" 
				Width="156px" DataTextField="ClientName" DataValueField="ClientUId">                                        
			</asp:DropDownList>
		</td>
		<td style=" text-align:left; padding-left:10px; height: 23px;">
			Select A Time Span:<asp:DropDownList ID="ddlGetTimeSpan" runat="server" 
				Width="156px">                                        
			</asp:DropDownList>
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
                    <td style="height: 10px;"></td>                            
                </tr>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cvError" runat="server" ErrorMessage=""></asp:CustomValidator>
                        <asp:GridView ID="gridCampaignReportWeekly" runat="server" AutoGenerateColumns="False"
                            Width="100%" PageSize="100">
                            <Columns>
                                <asp:TemplateField HeaderText="Campaign ID">
                                    <ItemTemplate>
                                        <asp:Label ID="CampaignId" runat="server" Text='<%# Eval("Campaign ID") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Campaign Name">
                                    <ItemTemplate>
                                       <asp:Hyperlink runat= "server" Text='<%# DataBinder.Eval(Container.DataItem,"Campaign Name").tostring%>'  
                                            NavigateUrl='<%# "ReportByDomainAdjustment.aspx?Name=" & DataBinder.Eval(Container.DataItem, "Campaign Name").ToString & _
                                                "&DomainID=" & DataBinder.Eval(Container.DataItem, "Domain Campaign Id").ToString & _
                                                 "&Domain=" & DataBinder.Eval(Container.DataItem, "[Domain Name]").ToString & _
                                                "&Opens=" & DataBinder.Eval(Container.DataItem, "[Domain Opens]").ToString & _
                                                "&Clicks=" & DataBinder.Eval(Container.DataItem, "[Domain Clicks]").ToString%>' ID="CampaignName" Target="_blank"/>   
                                    </ItemTemplate>   
                            
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="List Size">
                                    <ItemTemplate>
                                        <asp:Label ID="Quanity" runat="server" Text='<%# Eval("List Size") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Opens">
                                    <ItemTemplate>
                                        <asp:Label ID="Opens" runat="server" Text='<%# Eval("Opens") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total Clicks">
                                    <ItemTemplate>
                                        <asp:Label ID="Clicks" runat="server" Text='<%# Eval("Clicks") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Domain">
                                    <ItemTemplate>
                                        <asp:Label ID="Domain" runat="server" Text='<%# Eval("[Domain Name]") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Domain Opens">
                                    <ItemTemplate>
                                        <asp:Label ID="Domain" runat="server" Text='<%# Eval("[Domain Opens]") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Domain Clicks">
                                    <ItemTemplate>
                                        <asp:Label ID="Domain" runat="server" Text='<%# Eval("[Domain Clicks]") %>'>                                                
                                        </asp:Label>
                                    </ItemTemplate>   
                                </asp:TemplateField>
                                
                            </Columns>
                            <HeaderStyle BackColor="#6D7171" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#DDDDDD" ForeColor="#333333" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px;">
                        <asp:Label ID="Label1" runat="server"></asp:Label>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
     </div>        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

