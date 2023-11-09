<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptdurtmaster.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptdurtmaster" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostOnInsert%>
    <%=postFindShow%>
    <%=jsPostRetrieveSubgrp%>
    <script type="text/javascript">

//        function Validate() {
//            try {
//                for (i = 0; i <= objDwMain.RowCount(); i++) {
////                    var durt_regno = objDwMain.GetItem(i, "durt_regno");
////                    if (durt_regno == null || durt_regno == "") {
////                        alert("กรุณากรอกเลขทะเบียนครุภัณฑ์ให้ถูกต้อง");
////                        return;
////                    }
//                    var durt_name = objDwMain.GetItem(i, "durt_name");
//                    if (durt_name == null || durt_name == "") {
//                        alert("กรุณากรอกชื่อครุภัณฑ์ให้ถูกต้อง");
//                        return;
//                    }
//                    var durtgrp_code = objDwMain.GetItem(i, "durtgrp_code");
//                    if (durtgrp_code == null || durtgrp_code == "") {
//                        alert("กรุณากรอกหมวดครุภัณฑ์ให้ถูกต้อง");
//                        return;
//                    }
//                    var unit_code = objDwMain.GetItem(i, "unit_code");
//                    if (unit_code == null || unit_code == "") {
//                        alert("กรุณากรอกหน่วยนับให้ถูกต้อง");
//                        return;
//                    }
//                    var unit_price = getObjFloat(objDwMain,1, "unit_price");
//                    if (unit_price <= 0) {
//                        alert("กรุณากรอกราคาต่อหน่วยให้ถูกต้อง");
//                        return;
//                    }
//                    var buy_tdate = objDwMain.GetItem(i, "buy_date");
//                    if (buy_tdate == null || buy_tdate == "") {
//                        alert("กรุณากรอกวันที่ซื้อให้ถูกต้อง");
//                        return;
//                    }
//                    var devalue_percent = getObjFloat(objDwMain, 1, "devalue_percent");
//                    if (devalue_percent <= 0) {
//                        alert("กรุณากรอกค่าเสื่อม(%)ให้ถูกต้อง");
//                        return;
//                    }
//                    var devaluestart_tdate = objDwMain.GetItem(i, "devaluestart_date");
//                    if (devaluestart_tdate == null || devaluestart_tdate == "") {
//                        alert("กรุณากรอกวันที่เริ่มคิดค่าเสื่อมให้ถูกต้อง");
//                        return;
//                    }
//                    var devaluelastcal_year = objDwMain.GetItem(i, "devaluelastcal_year");
//                    if (devaluelastcal_year == null || devaluelastcal_year == "") {
//                        alert("กรุณากรอกค่าเสื่อมที่คิดปีล่าสุดให้ถูกต้อง");
//                        return;
//                    }
//                    var dept_code = objDwMain.GetItem(i, "dept_code");
//                    if (dept_code == null || dept_code == "") {
//                        alert("กรุณากรอกแผนกที่ใช้งานให้ถูกต้อง");
//                        return;
//                    }
//                    var holder_name = objDwMain.GetItem(i, "holder_name");
//                    if (holder_name == null || holder_name == "") {
//                        alert("กรุณากรอกชื่อผู้ใช้งานให้ถูกต้อง");
//                        return;
//                    }
//                }
//            }
//            catch (Error) { }
//            return confirm("ยืนยันการบันทึกข้อมูล");
//        }

        function SheetLoadComplete() {
            var CurTab = Gcoop.ParseInt(Gcoop.GetEl("HiddenFieldTab").value);
            if (isNaN(CurTab)) {
                CurTab = 1;
            }
            showTabPage(CurTab);
        }
        function postOnInsert() {
            jsPostOnInsert();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(650, 570, "w_dlg_cmd_ptdurtmaster.aspx", "");

        }

        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "durtgrp_code") {
                jsPostRetrieveSubgrp();
            }
        }

        function OnFindShow(durt_id, durt_name) {
            Gcoop.GetEl("HdurtId").value = durt_id;
            objDwMain.SetItem(1, "durt_id", durt_id);
            objDwMain.AcceptText();
            postFindShow();
        }

        function MenubarNew() {
            window.location = state.SsUrl + "Applications/cmd/w_sheet_cmd_ptdurtmaster.aspx";
        }

        function showTabPage(tab) {
            var i = 1;
            var tabamount = 3;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    Gcoop.GetEl("HiddenFieldTab").value = i + "";
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ptdurtmaster"
        LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="MenubarOpen" ClientEventItemChanged="OnDwMainChange">
    </dw:WebDataWindowControl>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer; font-size: 14px;"
                id="stab_1" width="20%" onclick="showTabPage(1);">
                รายละเอียดครุภัณฑ์
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer; font-size: 14px;"
                id="stab_2" width="20%" onclick="showTabPage(2);">
                ประวัติซ่อมบำรุงครุภัณฑ์
            </td>
            <td style="background-color: rgb(200,235,255); cursor: pointer; font-size: 14px;"
                id="stab_3" width="20%" onclick="showTabPage(3);">
                ประวัติโอนย้ายบำรุงครุภัณฑ์
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 350px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_ptdurtmaster_detail"
                        LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="DwDetailItemChanged" ClientFormatting="True" TabIndex="1000"
                        ClientEventButtonClicked="DwDetailButtonClicked">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: visible; position: absolute;">
                    <dw:WebDataWindowControl ID="DwRepair" runat="server" DataWindowObject="d_ptdurtmaster_repair"
                        LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="DwRepairItemChanged" ClientFormatting="True" TabIndex="2000"
                        ClientEventButtonClicked="DwRepairBClicked">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_3" style="visibility: visible; position: absolute;">
                    <dw:WebDataWindowControl ID="DwTran" runat="server" DataWindowObject="d_ptdurtmaster_chghold"
                        LibraryList="~/DataWindow/Cmd/cmd_ptdurtmaster.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="DwTranItemChanged" ClientFormatting="True" TabIndex="3000"
                        ClientEventButtonClicked="DwTranBClicked" Width="742px" Height="330px">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdurtId" runat="server" />
    <asp:HiddenField ID="HiddenFieldTab" runat="server" />
</asp:Content>
