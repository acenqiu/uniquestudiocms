<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sidebar.aspx.cs" Inherits="UniqueStudio.Admin.admin.sidebar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>引力中心实验室</title>
    <link rel="stylesheet" type="text/css" href="css/admin.css" />
    <link rel="stylesheet" type="text/css" href="css/index.css" />

    <script type="text/javascript" src="js/main.js" language="javascript"></script>
</head>
<body onload="addLiAction()">
    <div class="slider">
        <div class="admin-navigation">
            <ul>
                <li class="menu-activeted" onclick="javascript:changestate(this)"><span >文章组件</span>
                    <span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="compenents/com_content/admin/addpost.aspx" target="right">发表</a></li>
                            <li><a href="compenents/com_content/admin/postlist.aspx" target="right">管理</a></li>
                        </ul>
                    </div>
                </li>
                <li onclick="javascript:changestate(this)"><span>分类管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/categorylist.aspx" target="right">分类列表</a></li>
                        </ul>
                    </div>
                </li>
                <asp:Literal ID="ltlAdvancedMenus" runat="server">
                <li onclick="javascript:changestate(this)"><span>菜单管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/menulist.aspx" target="right">菜单列表</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>组件管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/compenentlist.aspx" target="right">组件列表</a></li>
                            <li><a href="background/installcompenent.aspx" target="right">组件安装</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>模块管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/modulelist.aspx" target="right">模块列表</a></li>
                            <li><a href="background/installmodule.aspx" target="right">模块安装</a></li>
                            <li><a href="background/controllist.aspx" target="right">控件列表</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>插件管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/pluginlist.aspx" target="right">插件列表</a></li>
                            <li><a href="background/installplugin.aspx" target="right">插件安装</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>权限管理</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/userlist.aspx" target="right">用户列表</a></li>
                            <li><a href="background/rolelist.aspx" target="right">角色列表</a></li>
                            <li><a href="background/permissionlist.aspx" target="right">权限列表</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>系统设置</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/websiteconfig.aspx" target="right">网站设置</a></li>
                            <li><a href="background/" target="right">服务器设置</a></li>
                            <li><a href="background/" target="right">安全设置</a></li>
                        </ul>
                    </div>
                </li>
                <li onclick="javascript:changestate(this)"><span>管理工具</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/pvlist.aspx" target="right">访问统计</a></li>
                            <li><a href="background/errorlog.aspx" target="right">系统日志</a></li>
                            <li><a href="background/file.aspx" target="right">文件管理</a></li>
                        </ul>
                    </div>
                </li>
                </asp:Literal>
                
                <li onclick="javascript:changestate(this)"><span>用户信息</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/userinfo.aspx" target="right">个人信息</a></li>
                            <li><a href="background/changepwd.aspx" target="right">修改密码</a></li>
                        </ul>
                    </div>
                </li>
                
                <li onclick="javascript:changestate(this)"><span>反馈信息</span><span class="collapse-icon"></span>
                    <div class="candy-menu">
                        <ul>
                            <li><a href="background/feedback.aspx" target="right">发送反馈</a></li>
                        </ul>
                    </div>
                </li>
            </ul>
        </div>
    </div>
</body>
</html>
