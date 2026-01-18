<%@ Page Language="VB" AutoEventWireup="false" Inherits="P1Page" Codebehind="P1Page.aspx.vb" %>
<%@ Register Src="Controls/CheckJS.ascx" TagName="CheckJS" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
	<!--<meta http-equiv="X-UA-Compatible" content="IE=7" />-->
<script language="JavaScript" type="text/javascript">
<!--
var browser=navigator.appName;

function myfunction() 
{   
    if (browser == "Microsoft Internet Explorer"){ 
        document.getElementById( 'targetlink' ).click();                       
    }    
    else{
    simulateClick();
    }        
}

function preventDef(event) {
  event.preventDefault();
}

function addHandler() {
  document.getElementById("targetlink").addEventListener("click", 
    preventDef, false);
}

function removeHandler() {
  document.getElementById("targetlink").removeEventListener("click",
    preventDef, false);
}

function simulateClick() {
  var evt = document.createEvent("MouseEvents");
  evt.initMouseEvent("click", true, true, window,
    0, 0, 0, 0, 0, false, false, false, false, 0, null);
  var cb = document.getElementById("targetlink"); 
  var canceled = !cb.dispatchEvent(evt);
  if(canceled) {
    // A handler called preventDefault
    //alert("canceled");
  } else {
    // None of the handlers called preventDefault
    //alert("not canceled");
  }
}

window.onload = myfunction;
//-->
</script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:CheckJS ID="CheckJS1" runat="server"  NonJSTargetURL="~/PNonJavaScriptProcess.aspx?" />
        <table>  
             <tr>
                <td>            
                    <a href="<%=Link%>" style="color:White;" onclick="location.href=this.href" id="targetlink">Click</a> 
                </td>        
            </tr>                                                                                                 
            <%  If ShowTable = True Then%>                   
	        <tr>
	            <td><input type="checkbox" visible="false" id="checkbox"/><label for="checkbox">Checkbox</label></td>	
	        </tr>
	        <tr>
	            <td><input type="button" visible="false" onclick="simulateClick();"
		            value="Simulate click" id="Button1"/>
		        </td>	
	        </tr>
	        <tr>
	            <td><input type="button" visible="false" onclick="addHandler();"
			        value="Add a click handler that calls preventDefault"/>
			    </td>	
	        </tr>
	        <tr>
	            <td><input type="button" visible="false" onclick="removeHandler();"
			        value="Remove the click handler that calls preventDefault"/>
			    </td>	
	        </tr>	        
	        <%End If%>	 
	         <tr>
                <td>
                    <a href="<%=Link%>" style="color:White;" id="linktarget" target="_self">click</a><br />           
                </td>
            </tr>        	        
        </table>      
    </div>

    </form>
</body>
</html>
