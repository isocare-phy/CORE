<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_as_bankbranch.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_bankbranch" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnDwMainClicked(s, r, c, v) {
            var branch_name = s.GetItem(r, "branch_name");
            var branch_id = s.GetItem(r, "branch_id");
            alert("branch_name :" + branch_name + " branch_id :" + branch_id);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_cm_bankbranch"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" ClientEventClicked="OnDwMainClicked" Height="270px">
    </dw:WebDataWindowControl>
</asp:Content>
