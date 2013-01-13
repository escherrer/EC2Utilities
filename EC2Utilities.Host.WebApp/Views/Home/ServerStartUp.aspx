<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<EC2Utilities.Host.WebApp.Models.ServerListModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ServerStartUp
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ServerStartUp</h2>

    <table>
        <tr>
            <th>
                Server Name
            </th>
            <th>
                Server Id
            </th>
            <th>
                Server Status
            </th>
            <th>
                Start Server
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%: item.ServerName %>
            </td>
            <td>
                <%: item.ServerId %>
            </td>
            <td>
                <%: item.ServerStatus %>
            </td>
            <td>
                <%
                    if (item.ServerStatus != "Running")
                    {
                        %>
                        <%: Html.ActionLink("Start Up", "StartServer", new { @instanceId = item.ServerId })%>
                        <%
                    }
                %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%: Html.ActionLink("Refresh", "ServerStartUp") %>
    </p>

</asp:Content>

