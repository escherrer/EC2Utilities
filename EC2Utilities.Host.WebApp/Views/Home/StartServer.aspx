<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<EC2Utilities.Host.WebApp.Models.StartServerModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	StartServer
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Start Server</h2>

    <p>
        Please enter your email address to receive notifcation when server is ready.
    </p>

    <% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true, "Unable to start server. Please correct the errors and try again.") %>
    <fieldset>
        <legend>Fields</legend>
        
        <div class="display-label" style="font-size: medium"><strong>Server Name</strong></div>
        <div class="display-field"><%: Html.DisplayFor(x => x.ServerName)%></div>
        
        <div class="display-label" style="font-size: medium"><strong>Server Id</strong></div>
        <div class="display-field"><%: Html.DisplayFor(x => x.ServerId) %></div>
        
        <div class="editor-label" style="font-size: medium"><strong>Notification Email Address</strong></div>
        <div class="editor-field">
        <%: Html.TextBoxFor(m => m.EmailAddress) %>
        <%: Html.ValidationMessageFor(m => m.EmailAddress) %>
        </div>
        
        <div><%: Html.HiddenFor(x => x.ServerId)%></div>

        <p>
            <input type="submit" value="Start Server" />
        </p>
    </fieldset>
    <% } %>
    <p>
        <%: Html.ActionLink("Back to Server List", "ServerStartUp") %>
    </p>

</asp:Content>

