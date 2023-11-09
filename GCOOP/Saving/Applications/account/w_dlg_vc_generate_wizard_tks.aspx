<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_dlg_vc_generate_wizard_tks.aspx.cs"
    Inherits="Saving.Applications.account.w_dlg_vc_generate_wizard_tks" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=JspostChangeDw %>
    <%=jsPostGetAccid%>
    <script type="text/javascript">
        function OnDwWizardItemChange(s, r, c, v)//เปลี่ยนวันที่
        {
            if (c == "voucher_tdate") {
                s.SetItem(1, "voucher_tdate", v);
                s.AcceptText();
                s.SetItem(1, "voucher_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            
            return 0;
        }

        function OnDwWizardClicked(s, r, c, v) {
            var vcDate = "";
            vcDate = objDw_wizard.GetItem(1, "voucher_tdate"); // ส่งชื่อฟิวส์วันที่ไทยเข้าไป

            if (c == "b_fin") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_fin_ctrl/w_dlg_vc_trn_fin.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //ทำแล้ว การเงิน
            }
            else if (c == "b_shrfilter") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_share_ctrl/w_dlg_vc_trn_share.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //ถอนหุ้นเงินสด
            }
            else if (c == "b_lonfilter") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_loan_ctrl/w_dlg_vc_trn_loan.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //เงินสดจ่ายกู้
            }
            else if (c == "b_lnrcv") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_loan_ctrl/w_dlg_vc_trn_loan.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //สินเชื่อ
            }
            else if (c == "b_shrcv") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_share_ctrl/w_dlg_vc_trn_share.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //ทะเบียนหุ้น
            } 
            else if (c == "b_lnrcv2") {
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_trn_loan_ctrl/w_dlg_vc_trn_loan.aspx", "?vcDate=" + vcDate + "&cashType=" + c); //สินเชื่อ
            }
        }

        function Validate() {

        }
        function OnButtonBackClick() {
            javascript: window.history.back();
        }

        function GetAccount(slip_no, cash_type) {
            Gcoop.GetEl("Hdacclist").value = slip_no;
            Gcoop.GetEl("HdCashType").value = cash_type;
            jsPostGetAccid();
        }
    
   
  
    
  
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <p>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <asp:Panel ID="Panel1" runat="server" Height="260px" Width="430px" BorderStyle="Inset">
            <dw:WebDataWindowControl ID="Dw_wizard" runat="server" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                DataWindowObject="d_vc_vcgen_wizard_vcdate" LibraryList="~/DataWindow/account/vc_genwizard_vcdate.pbl"
                ClientEventItemChanged="OnDwWizardItemChange" ClientEventButtonClicked="OnDwWizardClicked" 
                ClientFormatting="True" Height="250px">
            </dw:WebDataWindowControl>
        </asp:Panel>
        <table style="width: 100%; font-size: small;">
            <tr>
                <td colspan="3" valign="top">
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel2" runat="server" Width="396px">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <input id="Button1" type="button" value="ย้อนกลับ" onclick="OnButtonBackClick()" />
                        <%--                        <asp:Button ID="B_back" runat="server" Text="&lt; ย้อนกลับ" 
                            UseSubmitBehavior="False" Width="75px" onclick="window.back();" />--%>
                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="B_step" runat="server" OnClick="B_next_Step" Text="ต่อไป &gt;" UseSubmitBehavior="False"
                            Width="75px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="B_next" runat="server" OnClick="B_next_Click" Text="ดึงข้อมูล" UseSubmitBehavior="False"
                            Width="75px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="B_Print" runat="server" OnClick="B_Print_Click" Text="ออกรายงาน" UseSubmitBehavior="False"
                            Width="75px" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </asp:Panel>
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                    <asp:HiddenField ID="HdSys_App" runat="server" />
                    <asp:HiddenField ID="Hdacclist" runat="server" />
                    <asp:HiddenField ID="Hdrow" runat="server" />
                    <asp:HiddenField ID="HdVc_No" runat="server" />
                    <asp:HiddenField ID="HdOpenIFrame" runat="server" value="False"/>
                    <asp:HiddenField ID="HdCashType" runat="server" />
                    
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
    </p>
</asp:Content>
