<%@ Page Title="" Language="VB" MasterPageFile="~/portal/Admin.master" AutoEventWireup="false" CodeFile="CorrectCampaignLinks.aspx.vb" Inherits="portal_CorrectCampaignLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
     <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 Correct Campaign Links
           </td>
        </tr>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mstMain" Runat="Server">
      <div class="mainform">
        <div class="mainformcontent">
           <div style="text-align:center";>
            <table width="50%">
                <tr><td style="height:15px";>&nbsp;</td></tr>
                <tr><td>
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                    </td></tr>
                <tr>
                    <td>                        
                        <asp:Label ID="Label1" runat="server" Text="ReSet Links"></asp:Label>
                        &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="cmdReset" runat="server" Text="ReSet" />
                        
                    </td>
                </tr>
                 <tr><td style="height:15px";>&nbsp;</td></tr>
            </table>
            </div>
        </div>
      </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

