<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ViewCreative" title="Untitled Page" Codebehind="ViewCreative.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 View Creatives
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
                    <td>
                        <asp:DropDownList ID="ddCreative" runat="server" DataTextField="CampaignId" DataValueField="CampaignId">
                        </asp:DropDownList>
                        <asp:Button ID="cmdGetCreative" runat="server" Text="Get Creative" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RequiredFieldValidator ID="rvfcreative" runat="server" ControlToValidate="ddCreative"
                            ErrorMessage="You must select a creative to view." InitialValue="Select Creative "></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCreative" runat="server" Height="354px" Width="98%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>                                  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

