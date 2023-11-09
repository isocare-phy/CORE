<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_share_edit.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_share_edit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <%=jsPostYear%>
    <%=runProcess%>
    <%=popupReport%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function DwMainItemChange(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "member_no") {
                v = Gcoop.StringFormat(v, "00000000");
                s.SetItem(r, c, v);
                s.AcceptText();
                Gcoop.GetEl("HdMemberNo").value = v;
                Gcoop.SetLastFocus("shr_year_0");
                jsPostMemberNo();
            }
            if (c == "shr_year") {
                jsPostYear();
            }
        }

        function DwMainClick(s, r, c) {
            if (c == "b_search") {
                jsPostYear();
            }
            else if (c == "b_report") {
                runProcess();
            }
        }

        function SheetLoadComplete() {

            var focus = Gcoop.GetEl("HdFocus").value;
            if (focus == "") {
                Gcoop.GetEl("HdFocus").value = "";
                Gcoop.Focus("member_no_0");
            }
            else if (focus == "1") {
                Gcoop.GetEl("HdFocus").value = "";
                Gcoop.Focus("shr_year_0");
            }
            else if (focus == "3") {
                Gcoop.GetEl("HdFocus").value = "";
                Gcoop.Focus("shr_value_0");
            }
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                Gcoop.GetEl("HdOpenIFrame").value == "False"
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_cri_shr_edit" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange" ClientEventButtonClicked="DwMainClick">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
        DataWindowObject="d_asnshareperiod_edit" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        TabIndex="100">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HdCheck" runat="server" />
    <asp:HiddenField ID="HdRule" runat="server" />
    <asp:HiddenField ID="HdFocus" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
</asp:Content>
