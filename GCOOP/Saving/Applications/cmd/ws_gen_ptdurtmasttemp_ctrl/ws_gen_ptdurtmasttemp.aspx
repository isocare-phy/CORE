<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_gen_ptdurtmasttemp.aspx.cs" 
Inherits="Saving.Applications.cmd.ws_gen_ptdurtmasttemp_ctrl.ws_gen_ptdurtmasttemp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnCkPostPtmast() {
            CkPostPtmast();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <input type="button" value="สร้างข้อมูลลง PTDURTMASTER" style="width:250px" onclick="OnCkPostPtmast()" />
</asp:Content>
