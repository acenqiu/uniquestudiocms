<%@ Page Language="C#" MasterPageFile="background.Master" AutoEventWireup="true"
    CodeBehind="createuser.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.createuser" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            在“创建用户”部分您可以完成用户创建的工作，并且将其添加到某些角色。为了避免引起混淆，请不要将用户名设置成邮箱格式。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建用户</div>
        <div class="panel_body">
            <p>
                Email：<asp:TextBox ID="txtEmail" runat="server" />
                <asp:RequiredFieldValidator ID="requireEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="create" Display="None" ErrorMessage="请输入邮箱" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="您输入的邮箱格式不正确" Display="None" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                    ValidationGroup="create" />
                用户名：<asp:TextBox ID="txtUserName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                    ValidationGroup="create" Display="None" ErrorMessage="请输入用户名" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUserName"
                    ErrorMessage="您输入的用户名格式不正确" Display="None" ValidationExpression="^.{1,20}$" ValidationGroup="create" />
                密码：<asp:TextBox ID="txtPassword" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                    ValidationGroup="create" Display="None" ErrorMessage="请输入密码" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="您输入的密码格式不正确" Display="None" ValidationExpression="^[^ ]{1,40}$"
                    ValidationGroup="create" />
            </p>
            <p>
                所属角色：
                <asp:Repeater ID="rptRoles" runat="server">
                    <HeaderTemplate>
                        <table class="panel_table">
                            <tr class="panel_table_head">
                                <td width="10px">
                                    <input type="checkbox" onchange="selectall(this,'chkSelected_r')" id="chkSelectAll" />
                                </td>
                                <td width="200px">
                                    角色名称
                                </td>
                                <td>
                                    说明
                                </td>
                                <td width="200px">
                                    所属网站
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" name='chkSelected_r' onchange='selectRow(this)' value='<%# Eval("RoleID") %>' />
                            </td>
                            <td>
                                <%# Eval("RoleName") %>
                            </td>
                            <td>
                                <%# Eval("Description") %>
                            </td>
                            <td>
                                <%# Eval("SiteName") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </p>
            <asp:Button ID="btnCreate" runat="server" Text="创建" ValidationGroup="create" OnClick="btnCreate_Click" />
        </div>
    </div>
</asp:Content>
