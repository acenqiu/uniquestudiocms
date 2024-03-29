﻿<%@ Page MasterPageFile="Site.Master" EnableViewState="false" Language="C#" AutoEventWireup="true" CodeBehind="404.aspx.cs"
    Inherits="UniqueStudio.CGE._04" %>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="head">
    <style type="text/css">
        .main-wrapper
        {
            background: #FFFFCC;
        }
        .error-message
        {
            text-align: center;
            height: 400px;
            position: relative;
        }
        .error-message .error-text
        {
            font-size: 24px;
            font-family: 黑体, "Courier New" , Courier, monospace;
            background: url(img/error.png) no-repeat left;
            padding: 60px 0px;
            padding-left: 150px;
            display: inline;
            text-align: center;
            position: absolute;
            top: 30%;
            left: 20%;
        }
    </style>
</asp:Content>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="cphMain">
    <div class="error-message">
        <div class="error-text">
            对不起，您访问的页面不存在 :(</div>
    </div>
</asp:Content>
