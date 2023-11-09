<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_detailins.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_detailins" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postMemberNo%>
    <%=postListRow %>
    <%=postInsCost %>>
    <%=postNewIns %>>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้ายัน")) {
                window.location = Gcoop.GetUrl() + "Applications/" + Gcoop.GetApplication() + "/" + Gcoop.GetCurrentPage() + "";
            }
        }


        function SheetLoadComplete() {
            ShowTabPage2(Gcoop.ParseInt(Gcoop.GetEl("HdTabIndex").value));
        }

        function ShowTabPage2(tab) {
            var i = 1;
            var tabamount = 3;
            for (i = 1; i <= tabamount; i++) {
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).className = "tabTypeTdSelected";
                } else {
                    document.getElementById("tab_" + i).style.visibility = "hidden";
                    document.getElementById("stab_" + i).className = "tabTypeTdDefault";
                }
            }
            Gcoop.GetEl("HdTabIndex").value = tab + "";
        }
        function OnDwmbdetailItemChange(s, r, c, v) {
            if (c == "member_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postMemberNo();
            }
            return 0;
        }

        function OnDwlistClicked(s, r, c) {
            if (r > 0 && c != "datawindow") {
                Gcoop.GetEl("HdListRow").value = r + "";
                postListRow();
            }
        }

        function OnDwinsmasterItemChange(s, r, c, v) {
            if (c == "inscost_blance") {
                s.Setitem(r, c, v)
                s.AcceptText();
                postInsCost();
            }
        }

        function OnDwtabcontrolButtonClicked(s, r, b) {
            if (b == "b_add") {
                postNewIns();
            }
        }
    </script>
    <style type="text/css">
        .tabTypeDefault
        {
            width: 743px;
            border-spacing: 2px;
            margin-left: 6px;
        }
        .tabTypeTdDefault
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #777777;
            border: solid 1px #55A9CD;
            background-color: rgb(200,235,255);
            cursor: pointer;
        }
        .tabTypeTdSelected
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #660066;
            border: solid 1px #77CBEF;
            background-color: #76EFFF;
            cursor: pointer;
            text-decoration: underline;
        }
        .tabTypeTdDefault:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTypeTdSelected:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTableDetail
        {
            width: 743px;
            margin-left: 6px;
        }
        .tabTableDetail td
        {
        }
        .tableMessage
        {
            border: solid 1px #77771;
            width: 743px;
            margin-left: 6px;
        }
        .tableMessage td
        {
            font-family: Tahoma, Sans-Serif, Serif;
            font-size: 14px;
            font-weight: bold;
            border: solid 1px #EE0022;
            text-align: center;
            vertical-align: middle;
            margin-top: 15px;
            margin-bottom: 15px;
            background-color: #FDDDAA;
            height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hlist" runat="server" />
    <asp:HiddenField ID="hidden_search" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
        ข้อมูลสมาชิก
        <dw:WebDataWindowControl ID="dw_mbdetail" runat="server" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            DataWindowObject="d_sk_member_detail" LibraryList="~/DataWindow/app_assist/as_detailins.pbl"
            ClientEventItemChanged="OnDwmbdetailItemChange" ClientFormatting="True">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        ผู้ทำประกันร่วม
        <dw:WebDataWindowControl ID="dw_tabcontrol" runat="server" DataWindowObject="d_sk_tabcontrol"
            LibraryList="~/DataWindow/app_assist/as_detailins.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
            ClientScriptable="True" 
            ClientEventButtonClicked="OnDwtabcontrolButtonClicked" ClientFormatting="True">
        </dw:WebDataWindowControl>
        <br />
        <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_sk_list_editprakun"
            LibraryList="~/DataWindow/app_assist/as_detailins.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventClicked="OnDwlistClicked" ClientFormatting="True">
        </dw:WebDataWindowControl>
        <span class="linkSpan" onclick="OnUpdate()" style="font-size: small; color: Green;
            float: right">บันทึกข้อมูล</span> <span class="linkSpan" onclick="OnInsert()" style="font-size: small;
                color: Red; float: left">เพิ่มแถว</span>
    </asp:Panel>
    <table class="tabTypeDefault">
        <tr>
            <td class="tabTypeTdSelected" id="stab_1" onclick="ShowTabPage2(1);">
                รายละเอียดสมาชิก
            </td>
            <td class="tabTypeTdDefault" id="stab_2" onclick="ShowTabPage2(2);">
                รายการเคลื่อนไหว
            </td>
            <td class="tabTypeTdDefault" id="stab_3" onclick="ShowTabPage2(3);">
                ผู้รับผลประโยชน์
            </td>
        </tr>
    </table>
    <table class="tabTableDetail">
        <tr>
            <td style="height: 700px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label3" runat="server" Text="รายละเอียดสมาชิก" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    <br />
                    <dw:WebDataWindowControl ID="dw_insmaster" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_sk_prakunmaster" LibraryList="~/DataWindow/app_assist/as_detailins.pbl"
                        ClientEventButtonClicked="Clickb_search" 
                        ClientEventItemChanged="OnDwinsmasterItemChange" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: hidden; position: absolute;">
                    <asp:Label ID="Label2" runat="server" Text="รายการเคลื่อนไหว" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    <br />
                    <dw:WebDataWindowControl ID="dw_insstmt" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_sk_prakunstmt" ClientEventButtonClicked="Clickb_search" LibraryList="~/DataWindow/app_assist/as_detailins.pbl" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_3" style="visibility: hidden; position: absolute;">
                    <asp:Label ID="Label1" runat="server" Text="ผู้รับผลประโยชน์" Font-Bold="True" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="#0099CC"></asp:Label>
                    <br />
                    <dw:WebDataWindowControl ID="dw_insgain" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_sk_prakungain" LibraryList="~/DataWindow/app_assist/as_detailins.pbl"
                        ClientEventButtonClicked="Clickb_search" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdTabIndex" runat="server" Value="1" />
    <asp:HiddenField ID="HdListRow" runat="server" Value="1" />
</asp:Content>
