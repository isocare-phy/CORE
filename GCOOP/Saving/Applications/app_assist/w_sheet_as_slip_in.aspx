<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_slip_in.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_slip_in" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postRetreiveDwMem%>
    <%=postDelete%>
    <%=postRetrieveDwMain%>
    <%=postRefresh %>
    <%=postRetrieveBankBranch %>
    <%=postChangeAge%>
    <script type="text/javascript">
        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }
        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            try {
                objDwMem.SetItem(1, "member_no", memberno);
            } catch (ex) {
                objDwMem_public_funds.SetItem(1, "member_no", memberno);
            }

            Gcoop.GetEl("HfMemNo").value = memberno;
            Gcoop.GetEl("HdDwCode").value = "3";
            postRetreiveDwMem();
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
                sender.AcceptText();

                var member_no = objDwMem.GetItem(rowNumber, "member_no");
                if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ " + member_no)) {
                    postDelete();
                }
            }
        }
        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            try {
                objDwMem.SetItem(1, "member_no", memberno);
            } catch (ex) {
                objDwMem_public_funds.SetItem(1, "member_no", memberno);
            }
            Gcoop.GetEl("HfMemberNo").value = memberno;

            postRetreiveDwMem();
        }
        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;
            postRetrieveDwMain();
        }
        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            sender.SetItem(rowNumber, columnName, newValue);
            sender.AcceptText();
            if (columnName == "assist_amt") {
                //objDwSchool.SetItem(1, "assist_amt", newValue);
                postRefresh();
            }
            else if (columnName == "expense_bank") {
                sender.AcceptText();
                postRetrieveBankBranch();
            }
            else if (columnName == "req_tdate") {
                sender.AcceptText();
                Gcoop.GetEl("HdReqDate").value = newValue;
                postChangeAge();

            }
        }
        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            sender.SetItem(rowNumber, columnName, newValue);
            sender.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;

                postGetMemberDetail();
            }
        }
        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_dead_tdate") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;

                postChangeAmt();
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <%----as_mb_dead_normal----%>
    <asp:HiddenField ID="HdDwCode" runat="server" />
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HdReqDate" runat="server" />
    <%----as_shorlarship----%>
    <asp:HiddenField ID="HdDuplicate" runat="server" />
    <asp:HiddenField ID="HdNameDuplicate" runat="server" />
    <asp:HiddenField ID="HdActionStatus" runat="server" />
    <div>
        <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmember" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMem_public_funds" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_membmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange"
            TabIndex="100">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <div>
        <dw:WebDataWindowControl ID="DwMain_scholarship_mu" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_reqmaster_2" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain_public_funds" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain_mb_dead_normal" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain_family_dead" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwMain_enhance_quality_of_life" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClicked">
        </dw:WebDataWindowControl>
    </div>
    <div>
        <dw:WebDataWindowControl ID="DwDetail_scholarship_mu" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_reqschool_mu" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwSchoolItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwDetail_mb_dead_normal" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqmem_dead_normal" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwDetail_family_dead" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqfam_death" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwDetail_enhance_quality_of_life" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_reqenhance_quality_of_life" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwDetailButtonClicked" ClientEventItemChanged="DwDetailItemChange">
        </dw:WebDataWindowControl>
    </div>
</asp:Content>
