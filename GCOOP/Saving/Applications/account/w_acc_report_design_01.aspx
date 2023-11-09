<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
    CodeBehind="w_acc_report_design_01.aspx.cs" Inherits="Saving.Applications.account.w_acc_report_design_01" %>
    
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=changeValue%>
    <%=saveData%>
    <%=openProcess%>
    <%=initJavaScript%>
    
    <script type="text/javascript">
    function OnItemChanged(s,r,c,v){
        if(c == ""){
            objDwMain.SetItem(r, c, v);
            objDwMain.AcceptText();
            changeValue();
            //Sting d = objDwMain.GetItem(r, c);
        }
    }
    function OpenDlg(s,r,c)
    {
        //Gcoop.OpenIFrame(500,450,"w_dlg_acc_report_design_head_response.aspx", "");
        Gcoop.OpenDlg(500,450,"w_dlg_acc_report_design_head_response.aspx", "");
    }
    //get value from dlg
    function GetValueDlg(mtype){
            Gcoop.GetEl("HdSheetTypeCode").value = Gcoop.Trim(mtype);
            changeValue();
        }

//    function GetValueDlg(moneysheet_code)
//    {
//            Gcoop.GetEl("HdSheetTypeCode").value = Gcoop.Trim(moneysheet_code);
//            changeValue();
//        }
//    //gen money sheet
    function OpenProcess(s,r,c){
        var isconfirm = confirm("ยืนยันการจัดทำงบการเงิน ?");
        if (!isconfirm)
        {
        return false;        
        }
        openProcess();
    }
    </script>
    
    <style type="text/css">
        .style2
        {
            float: right;
            cursor: pointer;
            font-weight: bold;
            color: #000099;
            text-decoration: underline;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="right">
                <span id="dlg_link" class="style2" onclick="OpenDlg()">เลือกรายการงบการเงิน</span> 
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="center">
                <dw:WebDataWindowControl ID="dw_main" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientScriptable="True" DataWindowObject="d_acc_report_design_master" 
                    LibraryList="~/DataWindow/account/acc_report_design.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="right">
                <span id="process_link" class="style2" onclick="OpenProcess()">จัดทำงบการเงิน</span> 
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td valign="top" align="right">
                
                <dw:WebDataWindowControl ID="dw_rpt" runat="server" AutoRestoreContext="False" 
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
                    ClientScriptable="True" DataWindowObject="d_acc_pl" 
                    LibraryList="~/DataWindow/account/acc_report_design.pbl" 
                    BorderColor="#99CCFF" BorderStyle="Double">
                </dw:WebDataWindowControl>
                
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdSheetTypeCode" runat="server" />
    <asp:HiddenField ID="HdSheetHeadName" runat="server" />
    <asp:HiddenField ID="HdSheetHeadCol1" runat="server" />
    <asp:HiddenField ID="HdSheetHeadCol2" runat="server" />
</asp:Content>
