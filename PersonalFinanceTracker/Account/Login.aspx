<%@ Page Title="Log in" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="PersonalFinanceTracker.Account.Login" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Title %>.</h2>
    <div class="row">
        <div class="col-md-8">
            <section id="loginForm">
                <h4>Use a local account to log in.</h4>
                <hr />
                <asp:Label runat="server" ID="ErrorMessage" CssClass="text-danger" Visible="false" />
                <asp:ValidationSummary runat="server" CssClass="text-danger" />
                <div class="form-horizontal">
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Username" CssClass="col-md-2 control-label">Username</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Username" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                                CssClass="text-danger" ErrorMessage="The Username field is required."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Password" CssClass="form-control" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="The Password field is required."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <%-- Add Remember Me option later if needed --%>
                    <%--<div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <div class="checkbox">
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            </div>
                        </div>
                    </div>--%>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="LogIn_Click" Text="Log in" CssClass="btn btn-default" />
                        </div>
                    </div>
                    <p>
                        <%-- Added ID="RegisterHyperLink" --%>
                        <asp:HyperLink runat="server" ID="RegisterHyperLink" NavigateUrl="~/Account/Register" >Register as a new user?</asp:HyperLink>
                    </p>
                </div>
            </section>
        </div>
    </div>
</asp:Content>