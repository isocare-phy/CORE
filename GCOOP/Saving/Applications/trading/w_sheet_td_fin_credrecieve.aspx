<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_td_fin_credrecieve.aspx.cs" Inherits="Saving.Applications.trading.w_sheet_td_fin_credrecieve" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsSlipNo%>
    <%=jsDwDetailInsertRow %>
    <%=jsInitCred%>
    <%=jsInitCreditSlip %>
    <%=jsFilterBranch %>
    <%=jsFilterAccNo%>
    <%=jspay_cal%>
    <%=jsChickChkAll %>
    <%=jsChickChk%>
    <%=jsCalTax%>
    <%=jsCalTax2%>
    <%=jsbtn%>
     <%=jsbtn_2%>
    

    <script type="text/javascript">


        function Validate() { // บันทึก
            return confirm("ต้องการบันทึกหรือไม่");
        }

        function showTabPage(tab) {
            var i = 1;
            var tabamount = 2;
            for (i = 1; i <= tabamount; i++) {
                document.getElementById("tab_" + i).style.visibility = "hidden";
                document.getElementById("stab_" + i).style.backgroundColor = "rgb(200,235,255)";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).style.backgroundColor = "rgb(211,213,255)";
                    Gcoop.GetEl("HiddenFieldTab").value = i + "";
                }
            }
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
                        alert("กรุณากรอกเลขที่เจ้าหนี้");
                        return;
                    }
                    jsInitCreditSlip();
                    break;
                case "cred_no":
                    jsInitCred();
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
                case "bank_code":
                    jsFilterBranch();
                    break;
                case "bank_branch":
                    jsFilterAccNo();
                    break;
                case "chkflag":
                    Gcoop.GetEl("HdRow_Detail").value = r;
                    jsChickChk();
                    break;
                   
            }

        }

        function DwDetailInsertRow() {
            jsDwDetailInsertRow();
        }
        function GetDlgRecieve(slip_no) {
            objDwMain.SetItem(1, "debtdecdoc_no", slip_no);
            jsSlipNo();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("770", "600", "w_dlg_td_search_recieve.aspx", "?debtdectype_code=APRC");
        }
        function OnDwMainButtonClick(s, r, c) {
            switch (c) {
                case "b_1":
                    var cred_no = "";
                    try {
                        cred_no = s.GetItem(r, "cred_no");
                    } catch (err) {
                        alert("กรุณากรอกเลขที่เจ้าหนี้");
                        return;
                    }
                    if (cred_no == "" || cred_no == null) {
                        alert("กรุณากรอกเลขที่เจ้าหนี้");
                        return;
                    }
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_slip_apcredit.aspx", "?cred_no=" + cred_no);
                    break;
                case "b_2":
                    Gcoop.OpenIFrame("740", "600", "w_dlg_td_search_cred.aspx", "");
                    break;

                //add//  
                case "btn_01":
                    jsbtn();
                    break;

                case "btn_02":
                    jsbtn_2();
                    break;

                    
            }
        }

        function GetDlgCredNo(cred_no) {
            objDwMain.SetItem(1, "cred_no", cred_no);
            jsInitCred();
        }

        function GetDlgSlipNoApc(slip_no) {
            objDwMain.SetItem(1, "refdoc_no", slip_no);
            jsInitCreditSlip();
        }
        function ChickChkAll() {
            jsChickChkAll();
        }

        function OnDwTailerItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "isservicetax":
                    jsCalTax();
                    break;

                case "taxtype_code":
                    jsCalTax2();
                    break;
                    
            }

        }
        function SheetLoadComplete() {
            var tab = Gcoop.ParseInt(Gcoop.GetEl("HiddenFieldTab").value);
            showTabPage(tab);
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
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_tradesrv_fin_credrecieve_header"
        LibraryList="~/DataWindow/trading/sheet_td_fin_credrecieve.pbl" ClientScriptable="True"
        ClientEventButtonClicked="OnDwMainButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwMainItemChanged"
        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwMainClick"
        TabIndex="0">
    </dw:WebDataWindowControl>
    <table style="width: 100%; border: solid 1px; margin-top: 5px">
        <tr align="center" class="dwtab">
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_1" width="33.33%"
                onclick="showTabPage(1);">
                รายละเอียดการขออนุมัติ
            </td>
            <td style="background-color: rgb(211,213,255); cursor: pointer;" id="stab_2" width="33.33%"
                onclick="showTabPage(2);">
                ใบเสนอขออนุมัติการจ่ายเงิน
            </td>
        </tr>
    </table>
    <table style="width: 100%; border: solid 1px; margin-top: 2px" class="dwcontent">
        <tr>
            <td style="height: 350px;" valign="top">
                <div id="tab_1" style="visibility: visible; position: absolute;">
                    <asp:Label ID="Label3" runat="server" Text="เพิ่มแถว" onclick="DwDetailInsertRow();"
                        Font-Size="Medium" Font-Underline="True" ForeColor="#0000CC"></asp:Label>
                    <br />
                    <input type="checkbox" id="chkAll" runat="server" onchange="ChickChkAll();" />
                    เลือกทั้งหมด
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_tradesrv_fin_credrecieve_detail"
                        LibraryList="~/DataWindow/trading/sheet_td_fin_credrecieve.pbl" ClientScriptable="True"
                        ClientEventButtonClicked="OnDDwDetailButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwDetailItemChanged"
                        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwDetailClick"
                        Width="760px" Height="150px" TabIndex="50">
                    </dw:WebDataWindowControl>
                    <dw:WebDataWindowControl ID="DwTailer" runat="server" DataWindowObject="d_tradesrv_fin_credrecieve_tailer"
                        LibraryList="~/DataWindow/trading/sheet_td_fin_credrecieve.pbl" ClientScriptable="True"
                        ClientEventButtonClicked="OnDwTailerButtonClick" ClientEvents="true" ClientEventItemChanged="OnDwTailerItemChanged"
                        cAutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                        ClientFormatting="True" AutoRestoreContext="False" ClientEventClicked="OnDwTailerClick"
                        TabIndex="100">
                    </dw:WebDataWindowControl>
                </div>
                <div id="tab_2" style="visibility: visible; position: absolute;">
                    <dw:WebDataWindowControl ID="DwPrepay" runat="server" DataWindowObject="d_tradesrv_fin_recpay_present_approve"
                        LibraryList="~/DataWindow/trading/sheet_td_fin_credrecieve.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientEventItemChanged="DwPrepaytemChanged" ClientFormatting="True" TabIndex="1000"
                        Width="742px" Height="330px" ClientEventButtonClicked="DwPrepayButtonClicked">
                    </dw:WebDataWindowControl>
                </div>
             </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdRow" runat="server" />
    <asp:HiddenField ID="HdRow_Detail" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdOpenNum" runat="server" Value="" />
    <asp:HiddenField ID="HdOpenNumStaus" runat="server" Value="" />
    <asp:HiddenField ID="HiddenFieldTab" runat="server" Value="1"/>
    <asp:HiddenField ID="HdClikTab2" runat="server" />
    <asp:HiddenField ID="Hddebtdecdoc_no" runat="server" />
      
</asp:Content>
