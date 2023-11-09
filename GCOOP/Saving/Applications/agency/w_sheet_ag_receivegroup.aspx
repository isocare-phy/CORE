<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_receivegroup.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_receivegroup" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    <%=initJavaScript%>
    <%=postNewClear %>
    <%=postInitReceiveGroup%>
    <%=postrecv_amt%>
    <%=postSetBranch%>

    <script type="text/javascript">
    function SetBranch(BranchID)
    {
        Gcoop.GetEl("HdBranchId").value = BranchID;
        postSetBranch();
    }
    
    function OnDwDetailClick(s,r,c)
    {
        if(c == "operate_flag")
        {
            Gcoop.CheckDw(s,r,c,"operate_flag",1,0);
            Gcoop.GetEl("HdCurrentrow").value = r + "";
            postrecv_amt();
        }
        return 0;
    }
    
    function OnDwMainButtonClick(s,r,c)
    {
        if(c=="b_search")
        {
            var recv_tday = objDw_main.GetItem(1,"recv_tday");
            if(recv_tday == null || recv_tday == "")
            {
                alert("กรุณากรอกวันที่โอน");
            }
            else
            {
                postInitReceiveGroup();   
            }
        }
        return 0;
    }
    
    function OnDwMainItemChange(s,r,c,v)
    {
        if(c == "recv_tday")
        {
            objDw_main.SetItem(1, "recv_tday", v );
            objDw_main.AcceptText();
            objDw_main.SetItem(1, "recv_day", Gcoop.ToEngDate(v));
            objDw_main.AcceptText();  
        }
        return 0;
    }
    
    function OnDwDetailItemChange(s,r,c,v)
    {
        if(c=="expense_code")
        {
            objDw_detail.SetItem(r,"expense_code",v);
            objDw_detail.AcceptText();
        }
        else if(c == "expense_bank")
        {
            var expense_bank = "";
            objDw_detail.SetItem(r,"expense_bank",v);
            objDw_detail.AcceptText();
            expense_bank = objDw_detail.GetItem(r,"expense_bank");
            if(expense_bank == "" || expense_bank == null)
            {
                alert("กรุณาเลือกประเภทรายการ");
            }
            else
            {
                Gcoop.GetEl("HdCurrentrow").value = r + "";
                Gcoop.OpenDlg(500,500,"w_dlg_agent_bankbranch.aspx","?expense_bank="+ expense_bank);
            }
        }
        else if(c == "tofromacc_id")
        {
            objDw_detail.SetItem(r,"tofromacc_id",v);
            objDw_detail.AcceptText();
        }
        else if(c == "expense_branch")
        {
            objDw_detail.SetItem(r,"expense_branch",v);
            objDw_detail.AcceptText();
            Gcoop.GetEl("HdCurrentrow").value = r + "";
            postBankBranch();
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
        objDw_main.AcceptText();
        objDw_detail.AcceptText();
        return confirm("ยืนยันการบันทึกข้อมูล");
    }
   
    function SaveComplete()
    {
        postNewClear();
    }
    
    function SheetLoadComplete()
    {
        if(Gcoop.GetEl("HdSaveRecGroup").value == "true")
        {
            Gcoop.OpenProgressBar("โอนเงินชำระลูกหนี้ตัวแทนสังกัด(สังกัด)", true, false, SaveComplete);
        }
    }
    
   
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style1">
                เงื่อนไขการดึงข้อมูล
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel1" runat="server">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                        DataWindowObject="d_agentsrv_initreceivegroup_maingroup" LibraryList="~/DataWindow/agency/agent.pbl"
                        ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="OnDwMainButtonClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                รายการสังกัดลูกหนี้ตัวแทน
            </td>
        </tr>
        <tr>
            <td class="style1">
                <asp:CheckBox ID="CheckAll" runat="server" AutoPostBack="True" Font-Bold="False"
                    Text="เลือกทั้งหมด" OnCheckedChanged="CheckAll_CheckedChanged" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" Height="350px" ScrollBars="Horizontal" Width="750px">
                    <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" DataWindowObject="d_agentsrv_initreceivegroup_groupdetail"
                        LibraryList="~/DataWindow/agency/agent.pbl" ClientEventClicked="OnDwDetailClick"
                        ClientEventItemChanged="OnDwDetailItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HdSaveRecGroup" runat="server" />
                <asp:HiddenField ID="HdCurrentrow" runat="server" />
                <asp:HiddenField ID="HdBranchId" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
