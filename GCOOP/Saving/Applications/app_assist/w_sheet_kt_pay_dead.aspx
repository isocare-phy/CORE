<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_pay_dead.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_pay_dead" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postRetreiveDwMem %>
    <%=postDeadDate %>
    <%=postSelectReport%>
    <%=runProcess%>
	<%=popupReport%>
    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");

        }
        function Validate() {

            objDwMainP.AcceptText();
            objDwDetailP.AcceptText();

            var member_no = objDwMemP.GetItem(1, "member_no");
            //var assist_amt = objDwDetailP.GetItem(1, "assist_amt");

            var member_dead_tdate = objDwDetailP.GetItem(1, "member_dead_tdate");
            //var member_age = objDwDetailP.GetItem(1, "member_age");

            if (member_no != null && member_dead_tdate != null) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบ");
            }

        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "b2_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
            }
        }
        function DwDetailButtonClick(sender, rowNumber, buttonName) {

            if (buttonName == "report_pension") {
                //runreport
                Gcoop.GetEl("HdReport").value = "1";
                runProcess();
            }
            else if (buttonName == "report_65year") {
                //runreport
                Gcoop.GetEl("HdReport").value = "2";
                runProcess();
            }
        }

        function DwMemPItemChange(sender, rowNumber, columnName, newValue) {
            objDwMemP.SetItem(rowNumber, columnName, newValue);
            objDwMemP.AcceptText();

            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HdMemberNo").value = newValue;

                postRetreiveDwMem();

            }
        }
        function OnClickInsertRow() {
            var member_no = objDwMemP.GetItem(1, "member_no");
            if (member_no == "" || member_no == null) {
                alert("กรุณากรอกรหัสสมาชิก");

                return;
            }
            Gcoop.GetEl("HdRowReceive").value = "เพิ่ม";
            postInsertRow();
        }
        function OnClickDeleteRow() {
            if (objDwReceive.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdDwRow").value;
                var checkNo = "";
                try {
                    checkNo = Gcoop.Trim(objDwReceive.GetItem(currentRow, "cheque_no"));
                } catch (err) { }
                var confirmText = checkNo == "" || checkNo == null ? "ยืนยันการลบแถวที่ " + currentRow : "ยืนยันการลบเช็คเลขที่ " + checkNo;
                if (confirm(confirmText)) {
                    //                    Gcoop.GetEl("HdRowReceive").value = "ลบ";
                    postDeleteRow();
                }
            } else {
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการเช็ค");
            }
        }
        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function MenubarNew() {

            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
            return 0;
        }
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
        }
        function GetValueFromDlg(memberno, resign_date) {

            Gcoop.GetEl("HdMemberNo").value = memberno;
            Gcoop.GetEl("HdDeadDate").value = resign_date.toString();

            postRetreiveDwMem();
        }
        function DwDetailPItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "member_dead_tdate") {
                objDwDetailP.SetItem(rowNumber, columnName, newValue);
                objDwDetailP.SetItem(rowNumber, "member_dead_date", Gcoop.ToEngDate(newValue));
                objDwDetailP.AcceptText();
                postDeadDate();
            }
        }
        function OnClickInsertRow() {
            var member_no = objDwMemP.GetItem(1, "member_no");
            if (member_no == "" || member_no == null) {
                alert("กรุณากรอกรหัสสมาชิก");

                return;
            }
            Gcoop.GetEl("HdRowReceive").value = "เพิ่ม";
            postInsertRow();
        }
        function OnClickDeleteRow() {
            if (objDwReceive.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdDwRow").value;
                var checkNo = "";
                try {
                    checkNo = Gcoop.Trim(objDwReceive.GetItem(currentRow, "cheque_no"));
                } catch (err) { }
                var confirmText = checkNo == "" || checkNo == null ? "ยืนยันการลบแถวที่ " + currentRow : "ยืนยันการลบเช็คเลขที่ " + checkNo;
                if (confirm(confirmText)) {
                    //                    Gcoop.GetEl("HdRowReceive").value = "ลบ";
                    postDeleteRow();
                }
            } else {
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการเช็ค");
            }
        }
        function OnDwChequeClick(s, r, c) {
            Gcoop.CheckDw(s, r, c, "late_flag", 1, 0);
            if (r > 0) {
                Gcoop.GetEl("HdDwRow").value = r + "";
            }
        }
        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                Gcoop.GetEl("HdOpenIFrame").value == "False"
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMemP" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemPItemChange">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwMainP" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainPItemChange">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwDetailP" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_asn_test_d_date" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailPItemChange" ClientEventClicked="DwDetailButtonClick"
        ClientEventButtonClicked="DwDetailButtonClick">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="dw_pension" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_asn_test_pension" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="dw_65" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_asn_test_65year" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <br />
    <%--<asp:Label ID="Label7" runat="server" Text="รายชื่อผู้รับผลประโยชน์" Font-Bold="True"
        Font-Names="Tahoma" Font-Size="14px" ForeColor="#0099CC" Font-Overline="False"
        Font-Underline="True" />
    &nbsp; <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <asp:Panel ID="Panel2" runat="server" TabIndex="2">
        <dw:WebDataWindowControl ID="DwReceive" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
            DataWindowObject="d_65_member_recevie" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventClicked="OnDwChequeClick">
        </dw:WebDataWindowControl>
    </asp:Panel>--%>
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HdDeadDate" runat="server" />
    <asp:HiddenField ID="HdMoneyType" runat="server" />
    <asp:HiddenField ID="HdBirthDate" runat="server" />
    <asp:HiddenField ID="HdDwRow" runat="server" />
    <asp:HiddenField ID="HdRowReceive" runat="server" />
    <asp:HiddenField ID="HdPayout" runat="server" />
    <asp:HiddenField ID="HdMinOperatedate" runat="server" />
    <asp:HiddenField ID="HdMaxOperatedate" runat="server" />
    <asp:HiddenField ID="HdMaxYear" runat="server" />
    <asp:HiddenField ID="HdCheckOldMem" runat="server" />
    <asp:HiddenField ID="HdReport" runat="server" Value="false" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
</asp:Content>
