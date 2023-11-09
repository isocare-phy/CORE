<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_dp_approve_chgdept.aspx.cs" Inherits="Saving.Applications.ap_deposit.w_sheet_dp_approve_chgdept_ctrl.w_sheet_dp_approve_chgdept" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnDsMainClicked(s, r, c, v) {
            if (c == "b_search") {
                PostSearch();
            }
        }
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "all_check") {
                for (var i = 0; i < dsList.GetRowCount(); i++) {
                    dsList.SetItem(i, "choose_flag", v);
                }
            }
        }
        function Validate() {            
            return confirm("ยืนยันการเปลี่ยนแปลงฝากรายเดือน");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsList ID="dsList" runat="server" />
</asp:Content>