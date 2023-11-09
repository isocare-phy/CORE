<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_loan_requestment_return.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loanreqeust_return" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript %>
<%=cancelReturn  %>
<%=jsPostReqReturn  %>
<%=openReqReturn  %>
<%=refresh %>
 <%=popupReport%>
    <%=runProcess%>
<script type="text/javascript">
    function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
    }
     function GetValueFromDlg(memberno)
        {
                Gcoop.GetEl("HdMemberNo").value = memberno;
                jsPostReqReturn();
        }
    function GetValueLoanRequest (docNo)
        {
            objdw_head.SetItem(1, "ref_docno", docNo );
            objdw_head.AcceptText();
            jsPostReqReturn();
        }
    function GetValueLoanRequestS (docNo)
        {
            objdw_head.SetItem(1, "reqreturn_docno", docNo );
            objdw_head.AcceptText();
            openReqReturn();
            
        }
    function MenubarOpen()
        {
            Gcoop.OpenDlg('800', '600', 'w_dlg_sl_loanrequest_return_search.aspx', '');
        }
    function MenubarNew(){
            window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loanreqeust_return.aspx";
        }
    function ItemDataWindowChange(sender, rowNumber, columnName, newValue)
    {
           if( columnName == "member_no")
           {
                Gcoop.GetEl("HdMemberNo").value = newValue;
                jsPostReqReturn();
           }else if((columnName = "loantype_code_1")||(columnName = "loantype_code")){
//           alert("1");
                objdw_head.SetItem( rowNumber , "loantype_code" , newValue );
//                 alert("2");
                objdw_head.AcceptText();
//                 alert("3");
                jsPostReqReturn();
           }
    }
    function OnDwHeadButtonClicked(sender, rowNumber, buttonName)
    {
        if(buttonName == "b_search")
        {
            Gcoop.OpenDlg('600', '600', 'w_dlg_sl_member_search.aspx', '');
        }else if (buttonName == "b_refno")
        {
            Gcoop.OpenDlg('800', '600', 'w_dlg_sl_loanrequest_search.aspx', '');
        }else if (buttonName == "b_cancel")
        {
            cancelReturn();
        }
    }
    function OnDwDetailClick(sender, rowNumber, objectName)
    {
        if((objectName != "datawindow")&&(objectName != "topic")&&(objectName != "content01")&&(objectName != "content02")&&(objectName != "content03"))
        {
            Gcoop.CheckDw(sender, rowNumber, objectName, "choose_flag", 1, 0 );
            refresh();   
        }
       
    }
    function SheetLoadComplete(){
        var returnVal = Gcoop.GetEl("HdReturn").value;
        var msgVal = Gcoop.GetEl("HdMsg").value;
        if( msgVal != ""){
                var rowCount = objdw_message.RowCount()+1;
                for(var i = 1; i <  rowCount ; i++)
                {
                   msgVal = objdw_message.GetItem(i, "msgtext")+"";
                    var splitmsg = msgVal.split('|') ;
                    var msg = "";
                    for(var j=0 ; j<splitmsg.length ;j++){
                    msg +=splitmsg[j]+"\n";
                    }
                    alert (msg );
                }
                Gcoop.GetEl("HdMsg").value = "";
       }
       if(returnVal == "11"){
            Gcoop.GetEl("HdReturn").value = "";
            alert ("บันทึกรายการเรียบร้อยแล้ว");
            window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_return.aspx";
       }
    }
    //ฟังก์ชั่นรายงาน**********************************************************************
    function OnClickLinkNext() {

        objdw_head.AcceptText();
        popupReport();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
 <span style="cursor: pointer" onclick="OnClickLinkNext();">-พิมพ์ใบส่งคืนคำขอกู้</span> 
<table >
    <tr>
        <td>
         <dw:WebDataWindowControl ID="dw_head" runat="server" DataWindowObject="d_sl_loanrequest_return"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventItemChanged="ItemDataWindowChange" ClientEventButtonClicked="OnDwHeadButtonClicked"
                    ClientFormatting="True">
          </dw:WebDataWindowControl>
        </td>
    </tr>
    <tr>
        <td>
         <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_sl_loanrequest_returndet"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventClicked="OnDwDetailClick"
                    ClientFormatting="True">
          </dw:WebDataWindowControl>
        </td>
    </tr>
    <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="dw_message" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    DataWindowObject="d_ln_message" LibraryList="~/DataWindow/shrlon/sl_loan_requestment.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
</table>
 <asp:HiddenField ID="HdReturn" runat="server" />
 <asp:HiddenField ID="HdMsg" runat="server" />
 <asp:HiddenField ID="HdMemberNo" runat="server" />
 <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
    <asp:HiddenField ID="HdcheckPdf" runat="server" Value="False" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="Hdcommitreport" runat="server" Value="false" />
</asp:Content>
