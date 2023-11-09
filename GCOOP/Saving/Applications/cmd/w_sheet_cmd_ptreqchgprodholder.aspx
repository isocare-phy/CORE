<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptreqchgprodholder.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptreqchgprodholder" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostOnInsert%>
    <%=postFindShow%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (i = 0; i <= objDwMain.RowCount(); i++) {
                    var dept_code = objDwMain.GetItem(i, "dept_code");
                    if (dept_code == null || dept_code == "") {
                        alert("กรุณาระบุแผนกที่ใช้งานให้ถูกต้อง");
                        return;
                    }
                    var holder_name = objDwMain.GetItem(i, "holder_name");
                    if (holder_name == null || holder_name == "") {
                        alert("กรุณาระบุชื่อผู้ใช้งานให้ถูกต้อง");
                        return;
                    }
                    var branch_code = objDwMain.GetItem(i, "branch_code");
                    if (branch_code == null || branch_code == "") {
                        alert("กรุณาระบุสาขาใช้งานให้ถูกต้อง");
                        return;
                    }
                }
            }
            catch (Error) { }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function postOnInsert() {
            jsPostOnInsert();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(790, 570, "w_dlg_cmd_ptdurtmaster.aspx", "");
        }

        function OnFindShow(durt_id, durt_name) {
            Gcoop.GetEl("HdurtId").value = durt_id;
            objDwMain.SetItem(1, "durt_id", durt_id);
            objDwMain.AcceptText();
            postFindShow();
        }

        function MenubarNew() {
            window.location = state.SsUrl + "Applications/cmd/w_sheet_cmd_ptreqchgprodholder.aspx";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_detail_ptreqchgprodholder"
        LibraryList="~/DataWindow/Cmd/cmd_reqchgprodholder.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <br />
    <div style="height: 18px; vertical-align: bottom; padding-left:65px;">
        <%--<span class="linkSpan" onclick="postOnInsert()" style="font-size: small; color:Red;float: left">เพิ่มข้อมูล</span>--%>
    </div>
    <asp:HiddenField ID="HdurtId" runat="server" />
</asp:Content>
