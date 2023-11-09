<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_estsumday.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_pm_estsumday_ctrl.w_sheet_pm_estsumday" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsList = new DataSourceTool();
        var dsMain = new DataSourceTool();
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDsListClicked(s, r, c) {
            if (c == "b_del") {
//                dsList.SetRowFocus(r); //ให้รู้ว่ากดแถวไหนในลิส
//                PostdeleteRow(); //PostDeleteRow(); ไว้ลบแถวที่เลือก
            }
        }
        function OnDsMainItemChanged(s,r,c,v) {
            dsMain.SetItem(r, c, v);
            if (c == "dropdown") {
                Postretrivegropdown();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc2:DsMain ID="dsMain" runat="server" />
    <uc1:DsList ID="dsList" runat="server" />
</asp:Content>
