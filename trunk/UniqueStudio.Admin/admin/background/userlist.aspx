<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="userlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.userlist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Src="../controls/pagination.ascx" TagPrefix="US" TagName="Pagination" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            “用户列表”中列出了当前系统中已经创建的所有用户，点击用户邮箱可以对其进行设置； 勾选某几个用户，可对其进行批量操作。</p>
    </div>
    <US:Message ID="message" runat="server" />
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
                            <%--<td>
                                是否在线
                            </td>--%>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input type="checkbox" name='chkSelected' onchange='selectRow(this)' value='<%# Eval("UserId") %>' />
                        </td>
                        <td>
                            <a href='edituser.aspx?id=<%# Eval("UserId") %>' title="单击修改该用户">
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
                        <%--<td>
                            <asp:CheckBox ID="chkIsOnline" Enabled="false" Checked='<%# Eval("IsOnline") %>'
                                runat="server" />--%>
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
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClick="btnExcute_Click"
                OnClientClick="if (selectcheck('chkSelected')) return confirm('您确定执行你所选的操作吗？'); else return false;" />
                <US:Pagination ID="pagination" Url="userlist.aspx?page={0}" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
