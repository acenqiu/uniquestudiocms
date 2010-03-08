<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="userlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.userlist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="~/admin/controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    
    <div class="tip">
    <p>在“创建用户”部分您可以完成用户创建的工作，并且将其添加到某些角色。为了避免引起混淆，请不要将用户名设置成邮箱格式。</p>
        <p>“用户列表”中列出了当前系统中已经创建的所有用户，点击用户邮箱可以对其进行设置；
        勾选某几个用户，可对其进行批量操作。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建用户</div>
        <div class="panel_body">
            <table>
                <tr>
                    <td>
                        Email：<asp:TextBox ID="txtEmail" runat="server" />
                        <asp:RequiredFieldValidator ID="requireEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入邮箱" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="您输入的邮箱格式不正确" Display="None" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                            ValidationGroup="create" />
                    </td>
                    <td>
                        用户名：<asp:TextBox ID="txtUserName" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入用户名" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtUserName"
                            ErrorMessage="您输入的用户名格式不正确" Display="None" ValidationExpression="^.{1,20}$" ValidationGroup="create" />
                    </td>
                    <td>
                        密码：<asp:TextBox ID="txtPassword" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                            ValidationGroup="create" Display="None" ErrorMessage="请输入密码" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="您输入的密码格式不正确" Display="None" ValidationExpression="^[^ ]{1,40}$"
                            ValidationGroup="create" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        所属角色：
                        <asp:CheckBoxList ID="cblRoles" runat="server" RepeatDirection="Horizontal" RepeatColumns="10">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnCreate" runat="server" Text="创建" ValidationGroup="create" OnClick="btnCreate_Click" />
        </div>
    </div>
    <%--<div class="panel">
        <div class="panel_title">筛选</div>
        <div class="panel_body">
        <p>
                创建时间：<asp:TextBox ID="txtCreateDateS" runat="server" />到<asp:TextBox ID="txtCreateDateE"
                    runat="server" />
                上次登录时间：<asp:TextBox ID="txtLastActivityDateS" runat="server" />到<asp:TextBox ID="txtLastActivityDateE"
                    runat="server" />
            </p>
            <p>
                激活：<asp:DropDownList ID="ddlApproved" runat="server">
                    <asp:ListItem Selected="True" Value="none">不限</asp:ListItem>
                    <asp:ListItem Value="yes">已激活</asp:ListItem>
                    <asp:ListItem Value="no">未激活</asp:ListItem>
                </asp:DropDownList>
                锁定：<asp:DropDownList ID="ddlLockedOut" runat="server">
                    <asp:ListItem Selected="True" Value="none">不限</asp:ListItem>
                    <asp:ListItem Value="yes">已锁定</asp:ListItem>
                    <asp:ListItem Value="no">未锁定</asp:ListItem>
                </asp:DropDownList>
                在线：<asp:DropDownList ID="ddlOnline" runat="server">
                    <asp:ListItem Selected="True" Value="none">不限</asp:ListItem>
                    <asp:ListItem Value="yes">在线</asp:ListItem>
                    <asp:ListItem Value="no">离线</asp:ListItem>
                </asp:DropDownList>
                关键词(作用于Email和用户名)：<asp:TextBox ID="txtKeyWord" runat="server" />
            </p>
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">
            用户列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td>
                                Email
                            </td>
                            <td>
                                用户名
                            </td>
                            <td>
                                创建时间
                            </td>
                            <td>
                                上次登录时间
                            </td>
                            <td>
                                是否已激活
                            </td>
                            <td>
                                是否锁定
                            </td>
                            <td>
                                是否在线
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("UserId") %>' />
                        </td>
                        <td>
                            <a href='edituser.aspx?id=<%# Eval("UserId") %>'>
                            <%# Eval("Email") %></a>
                        </td>
                        <td>
                            <%# Eval("UserName") %>
                        </td>
                        <td>
                            <%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %>
                        </td>
                        <td>
                            <%# Convert.ToDateTime(Eval("LastActivityDate")).ToString("yyyy-MM-dd HH:mm") %>
                        </td>
                        <td>
                            <asp:CheckBox ID="chbIsApproved" Enabled="false" Checked='<%# Eval("IsApproved") %>'
                                runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chbIsLockedOut" Enabled="false" Checked='<%# Eval("IsLockedOut") %>'
                                runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsOnline" Enabled="false" Checked='<%# Eval("IsOnline") %>'
                                runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Selected="True" Value="delete" Text="删除" />
                    <asp:ListItem Value="approve" Text="激活" />
                    <asp:ListItem Value="lock" Text="锁定" />
                    <asp:ListItem Value="unlock" Text="解除锁定" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClientClick="return selectcheck('chkSelected');" OnClick="btnExcute_Click" />
                <US:Pagination ID="pagination" Url="userlist.aspx?page={0}" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
