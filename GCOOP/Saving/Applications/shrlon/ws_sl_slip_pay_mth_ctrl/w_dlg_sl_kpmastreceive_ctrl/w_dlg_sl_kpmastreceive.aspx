<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_sl_kpmastreceive.aspx.cs" Inherits="Saving.Applications.shrlon.ws_sl_slip_pay_mth_ctrl.w_dlg_sl_kpmastreceive_ctrl.w_dlg_sl_kpmastreceive" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool();
        var dsList = new DataSourceTool();

        function OnDsMainClicked(s, r, c) {

        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "RECV_PERIOD") {
                PostRefSystem();
            } else if (c == "MEMBER_NO") {
                PostMemberNo();
            }
        }

        function OnDsListClicked(s, r, c) {
            var memberno = dsList.GetItem(r, "MEMBER_NO");
            var receipt_no = dsList.GetItem(r, "RECEIPT_NO");
            var recv_period = dsList.GetItem(r, "RECV_PERIOD"); 

            try {
                window.opener.GetRefSlipFromDialog(memberno, receipt_no, recv_period);
                window.close();
            } catch (err) {
                parent.GetRefSlipFromDialog(memberno, receipt_no, recv_period);
                parent.RemoveIFrame();
            }
        }


        function OnDsListItemChanged(s, r, c, v) {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessageDlg" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsList ID="dsList" runat="server" />
</asp:Content>
