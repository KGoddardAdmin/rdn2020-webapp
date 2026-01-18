<%@ Control Language="VB" AutoEventWireup="false" CodeBehind="CheckJS.ascx.vb" Inherits="Controls_CheckJS" %>
<script type="text/javascript">
    function checkJavaScript() {
        document.cookie = "jsEnabled=true";
    }
    checkJavaScript();
</script>