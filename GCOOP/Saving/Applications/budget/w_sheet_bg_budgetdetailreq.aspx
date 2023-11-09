<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetdetailreq.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetdetailreq" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=InsertRow%>
    <%=DeleteRow%>
    <%=RetrieveBgType%>
    <%=RetrieveList%>
    <%=Refresh%>
    <%=CheckBgAmt%>
    <%=FilterBgGrp%>

    <script type="text/javascript">
        function OnClickInsertRow(){
            if(Gcoop.GetEl("HdInsert").value == "true"){
                InsertRow();
            }
        }
            
        function OnClickDeleteRow(){
            if(objDwList.RowCount() > 0){
                DeleteRow();
            }
        }
        
        function OnDwListClicked(sender, rowNumber, object){
            for (var i = 1; i <= sender.RowCount(); i++) {
                sender.SelectRow(i, false);
            }
            sender.SelectRow(rowNumber, true);
            sender.SetRow(rowNumber);
            Gcoop.GetEl("HdListRow").value = rowNumber + "";
        }
        
        function OnDwListItemChanged(sender, rowNumber, columnName, newValue){
            if(columnName == "budgetdetail_amt"){
                objDwList.SetItem(rowNumber, columnName, newValue);
                objDwList.AcceptText();
                var flag = objDwList.GetItem(rowNumber, "budgetdetail_flag");
                if (flag == 1){                  
                    CheckBgAmt();
                }
            }
            else if(columnName == "budgetdetail_flag"){
                objDwList.SetItem(rowNumber, columnName, newValue);
                objDwList.AcceptText();
                var amt = 0;
                try{
                    amt = objDwList.GetItem(rowNumber, "budgetdetail_amt");
                }catch(err){ amt = 0; }
                if (amt != 0 || amt != null){
                    CheckBgAmt();
                }
            }
        }
        
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                var bgGrpCode = 0;
                var bgYear = 0;
                var bgTypeCode = 0;
                try{
                    bgYear = objDwMain.GetItem(1, "budgetyear");
                }catch(err){ bgYear = 0; }
                try{
                    bgGrpCode = objDwMain.GetItem(1, "budgetgroup_code");
                }catch(err){ bgGrpCode = 0; }
                try{
                    bgTypeCode = objDwMain.GetItem(1, "budgettype_code");
                }catch(err){ bgTypeCode = 0; }
                
                if(( bgGrpCode != null && bgGrpCode != 0 ) && ( bgYear != 0 && bgYear != null ) && ( bgTypeCode != 0 && bgTypeCode != null )){
                    Gcoop.GetEl("HdInsert").value = "true";
                    RetrieveList();
                }
                else{
                    alert("กรุณาเลือกปี หมวด และประเภทงบประมาณก่อน");
                }    
            }
        }
        
        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue){
            if(columnName == "budgetgroup_code"){
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                RetrieveBgType();
            }
            else if(columnName == "budgetyear"){
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                FilterBgGrp();
            }
        }

        function Validate(){
            var check = false;
            var detailId = "";
            var detailDesc = "";
            var detailFlag = -9;
            var contrl = "";
            var checkBg = Gcoop.GetEl("HdBgAmt").value;
            for(var i = 1; i <= objDwList.RowCount(); i++){
                try{
                    detailId = objDwList.GetItem(i, "budgetdetail_id");
                }
                catch(err){detailId = "";}
                try{
                    detailDesc = objDwList.GetItem(i, "budgetdetail_desc");  
                }
                catch(err){detailDesc = "";}     
                try{
                    detailFlag = objDwList.GetItem(i, "budgetdetail_flag");  
                }
                catch(err){detailFlag = -9;}     
                try{
                    contrl = objDwList.GetItem(i, "budgetcontrol");  
                }
                catch(err){contrl = "";}  
                
                if(detailId != "" && detailId != null && detailDesc != "" && detailDesc != null && detailFlag != null && detailFlag != -9 && contrl != "" && contrl != null && checkBg == "true"){
                    check = true;
                } 
                else{
                    check = false;
                    break;
                }
            }
            if(check == true){
                return confirm("บันทึกข้อมูล ใช่หรือไม่?");
            }
            else if(objDwList.RowCount() < 1){
                alert("ไม่มีข้อมูล กรุณาทำการเพิ่มแถวก่อน");
            }
            else if(checkBg != "true"){
                alert("จำนวนเงินรายละเอียดทั้งหมด ไม่ควรมีค่ามากกว่างบประมาณที่ตั้งไว้");
            }
            else{
                alert("กรุณากรอกข้อมูลให้ครบถ้วน");
            }
        }
        
        function SheetLoadComplete(){
            var check = Gcoop.GetEl("HdBgAmt").value;
            if(check == "false"){
                alert("จำนวนเงินรายละเอียดทั้งหมด ไม่ควรมีค่ามากกว่างบประมาณที่ตั้งไว้");
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Label ID="Label1" runat="server" Text="รายละเอียดการตั้งงบประมาณ" Font-Bold="True"
                    Font-Names="Tahoma" Font-Size="16px" Font-Underline="True" />
            </td>
        </tr>
    </table>
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetdetailreq_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    
    <dw:WebDataWindowControl ID="DwAmt" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetdetailreq_amt"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <span onclick="OnClickInsertRow()" style="cursor: pointer;">
        <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
    <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
        <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
    <dw:WebDataWindowControl ID="DwList" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetdetailreq_list"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" RowsPerPage="15"
        ClientEventClicked="OnDwListClicked" ClientEventItemChanged="OnDwListItemChanged">
        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdInsert" runat="server" Value="false" />
    <asp:HiddenField ID="HdBgAmt" runat="server" />
</asp:Content>
