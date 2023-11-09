<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_cmd_dealer.aspx.cs" 
Inherits="Saving.Applications.cmd.ws_cmd_dealer_ctrl.ws_cmd_dealer" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();

    function MenubarOpen() {
        Gcoop.OpenIFrame2("600", "580", "w_dlg_dealer_search.aspx", "")
    }

    function MenubarNew() {
        newclear();
    }

    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    function SheetLoadComplete() {
    }

    function OnDsMainClicked(s, r, c, v) {
        if (c == "b_del") {
            if (confirm("ต้องการลบข้อมูลใช่หรือไม่")) {
                dsMain.SetRowFocus(r);
                Postdel();
            }
        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
</asp:Content>
