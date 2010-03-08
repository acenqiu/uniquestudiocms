<%@ Page MasterPageFile="~/admin/background/background.Master" Language="C#" AutoEventWireup="true"
    CodeBehind="categorylist.aspx.cs" Inherits="UniqueStudio.Admin.admin.background.categorylist" %>

<%@ Register Src="~/admin/controls/Message.ascx" TagPrefix="US" TagName="Message" %>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" CssClass="error" ValidationGroup="create"
        runat="server" DisplayMode="List" ForeColor="#333333" />
    <div class="panel">
        <div class="panel_title">
            创建分类</div>
        <div class="panel_body">
            <p>
                分类名称：<asp:TextBox ID="txtCategoryName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCategoryName"
                    Display="None" ValidationGroup="create" ErrorMessage="请输入分类名" />
                别名：<asp:TextBox ID="txtNiceName" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNiceName"
                    Display="None" ValidationGroup="create" ErrorMessage="请输入分类别名" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNiceName"
                    Display="None" ValidationGroup="create" ValidationExpression="^[a-z,_,1-9]+$"
                    ErrorMessage="别名格式错误，请使用字母、数字或下划线" />
                    隶属于：
            <asp:DropDownList ID="ddlCategories" runat="server">
            </asp:DropDownList></p>
            <p>
                说明：<asp:TextBox ID="txtDescription" Width="400px" runat="server" />
            </p>
            <asp:Button ID="btnCreate" runat="server" Text="创建" ValidationGroup="create" OnClick="btnCreate_Click" />
        </div>
    </div>
    <%--<div class="panel">
        <div class="panel_title">
            筛选</div>
        <div class="panel_body">
        </div>
    </div>--%>
    <div class="panel">
        <div class="panel_title">
            分类列表</div>
        <div class="panel_body">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table class="panel_table">
                        <tr class="panel_table_head">
                            <td width="10px">
                                <input type="checkbox" onchange="selectall(this,'chkSelected')" id="chkSelectAll" />
                            </td>
                            <td width="20px">
                                ID
                            </td>
                            <td width="100px">
                                分类名称
                            </td>
                            <td>
                                别名
                            </td>
                            <td>
                                说明
                            </td>
                            <td width="100px">
                                隶属于
                            </td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <input name='chkSelected' value='<%# Eval("CategoryId")%>' type="checkbox" />
                        </td>
                        <td>
                            <%# Eval("CategoryId")%>
                        </td>
                        <td>
                            <a href='editcategory.aspx?catId=<%# Eval("CategoryId")%>' title='编辑分类 <%# Eval("CategoryName") %>'>
                                <%# Eval("CategoryName") %></a>
                        </td>
                        <td>
                            <%# Eval("CategoryNiceName")%>
                        </td>
                        <td>
                            <%# Eval("Description") %>
                        </td>
                        <td>
                            <%# Eval("ParentCategoryName").ToString().Length == 0 ? "无" : Eval("ParentCategoryName")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
            <div>
                批量操作：<asp:DropDownList ID="ddlOperation" runat="server">
                    <asp:ListItem Value="delete" Text="删除" />
                </asp:DropDownList>
                <asp:Button ID="btnExcute" runat="server" Text="执行" OnClientClick="return selectcheck('chkSelected');"
                    OnClick="btnExcute_Click" />
            </div>
        </div>
    </div>
</asp:Content>
