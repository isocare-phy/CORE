<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cmducf_deptcode.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmducf_deptcode" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDelete%>
    <%=jsPostSetData%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (var i = 1; i <= objDwMain.RowCount(); i++) {
                    var dept_desc = "", dept_abb = "";
                    dept_desc = objDwMain.GetItem(i, "dept_desc");
                    dept_abb = objDwMain.GetItem(i, "dept_abb");
                    if (dept_desc == null || dept_desc == " ") {
                        alert("กรุณากรอกชื่อแผนก");
                        return;
                    }
                    if (dept_abb == null || dept_abb == "") {
                        alert("กรุณากรอกตัวย่อแผนก");
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

        function postOnInsert() {
            jsPostOnInsert();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_ucfdeptcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdeptcode.pbl" Width="750px" ClientScriptable="True" TabIndex="1"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwMainOnClick" ClientEventItemChanged="DwMainOnItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_detail_ucfdeptcode"
        LibraryList="~/DataWindow/Cmd/cmd_ucfdeptcode.pbl" Width="750px" ClientScriptable="True" TabIndex="10"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="DwDetailOnClick" ClientEventItemChanged="DwDetailOnItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HdStatus" runat="server" />
    
</asp:Content>