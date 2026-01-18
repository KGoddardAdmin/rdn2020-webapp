<%@ Page Title="" Language="VB" AutoEventWireup="false" Inherits="remove" Codebehind="remove.aspx.vb" %>

<head>
</head>

<body>
    <div class="singlecol" >To Be removed please submit your email address here:

<%--<form action="http://cl.exct.net/subscribe.aspx?lid=2080916" 
name="SubmitEmail" method="post" onsubmit="return checkForm(thankyou.aspx);"> 
<input type="hidden" name="thx" value="thankyou.aspx" /> 
<input type="hidden" name="err" value="remove.aspx" /> 
<input type="hidden" name="sub" value="thankyou.aspx" />
<input type="hidden" name="MID" value="61705" />--%><form>
    <asp:TextBox ID="EmailAddress" runat="server"></asp:TextBox>
    <asp:Button ID="SubmitEmail" runat="server" Text="Submit" /></form>
<%--    </form>--%></div>

</body>
