<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_tran_pension.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_tran_pension" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postRetreiveDwMain%>
    <%=postPay%>
    <%=postDate%>
    <%=AccFromDiv%>
    <%=QueryData%>
    <%=postNull%>
    <%=AccFromMemb%>
    <%=postChangeMoneyType%>
    <%=postChangeAccId%>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function MenubarNew() {

            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
            return 0;
        }

        function GetValueFromDlg(memberNo) {
            objDwMain.SetItem(1, "member_no", memberNo);
            objDwMain.AcceptText();
            postRetreiveDwMain();
        }

        function DwMainButtonClick(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "b_1") {
                QueryData();
            }
            if (c == "b_2") {

                postPay();
            }
            if (c == "b_3") {
                AccFromMemb();
            }
            if (c == "b_4") {
                AccFromDiv();
            }
        }
        function DwDetailButtonClick(s, r, c, v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();
            if (c == "b_1") {
                member_no = objDwDetail.GetItem(r, "member_no");
                Gcoop.OpenDlg(710, 300, "w_dlg_assist_dep.aspx", "?member_no=" + member_no + "&row=" + r);
            }
        }

        function SetExpense_Accid(bank_id, bank_name, bank_balance, bank_aval, dept_desc, row) {
            var r = row;
            var moneytype_desc = dept_desc;
            var bank_accid = bank_id;

            objDwDetail.SetItem(r, "expense_type", moneytype_desc);
            objDwDetail.SetItem(r, "expense_accid", bank_accid);
            objDwDetail.SetItem(r, "expense_bank", "000");
            objDwDetail.SetItem(r, "expense_branch", "0000");
            objDwDetail.SetItem(r, "remark", "ไม่ได้แจ้ง");
            postNull();

        }

        function DwDetailItemChange(s, r, c, v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();

            if (c == "expense_type") {
                Gcoop.GetEl("hdRow").value = r;
                postChangeMoneyType();
            }

            if (c == "expense_accid") {
                moneytype = objDwDetail.GetItem(r, "expense_type");
                if (moneytype == "CBT") {
                    Gcoop.GetEl("hdRow").value = r;
                    postChangeAccId();
                }
            }
        }

        function DwMainPItemChange(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "member_no") {
                member_no = Gcoop.StringFormat(v, "00000000");
                objDwMain.SetItem(r, c, member_no);

                postRetreiveDwMain();
            }
            if (c == "slip_tdate") {
                objDwMain.SetItem(r, c, v);
                Gcoop.GetEl("hdate").value = v;
                objDwMain.AcceptText();
                postDate();
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <div >
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
            DataWindowObject="d_kt_pension_tran_cri" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMainButtonClick" ClientEventItemChanged="DwMainPItemChange">
        </dw:WebDataWindowControl>
    </div>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/kt_pension.pbl"
        DataWindowObject="d_kt_tran_pension" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwDetailButtonClick" ClientEventItemChanged="DwDetailItemChange" RowsPerPage="20">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="hdate" runat="server" />
    <asp:HiddenField ID="hdRow" runat="server" />
</asp:Content>
