<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EC2Utilities.Host.WebApp.Models.ServerListModelContainer>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ServerStartUp
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Server Start Up</h2>
    
    <%: Html.ValidationSummary(true, "Server enumeration unsuccessful. Please refresh the page to try again.") %>

    <table>
        <tr>
            <th>
                Server Name
            </th>
            <th>
                Server Id
            </th>
            <th>
                Server Size
            </th>
            <th>
                Server Status
            </th>
            <th>
                Start Server
            </th>
        </tr>

    <% foreach (var item in Model.ServerListModels) { %>
    
        <tr>
            <td>
                <%: item.ServerName %>
            </td>
            <td>
                <%: item.ServerId %>
            </td>
            <td>
                <%: item.ServerType %>
            </td>
            <td>
                <%: item.ServerStatus %>
            </td>
            <td>
                <%
                    if (item.ServerStatus == "Stopped")
                    {
                        %>
                        <%: Html.ActionLink("Start Up", "StartServer", new { instanceId = item.ServerId })%>
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

