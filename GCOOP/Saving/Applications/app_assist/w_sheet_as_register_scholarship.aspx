<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_register_scholarship.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_register_scholarship" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postRefresh %>
    <%=postGetMoney %>
    <%=postNewClear %>
    <%=postChangeHeight %>
    <%=postCheckReQuest %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postFilterScholarship %>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <script type="text/javascript">

        function Validate() {

            objDwMain.AcceptText();
            var member_no = objDwMem.GetItem(1, "member_no");

            var capital_year = objDwMain.GetItem(1, "capital_year");
            var salary_amt = objDwMain.GetItem(1, "salary_amt");
            var assist_amt = objDwDetail.GetItem(1, "assist_amt");

            var childprename_code = objDwDetail.GetItem(1, "childprename_code");
            var child_name = objDwDetail.GetItem(1, "child_name");
            var child_surname = objDwDetail.GetItem(1, "child_surname");
            var child_sex = objDwDetail.GetItem(1, "child_sex");
            var childschool_name = objDwDetail.GetItem(1, "childschool_name");
            var child_gpa = objDwDetail.GetItem(1, "child_gpa");
            var childbirth_date = objDwDetail.GetItem(1, "childbirth_date");
            var scholarship_level = objDwDetail.GetItem(1, "scholarship_level");
            var scholarship_type = objDwDetail.GetItem(1, "scholarship_type");

            //            if (member_no != null && capital_year != null && assist_amt != null && childprename_code != null && child_name != null && child_surname != null && childbirth_date != null) {
            return confirm("ยืนยันการบันทึกข้อมูล");
            //            }
            //            else {
            //                alert("กรุณากรอกข้อมูลให้ครบ");
            //            }
        }

        function MenubarNew() {
            Gcoop.SetLastFocus("member_no_0");
            postNewClear();
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(800, 600, "w_dlg_as_scholarship_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
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

        function GetValueFromDlgChildList(child_prename, child_name, child_surname, child_sex, level_school, scholarship_level) {
            objDwDetail.SetItem(1, "childprename_code", child_prename);
            objDwDetail.SetItem(1, "child_name", child_name);
            objDwDetail.SetItem(1, "child_surname", child_surname);
            objDwDetail.SetItem(1, "child_sex", child_sex);
            objDwDetail.SetItem(1, "level_school", level_school);
            objDwDetail.SetItem(1, "scholarship_level", scholarship_level);
            objDwDetail.AcceptText();
            postFilterScholarship();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            var member_no = objDwMem.GetItem(1, "member_no");
            if (columnName == "expense_bank") {
                objDwMain.AcceptText();
                postRetrieveBankBranch();
            }
            else if (columnName == "capital_year" && member_no != null) {
                objDwMain.AcceptText();
                postCheckReQuest();
            }
            //            else if (columnName == "req_status") {
            //                Gcoop.GetEl("HfReqSts").value = newValue;
            //                postChangeHeight();
            //            }
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;
                postGetMemberDetail();
            }
        }

        function DwSchoolButtonClick(sender, rowNumber, buttonName) {
            var member_no = objDwMem.GetItem(1, "member_no");
            var memb_surname = objDwMem.GetItem(1, "memb_surname");
            if (member_no != null) {
                if (buttonName == "b_child_lists") {
                    Gcoop.OpenDlg(730, 300, "w_dlg_as_child_lists.aspx", "?member_no=" + member_no + "," + memb_surname);
                }
            }
            else {
                alert("กรุณากรอกเลขที่สมาชิกก่อน");
            }
        }

        function DwSchoolItemChange(sender, rowNumber, columnName, newValue) {
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
            var scholarship_type = objDwDetail.GetItem(1, "scholarship_type");
            var scholarship_level = objDwDetail.GetItem(1, "scholarship_level");
            if (columnName == "scholarship_type" || columnName == "scholarship_level") {
                if (scholarship_level != "" && scholarship_type != "" && scholarship_level != null && scholarship_type != null) {
                    objDwDetail.AcceptText();
                    postGetMoney();
                }
            }
            else if (columnName == "level_school") {
                objDwDetail.AcceptText();
                postFilterScholarship();
            }
            else if (columnName == "child_gpa") {
                var child_gpa = objDwDetail.GetItem(1, "child_gpa");
                child_gpa = (child_gpa * 100) / 4;
                objDwDetail.SetItem(1, "gpa_pers", child_gpa);
                objDwDetail.AcceptText();
            }
            else if (columnName == "gpa_pers") {
                var gpa_pers = objDwDetail.GetItem(1, "gpa_pers");
                gpa_pers = (gpa_pers * 4) / 100;
                objDwDetail.SetItem(1, "child_gpa", gpa_pers);
                objDwDetail.AcceptText();
            }
            else if (columnName == "childprename_code") {
                var childprename_code = objDwDetail.GetItem(1, "childprename_code");
                if (childprename_code == "37" || childprename_code == "01") {
                    objDwDetail.SetItem(1, "child_sex", "M");
                }
                else {
                    objDwDetail.SetItem(1, "child_sex", "F");
                }
            }
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {
                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqschool" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwSchoolItemChange" ClientEventButtonClicked="DwSchoolButtonClick">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
</asp:Content>
