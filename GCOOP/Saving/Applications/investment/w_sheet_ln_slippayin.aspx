<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_slippayin.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_slippayin" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <%=jsPostMoneytype%>
    <%=jsPostFlag%>
    <%=jsPostPayAmt%>
    <%=jsPostEtcDel%>
    <%=jsPostEtcChange%>
    <%=jsChangeOperatedate%>
    <%=jsPostInsertPayInExp%>
    <%=jspostSetOperateDate%>
    <%=jsPostLPMValue%>
    <script type="text/JavaScript">

        function Validate() {
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล ?");

            if (!isconfirm) {
                return false;
            }
            var RowDwPayInExp = objDwPayInExp.RowCount();
            var bool_moneytype_code = false;
            var bool_finbankacc_no = false;
            var bool_cheque_bank = false;
            var bool_cheque_tdate = false;
            var bool_cheque_no = false;
            var bool_slipitem_desc = false;
            var total_expense_amt = 0.00;
            var alertstr = "";

            var member_no = objDwMain.GetItem(1, "member_no");
            if (member_no == "" || member_no == null) {
                alertstr += "กรุณาระบุรหัสสมาชิก\n";
            }

            var operate_tdate = objDwPayIn.GetItem(1, "operate_tdate");
            if (operate_tdate == "" || operate_tdate == null || operate_tdate == "00000000") {
                alertstr = alertstr + "กรุณาระบุวันที่รายการ\n";
            }

            var slip_amt = objDwPayIn.GetItem(1, "slip_amt");
            if (slip_amt == 0) {
                alertstr = alertstr + "กรุณาทำรายการชำระ\n";
            }
            else if (slip_amt > 0) {
                for (var i = 1; i <= RowDwPayInExp; i++) {
                    var moneytype_code = objDwPayInExp.GetItem(i, "moneytype_code");
                    var finbankacc_no = objDwPayInExp.GetItem(i, "finbankacc_no");
                    var cheque_bank = objDwPayInExp.GetItem(i, "cheque_bank");
                    var cheque_tdate = objDwPayInExp.GetItem(i, "cheque_tdate");
                    var cheque_no = objDwPayInExp.GetItem(i, "cheque_no");
                    if ((moneytype_code == "" || moneytype_code == null || moneytype_code == "-1") && !bool_moneytype_code) {
                        alertstr = alertstr + "กรุณาเลือกประเภทเงิน\n";
                        bool_moneytype_code = true;
                    }
                    else if (moneytype_code == "CBT") {
                        if ((finbankacc_no == "" || finbankacc_no == null) && !bool_finbankacc_no) {
                            alertstr = alertstr + "กรุณาระบุบัญชีโอนเข้า\n";
                            bool_finbankacc_no = true;
                        }
                    }
                    else if (moneytype_code == "CHQ") {
                        if ((cheque_bank == "" || cheque_bank == null) && !bool_cheque_bank) {
                            alertstr = alertstr + "กรุณาระบุธนาคาร\n";
                            bool_cheque_bank = true;
                        }
                        if ((cheque_tdate == "" || cheque_tdate == null || cheque_tdate == "00000000") && !bool_cheque_tdate) {
                            alertstr = alertstr + "กรุณาระบุวันที่เช็ค\n";
                            bool_cheque_tdate = true;
                        }
                        if ((cheque_no == "" || cheque_no == null) && !bool_cheque_no) {
                            alertstr = alertstr + "กรุณาระบุเลขที่เช็ค\n";
                            bool_cheque_no = true;
                        }
                    }
                    var expense_amt = objDwPayInExp.GetItem(i, "expense_amt");
                    total_expense_amt += expense_amt;
                }
                if (slip_amt != total_expense_amt) {
                    alertstr = alertstr + "กรุณาระบุยอดเงินรายการให้ตรงกับยอดทำรายการ\n";
                }
            }

            var row = objDwPayInEtc.RowCount();
            for (var i = 1; i <= row; i++) {
                var slipitemtype_code = objDwPayInEtc.GetItem(i, "slipitemtype_code");
                var slipitem_desc = objDwPayInEtc.GetItem(i, "slipitem_desc");
                if (slipitemtype_code == "" || slipitemtype_code == null) {
                    alertstr += "กรุณาระบุรหัสรายการแถวที่ " + i + "\n";
                }
                if (slipitem_desc == "" || slipitem_desc == null) {
                    alertstr += "กรุณาระบุรายละเอียดแถวที่ " + i + "\n";
                }
                if (objDwPayOutEtc.GetItem(i, "item_payamt") == 0) {
                    alertstr += "กรุณาระบุยอดชำระแถวที่ " + i + "\n";
                }
            }

            if (alertstr == "") {

                return true;
            }
            else {
                alert(alertstr);
                return false;
            }
        }

        function OnDwMainItemChanged(sender, row, col, value) {
            if (col == "member_no") {
                sender.SetItem(row, col, value);
                sender.AcceptText();
                Gcoop.GetEl("Hdmemno").value = value;
                jsPostMemberNo();
            }
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_member_search_new.aspx", "");
        }

        function ReceiveMemberNo(member_no, memb_name) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMemberNo();
        }

        function OnClickInsertRowPayInExp() {
            jsPostInsertPayInExp();
        }

        function OnDwMoneytypeChanged(sender, row, col, value) {
            if (col == "moneytype_code") {
                sender.SetItem(row, col, value);
                sender.AcceptText();
                Gcoop.GetEl("Hd_row").value = row + "";
                jsPostMoneytype();
            }
        }

        function DwPayInExpButtonClicked(sender, row, bName) {
            if (bName == "b_del") {
                var isConfirm = confirm("ต้องการลบแถวที่ " + row + " ใช่หรือไม่ ?");
                if (isConfirm) {
                    objDwPayInExp.DeleteRow(row);
                }
            }
            else if (bName == "b_bank") {
                if (objDwPayInExp.GetItem(row, "moneytype_code") == "CHQ") {
                    Gcoop.GetEl("Hd_row").value = row;
                    Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + row);
                }
            }
        }

        function GetDlgBankAndBranch(sheetRow, bankCode, bankDesc, branchCode, branchDesc) {
            var row = Gcoop.GetEl("Hd_row").value;

            objDwPayInExp.SetItem(row, "cheque_bank", bankCode);
            objDwPayInExp.SetItem(row, "cheque_branch", branchCode);
            objDwPayInExp.SetItem(row, "bank_desc", Gcoop.Trim(bankDesc) + " " + branchDesc);
        }

        function OnDwPayInLoanItemChange(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            Gcoop.GetEl("Hd_row").value = row;
            Gcoop.GetEl("HdLnCol").value = col;
            switch (col) {
                case "operate_flag":
                    jsPostFlag();
                    break;
                case "principal_payamt":
                    jsPostPayAmt();
                    break;
                case "interest_payamt":
                    jsPostPayAmt();
                    break;
            }
        }

        function OnClickInsertRowPayInEtc() {
            objDwPayInEtc.InsertRow(0);
        }

        function OnClickDeleteRowPayInEtc(sender, row, bName) {
            if (bName == "b_del") {
                var isConfirm = confirm("ต้องการลบแถวที่ " + row + " ใช่หรือไม่ ?");
                if (isConfirm) {
                    Gcoop.GetEl("HdEtcRow").value = row;
                    jsPostEtcDel();
                }
            }
        }

        function DwPayInEtcItemChange(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            switch (col) {
                case "item_payamt":
                    Gcoop.GetEl("HdEtcRow").value = row;
                    jsPostEtcChange();
                    break;
            }
        }

        function OnDwPayInItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            if (col == "operate_tdate") {
                jsChangeOperatedate();
            }
            else if (col == "slip_tdate") {
                jspostSetOperateDate();
            }
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("455", "400", "w_dlg_list_noticemth.aspx", "?memno=" + Gcoop.GetEl("Hdmemno").value);
                Gcoop.GetEl("HdOpenIFrame").value = "false";
            }
        }

        function ReciveValue(notice_docno) {
            Gcoop.GetEl("HiddenLPMValue").value = notice_docno;
            jsPostLPMValue();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_memdet"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <br />
    <table style="width: 100%;">
        <tr valign="top">
            <td>
                <asp:Label ID="Label2" runat="server" Text="รายการชำระ" Font-Bold="False" Font-Size="Small"
                    ForeColor="#000000" Font-Underline="True" />
                <dw:WebDataWindowControl ID="DwPayIn" runat="server" DataWindowObject="d_lcwin_slippayin"
                    LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwPayInItemChanged">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="ประเภทเงินที่ชำระ" Font-Bold="False"
                    Font-Size="Small" ForeColor="#000000" Font-Underline="True" />
                <span onclick="OnClickInsertRowPayInExp()" style="cursor: pointer; margin-left: 70%;">
                    <asp:Label ID="LbInsert1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
                <dw:WebDataWindowControl ID="DwPayInExp" runat="server" DataWindowObject="d_lcwin_slippayin_exp"
                    LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" ClientEventButtonClicked="DwPayInExpButtonClicked"
                    ClientEventItemChanged="OnDwMoneytypeChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="Label3" runat="server" Text="รายการสัญญาเก่าที่หักลบ" Font-Bold="False"
        Font-Size="Small" ForeColor="#000000" Font-Underline="True" />
    <dw:WebDataWindowControl ID="DwPayInLon" runat="server" DataWindowObject="d_lcwin_slippayin_lon"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwPayInLoanItemChange">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label4" runat="server" Text="รายการหักอื่น ๆ" Font-Bold="False" Font-Size="Small"
        ForeColor="#000000" Font-Underline="True" />
    <span onclick="OnClickInsertRowPayInEtc()" style="cursor: pointer; margin-left: 78%;">
        <asp:Label ID="Label5" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwPayInEtc" runat="server" DataWindowObject="d_lcwin_slippayin_etc"
        LibraryList="~/DataWindow/investment/loan_slippayin.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventButtonClicked="OnClickDeleteRowPayInEtc"
        ClientEventItemChanged="DwPayInEtcItemChange">
    </dw:WebDataWindowControl>
    <br />
    <asp:HiddenField ID="HdEtcRow" runat="server" Value="" />
    <asp:HiddenField ID="Hd_row" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="HiddenLPMValue" runat="server" />
    <asp:HiddenField ID="Hdmemno" runat="server" />
    <asp:HiddenField ID="HdLnCol" runat="server" />
</asp:Content>
