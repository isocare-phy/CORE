<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_redemption_investment.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_redemption_investment" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>]
    <%=postAccountNo%>
    <%=DelSlipRow%>
    <%=postBank%>
    <%=jsGetGroupMoneyType%>
    <%=jsSetBankBranch%>
    <%=postTdate%>
    <script type="text/javascript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "account_no") {
                s.SetItem(r, c, Gcoop.StringFormat(v, "0000000000"));
                postAccountNo();
            } else if (c == "operate_tdate") {

                if (s.GetItem(1, "account_no") != "") {
                    postTdate();
                }
            }
        }

        function SlipInsertRow() {
            objDwSlipdet.InsertRow(0);
        }

        function OnDwSlipItemChanged(sender, rowNumber, objectName, Value) {
            if (objectName == "bank_code") {
                Gcoop.GetEl("HdBankCode").value = Value;
                Gcoop.GetEl("HdRow").value = rowNumber;
                sender.AcceptText();
                sender.SetItem(rowNumber, objectName, Value);
                postBank();
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
        }
        function ChooseAcc(account_no) {
            objDwMain.SetItem(1, "account_no", account_no);
            objDwMain.AcceptText();
            postAccountNo();
        }

        function OnDwMainButtonClicked(s, r, c) {
            if (c == "b_accountno") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
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

        function OnDwSlipItemChanged(sender, rowNumber, objectName, Value) {
            sender.SetItem(rowNumber, objectName, Value);
            sender.AcceptText();
            switch (objectName) {
                case "money_code":
                    Gcoop.GetEl("HdRow_Type").value = rowNumber;
                    objDwSlipdet.SetItem(rowNumber, "bank_desc", "");
                    objDwSlipdet.SetItem(rowNumber, "bank_code", "");
                    objDwSlipdet.SetItem(rowNumber, "bank_branch", "");
                    jsGetGroupMoneyType();
                    break;
            }

        }
        function OnDwSlipButtonClicked(s, r, c) {
            switch (c) {
                case "b_search":
                    var moneyGroup = objDwSlipdet.GetItem(r, "moneytype_group");
                    var member_no = objDwMain.GetItem(1, "member_no");
                    switch (moneyGroup) {
                        case "CHQ":
                            Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch_for_chq.aspx", "?sheetRow=" + r);
                            break;
                        case "TBK":
                            Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + r);
                            break;
                    }
                    break;
                case "b_delete":
                    if (confirm("แน่ใจว่าต้องการลบแถว")) {
                        Gcoop.GetEl("HdSlipRowDel").value = r;
                        DelSlipRow();
                    }
                    break;
            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server" align="center">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" Width="680px" align="center">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_pm_main_withdraw"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="OnDwMainItemChanged"
            ClientFormatting="True" TabIndex="1" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientEventClicked="OnDwMainClick">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" Width="680px">
        <asp:Label ID="Label3" runat="server" Text="รายละเอียดทำรายการ" Font-Bold="True"
            Font-Size="Medium" Font-Underline="True"></asp:Label>
        <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Small"
            ForeColor="#000066" Font-Underline="True" onclick="SlipInsertRow();"></asp:Label>
        <br />
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Width="680px" align="center">
        <dw:WebDataWindowControl ID="DwSlipdet" runat="server" DataWindowObject="d_pm_slipdet_withdraw"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="OnDwSlipItemChanged"
            ClientFormatting="True" TabIndex="230" ClientEventButtonClicked="OnDwSlipButtonClicked"
            ClientEventClicked="OnDwSlipClick">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
    <asp:HiddenField ID="HdSlipRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdBankCode" runat="server" Value="" />
    <asp:HiddenField ID="HdRow_Type" runat="server" Value="" />
    <asp:HiddenField ID="HdSheetRow" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_desc" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_desc" runat="server" Value="" />
</asp:Content>
