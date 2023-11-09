<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_returnreceive.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_returnreceive" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 19px;
            font-weight: bold;
            font-size: small;
        }
        .style2
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    <%=initJavaScript%>
    <%=postInitReturnReceive%>
    <%=postret_amt%>
    <%=postNewClear%>
    <%=postInitMember %>

    <script type="text/javascript">
    function OnDwMainButtonClick(s,r,c)
    {
        if(c=="b_memsearch")
        {
            Gcoop.OpenDlg(700,620, "w_dlg_ag_searchagentmem.aspx","");            
        }
        return 0;
    }
    
    function InitReturnReceive(){
        var recvPeriod = "";
        var memberNo = "";
        try{
            recvPeriod = objDw_main.GetItem(1, "recv_period");
        }
        catch(err){recvPeriod = "";}
        try{
            memberNo = objDw_main.GetItem(1, "member_no");
        }
        catch(err){memberNo = "";}
        
        if(recvPeriod != "" && recvPeriod != null && memberNo != "" && memberNo != null){
            postInitReturnReceive();             
        }
    }
    function OnDwMainItemChange(s,r,c,v)
    { 
        if( c == "recv_period" || c == "member_no"){
            objDw_main.SetItem(r, c, v);
            objDw_main.AcceptText();
            InitReturnReceive();
        }
    }
    
    function OnDwDetailItemChange(s,r,c,v)
    {
        if(c=="ret_tday")
        {
            objDw_detail.SetItem(1, "recv_tday", v );
            objDw_detail.AcceptText();
            objDw_detail.SetItem(1, "recv_day", Gcoop.ToEngDate(v));
            objDw_detail.AcceptText(); 
        }
        else if(c == "ret_amt")
        {
            objDw_detail.SetItem(r,c,v);
            objDw_detail.AcceptText();
            var retAmt = objDw_detail.GetItem(r,c);
            if( retAmt <= 0 ){
                alert("ยอดคืนควรมีค่ามากกว่า 0");
            }
            else{
                postret_amt();
            }
        }
        else if( c == "itempaytype_code" || c == "cause_code" ){
            objDw_detail.SetItem(r,c,v);
            objDw_detail.AcceptText();
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
        var recv_period = "";
        var member_no = "";
        var cause_code = "";
        var itempaytype_code = "";
        var ret_tday = "";
        var ret_amt = 0;
        
        try{
            recv_period = objDw_main.GetItem(1, "recv_period");
        }catch(Err){recv_period = "";}
        try{
            member_no = objDw_main.GetItem(1, "member_no");
        }catch(Err){member_no = "";}
        try{
            itempaytype_code = objDw_detail.GetItem(1, "itempaytype_code");
        }catch(Err){itempaytype_code = "";}
        try{
            cause_code = objDw_detail.GetItem(1, "cause_code");
        }catch(Err){cause_code = "";}
        try{
            ret_tday = objDw_detail.GetItem(1, "ret_tday");
        }catch(Err){ret_tday = "";}
        try{
            ret_amt = objDw_detail.GetItem(1, "ret_amt");
        }catch(Err){ret_amt = 0;}
        
        if(recv_period != "" && recv_period != null && member_no != "" && member_no != null && cause_code != "" && cause_code != null && ret_tday != "" && ret_tday != null && ret_amt != 0 && ret_amt != null && ret_amt > 0 && itempaytype_code != null && itempaytype_code != ""){
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        else{
            alert("กรุณากรอกข้อมูลให้ครบ และยอดคืนควรมีค่ามากกว่า 0");
        }
    }
    
     function SearchMember(member_no) 
     {
        objDw_main.SetItem(1, "member_no", member_no);
        objDw_main.AcceptText();
        postInitMember();
     }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style2">
                รายละเอียดลูกหนี้ตัวแทน
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_agentsrv_mem_main"
                    LibraryList="~/DataWindow/agency/agent.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                    ClientEventButtonClicked="OnDwMainButtonClick" ClientEventItemChanged="OnDwMainItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style1">
                ทำรายการคืนเงิน
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="d_agentsrv_initreturnreceive_detail"
                    LibraryList="~/DataWindow/agency/agent.pbl" ClientEventItemChanged="OnDwDetailItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
