<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cm_constant_config.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_cm_constant_config" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var timeout = 500;
        var closetimer = 0;
        var ddmenuitem = 0;

        function OnDwMenuListClick(sender, row, name) {
            window.location = "?uoucf=" + objDwMenuList.GetItem(row, "object_name");
            return 1;
        }
        function mopen(id) {
            mcancelclosetime();
            if (ddmenuitem) ddmenuitem.style.visibility = 'hidden';
            ddmenuitem = document.getElementById(id);
            ddmenuitem.style.visibility = 'visible';
        }
        function mclose() {
            if (ddmenuitem) ddmenuitem.style.visibility = 'hidden';
        }

        function mclosetime() {
            closetimer = window.setTimeout(mclose, timeout);
        }

        function mcancelclosetime() {
            if (closetimer) {
                window.clearTimeout(closetimer);
                closetimer = null;
            }
        }
    </script>

    <style type="text/css">
        .linkSpan
        {
            font-family: Tahoma, Sans-Serif;
            font-weight: bold;
            font-size: 12px;
            cursor: pointer;
            color: #005599;
            text-align: left;
            text-decoration: underline;
        }
        .menuCursor
        {
            top: 0px;
            left: 0px;
        }
        .menuCursor input
        {
            cursor: pointer;
        }
    </style>
    
    <% Page_LoadComplete(); %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
     <ul id="sddm">
        <li><a href="#" onclick="mopen('m1')" onmouseout="mclosetime()">เลือก รายการข้อกำหนด</a>
            <div id="m1" onmouseover="mcancelclosetime()" onmouseout="mclosetime()">
                <asp:Literal ID="ltr_cnstmenu" runat="server"></asp:Literal>
            </div>
        </li>
    </ul>
    <div style="float: right;">
        <asp:Label ID="lbl_cnstmenu" runat="server" Text="" CssClass="font14px"></asp:Label>
    </div>
    <table width="100%">
        <tr>
            <td width="100%" valign="top">
                <asp:Panel ID="Panel1" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
