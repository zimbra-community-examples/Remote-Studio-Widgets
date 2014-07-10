<%@ Page Language="C#"  AutoEventWireup="true" EnableViewState="false" %>
<%@ Register TagPrefix="rws" Namespace="Telligent.SimpleRemoteWebSite" Assembly="Telligent.SimpleRemoteWebSite" %>

<html>
    <head runat="server">
		<rws:Headers IsModal="False" runat="server" />
    </head>
    <body>
        <form runat="server">
			<%-- instance identifier is defined within the widget's XML implementation in the Widgets/ folder  --%>
            <rws:Widget InstanceIdentifier="71a243f32a2c4b5799feca862276b221" runat="server" />
        </form>
    </body>
</html>