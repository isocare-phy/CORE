<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_health_member.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_health_member" %>

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
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            objDwMain.AcceptText();
            objDwDetail.AcceptText();

            var member_no = objDwMem.GetItem(1, "member_no");

            var capital_year = objDwMain.GetItem(1, "capital_year");
            var salary_amt = objDwMain.GetItem(1, "salary_amt");
            var assist_amt = objDwDetail.GetItem(1, "assist_amt");
            var req_date = objDwDetail.GetItem(1, "req_date");
            var start_heal_date = Gcoop.GetEl("Hdstart_heal_date").value;
            var end_heal_date = Gcoop.GetEl("Hdend_heal_date").value;
            var member_age = objDwDetail.GetItem(1, "member_age");

            if (member_no != null && capital_year != null && assist_amt != null && req_date != null) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                alert("กรุณากรอกข้อมูลให้ครบ");
            }

        }

        function MenubarOpen() {
            Gcoop.OpenDlg(690, 550, "w_dlg_as_health_member_list.aspx", "");
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
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {

                newValue = Gcoop.StringFormat(newValue, "00000000");

                Gcoop.GetEl("HfMemberNo").value = newValue;
                postGetMemberDetail();
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
            if (columnName == "start_heal_tdate") {
                objDwDetail.AcceptText();
                Gcoop.GetEl("Hdstart_heal_date").value = newValue;
            }
            else if (columnName == "end_heal_tdate") {
                objDwDetail.AcceptText();
                Gcoop.GetEl("Hdend_heal_date").value = newValue;
            }
        }
        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function OnClickLinkNext() {
            postPopupReport();
        }
        function MenubarNew() {
            window.location = "";
            Gcoop.SetLastFocus("member_no_0");
        }
        function SheetLoadComplete() {
            var next = Gcoop.GetEl("HfFocus").value;
            if (next == "1") {
                Gcoop.SetLastFocus("addr_no_0");
                Gcoop.Focus();
            }
            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td align="left">
                <%--<span style="cursor: pointer" onclick="OnClickLinkNext();">พิมพ์ใบสำคัญจ่าย </span>--%>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>
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
        DataWindowObject="d_as_health_member" ClientScriptable="True" AutoRestoreContext="false"
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
    <asp:HiddenField ID="Hdstart_heal_date" runat="server" />
    <asp:HiddenField ID="Hdend_heal_date" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="HfFocus" runat="server" />
</asp:Content>
