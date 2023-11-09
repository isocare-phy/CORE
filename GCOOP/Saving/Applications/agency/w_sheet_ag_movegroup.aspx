<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ag_movegroup.aspx.cs" Inherits="Saving.Applications.agency.w_sheet_ag_movegroup" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postInitMoveGroup %>
    <%=postAgentGrp%>

    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarNew() {
            if (confirm("ยืนยันการลบข้อมูล ??")) {
                window.location = Gcoop.GetUrl() + "Applications/agency/w_sheet_ag_movegroup.aspx";
            }
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            var recv_period = objDwMain.GetItem(1, "recv_period");
            var member_no = objDwMain.GetItem(1, "member_no");
            if (recv_period != null && member_no != null) {
                objDwMain.AcceptText();
                postInitMoveGroup();
            }
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_memsearch") {
                Gcoop.OpenDlg(720, 500, "w_dlg_ag_searchagentmem.aspx", "");
            }
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
            if(columnName == "membgrp_code"){
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                postAgentGrp();
            }
            else if (columnName == "move_tday"){
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                return 0;
            }
        }

        function SearchMember(member_no) {
            objDwMain.SetItem(1, "member_no", member_no);
            var recv_period = objDwMain.GetItem(1, "recv_period");
            if (recv_period != null) {
                objDwMain.AcceptText();
                postInitMoveGroup();
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <b>รายละเอียดลูกหนี้ตัวแทน</b>
    
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_mem_main"
        LibraryList="~/DataWindow/agency/movegroup.pbl" ClientFormatting="True" ClientEventItemChanged="DwMainItemChange"
        ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <b>ทำรายการย้ายสังกัดลูกหนี้ตัวแทน</b>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_agentsrv_initmovegroup_detail" LibraryList="~/DataWindow/agency/movegroup.pbl"
        ClientFormatting="True" ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
</asp:Content>
