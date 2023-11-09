<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ag_adjustreceive.aspx.cs" Inherits="Saving.Applications.agency.w_sheet_ag_adjustreceive" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postCalMemMain %>
    <%=postInitAdjustReceive %>

    <script type="text/javascript">

        function Validate() {
            var recv_period = "";
            var member_no = "";
            var itempaytype_code = "";
            var adj_tday = "";
            var adj_amt = 0;
            var cause_code = "";
            try{
                recv_period = objDwMain.GetItem(1, "recv_period");
            }catch(Err){recv_period = "";}
            try{
                member_no = objDwMain.GetItem(1, "member_no");
            }catch(Err){member_no = "";}
            try{
                itempaytype_code = objDwDetail.GetItem(1, "itempaytype_code");
            }catch(Err){itempaytype_code = "";}
            try{
                cause_code = objDwDetail.GetItem(1, "cause_code");
            }catch(Err){cause_code = "";}
            try{
                adj_tday = objDwDetail.GetItem(1, "adj_tday");
            }catch(Err){adj_tday = "";}
            try{
                adj_amt = objDwDetail.GetItem(1, "adj_amt");
            }catch(Err){adj_amt = 0;}
            
            if(recv_period != "" && recv_period != null && member_no != "" && member_no != null && itempaytype_code != "" && itempaytype_code != null && adj_tday != "" && adj_tday != null && adj_amt != 0 && adj_amt != null && adj_amt > 0 && cause_code != "" && cause_code != null){
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else{
                alert("กรุณากรอกข้อมูลให้ครบ และค่าปรับปรุงควรมีค่ามากกว่า 0");
            }
            
        }

        function MenubarNew() {
            if (confirm("ยืนยันการลบข้อมูล ??")) {
                window.location = Gcoop.GetUrl() + "Applications/agency/w_sheet_ag_adjustreceive.aspx";
            }
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_memsearch") {
                Gcoop.OpenDlg(730, 600, "w_dlg_ag_searchagentmem.aspx", "");
            }
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            if (columnName == "recv_period" || columnName == "member_no") {
                var recv_period = objDwMain.GetItem(1, "recv_period");
                var member_no = objDwMain.GetItem(1, "member_no");
                objDwMain.AcceptText();
                if (recv_period != null && member_no != null) {
                    objDwMain.AcceptText();
                    postInitAdjustReceive();
                }
            }
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "adj_amt") {
                var itemType = "";
                try{
                    itemType = objDwDetail.GetItem(1,"itempaytype_code");
                }
                catch(Err){itemType = "";}
                if(itemType != "" && itemType != null){
                    objDwDetail.SetItem(rowNumber, columnName, newValue);                   
                    objDwDetail.AcceptText();
                    var adjAmt = objDwDetail.GetItem(rowNumber,columnName);
                    if(adjAmt < 0 || adjAmt == 0){
                        alert("ยอดปรับปรุงควรมีค่ามากกว่า 0");
                    }
                    else{
                        postCalMemMain();
                    }
                }
                else
                {
                    alert("กรุณาเลือกประเภทด้วยครับ");
                }
            }
            else if(columnName == "itempaytype_code"){
                var adjAmt = 0;
                try{
                    adjAmt = objDwDetail.GetItem(1, "adj_amt");
                }
                catch(Err){adjAmt = 0;}
                if(adjAmt != 0 && adjAmt != null){
                    objDwDetail.SetItem(rowNumber, columnName, newValue);
                    objDwDetail.AcceptText();
                    postCalMemMain();
                }
                else{
                    objDwDetail.SetItem(rowNumber, columnName, newValue);
                    objDwDetail.AcceptText();
                }
            }
            else if(columnName == "cause_code"){
                objDwDetail.SetItem(rowNumber,columnName,newValue);
                objDwDetail.AcceptText();
            }
            
        }

        function SearchMember(member_no) {
            objDwMain.SetItem(1, "member_no", member_no);
            var recv_period = objDwMain.GetItem(1, "recv_period");
            var member_no = objDwMain.GetItem(1, "member_no");
            if (recv_period != null && member_no != null) {
                objDwMain.AcceptText();
                postInitAdjustReceive();
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
        LibraryList="~/DataWindow/agency/adjustreceive.pbl" ClientFormatting="True" ClientEventItemChanged="DwMainItemChange"
        ClientEventButtonClicked="DwMainButtonClick">
    </dw:WebDataWindowControl>
    <br />
    <b>ทำรายการปรับปรุง</b>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_agentsrv_initadjustreceive_detail" LibraryList="~/DataWindow/agency/adjustreceive.pbl"
        ClientFormatting="True" ClientEventItemChanged="DwDetailItemChange">
    </dw:WebDataWindowControl>
</asp:Content>
