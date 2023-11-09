<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_clearagent.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_clearagent" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=checkAll%>
    <%=initClearAgent%>
    <script type="text/javascript">
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "b_search"){
                var recvPeriod = "";
                var sGrpCode = "";
                var eGrpCode = "";
                try{
                    recvPeriod = objDwMain.GetItem(1, "recv_period");
                }
                catch(err){recvPeriod = "";}
                try{
                    sGrpCode = objDwMain.GetItem(1, "sagentgrp_code");
                }
                catch(err){sGrpCode = "";}
                try{
                    eGrpCode = objDwMain.GetItem(1, "eagentgrp_code");
                }
                catch(err){eGrpCode = "";}
                if(recvPeriod != "" && recvPeriod != null && sGrpCode != "" && sGrpCode != null && eGrpCode != "" && eGrpCode != null){
                    initClearAgent();
                }
            }
        }
        
        function OnDwDetailClicked(sender, rowNumber, colunmName){
            Gcoop.CheckDw(sender,rowNumber,objectName,"operate_flag",1,0);
        }
         
        function ClickCheckAll(){
            if(objDwDetail.RowCount() > 0){
                checkAll();
            }
        }
                
        function Validate(){
            confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    เงื่อนไขการดึงข้อมูลลูกหนี้ตัวแทนมาล้างข้อมูล
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_initclearagent_main"
        LibraryList="~/DataWindow/agency/ag_clearagent.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <br />
    ทำรายการล้างข้อมูลสังกัดลูกหนี้ตัวแทน
    <br />
    <asp:CheckBox ID="chkAll" runat="server" Text="เลือกทั้งหมด" onclick="ClickCheckAll()" />
    <br />
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="750px">
    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_agentsrv_initclearagent_detail" LibraryList="~/DataWindow/agency/ag_clearagent.pbl"
        ClientFormatting="True" ClientEventClicked="OnDwDetailClicked" Width="750px">
    </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
