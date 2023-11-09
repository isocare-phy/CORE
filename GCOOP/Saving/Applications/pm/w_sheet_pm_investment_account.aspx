<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_investment_account.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_investment_account" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=DetailInsert%>
    <%=postNodueFlag%>
    <%=InterateInsert%>
    <%=DelDetailRow%>
    <%=DelInterateRow%>
    <%=JsPostTime%>
    <%=JsPostDueDate%>
    <%=PostUnitAmt%>
    <%=PostUnitCost%>
    <%=postMainBank%>
    <%=postDetailBank%>
    <%=JsSetInvestType%>
    <%=JsSetInvSource%>
    <%=JsChangeFollow%>
    <%=jsGetGroupMoneyType%>
    <%=jsSetBankBranch%>
    <script type="text/javascript">
        function SheetLoadComplete() {
            //            a = Gcoop.GetEl("HdFocus").value;
            //            alert(a);
            if (Gcoop.GetEl("HdFocus").value == "NotPostBack") {
                Gcoop.SetLastFocus("buy_tdate_" + (objDwMain.RowCount() - 1));
                Gcoop.Focus();

            }
            if (Gcoop.GetEl("HdFocus").value == "money_code") {
                Gcoop.SetLastFocus("bank_account_no_" + (objDwDetail.RowCount() - 1));
                Gcoop.Focus();
            }
            if (Gcoop.GetEl("HdFocus").value == "int_start_tdate") {
                try {
                    Gcoop.SetLastFocus("int_start_tdate_" + (objDwInterate.RowCount() - 1));
                    Gcoop.Focus();
                } catch (err) { }
            }
        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function DetailInsertRow() {
            DetailInsert();
        }

        function InterateInsertRow() {
            InterateInsert();
        }

        function OnDwDetailButtonClicked(s, r, c) {
            switch (c) {
                case "b_search":
                    var moneyGroup = objDwDetail.GetItem(r, "moneytype_group");
                    var member_no = objDwMain.GetItem(1, "member_no");
                    //alert(moneyGroup);
                    switch (moneyGroup) {
                        case "CHQ":
                            Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + r);
                            break;
                        case "TBK":
                            Gcoop.OpenIFrame("860", "620", "w_dlg_bank_and_branch.aspx", "?sheetRow=" + r);
                            break;
                    }
                    break;
                case "b_delete":
                    if (confirm("แน่ใจว่าต้องการลบแถว")) {
                        Gcoop.GetEl("HdDetailRowDel").value = r;
                        DelDetailRow();
                    }
                    break;
            }
        }

        function OnDwInterateButtonClicked(sender, rowNumber, objectName) {
            if (objectName == "b_delete") {
                if (confirm("แน่ใจว่าต้องการลบแถว")) {
                    Gcoop.GetEl("HdInterateRowDel").value = rowNumber;
                    DelInterateRow();
                }
            }
        }

        function OnDwDetailItemChanged(sender, rowNumber, objectName, Value) {
            if (objectName == "bank_code") {
                Gcoop.GetEl("HdBankCode").value = Value;
                Gcoop.GetEl("HdRow").value = rowNumber;
                objDwDetail.SetItem(rowNumber, objectName, Value);
                objDwDetail.AcceptText();
                postDetailBank();
            }
        }
        function OnDwInterateItemChanged(sender, rowNumber, objectName, Value) {
            objDwInterate.SetItem(rowNumber, objectName, Value);
            objDwInterate.AcceptText();
        }

        function Validate() {
            objDwMain.AcceptText();
            objDwDetail.AcceptText();
            objDwInterate.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDwMainItemChanged(s, r, c, v) {
            /*DEBUG*///alert(v);
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "invest_period_unit") {
                Gcoop.GetEl("HdCk").value = "True";
                JsPostTime();
            }
            if (c == "due_tdate") {
                JsPostDueDate();
            }
            if (c == "unit_amt") {
                PostUnitAmt();
            }
            if (c == "unit_cost") {
                PostUnitCost();
            }
            if (c == "bank_code") {
                postMainBank();
            }
            if (c == "nodue_flag") {
                postNodueFlag();
            }
            ////////////////////////
            if (c == "buy_tdate") {
                JsChangeFollow();
            }
            ////////////////////////
        }
        function OnDwDetailItemChanged(sender, rowNumber, objectName, Value) {
            sender.SetItem(rowNumber, objectName, Value);
            sender.AcceptText();
            switch (objectName) {
                case "money_code":
                    Gcoop.GetEl("HdRow_Type").value = rowNumber;
                    objDwDetail.SetItem(rowNumber, "bank_desc", "");
                    objDwDetail.SetItem(rowNumber, "bank_code", "");
                    objDwDetail.SetItem(rowNumber, "bank_branch", "");
                    jsGetGroupMoneyType();
                    break;
            }
        }
        function OnDwMainButtonClicked(s, r, c) {
            if (c == "b_1") {
                Gcoop.OpenIFrame("770", "350", "w_dlg_add_invsource.aspx", "");
            }
            if (c == "b_2") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_add_investment_type.aspx", "");
            }
            if (c == "b_3") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_add_business_type.aspx", "");
            }
            if (c == "b_4") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_add_investment_group.aspx", "");
            }
        }
        function ReTurnOfInvestType(investtype_code) {
            Gcoop.GetEl("HdInvestType").value = investtype_code;
            JsSetInvestType();
        }
        function ReTurnOfInvSource(invsource_code) {
            Gcoop.GetEl("HdInvSource").value = invsource_code;
            JsSetInvSource();
        }
        function GetDlgBankAndBranch(sheetRow, bankCode, bankDesc, branchCode, branchDesc) {
            Gcoop.GetEl("HdSheetRow").value = sheetRow;
            Gcoop.GetEl("HdBank_id").value = bankCode;
            Gcoop.GetEl("HdBank_desc").value = bankDesc;
            Gcoop.GetEl("HdBranch_id").value = branchCode;
            Gcoop.GetEl("HdBranch_desc").value = branchDesc;
            jsSetBankBranch();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server" align="center">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" Width="680px" align="center">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_pm_main_pmreqinvestment"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="OnDwMainItemChanged"
            ClientFormatting="True" TabIndex="1" ClientEventButtonClicked="OnDwMainButtonClicked">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
    <asp:Label ID="Label3" runat="server" Text="รายละเอียดทำรายการ" Font-Bold="True"
        Font-Size="Medium" Font-Underline="True"></asp:Label>
    <span onclick="DetailInsertRow()">&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Small"
            ForeColor="#000066" Font-Underline="True"></asp:Label>
    </span>
    <br />
    <asp:Panel ID="Panel2" runat="server" Width="680px" align="center">
        <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_pm_detail_pmreqinvestment"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" ClientEventItemChanged="OnDwDetailItemChanged"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" TabIndex="250" ClientEventButtonClicked="OnDwDetailButtonClicked">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Label ID="Label4" runat="server" Text="อัตราดอกเบี้ย" Font-Bold="True" Font-Size="Medium"
        Font-Underline="True"></asp:Label>&nbsp;&nbsp;
    <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="True" Font-Size="Small"
        ForeColor="#000066" Font-Underline="True" onclick="InterateInsertRow();"></asp:Label>
    <br />
    <asp:Panel ID="Panel3" runat="server" Width="680px" align="center">
        <dw:WebDataWindowControl ID="DwInterate" runat="server" DataWindowObject="d_pm_interate_pmreqinvestment"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False" ClientEventItemChanged="OnDwInterateItemChanged"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" TabIndex="310" ClientEventButtonClicked="OnDwInterateButtonClicked">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="HdDetailRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdInterateRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdBankCode" runat="server" Value="" />
    <asp:HiddenField ID="HdCk" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
    <asp:HiddenField ID="HdDate" runat="server" Value="" />
    <asp:HiddenField ID="HdInvestType" runat="server" Value="" />
    <asp:HiddenField ID="HdInvSource" runat="server" Value="" />
    <asp:HiddenField ID="HdFocus" runat="server" Value="" />
    <asp:HiddenField ID="HdRow_Type" runat="server" Value="" />
    <asp:HiddenField ID="HdSheetRow" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_id" runat="server" Value="" />
    <asp:HiddenField ID="HdBank_desc" runat="server" Value="" />
    <asp:HiddenField ID="HdBranch_desc" runat="server" Value="" />
</asp:Content>
