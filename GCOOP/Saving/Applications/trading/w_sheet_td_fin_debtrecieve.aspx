<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_fin_debtrecieve.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_fin_debtrecieve" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript %>
<%=jsSlipNo%>
<%=jsDwDetailInsertRow%>
<%=jsInitDebt%>
<%=jsInitCreditSlip %>

<%=jsPostAccountNo%>
<%=jspay_cal%>
<%=jsdiscount_amt%>
<%=jstax_amt%>
<%=jstransportfee%>
<%=jspaymentby%>



    <script type="text/javascript">


        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }
        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "refdoc_no":
                    var refdoc_no = "";
                    try {
                        refdoc_no = s.GetItem(r, "refdoc_no");
                    } catch (err) {
                        alert("กรุณากรอกเลขที่ลูกหนี้");
                        return;
                    }
                    jsInitCreditSlip();
                    break;
                case "debt_no":
                    jsInitDebt();
                    break;
            }
        }
        function OnDwDetailItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "pay_amt":
                    var pay_amt = s.GetItem(r, "pay_amt")
                    Gcoop.GetEl("HdRow").value = r;
                    jspay_cal();
                    break;
                case "paymentby":
                    Gcoop.GetEl("HdRow").value = r;
                    jspaymentby();
                    break;
                case "account_no":
                    Gcoop.GetEl("HdRow").value = r;
                    jsPostAccountNo();
                    break;
                case "discount_amt":
                    Gcoop.GetEl("HdRow").value = r;
                    jsdiscount_amt();
                    break;
                case "tax_amt":
                    Gcoop.GetEl("HdRow").value = r;
                    jstax_amt();
                    break;
                case "transportfee":
                    Gcoop.GetEl("HdRow").value = r;
                    jstransportfee();
                    break;
                case "account_no_1":
                    //Gcoop.GetEl("HdRow").value = r;
                    jsPostAccountNo();
                    break;
                    
            }

        } 
        function DwDetailInsertRow() {
            jsDwDetailInsertRow()
        }
        function GetDlgRecieve(slip_no) {
            objDwMain.SetItem(1, "debtdecdoc_no", slip_no);
            jsSlipNo();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_debtrecieve.aspx", "?debtdectype_code=ARRC");
        }
        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_1":
                    var debt_no = "";
                    try {
                        debt_no = s.GetItem(r, "debt_no");
                    } catch (err) {
                        alert("กรุณากรอกเลขที่ลูกหนี้");
                        return;
                    }
                    if (debt_no == "" || debt_no == null) {
                        alert("กรุณากรอกเลขที่ลูกหนี้");
                        return;
                    }
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_slip_apdebt.aspx", "?debt_no=" + debt_no);
                    break;
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_debt.aspx", "");
                    break;
            }
        }

        function GetDlgDebtNo(debt_no) {
            objDwMain.SetItem(1, "debt_no", debt_no);
            jsInitDebt();
        }

        function GetDlgSlipNoApd(slip_no) {
            objDwMain.SetItem(1, "refdoc_no", slip_no);
            jsInitCreditSlip();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdOpenIFrame").value == "True") {
                if (confirm("คุณต้องการพิมพ์ใบชำระหนี้ หรือไม่ ?")) {
                    Gcoop.OpenIFrame("200", "200", "../../../Criteria/dlg/w_dlg_report_progress.aspx?&app=<%=app%>&gid=<%=gid%>&rid=<%=rid%>&pdf=<%=pdf%>", "");
                    Gcoop.GetEl("HdOpenReport").value = "false";
                }
            }
        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_fin_debtrecieve_header"
        LibraryList="~/DataWindow/trading/sheet_td_fin_debtrecieve.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientFormatting="True" TabIndex="1"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_fin_debtrecieve_detail"
        LibraryList="~/DataWindow/trading/sheet_td_fin_debtrecieve.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        ClientEventItemChanged="OnDwDetailItemChanged" ClientFormatting="True" TabIndex="50"
        ClientEventButtonClicked="OnDwDetailButtonClicked" ClientEventClicked="OnDwDetailClicked"
        Width="742px" Height="399px">
    </dw:WebDataWindowControl>
    
    <table style="width: 100%;  margin-top: 5px">
        <tr align="center" >
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="33.33%"
                onclick="showTabPage(1);">
                
            </td>
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_2" width="33.33%"
                onclick="showTabPage(2);">
              
            </td>
        </tr>
    </table>

    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdRow" runat="server" />
          
</asp:Content>
