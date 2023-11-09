<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_slippayout.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_slippayout" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <%=jsGetGroupMoneyType%>
    <%=jsSetBankBranch%>
    <%=jsPostEtcDel%>
    <%=jsPostEtcChange%>
    <%=jsPostContno%>
    <%=jsPostFlag%>
    <%=jsPostPayAmt%>
    <%=jsChangeSlipdate%>
    <%=jsChangeRcvFlag%>
    <%=jsChangePayoutnet%>
    <%=jsPostInsertPayOutExp%>
    <script type="text/JavaScript">

        function Validate() {
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล ?");

            if (!isconfirm) {
                return false;
            }
            var RowDwPayOutExp = objDwPayOutExp.RowCount();
            var total_expense_amt = 0.00;
            var alertstr = "";
            var member_no = objDwMain.GetItem(1, "member_no");
            var payoutnet_amt = objDwPayOut.GetItem(1, "payoutnet_amt");

            if (member_no == "" || member_no == null) {
                alertstr += "กรุณาระบุรหัสสมาชิก\n";
            }
            if (objDwPayOut.GetItem(1, "payoutnet_amt") > 0) {
                if (RowDwPayOutExp == 0) {
                    alertstr += "กรุณาทำรายการประเภทเงินที่จ่าย\n";
                }
                else {
                    for (var i = 1; i <= RowDwPayOutExp; i++) {
                        var moneytype_code = objDwPayOutExp.GetItem(i, "moneytype_code");
                        var expense_amt = objDwPayOutExp.GetItem(i, "expense_amt");
                        var bank_desc = objDwPayOutExp.GetItem(i, "bank_desc");
                        var expense_accid = objDwPayOutExp.GetItem(i, "expense_accid");
                        var moneyGroup = objDwPayOutExp.GetItem(i, "moneytype_group");

                        if (moneyGroup == "CBT") {
                            if (moneytype_code == "" || moneytype_code == null) {
                                alertstr += "กรุณาเลือกประเภทเงินที่จ่ายแถวที่ " + i + "\n";
                            }
                            if (bank_desc == "" || bank_desc == null) {
                                alertstr += "กรุณาระบุธนาคารแถวที่ " + i + "\n";
                            }
                            if (expense_accid == "" || expense_accid == null) {
                                alertstr += "กรุณาระบุเลขที่บัญชีแถวที่ " + i + "\n";
                            }
                            if (expense_amt == "" || expense_amt == null) {
                                alertstr += "กรุณาระบุยอดเงินรายการแถวที่ " + i + "\n";
                            }
                        }
                    }
                    for (var i = 1; i <= RowDwPayOutExp; i++) {
                        total_expense_amt += objDwPayOutExp.GetItem(i, "expense_amt");
                    }
                    if (payoutnet_amt != total_expense_amt) {
                        alertstr += "กรุณาระบุยอดเงินรายการให้ตรงกับยอดจ่ายสุทธิ\n";
                    }
                }
            }

            var row = objDwPayOutEtc.RowCount();
            for (var i = 1; i <= row; i++) {
                var slipitemtype_code = objDwPayOutEtc.GetItem(i, "slipitemtype_code");
                var slipitem_desc = objDwPayOutEtc.GetItem(i, "slipitem_desc");
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

        function OnClickInsertRowPayOutExp() {
            jsPostInsertPayOutExp();
        }

        function OnClickInsertRowPayOutEtc() {
            objDwPayOutEtc.InsertRow(0);
        }

        function OnClickDeleteRowPayOutExp(sender, row, bName) {
            if (bName == "b_del") {
                var isConfirm = confirm("ต้องการลบแถวที่ " + row + " ใช่หรือไม่ ?");
                if (isConfirm) {
                    objDwPayOutExp.DeleteRow(row);
                }
            }
            else if (bName = "b_bank") {
                var moneyGroup = objDwPayOutExp.GetItem(row, "moneytype_group");
                switch (moneyGroup) {
                    case "CHQ":
                        Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + row);
                        break;
                    case "TBK": //เดิมเป็น CBT
                        Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + row);
                        break;
                }
            }
        }

        function OnClickDeleteRowPayOutEtc(sender, row, bName) {
            if (bName == "b_del") {
                var isConfirm = confirm("ต้องการลบแถวที่ " + row + " ใช่หรือไม่ ?");
                if (isConfirm) {
                    Gcoop.GetEl("HdEtcRow").value = row;
                    jsPostEtcDel();
                }
            }
        }

        function DwPayOutEtcItemChange(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            switch (col) {
                case "item_payamt":
                    Gcoop.GetEl("HdEtcRow").value = row;
                    jsPostEtcChange();
                    break;
            }
        }

        function OnDwPayOutExpItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            switch (col) {
                case "moneytype_code":
                    Gcoop.GetEl("HdRow_Type").value = row;
                    objDwPayOutExp.SetItem(row, "bank_desc", "");
                    objDwPayOutExp.SetItem(row, "branch_desc", "");
                    objDwPayOutExp.SetItem(row, "expense_bank", "");
                    objDwPayOutExp.SetItem(row, "expense_branch", "");
                    jsGetGroupMoneyType();
                    break;
            }
        }

        function OnDwPayOutItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            if (col == "slip_tdate") {
                objDwPayOut.SetItem(1, "slip_tdate", value);
                objDwPayOut.AcceptText();
                objDwPayOut.SetItem(1, "slip_date", Gcoop.ToEngDate(value));
                objDwPayOut.AcceptText();

                jsChangeSlipdate();
            }
            else if (col == "rcvperiod_flag") {
                jsChangeRcvFlag();
            }
            else if (col == "payout_amt") {
                var payout_amt = objDwPayOut.GetItem(1, "payout_amt");
                var payoutclr_amt = objDwPayOut.GetItem(1, "payoutclr_amt");
                var payoutnet_amt = objDwPayOut.GetItem(1, "payoutnet_amt");

                if ((payout_amt - payoutclr_amt) != payoutnet_amt) {
                    jsChangePayoutnet();
                }
            }
        }

        function OnDwPayOutLoanItemChange(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            Gcoop.GetEl("HdRow").value = row;
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

        function GetDlgBankAndBranch(sheetRow, bankCode, bankDesc, branchCode, branchDesc) {
            Gcoop.GetEl("HdSheetRow").value = sheetRow;
            Gcoop.GetEl("HdBank_id").value = bankCode;
            Gcoop.GetEl("HdBank_desc").value = bankDesc;
            Gcoop.GetEl("HdBranch_id").value = branchCode;
            Gcoop.GetEl("HdBranch_desc").value = branchDesc;
            jsSetBankBranch();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                Gcoop.OpenIFrame("750", "400", "w_dlg_lncont.aspx", "?memno=" + Gcoop.GetEl("Hdmemno").value);
                Gcoop.GetEl("HdOpenIFrame").value = "false";
            }
        }

        function ReciveLoan(loancontcoop_id, loancontract_no) {
            Gcoop.GetEl("Hdcontcoopid").value = loancontcoop_id;
            Gcoop.GetEl("Hdcontno").value = loancontract_no;
            jsPostContno();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_slippayout_contdet"
        LibraryList="~/DataWindow/investment/loan_slippayout.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventButtonClicked="MenubarOpen">
    </dw:WebDataWindowControl>
    <br />
    <table style="width: 100%;">
        <tr valign="top">
            <td>
                <asp:Label ID="Label2" runat="server" Text="รายการจ่ายเงิน" Font-Bold="False" Font-Size="Small"
                    ForeColor="#000000" Font-Underline="True" />
                <dw:WebDataWindowControl ID="DwPayOut" runat="server" DataWindowObject="d_lcwin_slippayout"
                    LibraryList="~/DataWindow/investment/loan_slippayout.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwPayOutItemChanged">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <asp:Label ID="Label1" runat="server" Text="ประเภทเงินที่จ่าย" Font-Bold="False"
                    Font-Size="Small" ForeColor="#000000" Font-Underline="True" />
                <span onclick="OnClickInsertRowPayOutExp()" style="cursor: pointer; margin-left: 69%;">
                    <asp:Label ID="Label6" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
                <dw:WebDataWindowControl ID="DwPayOutExp" runat="server" DataWindowObject="d_lcwin_slippayout_exp"
                    LibraryList="~/DataWindow/investment/loan_slippayout.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" ClientEventButtonClicked="OnClickDeleteRowPayOutExp"
                    ClientEventItemChanged="OnDwPayOutExpItemChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="Label3" runat="server" Text="รายการสัญญาเก่าที่หักลบ" Font-Bold="False"
        Font-Size="Small" ForeColor="#000000" Font-Underline="True" />
    <dw:WebDataWindowControl ID="DwPayOutLon" runat="server" DataWindowObject="d_lcwin_slippayout_clrlon"
        LibraryList="~/DataWindow/investment/loan_slippayout.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwPayOutLoanItemChange">
    </dw:WebDataWindowControl>
    <br />
    <asp:Label ID="Label4" runat="server" Text="รายการหักอื่น ๆ" Font-Bold="False" Font-Size="Small"
        ForeColor="#000000" Font-Underline="True" />
    <span onclick="OnClickInsertRowPayOutEtc()" style="cursor: pointer; margin-left: 78%;">
        <asp:Label ID="Label5" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
    <dw:WebDataWindowControl ID="DwPayOutEtc" runat="server" DataWindowObject="d_lcwin_slippayout_clretc"
        LibraryList="~/DataWindow/investment/loan_slippayout.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventButtonClicked="OnClickDeleteRowPayOutEtc"
        ClientEventItemChanged="DwPayOutEtcItemChange">
    </dw:WebDataWindowControl>
    <br />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
    <asp:HiddenField ID="HdRow_Type" runat="server" Value="" />
    <asp:HiddenField ID="HdEtcRow" runat="server" Value="" />
    <asp:HiddenField ID="HdSheetRow" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_desc" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_desc" runat="server" Value="" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="Hdmemno" runat="server" />
    <asp:HiddenField ID="Hdcontno" runat="server" />
    <asp:HiddenField ID="Hdcontcoopid" runat="server" />
</asp:Content>
