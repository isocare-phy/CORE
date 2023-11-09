<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_receivemem.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_receivemem" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsPostMember%>
    <%=jsPostcalmemmain%>
    <%=newClear%>

    <script type="text/javascript">
        function Validate() {
            objdw_detail.AcceptText();
            objdw_main.AcceptText();

            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MainChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                objdw_main.SetItem(rowNumber, columnName, Gcoop.StringFormat(newValue, "000000"));
                objdw_main.AcceptText();
                Gcoop.GetEl("Hmember_no").value = objdw_main.GetItem(rowNumber, "member_no");
                Gcoop.GetEl("Hrev_period").value = objdw_main.GetItem(rowNumber, "recv_period");
                jsPostMember();
            }

        }


        function DetailChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "recv_amt") {
                objdw_detail.SetItem(rowNumber, columnName, newValue);
                objdw_detail.AcceptText();
                Gcoop.GetEl("Hcolumnname").value = columnName;
                var recv_amt = Gcoop.ParseFloat(objdw_detail.GetItem(rowNumber, "recv_amt")) + 0;
                Gcoop.GetEl("Hrecvamt").value = recv_amt + "";
                jsPostcalmemmain();
            }
        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {

                newClear();
            }
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('590', '590', 'w_dlg_ag_searchagentmem.aspx', '');

        }
        function Click_memsearch(s, r, c) {
            if (c == "b_memsearch") {
                Gcoop.OpenDlg('590', '590', 'w_dlg_ag_searchagentmem.aspx', '');
            }

        }
        function SearchMember(member_no) {
            objdw_main.SetItem(1, "member_no", member_no);
            objdw_main.AcceptText();
            Gcoop.GetEl("Hmember_no").value = objdw_main.GetItem(1, "member_no");
            Gcoop.GetEl("Hrev_period").value = objdw_main.GetItem(1, "recv_period");
            jsPostMember();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hmember_no" runat="server" />
    <asp:HiddenField ID="Hrev_period" runat="server" />
    <asp:HiddenField ID="Hcolumnname" runat="server" />
    <asp:HiddenField ID="Hrecvamt" runat="server" />
    <asp:TextBox ID="TextXmlHead" runat="server" Visible="False"></asp:TextBox>
    <asp:TextBox ID="TextXmlHead2" runat="server" Visible="False"></asp:TextBox>
    <table style="width: 100%;">
        <tr>
            <td colspan="2" valign="top">
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_agentsrv_mem_main"
                    LibraryList="~/DataWindow/agency/agent.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" Width="720px" ClientEventItemChanged="MainChanged"
                    TabIndex="1" ClientFormatting="True" ClientEventButtonClicked="Click_memsearch">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td rowspan="2" valign="top">
                <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_agentsrv_initreceivemem_detail"
                    LibraryList="~/DataWindow/agency/agent.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventClicked="ListClick"
                    ClientFormatting="True" TabIndex="40" ClientEventItemChanged="DetailChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
