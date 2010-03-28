<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostEditor.ascx.cs"
    Inherits="UniqueStudio.ComContent.PL.PostEditor" %>
<%@ Register Src="Message.ascx" TagPrefix="US" TagName="Message" %>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Src="attachment.ascx" TagName="Attachment" TagPrefix="US" %>

<script src="jquery.min.js" type="text/javascript"></script>

<div class="postEditor">
    <US:Message ID="message" runat="server" />
    <asp:ValidationSummary ID="validationSummary" runat="server" ValidationGroup="post"
        CssClass="error" DisplayMode="List" ForeColor="#333333" />
    <asp:Panel ID="pnlEditor" runat="server">
        <div class="form-item">
            <span class="form-item-label">标题：</span> <span class="form-item-input">
                <asp:TextBox ID="txtTitle" runat="server" Width="450px" />
                <asp:RequiredFieldValidator ControlToValidate="txtTitle" runat="server" Display="None"
                    ErrorMessage="请输入标题" ValidationGroup="post"></asp:RequiredFieldValidator>
            </span>
        </div>
        <div class="form-item" style="display: none">
            <span class="form-item-label">副标题：</span> <span class="form-item-input">
                <asp:TextBox ID="txtSubTitle" runat="server" Width="450px" /></span>
        </div>
        <div class="form-item" style="display: none">
            <span class="form-item-label">显示属性：</span> <span class="form-item-input"><span>
                <asp:CheckBox ID="chbRecommend" runat="server" Text="推荐" /></span> <span>
                    <asp:CheckBox ID="chbHot" runat="server" Text="热点" /></span> <span>
                        <asp:CheckBox ID="chbTop" runat="server" Text="置顶" /></span></span>
        </div>
        <div class="form-item">
            <span class="form-item-label">分类：</span> <span class="form-item-input">
                <asp:CheckBoxList ID="cblCategory" runat="server" RepeatColumns="7" RepeatDirection="Horizontal">
                </asp:CheckBoxList>
            </span>
        </div>
        <div class="form-item">
            <span class="form-item-label">作者：</span> <span class="form-item-input">
                <asp:TextBox ID="txtAuthor" runat="server" Width="250px" /><asp:RequiredFieldValidator
                    runat="server" ControlToValidate="txtAuthor" ErrorMessage="请输入作者" ValidationGroup="post"
                    Display="None"></asp:RequiredFieldValidator></span>
        </div>
        <div class="form-item" id="attachments">
            <span class="form-item-label">新闻图片：</span> <span class="form-item-input">
                <asp:Label runat="server" ID="lblImageName" Visible="false"></asp:Label>
                <asp:FileUpload runat="server" ID="fuNewsImage" EnableViewState="false" /></span>
            <br />
            <span class="form-item-label">附件：</span> <span class="form-item-input">
                <US:Attachment ID="attcontrol" runat="server" />
            </span>
            <br />
            <div id="text" runat="server">
            </div>
            <%--<asp:Button ID="upfilebtn" runat="server" Text="附件上传" OnClick="upfilebtn_Click" />--%>
        </div>
        <div class="form-item" style="display: none">
            <span class="form-item-label">来源：</span> <span class="form-item-input">
                <asp:TextBox ID="txtSource" runat="server" Enabled="false" Width="250px" /></span>
        </div>
        <div class="form-item">
            <span class="form-item-label" style="height: 400px">内容(点击工具栏上的“全屏编辑”按钮可以将编辑窗口最大化)：</span>
            <span class="form-item-input">
                <FCKeditorV2:FCKeditor ID="fckContent" runat="server" />
            </span>
            <%--<span class="form-item-label">
                <div id="autosavestate">
                </div>
                <input type="button" value="载入草稿" onclick="GetDraft()" />
            </span>--%>
        </div>
        <div class="form-item">
            <span class="form-item-label">摘要：</span> <span class="form-item-input">
                <FCKeditorV2:FCKeditor ID="fckSummary" runat="server" ToolbarSet="Basic" />
            </span>
        </div>
        <div class="form-item">
            <span class="form-item-label">添加时间：</span> <span class="form-item-input">
                <asp:TextBox ID="txtAddDate" runat="server" Width="250px" /></span>
        </div>
        <div class="form-item">
            <span class="form-item-label"></span><span class="form-item-input">
                <asp:CheckBox ID="chkTitle" runat="server" Text="不显示标题" Checked="false" />
            </span>
        </div>
        <div class="form-item">
            <span class="form-item-label"></span><span class="form-item-input">
                <asp:CheckBox runat="server" ID="chkOther" Text="不显示作者、发表时间、阅读次数等信息" Checked="false" />
            </span>
        </div>
        <div class="form-item" style="display: none">
            <span class="form-item-label">标签：</span> <span class="form-item-input">
                <asp:TextBox ID="txtTags" runat="server" Width="250px" /></span>
        </div>
        <div class="form-item" style="display: none">
            <span class="form-item-label">评论设置：</span> <span class="form-item-input">
                <asp:CheckBox ID="chbAllowComment" runat="server" Checked="true" Text="允许评论" /></span>
        </div>
        <p class="submits">
            <asp:Button ID="btnPublish" runat="server" OnClick="btnPublish_Click" ValidationGroup="post"
                Text="发表" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" ValidationGroup="post"
                Text="保存为草稿" />
    </asp:Panel>
</div>
