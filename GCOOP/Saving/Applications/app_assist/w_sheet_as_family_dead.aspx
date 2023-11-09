<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_family_dead.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_family_dead" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeAmt %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postChangeHeight %>
    <%=postChangeType %>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <%=postDelete%>
    <%=postChangeAge%>
    <%=postPopupReport%>
    <%=postDepptacount%>
    <%=postToFromAccid%>
    <%=jsformatIDCard%>
    <%=jsformatDeptaccid%>
    <%=postSetBranch %>
    <%=postMateName%>
    <script type="text/javascript">

        function Validate() {

            var sumdate = Gcoop.GetEl("Hfsumdate").value;
            if (sumdate >= 365.00) {
                alert("ไม่สามารถบันทึกการทำรายการได้เนื่องจากเกิน 365 วัน");
            }
            else {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }

        }


        function MenubarOpen() {
            Gcoop.OpenDlg(655, 550, "w_dlg_as_family_dead_list.aspx", "");
            //Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
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
            //Gcoop.GetEl("HfMemNo").value = member_no;

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
            else if (columnName == "moneytype_code") {
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

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {

            if (columnName == "family_type") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("family").value = newValue;

                postChangeAmt();
            }
            else if (columnName == "marriage_dead_tdate" || columnName == "family_dead_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                postChangeAmt();
            }
            else if (columnName == "card_person") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                jsformatIDCard();
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
            } else if (columnName == "expense_branch") {
                objDwDetail.AcceptText();
                objDwDetail.SetItem(rowNumber, "expense_branch", newValue);
                postSetBranch();
            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function MenubarNew() {
            window.location = "";
            Gcoop.SetLastFocus("member_no_0");
        }
        function SheetLoadComplete() {
            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
        }
        function OnClickLinkNext() {
            postPopupReport();
        }

        $(function () {
//            $('input[name="entry_tdate_0"]').click(function () {
//                $('input[name="entry_tdate_0"]').val("")
//            })

//            $('input[name="req_tdate_0"]').click(function () {
//                $('input[name="req_tdate_0"]').val("")
//            })

//            $('input[name="family_dead_tdate_0"]').click(function () {
//                $('input[name="family_dead_tdate_0"]').val("")
//            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
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
    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmember_child" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
    <%-- <br />--%>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmaster_child" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqfam_death" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="family" runat="server" />
    <asp:HiddenField ID="HdReqDate" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="Hfsumdate" runat="server" />
</asp:Content>
