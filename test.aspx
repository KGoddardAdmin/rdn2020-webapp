<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head>
    <title>ASP.NET Test - rdn2020.dev</title>
    <style>
        body { font-family: Arial; padding: 20px; }
        .info { background: #f0f0f0; padding: 20px; border-radius: 5px; }
        .success { color: green; }
    </style>
</head>
<body>
    <h1 class="success">âœ“ ASP.NET is working on rdn2020.dev!</h1>
    <div class="info">
        <p><strong>Server Time:</strong> <%= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %></p>
        <p><strong>.NET Version:</strong> <%= Environment.Version.ToString() %></p>
        <p><strong>Server:</strong> <%= Environment.MachineName %></p>
        <p><strong>App Pool:</strong> <%= System.Security.Principal.WindowsIdentity.GetCurrent().Name %></p>
    </div>
</body>
</html>
