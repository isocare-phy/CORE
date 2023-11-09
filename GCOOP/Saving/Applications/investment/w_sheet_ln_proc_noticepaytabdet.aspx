<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_proc_noticepaytabdet.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_proc_noticepaytabdet" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostLncontNo%>
    <%=jsPostGen%>
    <%=JsReport %>
    <script type="text/JavaScript">
        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen(sender, row, bName) {
            Gcoop.OpenIFrame("600", "450", "w_dlg_member_search_loancontractno.aspx", "");
        }

        function PostLoancontractNo(member_no, loancontract_no) {
            objDwMain.SetItem(1, "loancontract_no", loancontract_no);
            objDwMain.AcceptText();
            jsPostLncontNo();
        }

        function OnDwMainItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            if (col == "loancontract_no") {
                jsPostLncontNo();
            }
        }

        function OnGenClick() {
            var loancontract_no = objDwMain.GetItem(1, "loancontract_no");
            if (loancontract_no != "" && loancontract_no != null) {
                jsPostGen();
            }
            else {
                alert("กรุณาระบุเลขสัญญาก่อน");
            }
        }

        function OnClickReport() {
            JsReport();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                if (confirm("ต้องการพิมพ์ใบรายการ ?")) {
                    Gcoop.OpenIFrame("220", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                    Gcoop.GetEl("HdOpenIFrame").value = "false";
                }

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcsrv_detail_contract"
        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <input id="B_gen" type="button" value="Gen ตารางการรับชำระ" onclick="OnGenClick()" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td>
                <asp:Panel ID="Panel6" runat="server" Visible="False">
                    <input id="B_print" type="button" value="พิมพ์ตารางการรับชำระ" onclick="OnClickReport()" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="Label3" runat="server" Text="ตารางการรับชำระ" Font-Bold="False" Font-Size="Larger"
        ForeColor="#000000" Font-Underline="False" />
    <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_lcsrv_proc_noticepaytabdet"
        LibraryList="~/DataWindow/investment/loan_planrcv_money.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwListItemChanged"
        ClientEventButtonClicked="OnDwListButtonClicked" ClientEventClicked="OnDwListClicked"
        RowsPerPage="12">
        <PageNavigationBarSettings NavigatorType="Numeric" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="HdContno" runat="server" />
</asp:Content>
