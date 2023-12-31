﻿<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true" CodeBehind="wd_mem_adjmembgroup.aspx.cs" Inherits="Saving.Applications.mbshr.ws_mem_ucfmembgroup_ctrl.wd_mem_adjmembgroup_ctrl.wd_mem_adjmembgroup" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;

        function Validate() {
        }
        function OnDsMainClicked(s, r, c) {
            if (c == "b_save") {
                PostSave();
            }
        }

        function OnDsMainItemChanged(s, r, c) {
            if (c == "addr_province") {
                PostProvince();
            } else if (c == "addr_amphur") {
                PostAmphur();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <div align="center">
        <uc1:DsMain ID="dsMain" runat="server" />
    </div>
    <br />
</asp:Content>
