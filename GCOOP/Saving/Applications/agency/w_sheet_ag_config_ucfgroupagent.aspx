﻿<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_config_ucfgroupagent.aspx.cs" Inherits="Saving.Applications.agency.w_sheet_ag_config_ucfgroupagent" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<%=postInsertRow%>
<%=postDeleteRow%>

    <script type="text/javascript">
     
     function MenubarNew(){
        postInsertRow();
     }
     
    // ฟังก์ชันการเพิ่มแถวข้อมูล
     function AddRow_DwMain() {
        postInsertRow();
     }

    // ฟังก์ชันการลบข้อมูล
     function DeleteRow_DwMain(sender, row, bName) {
         if (bName == "b_del") {
             var agentgrp_code = objDw_main.GetItem(row,"agentgrp_code");
             
             if (agentgrp_code == "" || agentgrp_code == null ){
                Gcoop.GetEl("Hd_row").value = row + "";
                postDeleteRow(); 
             }else {
                var isConfirm = confirm("ต้องการลบข้อมูลสังกัดลูกหนี้ตัวแทนรหัส : " + agentgrp_code + "ใช่หรือไม่ ?" );
                 if (isConfirm) {
                    Gcoop.GetEl("Hd_row").value = row + "";
                    postDeleteRow(); 
                }
             }  
         }
         return 0;
     }
     
 
       
    //ฟังก์ชันการเช็คค่าข้อมูลก่อน save
    function Validate() 
    {
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width:100%;">
        <tr>
            <td>
                <span class="linkSpan" onclick="AddRow_DwMain()" 
                    
                    style="font-family: Tahoma; font-size: small; float: left; color: #0000CC;">เพิ่มแถว</span></td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Height="450px" 
                    ScrollBars="Vertical" Width="750px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" 
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                        ClientFormatting="True" ClientScriptable="True" 
                        DataWindowObject="d_agent_config_ucfgroupagent" 
                        LibraryList="~/DataWindow/agency/agent.pbl" ClientEventButtonClicked="DeleteRow_DwMain">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                    <asp:HiddenField ID="Hd_row" runat="server" />
                </td>
        </tr>
    </table>
</asp:Content>
