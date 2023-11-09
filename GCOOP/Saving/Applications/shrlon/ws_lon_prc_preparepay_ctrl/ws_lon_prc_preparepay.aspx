<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_lon_prc_preparepay.aspx.cs" Inherits="Saving.Applications.shrlon.ws_lon_prc_preparepay_ctrl.ws_lon_prc_preparepay" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsLoan.ascx" TagName="DsLoan" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsLoan = new DataSourceTool;

        function OnDsMainItemChanged(s, r, c, v) {
            //          if (c == "bizz_period") {
            //              PostSetAccDate();
            //          }
            if (c == "proc_type") {
                var proc_type = dsMain.GetItem(0, "proc_type");
                if (proc_type == 1) {
                    dsMain.SetItem(0, "proc_label", "ทั้งหมด :");
                    dsMain.GetElement(0, "proc_text").disabled = true;
                } else if (proc_type == 20) {
                    dsMain.SetItem(0, "proc_label", "รหัสประเภทสมาชิก :");
                    dsMain.GetElement(0, "proc_text").disabled = false;
                } else if (proc_type == 40) {
                    dsMain.SetItem(0, "proc_label", "รหัสสังกัด :");
                    dsMain.GetElement(0, "proc_text").disabled = false;
                } else if (proc_type == 60) {
                    dsMain.SetItem(0, "proc_label", "เลขสมาชิก :");
                    dsMain.GetElement(0, "proc_text").disabled = false;
                }
                dsMain.GetElement(0, "proc_label").style.background = "#D3E7FF";
            }
        }
        function PostBtall() {
            var all = dsLoan.GetItem(0, "all_flag");            
            if (all == 0) {
                dsLoan.SetItem(0, "all_flag", 1);
                for (var i = 0; i < dsLoan.GetRowCount(); i++) {
                    dsLoan.SetItem(i, "operate_flag", 1);

                }
            } else {
                dsLoan.SetItem(0, "all_flag", 0);
                for (var i = 0; i < dsLoan.GetRowCount(); i++) {
                    dsLoan.SetItem(i, "operate_flag", 0);
                }
            } 
        }
        function OnDsLoanItemChanged(s, r, c, v) {

        }

        function SheetLoadComplete() {

            if (Gcoop.GetEl("Hd_process").value == "true") {
                Gcoop.OpenProgressBar("ประมวลผลชำระเงินกู้ล่วงหน้า", true, true, ProcPreparepayComplete);
            }
        }

        function ProcDivavgComplete() {
            PostCancel();
        }

        function b_next() {
            PostNext();
        }

        function b_previous() {
            PostPrevious();
        }

        function b_cancel() {
            PostCancel();
        }

        function b_process() {
            PostProcess();
        }   
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:HiddenField ID="HDCoop_ID" runat="server" />
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td colspan="5">
                <asp:Panel ID="Panel1" runat="server">
                    <uc1:DsMain ID="dsMain" runat="server" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="5" align="center">
                <asp:Panel ID="Panel2" runat="server" Height="400px">
                    <uc2:DsLoan ID="dsLoan" runat="server" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                <asp:Panel ID="Panel3" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td width="40%" align="right">
                                <asp:Button ID="b_next" Style="padding: 5px 20px;" runat="server" Text="ต่อไป &gt;"
                                    UseSubmitBehavior="False" OnClientClick="b_next()" />
                            </td>
                            <td width="10%" align="center">
                            </td>
                            <td width="40%" align="left">
                                <div>
                                    <asp:Button ID="b_cancel" Style="padding: 5px 20px;" runat="server" Text="ยกเลิก"
                                        UseSubmitBehavior="False" OnClientClick="b_cancel()" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="3">
                <asp:Panel ID="Panel4" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td width="30%" align="right">
                                <asp:Button ID="b_previous" Style="padding: 5px 15px;" runat="server" Text="&lt; ย้อนกลับ"
                                    UseSubmitBehavior="False" OnClientClick="b_previous()" />
                            </td>
                            <td width="40%" align="center">
                                <asp:Button ID="b_process" type="button" Text="ประมวลผล" OnClientClick="b_process()"
                                    runat="server" Style="padding: 5px 20px;" />
                            </td>
                            <td width="30%" align="left">
                                <asp:Button ID="b_close" Style="padding: 5px 20px;" runat="server" Text="ยกเลิก"
                                    UseSubmitBehavior="False" OnClientClick="b_cancel()" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:HiddenField ID="Hd_process" runat="server" />
                <asp:HiddenField ID="Hd_row" runat="server" />
            </td>
        </tr>
    </table>
    <%=outputProcess%>
</asp:Content>
