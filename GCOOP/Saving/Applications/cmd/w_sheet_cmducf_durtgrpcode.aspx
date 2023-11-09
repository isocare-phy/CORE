<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmducf_durtgrpcode.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_durtgrpcode" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDelete%>
    <%=jsPostSetData%>
    <script type="text/javascript">

function Validate() {
            try {
                for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var durtgrp_desc = "", durtgrp_abb = "";
                    var devalue_percent = getObjFloat(objDwMain, i, "devalue_percent");
                    durtgrp_abb = objDwMain.GetItem(i, "durtgrp_abb");
                    durtgrp_desc = objDwMain.GetItem(i, "durtgrp_desc");
                    if (durtgrp_desc == null || durtgrp_desc == " ") {
                        alert("กรุณากรอกหมวดครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
//                    if (durtgrp_abb == null || durtgrp_abb == "") {
//                        alert("กรุณากรอกตัวย่อหมวดครุภัณฑ์");
//                        return;
//                    }
                    if (devalue_percent <= 0) {
                        alert("กรอกค่าเสื่อมให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwDetailOnClick(s, r, c) {
            if (c == "b_edit") {
                Gcoop.GetEl("HdR").value = r;
                jsPostSetData();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfdurtgrpcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdurtgrpcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwMainOnClick" ClientEventItemChanged="DwMainOnItemChange">
    </dw:WebDataWindowControl>
    <br />
   <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfdurtgrpcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdurtgrpcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HdStatus" runat="server" />
</asp:Content>
