<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_as_assistbegin.aspx.cs" Inherits="Saving.Applications.assist.ws_as_assistbegin_ctrl.ws_as_assistbegin" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    var dsMain = new DataSourceTool();
    var dsList = new DataSourceTool();


    function MenubarNew() {
        newclear();
    }


    function OnDsMainClicked(s, r, c) {
        if (c == "b_select") {
            var assist_year = dsMain.GetItem(0, "assist_year");
            if (assist_year == "" || assist_year == null ) {
                alert("กรุณาระบุปีสวัสดิการให้ครบถ้วน");
            }
            else {
                jsPostGetlist();
            }
        }
    }


    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล")
    }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
    <uc2:DsList ID="dsList" runat="server" />
    <br />



    
</asp:Content>
