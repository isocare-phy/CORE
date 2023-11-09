<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_dlg_ag_agentprocess.aspx.cs" Inherits="Saving.Applications.agency.w_dlg_ag_agentprocess" Title="Untitled Page" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
        .style2
        {
            width: 140px;
        }
        .style3
        {}
    </style>
    <%=initJavaScript%>
    <%=postRefresh%>
    <%=postNewClear%>
    <%=postAgentProcess%>
    <script type ="text/javascript" >
    function B_CancelClick()
    {
        if (confirm("ยืนยันการยกเลิก")) 
       {
           postNewClear();
       }    
    }
    
    function B_ProcessClick()
    {
        var receive_year = objDw_main.GetItem(1,"receive_year");
        var receive_month = objDw_main.GetItem(1,"receive_month");
        var proc_status = objDw_main.GetItem(1,"proc_status");
        if(proc_status == 2)
        {
            var group_text = objDw_main.GetItem(1,"group_text");
            if(group_text == null || group_text == "")
            {
                alert("กรุณากรอกสังกัดลูกหนี้ตัวแทน");
            }
            else
            {
                 postAgentProcess();
            }
        }
        else if(proc_status == 3)
        {
            var mem_text = objDw_main.GetItem(1,"mem_text")
            if(mem_text == null || mem_text == "")
            {
                alert("กรุณากรอกเลขที่สมาชิก");
            }
            else
            {
                 postAgentProcess();
            }
        }
        else
        {
            if(receive_year == null || receive_year == "" || receive_month == null || receive_month == "")
            {
                alert("กรุณากรอกข้อมูล ปี /เดือน");
            }
            else
            {
                postAgentProcess();
            }
        }
    }
    
    function AgentComplete()
    {
        postNewClear();
    }
    
    function OnDwMainItemChange(s,r,c,v)
    {
        if(c == "proc_status")
        {
            objDw_main.SetItem(1,"proc_status",v);
            objDw_main.AcceptText();
            postRefresh();
        }
        else if(c == "receive_month")
        {
            objDw_main.SetItem(1,"receive_month",v);
            objDw_main.AcceptText();    
        }
        else if(c == "group_text")
        {
            objDw_main.SetItem(1,"group_text",v);
            objDw_main.AcceptText();
        }
        else if(c == "mem_text")
        {
            objDw_main.SetItem(1,"mem_text",v);
            objDw_main.AcceptText();    
        }
        else if(c == "proc_type")
        {
            objDw_main.SetItem(1,"proc_type",v);
            objDw_main.AcceptText();
        }
        return 0;
    }
    
     function MenubarNew()
    {
       if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) 
       {
           postNewClear();
       }
    }
    
      function Validate()
    {
    
    }
   
    function SheetLoadComplete()
    {
        if(Gcoop.GetEl("HdAgentProcess").value == "true")
        {
            Gcoop.OpenProgressBar("ประมวลเรียกเก็บลูกหนี้ตัวแทน", true, false, AgentComplete);
        }
    }
    
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width:100%;">
        <tr>
            <td class="style1" colspan="4">
                เงื่อนไขการประมวลผลดึงข้อมูลลูกหนี้ตัวแทน</td>
        </tr>
        <tr>
            <td colspan="4">
                <dw:WebDataWindowControl ID="Dw_main" runat="server" 
                    DataWindowObject="d_ag_trnkeep_option" 
                    LibraryList="~/DataWindow/agency/agent.pbl" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientEventItemChanged="OnDwMainItemChange" ClientFormatting="True" 
                    ClientScriptable="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style2">
                <input id="B_process" type="button" value="ประมวล" onclick = "B_ProcessClick()"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <input id="B_cancel" type="button" value="ยกเลิก" onclick = "B_CancelClick()"/></td>
            <td class="style3" colspan="3">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="HdAgentProcess" runat="server" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
