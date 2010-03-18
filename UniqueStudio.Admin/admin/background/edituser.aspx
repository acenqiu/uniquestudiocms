<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="edituser.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.edituser" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            在该页面您可以为所选用户重新分配角色。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            编辑用户</div>
        <div class="panel_body">
            <table width="100%">
                <tr>
                    <td>
                        Email：<asp:Literal ID="ltlEmail" runat="server" />
                    </td>
                    <td>
                        用户名：<asp:Literal ID="ltlUserName" runat="server" />
                    </td>
                    <td>
                        创建时间：<asp:Literal ID="ltlCreateTime" runat="server" />
                    </td>
                    <td>
                        上次登录时间：<asp:Literal ID="ltlLastActivityDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="chbIsApproved" Text="是否已激活" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chbIsLockedOut" Text="是否锁定" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chbIsOnline" Text="是否在线" Enabled="false" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>
             </table>
               <br />
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
                                        <input type="checkbox" name='chkSelected_r' <asp:Literal ID="ltlSelected" runat="server"/> onchange='selectRow(this)' value='<%# Eval("ID") %>' />
                                    </td>
                                    <td>
                                        <a href='editrole.aspx?roleId=<%# Eval("ID") %>&ret=<%= HttpUtility.UrlEncode(Request.Url.Query) %>'
                                            title="单击编辑该角色">
                                            <%# Eval("RoleName") %></a>
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
                    
                       <br />
                        具有的权限：<br />
                        <asp:Repeater ID="rptPermissions" runat="server">
                            <HeaderTemplate>
                                <table class="panel_table">
                                    <tr class="panel_table_head">
                                        <td>
                                            权限名称
                                        </td>
                                        <td>
                                            说明
                                        </td>
                                        <td>
                                            权限提供者
                                        </td>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%# Eval("PermissionName")%>
                                    </td>
                                    <td>
                                        <%# Eval("Description")%>
                                    </td>
                                    <td>
                                        <%# Eval("Provider")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    
            <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="返回" OnClick="btnCancel_Click" />
        </div>
    </div>
</asp:Content>
