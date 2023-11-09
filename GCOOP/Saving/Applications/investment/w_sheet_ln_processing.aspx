<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_processing.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_processing" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postRefresh%>
    <%=postNewClear %>
    <script type="text/JavaScript">
        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการประมวลผล");
        }

        function OnDwMainItemChange(s, r, c, v) {
            if (c == "datarange_type") {
                objDwMain.SetItem(1, "datarange_type", v);
                objDwMain.AcceptText();
                postRefresh();
            }
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("Hd_process").value == "true") {
                Gcoop.OpenProgressBar("ประมวลผลแจ้งยอดเงินที่ต้องชำระ", true, true, ProcComplete);
            }
        }

        function ProcComplete() {
            postNewClear();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcsrv_proc_noticemthrecv_option"
        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        AutoRestoreContext="False" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="Hd_process" runat="server" />

    <br />
</asp:Content>
