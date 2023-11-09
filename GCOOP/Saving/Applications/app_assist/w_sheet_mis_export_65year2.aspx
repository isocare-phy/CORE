<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_mis_export_65year2.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_mis_export_65year2" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsButtonShowData%>
    <%=expExcel%>
    <script type="text/javascript">

        function OnDwmainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_retrive") {
                objDw_main.AcceptText();
                jsButtonShowData();
            }
            else if (buttonName == "b_export") {
                objDw_main.AcceptText();
                expExcel();
            }
        }
        function OnDwmainItemChanged(sender, rowNumber, columnName, newValue) {
            objDw_main.SetItem(rowNumber, columnName, newValue);
            objDw_main.AcceptText();
            if (columnName == "start_tdate") {
                objDw_main.SetItem(rowNumber, "start_date", Gcoop.ToEngDate(newValue));
                objDw_main.AcceptText();
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            font-size: small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style1" colspan="2">
                <strong>วันที่ทำรายการ</strong>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Panel ID="Panel1" runat="server" Height="70px" Width="700px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_dlg_daterange_p1"
                        LibraryList="~/DataWindow/mis/smsobjects.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwMainItemChange" ClientEventClicked="OnDwMainClick"
                        ClientEventButtonClicked="OnDwmainButtonClick">
                    </dw:WebDataWindowControl>
                    <br />
                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" 
                        oncheckedchanged="CheckBox1_CheckedChanged" />
                    <asp:Button ID="Button1" runat="server" Text="ดาวน์โหลดข้อมูล" OnClick="Button1_Click"
                        EnableTheming="True" Enabled="False" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <strong>ข้อมูลรายละเอียดกองทุนสวัสดิการเงินสะสม 65 ปีมีสุข แบบที่2</strong>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main1" runat="server" DataWindowObject="d_export_egat_sms_request_65y2"
                    LibraryList="~/DataWindow/mis/smsobjects.pbl" AutoRestoreContext="false" AutoRestoreDataCache="true"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked="OnDwMain1Click"
                    RowsPerPage="20">
                    <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                    <BarStyle HorizontalAlign="Center" />
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="HdDeptAccNo" runat="server" />
                <asp:HiddenField ID="HdDeptAccName" runat="server" />
            </td>
        </tr>
    </table>
    <br />
</asp:Content>