<%@ Page Language="VB" MasterPageFile="Admin.master" AutoEventWireup="false" Inherits="ViewErrors" title="Untitled Page" ValidateRequest="false" Codebehind="ViewErrors.aspx.vb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mstHeader" Runat="Server">
    <div id="ContentHeader">
    <table width="100%" cellpadding="0" cellspacing="0" class="toolbar">
        <tr>
            <td style="text-align: left; padding-left: 10px; font-weight:bold;">
                 View Errors
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
                        <asp:TextBox ID="txtmsg" runat="server" Height="354px" Width="98%" TextMode="MultiLine"></asp:TextBox>            
                    </td>
                </tr>
                <tr>
                    <td>                        
                        <asp:Button ID="cmdClear" runat="server" Text="Next Error" />&nbsp;
                        <asp:HiddenField ID="ErrorId" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
     </div>
        
                
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mstFooter" Runat="Server">
</asp:Content>

