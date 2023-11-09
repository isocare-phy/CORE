<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_print_loancontrct.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_print_loancontrct" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=GetLoanReqData%>
    <%=jspostMainChanged%>
    <%=jspostCollChanged%>
    <%=jsReport%>
    <%=jsReport2%>
    <%=jsReport3%>
    <script type="text/JavaScript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function GetLoanReq(loanrequest_docno, member_no, loanrequest_status, loantype_code) {
            objDwMain.SetItem(1, "loanrequest_docno", loanrequest_docno);
            objDwMain.SetItem(1, "loantype_code", loantype_code);
            objDwMain.AcceptText();
            Gcoop.GetEl("Hfdoc_no").value = loanrequest_docno;
            Gcoop.GetEl("HdloanType").value = loantype_code;
            GetLoanReqData();
        }

        function OnLnReqChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();

            if (c == "loanrequest_docno") {
                Gcoop.GetEl("Hfdoc_no").value = v;
                GetLoanReqData();
            }
        }

        function OnLnReqKick(sender, row, bName) {

            if (bName == "b_slnreq") {
                Gcoop.OpenIFrame("660", "350", "w_dlg_open_loanreq.aspx", "");
            }
        }

        function OnContSignlist1Insert() {
            objDwSignlist1.InsertRow(0);
        }

        function OnContSignlist2Insert() {
            objDwSignlist2.InsertRow(0);
        }

        function OnContCollInsert() {
            objDwColl.InsertRow(0);
        }

        function OnSignlist1Kicked(sender, row, bName) {
            if (bName == "b_del") {
                if (confirm("ยืนยันการลบแถว")) {
                    Gcoop.GetEl("HdRow").value = row;
                    sender.DeleteRow(row);
                }
            }
        }

        function OnSignlist2Kicked(sender, row, bName) {
            if (bName == "b_del") {
                if (confirm("ยืนยันการลบแถว")) {
                    Gcoop.GetEl("HdRow").value = row;
                    sender.DeleteRow(row);
                }
            }
        }

        function OnContCollChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "birth_tdate") {
                jspostCollChanged();
            }
        }

        function OnContCollKicked(sender, row, bName) {
            if (bName == "b_del") {
                if (confirm("ยืนยันการลบแถว")) {
                    Gcoop.GetEl("HdRow").value = row;
                    sender.DeleteRow(row);
                }
            }
        }

        function OnclickReport2() {
            jsReport2();
        }

        function OnclickReport3() {
            jsReport3();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenReport").value == "True") {
                if (confirm("ต้องการพิมพ์ใบรายการ ?")) {
                    Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                    Gcoop.GetEl("HdOpenReport").value = "false";
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="OnclickReport2()" style="cursor: pointer; margin-left: 0;">
        <asp:Label ID="Label3" runat="server" Text="หนังสือสัญญากู้เงิน" Font-Bold="False"
            Font-Names="Tahoma" Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <br />
    <span onclick="OnclickReport3()" style="cursor: pointer; margin-left: 0;">
        <asp:Label ID="Label4" runat="server" Text="หนังสือสัญญาค้ำประกันเงินกู้" Font-Bold="False"
            Font-Names="Tahoma" Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcsrv_print_loancontrct"
        LibraryList="~/DataWindow/investment/print_loancontrct.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnLnReqChanged"
        ClientEventButtonClicked="OnLnReqKick">
    </dw:WebDataWindowControl>
    <br />
    <span onclick="OnContSignlist1Insert()" style="cursor: pointer; margin-left: 0;">
        <asp:Label ID="Label5" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwSignlist1" runat="server" DataWindowObject="d_lcsrv_print_loancontrctsignlist1"
        LibraryList="~/DataWindow/investment/print_loancontrct.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnSignlist1Changed"
        ClientEventButtonClicked="OnSignlist1Kicked" RowsPerPage="10">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <br />
    <span onclick="OnContSignlist2Insert()" style="cursor: pointer; margin-left: 0;">
        <asp:Label ID="Label2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwSignlist2" runat="server" DataWindowObject="d_lcsrv_print_loancontrctsignlist2"
        LibraryList="~/DataWindow/investment/print_loancontrct.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnSignlist2Changed"
        ClientEventButtonClicked="OnSignlist2Kicked" RowsPerPage="10">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <br />
    <span onclick="OnContCollInsert()" style="cursor: pointer; margin-left: 0;">
        <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwColl" runat="server" DataWindowObject="d_lcsrv_print_loancontrctcoll"
        LibraryList="~/DataWindow/investment/print_loancontrct.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnContCollChanged"
        ClientEventButtonClicked="OnContCollKicked" RowsPerPage="10">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="Hfdoc_no" runat="server" />
    <asp:HiddenField ID="HdRow" runat="server" />
    <asp:HiddenField ID="HdOpenReport" runat="server" />
    <asp:HiddenField ID="HdloanType" runat="server" />
</asp:Content>
