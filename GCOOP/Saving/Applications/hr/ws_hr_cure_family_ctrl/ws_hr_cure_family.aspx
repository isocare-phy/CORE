<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_cure_family.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_cure_family_ctrl.ws_hr_cure_family" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsDetail.ascx" TagName="DsDetail" TagPrefix="uc2" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsDetail = new DataSourceTool;
        var dsList = new DataSourceTool;

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame2('685', '460', 'ws_dlg_hr_master_search.aspx', '');
        }

        function GetEmpNoFromDlg(emp_no) {
            dsMain.SetItem(0, "emp_no", emp_no);
            PostEmpno();
        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "emp_no") {
                PostEmpno();
            }
        }

        

        function OnDsDetailChanged(s, r, c, v) {
            if (c == "approve_total") {
                PostCalhalf();
            }
        }

        function OnDsDsListItemChanged(s, r, c, v) {
            if (c == "money_learn") {
                dsList.SetRowFocus(r);
                PostCalMoney();
            } else if (c == "money_use") {
                dsList.SetRowFocus(r);
                PostCalMoney();
            }
        }

        function OnDsListClicked(s, r, c) {
            if (c == "b_detail") {
                dsList.SetRowFocus(r);
                PostEmpLlist();
            }
        }

        function SheetLoadComplete() {

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsDetail ID="dsDetail" runat="server" />
    <uc3:DsList ID="dsList" runat="server" />
</asp:Content>
