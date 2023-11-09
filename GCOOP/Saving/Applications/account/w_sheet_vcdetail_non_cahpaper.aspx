<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_vcdetail_non_cahpaper.aspx.cs"
    Inherits="Saving.Applications.account.w_sheet_vcdetail_non_cahpaper" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=GetVcdetailForsetNoncash%>
    <%=refresh%>
    <script type="text/javascript">
        function Validate(){
            var startDate = "";
            var endDate = "";
            var accId = "";

            try{
                startDate = objDwMain.GetItem(1,"start_tdate");
            }catch(err){startDate = "";}
            try{
                endDate = objDwMain.GetItem(1,"endDate");
            }catch(err){endDate = "";}
            try{
                accId = objDwMain.GetItem(1,"account_id");
            }catch(err){accId = "";}
            if(startDate != "" && startDate != null && endDate != "" && endDate != null && accId != "" && accId != null){
                return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?่");
            }
            else{
                alert("กรุณากรอกข้อมูลให้ครบ");
            }
        }
        
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
            if(columnName == "start_tdate" || columnName == "end_tdate"){
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                return 0;
            }
            else if(columnName == "account_id"){
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                refresh();
            }
        }
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                var startDate = "";
                var endDate = "";
                var accId = "";

                try{
                    startDate = objDwMain.GetItem(1,"start_tdate");
                }catch(err){startDate = "";}
                try{
                    endDate = objDwMain.GetItem(1,"endDate");
                }catch(err){endDate = "";}
                try{
                    accId = objDwMain.GetItem(1,"account_id");
                }catch(err){accId = "";}
                
                if(startDate != "" && startDate != null && endDate != "" && endDate != null && accId != "" && accId != null){
                    GetVcdetailForsetNoncash();
                }
                else{
                    alert("กรุณากรอกข้อมูลให้ครบ");
                }      
            }
        }
        
        function OnDwDetailClicked(sender, rowNumber, objName){
            Gcoop.CheckDw(sender, rowNumber, objName, "non_cashpaper", 1, 0);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_vc_vcedit_vcdetail_main"
        LibraryList="~/DataWindow/account/vc_vcedit_vcdetail_non_cahpaper.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_vc_vcedit_vcdetail_non_cahpaper" LibraryList="~/DataWindow/account/vc_vcedit_vcdetail_non_cahpaper.pbl"
        ClientFormatting="True" ClientEventClicked="OnDwDetailClicked">
    </dw:WebDataWindowControl>
</asp:Content>
