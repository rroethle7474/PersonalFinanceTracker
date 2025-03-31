<%@ Page Title="Accounts" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Accounts.aspx.vb" Inherits="PersonalFinanceTracker.CustomPages.Accounts" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <asp:Panel ID="AuthenticatedContent" runat="server" Visible="false">
        <div class="row mb-3">
            <div class="col">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Account" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
            </div>
        </div>

        <asp:Panel ID="pnlList" runat="server">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            
            <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered" DataKeyNames="AccountID"
                OnRowCommand="gvAccounts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="AccountName" HeaderText="Name" />
                    <asp:BoundField DataField="AccountType" HeaderText="Type" />
                    <asp:BoundField DataField="CurrentBalance" HeaderText="Balance" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="CurrencyCode" HeaderText="Currency" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# If(Convert.ToBoolean(Eval("IsActive")), "Active", "Inactive") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("AccountID") %>'
                                CssClass="btn btn-sm btn-info">Edit</asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("AccountID") %>'
                                CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this account?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info">
                        No accounts found. Click "Add New Account" to create one.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <h4><asp:Literal ID="litEditMode" runat="server"></asp:Literal></h4>
            <hr />
            
            <div class="form-horizontal">
                <asp:HiddenField ID="hdnAccountID" runat="server" />
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtAccountName" CssClass="col-md-2 control-label">Account Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtAccountName" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAccountName"
                            CssClass="text-danger" ErrorMessage="Account name is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlAccountType" CssClass="col-md-2 control-label">Account Type</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlAccountType" CssClass="form-control">
                            <asp:ListItem Value="">-- Select Type --</asp:ListItem>
                            <asp:ListItem Value="Checking">Checking</asp:ListItem>
                            <asp:ListItem Value="Savings">Savings</asp:ListItem>
                            <asp:ListItem Value="Credit Card">Credit Card</asp:ListItem>
                            <asp:ListItem Value="Investment">Investment</asp:ListItem>
                            <asp:ListItem Value="Loan">Loan</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlAccountType"
                            CssClass="text-danger" ErrorMessage="Account type is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtCurrentBalance" CssClass="col-md-2 control-label">Current Balance</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtCurrentBalance" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrentBalance"
                            CssClass="text-danger" ErrorMessage="Current balance is required." Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCurrentBalance"
                            CssClass="text-danger" ErrorMessage="Please enter a valid amount." 
                            ValidationExpression="^-?\d+(\.\d{1,2})?$" Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtCurrencyCode" CssClass="col-md-2 control-label">Currency Code</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtCurrencyCode" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrencyCode"
                            CssClass="text-danger" ErrorMessage="Currency code is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlCategory" CssClass="col-md-2 control-label">Category</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlCategory" CssClass="form-control">
                            <asp:ListItem Value="">-- Select Category --</asp:ListItem>
                            <asp:ListItem Value="Personal">Personal</asp:ListItem>
                            <asp:ListItem Value="Business">Business</asp:ListItem>
                            <asp:ListItem Value="Investment">Investment</asp:ListItem>
                            <asp:ListItem Value="Retirement">Retirement</asp:ListItem>
                            <asp:ListItem Value="Education">Education</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="form-check">
                            <asp:CheckBox runat="server" ID="chkIsActive" Checked="true" />
                            <asp:Label runat="server" AssociatedControlID="chkIsActive">Active</asp:Label>
                        </div>
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="form-check">
                            <asp:CheckBox runat="server" ID="chkIsFinancialInstitution" />
                            <asp:Label runat="server" AssociatedControlID="chkIsFinancialInstitution">Financial Institution</asp:Label>
                        </div>
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="form-check">
                            <asp:CheckBox runat="server" ID="chkIsMerchant" />
                            <asp:Label runat="server" AssociatedControlID="chkIsMerchant">Merchant</asp:Label>
                        </div>
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" ID="btnSave" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn btn-secondary" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    
    <asp:Panel ID="UnauthenticatedContent" runat="server" Visible="false">
        <div class="alert alert-warning">
            <p>You must be logged in to access this page.</p>
            <p><asp:HyperLink runat="server" NavigateUrl="~/AccountPages/Login">Log in</asp:HyperLink> or 
            <asp:HyperLink runat="server" NavigateUrl="~/AccountPages/Register">Register</asp:HyperLink> to continue.</p>
        </div>
    </asp:Panel>
</asp:Content>
