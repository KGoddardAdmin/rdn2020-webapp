<%@ Page Language="VB" MasterPageFile="Admin.master" Debug="true"  AutoEventWireup="false" Inherits="Reportlinkbycampaign" title="Untitled Page" Codebehind="Reportlinkbycampaign.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
        <table width="100%" cellpadding="0" cellspacing="0" >
            <tr>
                <td>
                    Link Report
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
                <td>
                    <asp:Label ID="lblhmsg" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
    <div class="mainform">
        <div class="mainformcontent">
            <table width="100%">
                <tr><td style=" height:5px;">&nbsp;<asp:Label ID="lblmsg" runat="server"></asp:Label></td></tr>
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
                            <asp:BoundField DataField="ClickCount" HeaderText="Total Clicks" />
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
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

