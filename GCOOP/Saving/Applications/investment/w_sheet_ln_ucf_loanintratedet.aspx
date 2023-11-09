<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_ucf_loanintratedet.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_ucf_loanintratedet" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postInsertRow%>
    <%=postDeleteRow%>
    <%=postLoanintrate%>
    <%=postSetEffective%>
    <%=postSetExpire%>
    <script type="text/javascript">
        function OnClickInsertRow() { //function เพิ่มแถวข้อมูล
            postInsertRow();
        }

        function OnDwMainItemChanged(sender, row, col, Value) {
            Gcoop.GetEl("Hd_row").value = row + "";
            if (col == "effective_tdate") {
                objDwMain.SetItem(row, "effective_tdate", Value);
                objDwMain.AcceptText();
                objDwMain.SetItem(row, "effective_date", Gcoop.ToEngDate(Value));
                objDwMain.AcceptText();
                postSetEffective();
            }
            else if (col == "expire_tdate") {
                objDwMain.SetItem(row, "expire_tdate", Value);
                objDwMain.AcceptText();
                objDwMain.SetItem(row, "expire_date", Gcoop.ToEngDate(Value));
                objDwMain.AcceptText();
                postSetExpire();
            }
        }
        function OnDwHeadItemChanged(sender, row, col, Value) {
            if (col == "loanintrate_code") {
                sender.SetItem(row, col, Value);
                sender.AcceptText();
                postLoanintrate();
            }
        }

        function Delete_ucf_district(sender, row, bName) { //function ลบแถวข้อมูล
            if (bName == "b_del") {
                var seq_no = objDwMain.GetItem(row, "seq_no");

                if (seq_no == "" || seq_no == null) {
                    Gcoop.GetEl("Hd_row").value = row + "";
                    postDeleteRow();
                }
                else {
                    var isConfirm = confirm("ต้องการลบข้อมูลลำดับที่ " + seq_no + " ใช่หรือไม่ ?");
                    if (isConfirm) {
                        Gcoop.GetEl("Hd_row").value = row + "";
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }

        function Validate() { //function เช็คค่าข้อมูลก่อน save
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล ?");

            if (!isconfirm) {
                return false;
            }
            var RowDetail = objDwMain.RowCount();
            var alertstr = "";

            for (var i = 1; i <= RowDetail; i++) {
                var effective_tdate = objDwMain.GetItem(i, "effective_tdate");
                var lower_amt = objDwMain.GetItem(i, "lower_amt");
                var upper_amt = objDwMain.GetItem(i, "upper_amt");
                var interest_rate = objDwMain.GetItem(i, "interest_rate");

                if (effective_tdate == "" || effective_tdate == null || effective_tdate == "00000000" || lower_amt == "" || lower_amt == null || upper_amt == "" ||
                 upper_amt == null || interest_rate == "" || interest_rate == null) {
                    alert("กรุณาระบุข้อมูลให้ครบถ้วน");
                    return false;
                }
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwHead" runat="server" DataWindowObject="dw_lc_lccfloanintratedet_head"
        LibraryList="~/DataWindow/investment/loan_ucf.pbl" ClientScriptable="True"
        ClientEventItemChanged="OnDwHeadItemChanged" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True">
    </dw:WebDataWindowControl>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_lc_lccfloanintratedet"
        LibraryList="~/DataWindow/investment/loan_ucf.pbl" ClientScriptable="True"
        ClientEventButtonClicked="Delete_ucf_district" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" RowsPerPage="20" ClientEventItemChanged="OnDwMainItemChanged">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="Hd_row" runat="server" />
</asp:Content>
