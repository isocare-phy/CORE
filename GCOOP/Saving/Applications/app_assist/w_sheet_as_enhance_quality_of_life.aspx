<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_enhance_quality_of_life.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_enhance_quality_of_life" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <%=postHoardCase%>
    <%=postCheckYear%>
    <%=postDelete%>
    <%=postChangeAge%>
    <%=postPopupReport%>
    <%=postChangeAmt%>
    <%=postDepptacount%>
    <%=postToFromAccid%>
    <%=postChildAge%>
    <%=jsformatDeptaccid%>
    <%=jsformatIDCard%>
    <%=postSetBranch %>
    <%=postMateName%>
    <script type="text/javascript">
        function calcAge(date, month, year) {
            month = month - 1;
            year = year - 543;

            today = new Date();
            dateStr = today.getDate();
            monthStr = today.getMonth();
            yearStr = today.getFullYear();

            theYear = yearStr - year;
            theMonth = monthStr - month;
            theDate = dateStr - date;

            var days = "";
            if (monthStr == 0 || monthStr == 2 || monthStr == 4 || monthStr == 6 || monthStr == 7 || monthStr == 9 || monthStr == 11) days = 31;
            if (monthStr == 3 || monthStr == 5 || monthStr == 8 || monthStr == 10) days = 30;
            if (monthStr == 1) days = 28;

            inYears = theYear;

            if (month < monthStr && date > dateStr) {
                inYears = parseInt(inYears) + 1;
                inMonths = theMonth - 1;
            };

            if (month < monthStr && date <= dateStr) {
                inMonths = theMonth;
            } else if (month == monthStr && (date < dateStr || date == dateStr)) {
                inMonths = 0;
            } else if (month == monthStr && date > dateStr) {
                inMonths = 11;
            } else if (month > monthStr && date <= dateStr) {
                inYears = inYears - 1;
                inMonths = ((12 - -(theMonth)) + 1);
            } else if (month > monthStr && date > dateStr) {
                inMonths = ((12 - -(theMonth)));
            };

            if (date < dateStr) {
                inDays = theDate;
            } else if (date == dateStr) {
                inDays = 0;
            } else {
                inYears = inYears - 1;
                inDays = days - (-(theDate));
            };

            var result = ['day', 'month', 'year'];
            result.day = inDays;
            result.month = inMonths;
            result.year = inYears;

            return result;
        }

        function Validate() {
            objDwMain.AcceptText();
            objDwMain.AcceptText();
            objDwDetail.AcceptText();

            var member_no = objDwMem.GetItem(1, "member_no");

            var capital_year = objDwMain.GetItem(1, "capital_year");
            var salary_amt = objDwMain.GetItem(1, "salary_amt");
            var assist_amt = objDwDetail.GetItem(1, "assist_amt");
            var req_date = objDwDetail.GetItem(1, "req_date");
            var child_age = objDwDetail.GetItem(1, "child_age");
            if (child_age <= 4.00) {
                if (member_no != null && capital_year != null && assist_amt != null && req_date != null) {
                    return confirm("ยืนยันการบันทึกข้อมูล");
                }
                else {
                    alert("กรุณากรอกข้อมูลให้ครบ");
                }
            } else { alert("ยื่นคำขอรับทุน  เกินกำหนด  120 วัน"); }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(565, 550, "w_dlg_as_enchance_quality_of_life_list.aspx", "");
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
            else if (columnName == "req_tdate") {
                objDwMain.AcceptText();
                Gcoop.GetEl("HdReqDate").value = newValue;
                postChangeAge();
            }
            else if (columnName == "paytype_code") {
                //                if (newValue == "05") {
                //                    postDepptacount();
                //                }
                postToFromAccid();
            }
            else if (columnName == "deptaccount_no") {
                jsformatDeptaccid();
            }
        }
        function OnDwMainButtonClicked(s, r, c) {
            if (c == "b_dept_search") {
                var member_no = objDwMem.GetItem(1, "member_no");
                Gcoop.OpenIFrame(580, 500, "w_dlg_as_deptaccount_no.aspx", "?member_no=" + member_no);
            }
        }
        function GetDeptAccountNo(deptaccount_no) {
            Gcoop.GetEl("Hfdeptaccount_no").value = deptaccount_no;
            postDepptacount();
        }
        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {

                newValue = Gcoop.StringFormat(newValue, "00000000");

                Gcoop.GetEl("HfMemberNo").value = newValue;
                postGetMemberDetail();
            } else if (columnName == "mate_name" || columnName == "mate_cardperson" || columnName == "mate_salaryid") {
                postMateName();
            }
        }
        function DwDamageItemClicked(sender, rowNumber, buttonName) {
            if (buttonName == "b_del") {
                objDwMain.AcceptText();
                var member_no = objDwMem.GetItem(rowNumber, "member_no");
                if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ " + member_no)) {
                    postDelete();
                }
            }
        }
        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
            if (columnName == "enhance_case") {
                postChangeAmt();
            }
            if (columnName == "child_sex") {
                if (newValue == "01") { objDwDetail.SetItem(rowNumber, "childprename_code", "51"); }
                else { objDwDetail.SetItem(rowNumber, "childprename_code", "52"); }
            }
            if (columnName == "childprename_code") {
                if (newValue == "51") { objDwDetail.SetItem(rowNumber, "child_sex", "01"); }
                else { objDwDetail.SetItem(rowNumber, "child_sex", "02"); }
            }
            if (columnName == "child_birth_tday") {
//                alert(Gcoop.GetEl("HfChildBirthTDay").value);
                Gcoop.GetEl("HfChildBirthTDay").value = newValue;
                postChildAge();
                //Doys
                var newnewValue = newValue.replace("/", "");
                newnewValue = newnewValue.replace("/", "");
                newnewValue = newnewValue.replace("/", "");

//                alert(newnewValue);

                var dddd = Gcoop.ParseInt(newnewValue.substr(0, 2));
                var mmmm = Gcoop.ParseInt(newnewValue.substr(2, 2));
                var yyyy = Gcoop.ParseInt(newnewValue.substr(4));
//                var child_age = calcAge(dddd, mmmm, yyyy);
                var child_age = objDwDetail.GetItem(1, "child_years_disp");
//                alert(child_age);
//                if (child_age > 4) { alert("ยื่นคำขอรับทุน  เกินกำหนด  120 วัน"); }
            }
            if (columnName == "child_card_person") {
                if ($('input[name="child_card_person_0"]').val().length != 13) {
                    alert("กรุณากรอกเลขที่ บปช.ของบุตรให้ถูกต้อง");
                } else {
                    jsformatIDCard();
                }
            }
            if (columnName == "moneytype_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                //if (newValue == "05") {
                //postDepptacount();
                //}
                postToFromAccid();
            }
            if (columnName == "deptaccount_no") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                jsformatDeptaccid();
            }
            else if (columnName == "expense_branch") {
                objDwDetail.AcceptText();
                objDwDetail.SetItem(rowNumber, "expense_branch", newValue);
                postSetBranch();
            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function OnClickLinkNext() {
            postPopupReport();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {
                //                 alert(Gcoop.GetEl("HdIsPostBack").value);
                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
        }

        $(function () {
            //            $('input[name="entry_tdate_0"]').click(function () {
            //                $('input[name="entry_tdate_0"]').val("")
            //            })

            //            $('input[name="req_tdate_0"]').click(function () {
            //                $('input[name="req_tdate_0"]').val("")
            //            })

            //            $('input[name="child_birth_tday_0"]').click(function () {
            //                $('input[name="child_birth_tday_0"]').val("")
            //            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <%-- <table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer" onclick="OnClickLinkNext();">พิมพ์ใบสำคัญจ่าย
                    </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>--%>
    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmember_child" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmaster_child" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqenhance_quality_of_life" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwDamageItemClicked" ClientEventItemChanged="DwDetailItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HdfHoardTdate" runat="server" />
    <asp:HiddenField ID="HdReqDate" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="HfChildBirthTDay" runat="server" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
</asp:Content>
