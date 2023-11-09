<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kt_65years.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_65years" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeAmt %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postGetMemberDetail %>
    <%=postGetMemberDetailpension%>
    <%=postRetrieveBankBranch %>
    <%=postChangeAssist %>
    <%=postBPayclick %>
    <%=postBDeadPayclick %>
    <%=postMoneyType %>
    <%=postFilterBranch%>
        <%=postInsertRow %>
    <%=postDeleteRow %>
    <script type="text/javascript">

        function Validate() {

            var stat = Gcoop.GetEl("HdTy").value;

            if (stat == "80") {
                objDwMainP.AcceptText();
                var member_no = objDwMemP.GetItem(1, "member_no");
                if (member_no != null) {
                    return confirm("ยืนยันการบันทึกข้อมูล");
                }
                else {
                    alert("ไม่สามารถทำการบันทึกได้");
                }
            } else if (stat == "90") {
                objDwMain65.AcceptText();
                var member_no = objDwMem65.GetItem(1, "member_no");
                if (member_no != null) {
                    return confirm("ยืนยันการบันทึกข้อมูล");
                }
                else {
                    alert("ไม่สามารถทำการบันทึกได้");
                }
            }
            else {
                objDwMain.AcceptText();
                objDwDetail.AcceptText();

                var member_no = objDwMem.GetItem(1, "member_no");
                var capital_year = objDwMain.GetItem(1, "capital_year");
                var salary_amt = objDwMain.GetItem(1, "salary_amt");
                var assist_amt = objDwDetail.GetItem(1, "assist_amt");
                var req_tdate = objDwMain.GetItem(1, "req_tdate");

                var member_receive = objDwDetail.GetItem(1, "member_receive");
                var member_dead_tdate = objDwDetail.GetItem(1, "member_dead_tdate");
                var member_age = objDwDetail.GetItem(1, "member_age");

                if (member_no != null && capital_year != null && assist_amt != null && req_date != null && member_receive != null && member_dead_tdate != null && member_age != null) {
                    return confirm("ยืนยันการบันทึกข้อมูล");
                }
                else {
                    alert("กรุณากรอกข้อมูลให้ครบ");
                }
            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_65.aspx", "");

        }


        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search.aspx", "");
            }
            if (buttonName == "b2_search") {

                Gcoop.GetEl("HdDateStatus").value = "0";
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_65.aspx", "");
            }
        }
        function DwMain65ItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain65.SetItem(rowNumber, columnName, newValue);
            objDwMain65.AcceptText();
            if (columnName == "moneytype_code") {
                postMoneyType();
            }
            else if (columnName == "asnslippayout_bank_code") {
                postFilterBranch();
            }
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemberNo").value = memberno;
            postRetreiveDwMem();
        }
        function GetValueFromDlg2(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            if (Gcoop.GetEl("HdTy").value == "80") {
                objDwMemP.SetItem(1, "member_no", memberno);
                Gcoop.GetEl("HfMemberNo").value = memberno;
                postRetreiveDwMem();
            } else if (Gcoop.GetEl("HdTy").value == "90") {
                objDwMem65.SetItem(1, "member_no", memberno);
                Gcoop.GetEl("HfMemberNo").value = memberno;
                postRetreiveDwMem();
            }
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            //objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;

            postRetrieveDwMain();
        }


        function DwMem65ItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem65.SetItem(rowNumber, columnName, newValue);
            objDwMem65.AcceptText();

            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;
                Gcoop.GetEl("HdDateStatus").value = "0";

                postRetreiveDwMem();

            }
        }


        function DwDetail65ItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "member_dead_tdate") {
                objDwDetail65.SetItem(rowNumber, columnName, newValue);
                Gcoop.GetEl("hdate2").value = newValue;

                if (Gcoop.GetEl("HdDateStatus").value == "1") {
                    Gcoop.GetEl("HdDateStatus").value = "2";
                }

                objDwDetail65.AcceptText();
                postChangeAmt();
            }
        }



        function DwDetailClickPay(sender, rowNumber, buttonName) {
            if (buttonName == "b_pay") {
                postBPayclick();

            }
            else if (buttonName == "b_deadpay") {
                postBDeadPayclick();
                alert("จ่ายสำเร็จ");
            }

        }

        function OnClickInsertRow() {
            var member_no = objDwMem65.GetItem(1, "member_no");
            if (member_no == "" || member_no == null) {
                alert("กรุณากรอกรหัสสมาชิก");
                return;
            }
            Gcoop.GetEl("HdRowReceive").value = "เพิ่ม";
            postInsertRow();
        }

        function OnDwChequeClick(s, r, c) {
            Gcoop.CheckDw(s, r, c, "late_flag", 1, 0);
            if (r > 0) {
                Gcoop.GetEl("HdDwRow").value = r + "";
            }
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
                //                NewClear();
            }
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMem65" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" 
        ClientEventItemChanged="DwMem65ItemChange" >
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwMain65" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMain65ItemChange" >
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwDetail65" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_kt_reqmem_dead" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetail65ItemChange" 
        ClientEventClicked="DwDetailClickPay" >
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label7" runat="server" Text="รายชื่อผู้รับผลประโยชน์" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="14px" ForeColor="#0099CC" Font-Overline="False" Font-Underline="True" />
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
        ClientEventClicked="OnDwChequeClick"
        >
    </dw:WebDataWindowControl>
     </asp:Panel>

    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HfDate" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="hdate2" runat="server" />
    <asp:HiddenField ID="HdSumpay" runat="server" />
    <asp:HiddenField ID="HdTotalpay" runat="server" />
    <asp:HiddenField ID="Hdck" runat="server" />
    <asp:HiddenField ID="HdTy" runat="server" />
    <asp:HiddenField ID="HdDateStatus" runat="server" />
    <asp:HiddenField ID="HdDate" runat="server" />
    <asp:HiddenField ID="HdDayDate" runat="server" />
    <asp:HiddenField ID="HdMoneyType" runat="server" />
    <asp:HiddenField ID="HdDwRow" runat="server" />
    <asp:HiddenField ID="HdRowReceive" runat="server" />
</asp:Content>