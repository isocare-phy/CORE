<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_cancel_expenses.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_cancel_expenses" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postSearchList %>
    <%=postApproveExpenses %>
    <%=postSetBranch %>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwTypeItemClicked(sender, rowNumber, buttonName) {
            if (buttonName == "btn_edit") {
                objDwType.AcceptText();
                postSearchList();
            }
        }

        function DwMainBtnClick(sender, rowNumber, buttonName) {
            var rowTotal = objDwMain.RowCount();
            var cbxselect_flag;

            if (buttonName == "btn_expenses") {
                objDwMain.AcceptText();
                postApproveExpenses();
            }

            if (buttonName == "b_search") {
                objDwMain.AcceptText();
                var bank_code = Gcoop.GetEl("HdExpense_bank").value;
                Gcoop.OpenIFrame(300, 300, "w_dlg_as_bankbranch.aspx", "?bank_code=034"); //HC bank_code = ธกส. MiW
            }

            if (buttonName == "b_con") {
                for (var j = 1; j <= rowTotal; j++) {
                    cbxselect_flag = objDwMain.GetItem(j, "cbxselect_flag");
                    if (cbxselect_flag == 1) {
                        objDwMain.SetItem(j, "pay_status", "1");
                    }
                }

                objDwMain.AcceptText();
            }
            else if (buttonName == "b_can") {
                for (var j = 1; j <= rowTotal; j++) {
                    cbxselect_flag = objDwMain.GetItem(j, "cbxselect_flag");
                    if (cbxselect_flag == 1) {
                        objDwMain.SetItem(j, "pay_status", "-1");
                    }
                }

                objDwMain.AcceptText();
            }
            else if (buttonName == "b_not") {
                for (var j = 1; j <= rowTotal; j++) {
                    cbxselect_flag = objDwMain.GetItem(j, "cbxselect_flag");
                    if (cbxselect_flag == 1) {
                        objDwMain.SetItem(j, "pay_status", "-11");
                    }
                }

                objDwMain.AcceptText();
            }
        }

        function DwMainEventClick(sender, rowNumber, column) {
            if (column == "txt_deptaccount_no") {
                var deptaccount_no = objDwMain.GetItem(rowNumber, "deptaccount_no");
                alert(deptaccount_no);
            }
        }

        function DwTypeItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "member_no") {
                postSearchList();
            }
        }

        function DwMainItemChange(s, r, c, v) {
            if (c == "expense_bank") {
                objDwMain.AcceptText();
                Gcoop.GetEl("HdExpense_bank").value = v;
            }
            else if (c == "expense_branch") {
                objDwMain.AcceptText();
                objDwMain.SetItem(r, "expense_branch", v);
                postSetBranch();
            }
        }

        $(function () {
            var rowTotal = objDwMain.RowCount();
            var str = "sum_amt_", strtemp;

            for (var j = 1; j <= rowTotal; j++) {
                strtemp = str + (j - 1);
                SetDisable("input[name=" + strtemp + "]", true);
            }
        });

        function ChkAllClick(checkbox) {
            var rowTotal = objDwMain.RowCount();
            if (checkbox.checked) {
                for (var j = 1; j <= rowTotal; j++) {
                    objDwMain.SetItem(j, "cbxselect_flag", "1");
                }
            } else {
                for (var j = 1; j <= rowTotal; j++) {
                    objDwMain.SetItem(j, "cbxselect_flag", "-1");
                }
            }

            objDwMain.AcceptText();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwType" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_cancel_type_expenses" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwTypeItemClicked" ClientEventItemChanged="DwTypeItemChange">
    </dw:WebDataWindowControl>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="500px">
        <asp:CheckBox ID="ChkAll" runat="server"  Text="ยกเลิกจ่ายทั้งหมด"
             onclick="ChkAllClick(this)" />
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_cancel_expenses" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            RowsPerPage="20" ClientEventButtonClicked="DwMainBtnClick" ClientEventClicked="DwMainEventClick"
            HorizontalScrollBar="Auto" ClientEventItemChanged="DwMainItemChange">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HdExpense_bank" runat="server" />
</asp:Content>
