<%@ Page MasterPageFile="background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="permissionlist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.permissionlist" %>

<%@ Register Src="../controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="tip">
        <p>
            在此显示本系统中存在的所有权限，一些关键性的操作将受到权限的控制。</p>
    </div>
    <US:Message ID="message" runat="server" />
    <div class="panel">
        <div class="panel_title">
            权限列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
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
        </div>
    </div>
</asp:Content>
