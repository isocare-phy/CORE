<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_member_dead_normal_edit.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_member_dead_normal_edit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeAmt %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain%>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <%=postDelete%>
    <%=postChangeAge%>
    <%=postPopupReport%>
    <%=postDepptacount%>
    <%=postToFromAccid%>
    <%=jsformatDeptaccid%>
    <script type="text/javascript">

        function Validate() {

            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(630, 550, "w_dlg_as_memberdead_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function DwDetailButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "b_del") {
                objDwMain.AcceptText();
                var member_no = objDwMem.GetItem(rowNumber, "member_no");
                if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ " + member_no)) {
                    postDelete();
                }
            }
        }
        function OnDwMainButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "b_dept_search") {
                var member_no = objDwMem.GetItem(1, "member_no");
                Gcoop.OpenIFrame(580, 500, "w_dlg_as_deptaccount_no.aspx", "?member_no=" + member_no);
            }
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemberNo").value = memberno;
            Gcoop.GetEl("HdOpenIFrame").value = "false";
            postRetreiveDwMem();
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;
            Gcoop.GetEl("HdOpenIFrame").value = "false";
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
            else if (columnName == "member_dead_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                Gcoop.GetEl("HfFocus").value = "1";
                postChangeAmt();
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
                Gcoop.GetEl("HdOpenIFrame").value = "false";
                Gcoop.GetEl("HfFocus").value = "1";
                postGetMemberDetail();
            }
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "member_dead_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                Gcoop.GetEl("HfFocus").value = "1";
                postChangeAmt();
            } else if (columnName == "moneytype_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                //if (newValue == "05") {
                //postDepptacount();
                //}
                postToFromAccid();
            } else if (columnName == "deptaccount_no") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                jsformatDeptaccid();
            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }

        function MenubarNew() {
            window.location = "";
            Gcoop.SetLastFocus("member_no_0");
        }
        //        function SheetLoadComplete() {
        //            Gcoop.SetLastFocus("member_no_0");
        //            Gcoop.Focus();
        //            var OpenIFrame = Gcoop.GetEl("HdOpenIFrame").value
        //            var member_no = objDwMem.GetItem(1, "member_no");
        //            if (OpenIFrame == "true") {
        //                Gcoop.GetEl("HdOpenIFrame").value = "false";
        //                Gcoop.OpenIFrame(510, 300, "w_dlg_as_senior_assist_paylist.aspx", "?member_no=" + member_no);
        //            }
        //        }
        function OnClickLinkNext() {
            postPopupReport();
        }
        function SheetLoadComplete() {
            var next = Gcoop.GetEl("HfFocus").value;
            if (next == "1") {
                Gcoop.SetLastFocus("addr_no_0");
                Gcoop.Focus();
            }
        }

        //function formatIDCard(number) {
        //var N1 = number.substring(0, 1);
        //var N2 = number.substring(1, 5);
        //var N3 = number.substring(6, 10);
        //var N4 = number.substring(10, 12);
        //var N5 = number.substring(12);
        //var valueReturn = N1 + "-" + N2 + "-" + N3 + "-" + N4 + "-" + N5;
        //return valueReturn
        //}
        //function formatAccNo(number) {
        //var N1 = number.substring(0, 2);
        //var N2 = number.substring(2, 5);
        //var N3 = number.substring(5, 6);
        //var N4 = number.substring(6, 11);
        //var N5 = number.substring(11, 12);
        //var valueReturn = N1 + "-" + N2 + "-" + N3 + "-" + N4 + "-" + N5;
        //return valueReturn
        //}

        $(function () {
            //            $('input[name="entry_tdate_0"]').click(function () {
            //                $('input[name="entry_tdate_0"]').val("")
            //            })

            //            $('input[name="req_tdate_0"]').click(function () {
            //                $('input[name="req_tdate_0"]').val("")
            //            })

            //            $('input[name="member_dead_tdate_0"]').click(function () {
            //                $('input[name="member_dead_tdate_0"]').val("")
            //            })
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"> </asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <%--<table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer" onclick="OnClickLinkNext();">พิมพ์ใบสำคัญจ่าย </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>--%>
    <div>
        <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmember" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
        </dw:WebDataWindowControl>
    </div>
    <%--<br />--%>
    <div>
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
        </dw:WebDataWindowControl>
    </div>
    <%--<br />--%>
    <div>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmem_dead_normal" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
        </dw:WebDataWindowControl>
    </div>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HfDate" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HdReqDate" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="HfFocus" runat="server" />
</asp:Content>
