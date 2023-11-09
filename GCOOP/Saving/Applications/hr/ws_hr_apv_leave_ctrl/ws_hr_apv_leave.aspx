<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_apv_leave.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_apv_leave_ctrl.ws_hr_apv_leave" %>

<%@ Register Src="DsSearch.ascx" TagName="DsSearch" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsSearch = new DataSourceTool;
        var dsMain = new DataSourceTool;

        function OnDsSearchClicked(s, r, c) {
            if (c == "b_search") {
                dsSearch.SetRowFocus(r);
                PostAPVLEAVE();
            }
        }
        function OnDsMainClicked(s, r, c) {
            if (c == "b_apv") {
                dsMain.SetRowFocus(r);
                var seq_no = dsMain.GetItem(r, "seq_no");
                dsMain.SetItem(r, "seq_no", seq_no);
                PostAPV();
            }
            else if (c == "b_apv_cancel") {
                dsMain.SetRowFocus(r);
                var seq_no = dsMain.GetItem(r, "seq_no");
                dsMain.SetItem(r, "seq_no", seq_no);
                PostCancel();
            }
            else if (c == "b_detail") {
                var fullname = dsMain.GetItem(r, "fullname");
                var leave_type = dsMain.GetItem(r, "leave_desc");
                var leave_from = dsMain.GetItem(r, "leave_from");
                var leave_to = dsMain.GetItem(r, "leave_to");
                var totalday = dsMain.GetItem(r, "totalday");
                var leave_cause = dsMain.GetItem(r, "leave_cause");
                var msg=fullname+"\n"+leave_type+"\n"+leave_from+"\n"+leave_to+"\n"+totalday+"\n"+leave_cause
                alert(msg);
                //Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
            }
        }
        function SheetLoadComplete() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <uc1:DsSearch ID="dsSearch" runat="server" />
    <uc2:DsMain ID="dsMain" runat="server" />
    <asp:HiddenField ID="hdGetDate" runat="server" Value="" />
</asp:Content>
