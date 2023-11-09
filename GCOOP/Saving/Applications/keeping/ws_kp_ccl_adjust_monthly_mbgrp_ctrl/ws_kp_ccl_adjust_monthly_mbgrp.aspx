<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_kp_ccl_adjust_monthly_mbgrp.aspx.cs" 
Inherits="Saving.Applications.keeping.ws_kp_ccl_adjust_monthly_mbgrp_ctrl.ws_kp_ccl_adjust_monthly_mbgrp" %>
<%@ Register Src="DsProc.ascx" TagName="DsProc" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsProc = new DataSourceTool();
        var dsMain = new DataSourceTool();
        var dsDetail = new DataSourceTool();

        function OnDsProcClicked(s, r, c) {
            if (c == "b_process") {
                if (confirm("คุณต้องการประมวลยกเลิกใช่หรือไม่ ?")) {
                    JsPostProcessCcl();
                }
            } 
        }

        function OnDsProcItemChanged(s, r, c, v) {
            if (c == "membgroup_desc") {
                var membcode = $('#ctl00_ContentPlace_dsProc_FormView1_membgroup_desc').find(":selected").val().trim();
                dsProc.SetItem(0, "membgroup_code", membcode);
            } else if (c == "slipretcause_desc") {
                var slipretcausecode = $('#ctl00_ContentPlace_dsProc_FormView1_slipretcause_desc').find(":selected").val().trim();
                dsProc.SetItem(0, "slipretcause_code", slipretcausecode);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsProc ID="dsProc" runat="server" />
    <uc2:DsMain ID="dsMain" runat="server" Visible="false" />
    <uc3:DsDetail ID="dsDetail" runat="server" Visible="false" />
</asp:Content>
