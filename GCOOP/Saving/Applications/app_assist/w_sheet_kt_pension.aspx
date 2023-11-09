<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kt_pension.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_pension" %>


<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeAmt %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <%=postBPayclick %>
    <%=postBDeadPayclick %>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
//            objDwDetail.AcceptText();

            var member_no = objDwMem.GetItem(1, "member_no");
            //var capital_year = objDwMain.GetItem(1, "capital_year");
            //var salary_amt = objDwMain.GetItem(1, "salary_amt");
            //var assist_amt = objDwDetail.GetItem(1, "assist_amt");
            //var req_tdate = objDwMain.GetItem(1, "req_tdate");

            //var member_receive = objDwDetail.GetItem(1, "member_receive");
            //var member_dead_tdate = objDwDetail.GetItem(1, "member_dead_tdate");
//            var member_age = objDwDetail.GetItem(1, "member_age");
//            var statuspay = document.getElementById("HdTotalpay");
//            alert(statuspay);

            if (member_no != null ) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                alert("ไม่สามารถทำการบันทึกได้");
            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(570, 400, "w_dlg_kt_memberdead_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b2_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search_pension.aspx", "");
            }
        }

        function GetValueFromDlg2(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemberNo").value = memberno;

            postRetreiveDwMem();
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            //objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;

            postRetrieveDwMain();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            if (columnName == "assist_amt") {
                //objDwSchool.SetItem(1, "assist_amt", newValue);
                postRefresh();
            }
            else if (columnName == "expense_bank") {
                objDwMain.AcceptText();
                postRetrieveBankBranch();
            }
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;
                postRetreiveDwMem();
//                postGetMemberDetail();
            }
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "member_dead_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                Gcoop.GetEl("hdate1").value = newValue;

                objDwDetail.AcceptText();
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

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                NewClear();
            }
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_reqmem_dead" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailItemChange" ClientEventClicked="DwDetailClickPay">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HfDate" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HdSumpay" runat="server" />
    <asp:HiddenField ID="HdTotalpay" runat="server" />
    <asp:HiddenField ID="Hdck" runat="server" />
</asp:Content>
