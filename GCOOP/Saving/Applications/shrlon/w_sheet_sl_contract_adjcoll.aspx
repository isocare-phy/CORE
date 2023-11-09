<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_contract_adjcoll.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_contract_adjcoll" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jscontract_no%>
    <%=newClear%>
    <%=jsSetmembinfo%>
    <%=jsSetCollContno%>

    <script type="text/javascript">
        function Validate() {
            objdw_main.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");

        }
        function ItemChangedMain(sender, rowNumber, columnName, newValue) {
            if (columnName == "loancontract_no") {
                objdw_main.SetItem(rowNumber, "loancontract_no", newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("HContract").value = objdw_main.GetItem(1, "loancontract_no");
                jscontract_no();
            } else if (columnName == "member_no") {
                objdw_main.SetItem(rowNumber, "member_no", newValue);
                objdw_main.AcceptText();
                Gcoop.GetEl("HdMember_no").value = newValue;
                jsSetmembinfo();
            }
        }
        function b_Click(s, r, c) {
            if (c == "b_contract") {
                var membno = objdw_main.GetItem(1,"member_no");
                Gcoop.OpenDlg('640', '650', 'w_dlg_sl_loancontract_search_memno.aspx', '?memno=' + membno);
            }
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('640', '650', 'w_dlg_sl_loancontract_search.aspx', '');
        }
        
        function GetContFromDlg(loancontractno) {
            objdw_main.SetItem(1, "loancontract_no", loancontractno );
            objdw_main.AcceptText();
            Gcoop.GetEl("HContract").value = loancontractno ;
            jsSetCollContno();
        }
        function GetValueFromDlg(loancontractno) {
            objdw_main.SetItem(1, "loancontract_no", Gcoop.Trim(loancontractno));
            objdw_main.AcceptText();
            Gcoop.GetEl("HContract").value = objdw_main.GetItem(1, "loancontract_no");
            jscontract_no();
        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                newClear();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HContract" runat="server" />
    <asp:HiddenField ID="Hcause" runat="server" />
    <table style="width: 100%;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_loansrv_req_contadjmain_coll"
                    LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientFormatting="true"
                    ClientEventItemChanged="ItemChangedMain" ClientEventButtonClicked="b_Click">
                </dw:WebDataWindowControl>
            </td>
        </tr>
         <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_sl_contadjust_coll"
                    LibraryList="~/DataWindow/shrlon/sl_contract_adjust.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientFormatting="true"
                    ClientEventItemChanged="ItemChangedMain" ClientEventButtonClicked="b_Click">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdRowNumber" runat="server" />
    <asp:HiddenField ID="HdMember_no" runat="server" />
</asp:Content>
