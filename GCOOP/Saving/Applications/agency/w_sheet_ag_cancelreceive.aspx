<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_cancelreceive.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_cancelreceive" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initCancelRecive%>
    <%=calMemMain%>

    <script type="text/javascript">
    
        function OnDwMainButtonClick(sender, rowNumber, colunmName){
            if(colunmName == "b_memsearch"){
                Gcoop.OpenDlg("800px","600px","w_dlg_ag_searchagentmem.aspx","");
            }
        }
        function InitCanCelRecive(){
            var recvPeriod = "";
            var memberNo = "";
            try{
                recvPeriod = objDwMain.GetItem(1, "recv_period");
            }
            catch(err){recvPeriod = "";}
            try{
                memberNo = objDwMain.GetItem(1, "member_no");
            }
            catch(err){memberNo = "";}
            
            if(recvPeriod != "" && recvPeriod != null && memberNo != "" && memberNo != null){
                initCancelRecive();             
            }
        }
        function OnDwMainItemChanged(sender, rowNumber, colunmName, newValue){
            if( colunmName == "recv_period" || colunmName == "member_no"){
                objDwMain.SetItem(rowNumber, colunmName, newValue);
                objDwMain.AcceptText();
                InitCanCelRecive();
            }
        }
        
        function SearchMember(member_no){
            objDwMain.SetItem(1, "", member_no);
            objDwMain.AcceptText();
            InitCanCelRecive();
        }
        
        function OnDwDetailItemChanged(sender, rowNumber, colunmName, newValue){
            if(colunmName == "cancel_amt"){
                objDwDetail.SetItem(rowNumber, colunmName, newValue);
                objDwDetail.AcceptText();
                calMemMain();             
            }
            else if(colunmName == "cancel_tday"){
                objDwDetail.SetItem(rowNumber, colunmName, newValue);
                objDwDetail.AcceptText();
                return 0;
            }
            else if(colunmName == "cause_code"){
                objDwDetail.SetItem(rowNumber, colunmName, newValue);
                objDwDetail.AcceptText();
            }
        }
        
        function Validate(){
        
            var recv_period = "";
            var member_no = "";
            var cause_code = "";
            var cancel_tday = "";
            var cancel_amt = 0;
            
            try{
                recv_period = objDwMain.GetItem(1, "recv_period");
            }catch(Err){recv_period = "";}
            try{
                member_no = objDwMain.GetItem(1, "member_no");
            }catch(Err){member_no = "";}
            try{
                cause_code = objDwDetail.GetItem(1, "cause_code");
            }catch(Err){cause_code = "";}
            try{
                cancel_tday = objDwDetail.GetItem(1, "cancel_tday");
            }catch(Err){cancel_tday = "";}
            try{
                cancel_amt = objDwDetail.GetItem(1, "cancel_amt");
            }catch(Err){cancel_amt = 0;}
            
            if(recv_period != "" && recv_period != null && member_no != "" && member_no != null && cause_code != "" && cause_code != null && cancel_tday != "" && cancel_tday != null && cancel_amt != 0 && cancel_amt != null && cancel_amt > 0){
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else{
                alert("กรุณากรอกข้อมูลให้ครบ และยอดยกเลิกควรมีค่ามากกว่า 0");
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <b>รายละเอียดลูกหนี้ตัวแทน</b>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_mem_main"
        LibraryList="~/DataWindow/agency/ag_cancelreceive.pbl" ClientFormatting="True"
        ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="OnDwMainButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <b>ทำรายการยกเลิก</b>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_agentsrv_initcancelreceipt_detail" LibraryList="~/DataWindow/agency/ag_cancelreceive.pbl"
        ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChanged">
    </dw:WebDataWindowControl>
</asp:Content>
