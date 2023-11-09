<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_scholarship_mu.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_scholarship_mu" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeHost %>
    <%=postCheckDate %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postRetrieveBankBranch %>
    <%=postGetMemberDetail %>
    <%=postCopyAddress%>
    <%=postMemProvince%>
    <%=postMemDistrict%>
    <%=postMemTambol%>
    <%=postMainProvince%>
    <%=postMainDistrict%>
    <%=postMainTambol%>
    <%=postNewClear %>
    <%=saveBranchId %>
    <%=postFilterScholarship %>
    <%=postGetLevelSchool%>
    <%=postGetMoney %>
    <%=postDepptacount%>
    <%=postToFromAccid%>
    <%=jsformatDeptaccid%>
    <%=jsformatIDCard%>
    <%=postSetBranch %>
    <%=postChildAge%>
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
            var capital_year = objDwMain.GetItem(1, "capital_year");
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(760, 600, "w_dlg_as_public_funds_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function DwDetailClick(sender, rowNumber, objectName) {
            if (objectName == "home_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "home_flag", 1, 0);
                postChangeHost();
            }
        }

        //        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code, memb_addr, addr_group, soi, mooban, road, tambol, district_code, province_code, postcode) {
        //            objDwMem.SetItem(1, "member_no", memberno);
        //            objDwMem.SetItem(1, "memb_addr", memb_addr);
        //            Gcoop.GetEl("HfMemberNo").value = memberno;
        //            postRetreiveDwMem();
        //        }
        function GetValueFromDlg(memberno) {
            objDwMem.SetItem(1, "member_no", memberno);
            //            objDwMem.SetItem(1, "memb_addr", memb_addr);
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
            if (columnName == "tambol_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainTambol();
            } else if (columnName == "district_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainDistrict()
            } else if (columnName == "province_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainProvince();
            }
            else if (columnName == "branch_id") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                saveBranchId();
            }
            else if (columnName == "moneytype_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postToFromAccid();
            }
            else if (columnName == "deptaccount_no") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                jsformatDeptaccid();
            }
            return 0;
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {
                Gcoop.GetEl("HfFocus").value = "1";
                postGetMemberDetail();
            } else if (columnName == "tambol_code") {
                postMemTambol();
            } else if (columnName == "district_code") {
                postMemDistrict()
            } else if (columnName == "province_code") {
                postMemProvince();
            } else if (columnName == "mate_name" || columnName == "mate_cardperson" || columnName == "mate_salaryid") {
                postMateName();
            }
            return 0;
        }

        function DwDamageItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "damage_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                postCheckDate();
            }
            if (columnName == "childbirth_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                postCheckDate();
            }
            return 0;
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function MenubarNew() {
            //window.location = "";
            postNewClear();
            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
            //Gcoop.SetLastFocus("member_no_0");
            //postNewClear();
            //return 0;
            //Gcoop.SetLastFocus("member_no_0");

        }
        function OnDwMainButtonClicked(s, r, c) {
            if (c == "b_copy") {
                postCopyAddress();
            }
            else if (c == "b_dept_search") {
                var member_no = objDwMem.GetItem(1, "member_no");
                Gcoop.OpenIFrame(580, 500, "w_dlg_as_deptaccount_no.aspx", "?member_no=" + member_no);
            }
        }
        function GetDeptAccountNo(deptaccount_no) {
            Gcoop.GetEl("Hfdeptaccount_no").value = deptaccount_no;
            postDepptacount();
        }
        //------------------------ ทุนศึกษา -------------------------------------------
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

        function DwSchoolItemChange(sender, rowNumber, columnName, newValue) {
            objDwDetail.SetItem(rowNumber, columnName, newValue);
            objDwDetail.AcceptText();
            var scholarship_type = objDwDetail.GetItem(1, "scholarship_type");
            var scholarship_level = objDwDetail.GetItem(1, "scholarship_level");
            if (columnName == "scholarship_type" || columnName == "scholarship_level") {
                if (scholarship_type != "" && scholarship_type != null) {
                    Gcoop.GetEl("Hdscholarship_type").value = newValue;
                    objDwDetail.AcceptText();
                    postGetMoney();
                }
            }
            else if (columnName == "level_school") {
                Gcoop.GetEl("Hdlevel_school").value = newValue;
                objDwDetail.AcceptText();
                postGetMoney();
                postFilterScholarship();
            }

            else if (columnName == "childbirth_tdate") {

                var childbirth = objDwDetail.GetItem(1, "childbirth_tdate");
                //Doys
                var newnewValue = newValue.replace("/", "");
                newnewValue = newnewValue.replace("/", "");
                newnewValue = newnewValue.replace("/", "");

                //                alert(newnewValue);

                var dddd = Gcoop.ParseInt(newnewValue.substr(0, 2));
                var mmmm = Gcoop.ParseInt(newnewValue.substr(2, 2));
                var yyyy = Gcoop.ParseInt(newnewValue.substr(4));
                var result = calcAge(dddd, mmmm, yyyy);
                //                alert(result.day + ":" + Gcoop.StringFormat(result.month, "00") + ":" + result.year);
                //Doys - end
                //                3. เช็คว่า อายุบุตรถึง 3 ปี หรือไม่ :ถ้าไม่ ให้ฟ้องเตือนว่า อายุบุตรไม่ถึง 3 ปี
                //                if (result.year < 3) { alert("อายุบุตรไม่ถึง 3 ปี"); return; }
                objDwDetail.SetItem(1, "child_age", result.year + "." + Gcoop.StringFormat(result.month, "00")); //LEK เปลี่ยนจากเดิม result.month ใหม่ Gcoop.StringFormat(result.month, "00")
                objDwDetail.AcceptText();
                postChildAge();
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
                if (childprename_code == "51" || childprename_code == "01") { //51 = ด.ช. || 01 = นาย 
                    objDwDetail.SetItem(1, "child_sex", "01"); //01 = ชาย
                }
                else {
                    objDwDetail.SetItem(1, "child_sex", "02"); //02 = หญิง
                }
            }
            else if (columnName == "child_card_person") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                jsformatIDCard();
            }
            else if (columnName == "moneytype_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                //if (newValue == "05") {
                //postDepptacount();
                //}
                postToFromAccid();
            }
            else if (columnName == "deptaccount_no") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                jsformatDeptaccid();
            }
            else if (columnName == "tofrom_accid") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postDepptacount();
            }
            else if (columnName == "expense_branch") {
                objDwDetail.AcceptText();
                objDwDetail.SetItem(rowNumber, "expense_branch", newValue);
                postSetBranch();
            }
            return 0;
        }
        function SheetLoadComplete() {
            var next = Gcoop.GetEl("HfFocus").value;
            if (next == "1") {
                Gcoop.SetLastFocus("moneytype_code_0");
                Gcoop.Focus();
            }
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {
                //                 alert(Gcoop.GetEl("HdIsPostBack").value);
                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
            }
        }

        $(function () {
            //            $('input[name="req_tdate_0"]').click(function () {
            //                $('input[name="req_tdate_0"]').val("")
            //            })

            //            $('input[name="childbirth_tdate_0"]').click(function () {
            //                $('input[name="childbirth_tdate_0"]').val("")
            //            })

            //            $('input[name="approve_tdate_0"]').click(function () {
            //                $('input[name="approve_tdate_0"]').val("")
            //            })
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <div>
        <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_membmaster_2" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange"
            TabIndex="100">
        </dw:WebDataWindowControl>
    </div>
    <%--<br />--%>
    <div>
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_reqmaster_2" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
    </div>
    <%--<br />--%>
    <div>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_reqschool_mu" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwSchoolItemChange" ClientEventButtonClicked="DwSchoolButtonClick">
        </dw:WebDataWindowControl>
    </div>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HdDuplicate" runat="server" />
    <asp:HiddenField ID="HdNameDuplicate" runat="server" />
    <asp:HiddenField ID="HdActionStatus" runat="server" />
    <asp:HiddenField ID="Hdscholarship_type" runat="server" />
    <asp:HiddenField ID="Hdlevel_school" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="HfFocus" runat="server" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
</asp:Content>
