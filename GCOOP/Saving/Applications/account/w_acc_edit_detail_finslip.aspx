<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_acc_edit_detail_finslip.aspx.cs" Inherits="Saving.Applications.account.w_acc_edit_detail_finslip" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
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
                <strong>เลือกช่วงเวลาการออกรายงาน</strong>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:Panel ID="Panel1" runat="server" Height="70px" Width="700px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_dlg_daterange_finslip"
                    LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                        ClientEventItemChanged="OnDwMainItemChange" ClientEventClicked="OnDwMainClick"
                        ClientEventButtonClicked="OnDwmainButtonClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                <strong>ข้อมูลรายละเอียด</strong>
            </td>
        </tr>
        <tr>
            <td>
            <div id="detail" runat="server">
                <dw:WebDataWindowControl ID="Dw_main1" runat="server" DataWindowObject="d_vc_edit_detail_finslip"
                    LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="false" AutoRestoreDataCache="true"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventClicked="OnDwMain1Click"
                    RowsPerPage="20">
                    <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                        <BarStyle HorizontalAlign="Center" />
                    </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </div>
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



