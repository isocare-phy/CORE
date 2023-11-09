<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_agentdetail.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_agentdetail" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initAgentList%>
    <%=initAgentDetail%>
    <%=PostsearchMembNo %>

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
                    initAgentList();
                }
            }
        }
        
        function OnDwListClicked(sender, rowNumber, objectName){
            if( rowNumber != 0 && objectName != "agentgrp_code_t" && objectName != "member_type_t")
            {
                Gcoop.GetEl("HdRowList").value = rowNumber + ""; 
                initAgentDetail();
            } 
        }
        
        function showTabPage2(tab) {
            var i = 1;
            var tabamount = 7;
            for (i = 1; i <= tabamount; i++) {
                    document.getElementById("tab_" + i).style.visibility = "hidden";
                    document.getElementById("stab_" + i).className = "tabTypeTdDefault";
                if (i == tab) {
                    document.getElementById("tab_" + i).style.visibility = "visible";
                    document.getElementById("stab_" + i).className = "tabTypeTdSelected";
                    Gcoop.GetEl("HDCurrentTab").value = i + "";
                }   
            }
        }
        
        function SheetLoadComplete(){
            var tab = Gcoop.ParseInt(Gcoop.GetEl("HDCurrentTab").value);
            showTabPage2(tab);
        }
        
        function searchMembNo() {
            PostsearchMembNo();
        }
        
    </script>

    <style type="text/css">
        .tabTypeDefault
        {
            width: 100%;
            border-spacing: 2px;
        }
        .tabTypeTdDefault
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #777777;
            border: solid 1px #55A9CD;
            background-color: rgb(200,235,255);
            cursor: pointer;
        }
        .tabTypeTdSelected
        {
            width: 20%;
            height: 45px;
            font-family: Tahoma, Sans-Serif, Times;
            font-size: 12px;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
            color: #660066;
            border: solid 1px #77CBEF;
            background-color: #76EFFF;
            cursor: pointer;
            text-decoration: underline;
        }
        .tabTypeTdDefault:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTypeTdSelected:hover
        {
            color: #882288;
            border: solid 1px #77CBEF;
            background-color: #98FFFF;
        }
        .tabTableDetail
        {
            width: 99%;
        }
        .tabTableDetail td
        {
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td class="style5">
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge">
                    <span class="style3">
                        <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_initagentdetail_main"
                            LibraryList="~/DataWindow/agency/ag_agentdetail.pbl" ClientFormatting="True"
                            ClientEventButtonClicked="OnDwMainButtonClicked">
                        </dw:WebDataWindowControl>
                    </span>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td class="style5" valign="top" width="280px">
                <span class="style3">
                    <asp:Panel ID="Panel2" runat="server" Height="400px" ScrollBars="Vertical" BorderStyle="Ridge">
                        <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_initagentdetail_list"
                            LibraryList="~/DataWindow/agency/ag_agentdetail.pbl" ClientFormatting="True"
                            Width="280px" ClientEventClicked="OnDwListClicked">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </span>
            </td>
            <td>
                <table class="tabTypeDefault">
                    <tr valign="top">
                        <td class="tabTypeTdSelected" id="stab_1" onclick="showTabPage2(1);">
                            สังกัด
                        </td>
                        <td class="tabTypeTdDefault" id="stab_2" onclick="showTabPage2(2);">
                            ทะเบียน
                        </td>
                        <td class="tabTypeTdDefault" id="stab_3" onclick="showTabPage2(3);">
                            ชำระเพิ่ม
                        </td>
                        <td class="tabTypeTdDefault" id="stab_4" onclick="showTabPage2(4);">
                            คืนเงิน
                        </td>
                        <td class="tabTypeTdDefault" id="stab_5" onclick="showTabPage2(5);">
                            ยกเลิก
                        </td>
                        <td class="tabTypeTdDefault" id="stab_6" onclick="showTabPage2(6);">
                            ย้ายเข้า
                        </td>
                        <td class="tabTypeTdDefault" id="stab_7" onclick="showTabPage2(7);">
                            ย้ายออก
                        </td>
                    </tr>
                </table>
                <span class="style3">ค้นหาเลขสมาชิก
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="ค้นหา" OnClientClick="searchMembNo()" />
                    <table class="tabTableDetail">
                        <tr>
                            <td valign="top" style="height: 400px;">
                                <div id="tab_1" style="visibility: visible; position: absolute;">
                                    <span class="style3">
                                        <asp:Panel ID="Panel3" runat="server" Height="350px" Width="450px" BorderStyle="Ridge"
                                            ScrollBars="Auto">
                                            <dw:WebDataWindowControl ID="DwTabGrp" runat="server" AutoRestoreContext="False"
                                                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                                DataWindowObject="d_agentsrv_initagentdetail_group" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                                ClientFormatting="True" Width="450px">
                                            </dw:WebDataWindowControl>
                                        </asp:Panel>
                                    </span>
                                </div>
                                <div id="tab_2" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel4" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabMem" runat="server" AutoRestoreContext="False"
                                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                            DataWindowObject="d_agentsrv_initagentdetail_mem" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                            ClientFormatting="True" Width="450px" RowsPerPage="10">
                                            <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                                            </PageNavigationBarSettings>
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                                <div id="tab_3" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel5" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabAdd" runat="server" AutoRestoreContext="False"
                                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                            DataWindowObject="d_agentsrv_initagentdetail_add" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                            ClientFormatting="True" Width="450px">
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                                <div id="tab_4" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel6" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabAdj" runat="server" AutoRestoreContext="False"
                                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                            DataWindowObject="d_agentsrv_initagentdetail_retadj" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                            ClientFormatting="True" Width="450px">
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                                <div id="tab_5" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel7" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabCancel" runat="server" AutoRestoreContext="False"
                                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                            DataWindowObject="d_agentsrv_initagentdetail_cancel" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                            ClientFormatting="True" Width="450px">
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                                <div id="tab_6" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel8" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabIn" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                                            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_initagentdetail_movegrpin"
                                            LibraryList="~/DataWindow/agency/ag_agentdetail.pbl" ClientFormatting="True"
                                            Width="450px">
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                                <div id="tab_7" style="visibility: hidden; position: absolute;">
                                    <asp:Panel ID="Panel9" runat="server" Height="350px" BorderStyle="Ridge" Width="450px"
                                        ScrollBars="Auto">
                                        <dw:WebDataWindowControl ID="DwTabOut" runat="server" AutoRestoreContext="False"
                                            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                            DataWindowObject="d_agentsrv_initagentdetail_movegrpout" LibraryList="~/DataWindow/agency/ag_agentdetail.pbl"
                                            ClientFormatting="True" Width="450px">
                                        </dw:WebDataWindowControl>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HDCurrentTab" runat="server" Value="01" />
    <asp:HiddenField ID="HdRowList" runat="server" />
</asp:Content>
