<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_as_deptaccount_no.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_deptaccount_no" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnDwMainClicked(sender, rowNumber, objectName) {
            var deptaccount_no = sender.GetItem(rowNumber, "deptaccount_no");
            //alert(deptaccount_no);
            parent.GetDeptAccountNo(deptaccount_no);
            parent.close;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_public_dept_account_no"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" ClientEventClicked="OnDwMainClicked">
    </dw:WebDataWindowControl>
</asp:Content>
