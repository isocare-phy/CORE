<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_dp_yearproc_wizard_new.aspx.cs" Inherits="Saving.Applications.ap_deposit.w_dlg_dp_yearproc_wizard_new" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postCloseYear%>
    <script type="text/javascript">
        function CloseDayFinish() {
            Gcoop.RemoveIFrame();
        }

        function OnDsMainClicked(sender, row, bName) {
            if (bName == "b_closeyear") {
                var isConfirm = confirm("ต้องการปิดงานสิ้นปีใช่หรือไม่");
                if (isConfirm) {
                    postCloseYear();
                }
            }
            return 0;
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdCloseyear").value == "true") {
                //Gcoop.OpenIFrame(450, 200, "w_dlg_dp_closeday.aspx", "");
                //Gcoop.OpenProgressBar("ประมวลผลปิดสิ้นปี", true, false, CloseDayFinish);
            }
        }

        function Validate() {
            alert("หน้าจอประมวลผลปิดสิ้นปี ไม่มีคำสั่งเซฟ");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <asp:HiddenField ID="HdCloseyear" runat="server" />
</asp:Content>
