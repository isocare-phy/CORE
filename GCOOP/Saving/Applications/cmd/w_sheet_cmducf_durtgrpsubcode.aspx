<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmducf_durtgrpsubcode.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_durtgrpsubcode" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDelete%>
    <%=jsPostSetData%>
    <%=jsPostRetriveDurtgrpsub%>
    <script type="text/javascript">

function Validate() {
            try {
                for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var durtgrpsub_desc = "", durtgrpsub_abb = "";
                    var devalue_percent = getObjFloat(objDwMain, i, "devalue_percent");
                    durtgrpsub_abb = objDwMain.GetItem(i, "durtgrpsub_abb");
                    durtgrpsub_desc = objDwMain.GetItem(i, "durtgrpsub_desc");
                    if (durtgrpsub_desc == null || durtgrpsub_desc == " ") {
                        alert("กรุณากรอกหมวดครุภัณฑ์ให้ถูกต้อง");
                        return;
                    }
//                    if (durtgrpsub_abb == null || durtgrpsub_abb == "") {
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

        function DwMainOnItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "durtgrp_code") {
                jsPostRetriveDurtgrpsub();
            }
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
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfdurtgrpsubcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdurtgrpsubcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwMainOnClick" ClientEventItemChanged="DwMainOnItemChange">
    </dw:WebDataWindowControl>
    <br />
   <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfdurtgrpsubcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdurtgrpsubcode.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HdStatus" runat="server" />
</asp:Content>
