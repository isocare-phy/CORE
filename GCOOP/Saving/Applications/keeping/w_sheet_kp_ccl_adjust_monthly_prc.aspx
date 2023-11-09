<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kp_ccl_adjust_monthly_prc.aspx.cs"
    Inherits="Saving.Applications.keeping.w_sheet_kp_ccl_adjust_monthly_prc" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=runProcess%>
    <script type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table class="DataSourceFormView">
        <tr>
            <td width="15%">
                <div>
                    <span>งวด : </span>
                </div>
            </td>
            <td width="35%">
                <asp:TextBox ID="recv_period" runat="server" Style="text-align: center"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td width="15%">
                <div>
                    <span>ตั้งแต่สังกัด : </span>
                </div>
            </td>
            <td width="35%">
                <asp:DropDownList ID="membgroup_code" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
        <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_kp_adjust_monthly_main"
            LibraryList="~/DataWindow/keeping/kp_ccl_adjust_monthly.pbl" ClientScriptable="True"
            ClientEventItemChanged="itemChanged" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventButtonClicked="Click_search"
            ClientEventClicked="checkMain" TabIndex="1">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_kp_adjust_monthly_detail"
            LibraryList="~/DataWindow/keeping/kp_ccl_adjust_monthly.pbl" ClientScriptable="True"
            AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" TabIndex="500" ClientEventButtonClicked="OnDwdetailButtonClicked"
            ClientEventClicked="OnDwDetailClick" ClientEventItemChanged="OnDwDetailItemChange">
        </dw:WebDataWindowControl>
    <asp:Button ID="B_Pro" runat="server" Text="B_Pro" UseSubmitBehavior="False" OnClick="B_Pro_Click" />
    <asp:HiddenField ID="HdLogFile" runat="server" />
    <asp:HiddenField ID="HdERR" runat="server" />
    <asp:HiddenField ID="Hdmode" runat="server" />
    <%=outputProcess%>
</asp:Content>
