<%@ Page Title="Transactions" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false" CodeBehind="Transactions.aspx.vb" Inherits="PersonalFinanceTracker.CustomPages.Transactions" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <asp:Panel ID="AuthenticatedContent" runat="server" Visible="false">
        <div class="row mb-3">
            <div class="col">
                <asp:Button ID="btnAddNew" runat="server" Text="Add New Transaction" CssClass="btn btn-primary" OnClick="btnAddNew_Click" />
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <h5>Filter Transactions</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="<%= txtStartDate.ClientID %>">Start Date</label>
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="<%= txtEndDate.ClientID %>">End Date</label>
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="<%= ddlCategory.ClientID %>">Category</label>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All Categories</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="<%= ddlTransactionType.ClientID %>">Type</label>
                                    <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="">All Types</asp:ListItem>
                                        <asp:ListItem Value="Income">Income</asp:ListItem>
                                        <asp:ListItem Value="Expense">Expense</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-3">
                            <div class="col-md-12">
                                <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" CssClass="btn btn-primary" OnClick="btnApplyFilter_Click" />
                                <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" CssClass="btn btn-secondary" OnClick="btnClearFilter_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <asp:Panel ID="pnlList" runat="server">
            <asp:Label ID="lblMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="text-danger" Visible="false"></asp:Label>
            
            <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" 
                CssClass="table table-striped table-bordered" DataKeyNames="TransactionID"
                OnRowCommand="gvTransactions_RowCommand">
                <Columns>
                    <asp:BoundField DataField="TransactionDate" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="MerchantName" HeaderText="Merchant" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                    <asp:TemplateField HeaderText="Amount">
                        <ItemTemplate>
                            <span class='<%# If(Convert.ToBoolean(Eval("IsIncome")), "text-success", "text-danger") %>'>
                                <%# If(Convert.ToBoolean(Eval("IsIncome")), "+", "-") %><%# String.Format("{0:C}", Math.Abs(Convert.ToDecimal(Eval("Amount")))) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditItem" CommandArgument='<%# Eval("TransactionID") %>'
                                CssClass="btn btn-sm btn-info">Edit</asp:LinkButton>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("TransactionID") %>'
                                CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this transaction?');">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <div class="alert alert-info">
                        No transactions found. Click "Add New Transaction" to create one.
                    </div>
                </EmptyDataTemplate>
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="pnlEdit" runat="server" Visible="false">
            <h4><asp:Literal ID="litEditMode" runat="server"></asp:Literal></h4>
            <hr />
            <div class="form-horizontal">
                <asp:HiddenField ID="hdnTransactionID" runat="server" />
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtTransactionDate" CssClass="col-md-2 control-label">Date</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtTransactionDate" CssClass="form-control" TextMode="Date" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTransactionDate"
                            CssClass="text-danger" ErrorMessage="Date is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtMerchantName" CssClass="col-md-2 control-label">Merchant</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtMerchantName" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMerchantName"
                            CssClass="text-danger" ErrorMessage="Merchant name is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtDescription" CssClass="col-md-2 control-label">Description</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlEditCategory" CssClass="col-md-2 control-label">Category</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlEditCategory" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="txtAmount" CssClass="col-md-2 control-label">Amount</asp:Label>
                    <div class="col-md-10">
                        <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAmount"
                            CssClass="text-danger" ErrorMessage="Amount is required." Display="Dynamic" />
                        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtAmount"
                            CssClass="text-danger" ErrorMessage="Please enter a valid amount." 
                            ValidationExpression="^\d+(\.\d{1,2})?$" Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <asp:Label runat="server" AssociatedControlID="ddlEditTransactionType" CssClass="col-md-2 control-label">Transaction Type</asp:Label>
                    <div class="col-md-10">
                        <asp:DropDownList runat="server" ID="ddlEditTransactionType" CssClass="form-control">
                            <asp:ListItem Value="">-- Select Type --</asp:ListItem>
                            <asp:ListItem Value="Recurring">Recurring</asp:ListItem>
                            <asp:ListItem Value="One-Time">One-Time</asp:ListItem>
                            <asp:ListItem Value="Transfer">Transfer</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEditTransactionType"
                            CssClass="text-danger" ErrorMessage="Transaction type is required." Display="Dynamic" />
                    </div>
                </div>
                
                <div class="form-group row mb-3">
                    <div class="col-md-offset-2 col-md-10">
                        <div class="form-check">
                            <asp:CheckBox runat="server" ID="chkIsIncome" />
                            <asp:Label runat="server" AssociatedControlID="chkIsIncome">Is Income</asp:Label>
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
