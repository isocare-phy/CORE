<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_mbshr_mbgain_history.aspx.cs" Inherits="Saving.Applications.mbshr.dlg.w_dlg_mbshr_mbgain_history_ctrl.w_dlg_mbshr_mbgain_history" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function OnDsMainClicked(s, r, c) {
            if (c == "b_search") {
                PostSearch();
            }
        }

        function OnDsMainItemChanged(s, r, c, v) {

            if (c == "member_no") {
                PostMemberNo();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <uc2:DsList ID="dsList" runat="server" />
</asp:Content>
