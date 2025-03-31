<%@ Page Title="Payment Methods" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="PaymentMethods.aspx.vb" Inherits="PersonalFinanceTracker.CustomPages.PaymentMethods" Async="true"%>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <asp:Panel ID="AuthenticatedContent" runat="server" Visible="false">
        <div class="row mb-3">
            <div class="col">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Payment Method" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
            </div>
        </div>
        <asp:Panel ID="pnlList" runat="server">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            
            <asp:GridView ID="gvPaymentMethods" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered" DataKeyNames="PaymentMethodID"
                OnRowCommand="gvPaymentMethods_RowCommand">
                <Columns>
                    <asp:BoundField DataField="MethodName" HeaderText="Name" />
                    <asp:BoundField DataField="MethodType" HeaderText="Type" />
                    <asp:BoundField DataField="CurrentBalance" HeaderText="Balance" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# If(Convert.ToBoolean(Eval("IsActive")), "Active", "Inactive") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("PaymentMethodID") %>'
                                CssClass="btn btn-sm btn-info">Edit</asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("PaymentMethodID") %>'
                                CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this payment method?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info">
                        No payment methods found. Click "Add New Payment Method" to create one.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <h4><asp:Literal ID="litEditMode" runat="server"></asp:Literal></h4>
            <hr />
            
            <div class="form-horizontal">
                <asp:HiddenField ID="hdnPaymentMethodID" runat="server" />
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtMethodName" CssClass="col-md-2 control-label">Name</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtMethodName" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMethodName"
                            CssClass="text-danger" ErrorMessage="Name is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlMethodType" CssClass="col-md-2 control-label">Type</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlMethodType" CssClass="form-control">
                            <asp:ListItem Value="">-- Select Type --</asp:ListItem>
                            <asp:ListItem Value="Credit Card">Credit Card</asp:ListItem>
                            <asp:ListItem Value="Debit Card">Debit Card</asp:ListItem>
                            <asp:ListItem Value="Bank Account">Bank Account</asp:ListItem>
                            <asp:ListItem Value="Cash">Cash</asp:ListItem>
                            <asp:ListItem Value="Digital Wallet">Digital Wallet</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMethodType"
                            CssClass="text-danger" ErrorMessage="Type is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtCurrentBalance" CssClass="col-md-2 control-label">Current Balance</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtCurrentBalance" CssClass="form-control" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCurrentBalance"
                            CssClass="text-danger" ErrorMessage="Please enter a valid amount." 
                            ValidationExpression="^-?\d+(\.\d{1,2})?$" Display="Dynamic" />
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
            <p><asp:HyperLink runat="server" NavigateUrl="~/Account/Login">Log in</asp:HyperLink> or 
            <asp:HyperLink runat="server" NavigateUrl="~/Account/Register">Register</asp:HyperLink> to continue.</p>
        </div>
    </asp:Panel>
</asp:Content>
