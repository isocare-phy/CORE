<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_as_balance_carried.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_balance_carried" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Start ส่วนของ javascript--%>
    <%= initJavaScript     %>
    <%= postAddDistance   %>
    <%= postEditDistance %>
    <%= postSaveEdit   %>
    <%= postRefresh   %>
  
   
<script type="text/javascript">

//        function Validate(){
//            return confirm("ยืนยันการบันทึกข้อมูล?");
//        }

    function DwShowitemButtonClick(sender, rowNumber, buttonName) {
        if (buttonName == "b_show") {

            var = ObjectDwmain.GetItem(1,"envcode");
            var = ObjectDwMain.GetItem(1,"month");
            var = ObjectDwMain.GetItem(1,"year");  
            objDwAdd.AcceptText();
            postShowItem();

        }
    }
            function DwShowItemMain(sender, rowNumber, buttonName) {
            if (rowNumber == "budget_envcode") {
                objDwMain.SetItem(1, sender, rowNumber);
                objDwMain.AcceptText();
                postShowItemMain();
            }
        }

    function DwAddButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_add") {

            var = ObjectDwmain.GetItem(1,"envcode");
            var = ObjectDwMain.GetItem(1,"month");
            var = ObjectDwMain.GetItem(1,"year");  
            objDwAdd.AcceptText();
            postAddDistance();
        }
    }

    function DwEditButtonClick(sender, rowNumber, buttonName) {
        if (buttonName == "b_edit") {
            objDwEdit.AcceptText();
            postSaveEdit();
        }
    }

    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
</asp:Content>
