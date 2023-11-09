<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_listpawn.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_listpawn" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=GetLoan%>
    <%=DelMainRow%>
    <%=JsPostDate%>
    <%=JsWarrantyChange%>
    <script type="text/JavaScript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnClickCri(s, r, c,v) {
            if (c == "b_accountno") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
            }
        }

        function OnChangeCri(s, r, c, v) {
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            if (c == "account_no") {
                GetLoan();
            }
        }
        function OnDwMainChange(s, r, c, v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();
//            if (c == "warranty_status") {
//                Gcoop.GetEl("HdRow").value = r;
//                JsWarrantyChange();
//            }
        }
        function ChooseAcc(account_no) {
            objDwMain.SetItem(1, "account_no", account_no);
            objDwMain.AcceptText();
            GetLoan();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx", "");
        }

        function InsertRow() {
            account_no = objDwMain.GetItem(1, "account_no");
            if (account_no != null && account_no != "") 
            {
                JsPostDate();
            }
        }

        function DelRow() {
            if (confirm("แน่ใจว่าต้องการลบแถว")) {
                DelMainRow();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HRow" runat="server" Value="0" />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_show_account_detail"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientFormatting="True" 
        ClientEventItemChanged="OnChangeCri" ClientEventButtonClicked="OnClickCri">
    </dw:WebDataWindowControl>
    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Width="750px">
        <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_warranty_list"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientFormatting="True" ClientEventItemChanged="OnDwMainChange">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
    <table width="100%">
        <tr>
            <td align="right">
                <input type="button" id="Insert" value="เพิ่มแถว" onclick="InsertRow();" />
                <input type="button" id="Delete" value="ลบแถว" onclick="DelRow();" />
            </td>
        </tr>
    </table>

    <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
</asp:Content>
