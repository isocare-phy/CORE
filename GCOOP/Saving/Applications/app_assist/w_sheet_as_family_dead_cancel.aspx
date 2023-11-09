<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_family_dead_cancel.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_family_dead_cancel" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <script type="text/javascript">

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function InsertRow() {
            objdw_status.InsertRow(0);
        }

//        function OnDwMainButtonClick(s, r, c) {
//            if (c == "b_pay") {
//                postPay();
//            }
//        }

        function DwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" Width="800px" ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_as_family_pay_cancel"
                        LibraryList="~/DataWindow/app_assist/kt_paydead.pbl" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientFormatting="True"  ClientEventItemChanged="DwMainItemChange">
                        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                            <BarStyle HorizontalAlign="Center" />
                            <NumericNavigator FirstLastVisible="True" />
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                   <%-- ClientEventButtonClicked="OnDwMainButtonClick"--%>
                    <br />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdDelRow" runat="server" />
    <asp:HiddenField ID="HdCountStatusRow" runat="server" />
</asp:Content>
