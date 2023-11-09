<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_sum_balance_carried_forward.aspx.cs" 
    Inherits="Saving.Applications.app_assist.w_sheet_as_sum_balance_carried_forward" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Start ส่วนของ javascript--%>
    <%= initJavaScript     %>
    <%= postAddDistance %>
    <%= postEditDistance %>
    <%= postSaveEdit   %>
    <%= postRefresh   %>  
    <%= postShowItemMain%> 

   
   
<script type="text/javascript">

    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล?");
    }

    function DwShowItemButtonClick(sender, rowNumber, buttonName ) {
        if (buttonName == "b_show") {
            alert(buttonName  = "ยืนยันการประมวณผล");
            objDwMain.AcceptText();
            postShowItemMain();

            }
    }

    function DwEditButtonClick(sender, rowNumber, buttonName) {
        if (buttonName == "b_edit") {
            alert(buttonName);
            objDwAdd.AcceptText();
            postSaveEdit();

        }
    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="text-align="center">  
        <tr>
            <td align="left" style="width: 500px">
                <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    ClientFormatting="True" ClientScriptable="True" DataWindowObject="as_main_bacarried_forward"
                    LibraryList="~/DataWindow/app_assist/assis.pbl" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientEventButtonClicked="DwShowItemButtonClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 500px">
                <dw:WebDataWindowControl ID="DwAdd" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" ClientFormatting="True" ClientScriptable="True" 
                    DataWindowObject="as_detail_bacarried_forward"
                    LibraryList="~/DataWindow/app_assist/assis.pbl" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientEventButtonClicked="DwEditButtonClick"> 
                </dw:WebDataWindowControl>
            </td>
        </tr>

    </table>
     <asp:HiddenField ID="HdDetailRow" runat="server" />
<%--<asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HfProjectId" runat="server" />
    <asp:HiddenField ID="HfCourseId" runat="server" />
   
    <asp:HiddenField ID="HfColumn" runat="server" />--%>
</asp:Content>
