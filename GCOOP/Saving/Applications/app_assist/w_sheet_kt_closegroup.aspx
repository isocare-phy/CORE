<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_closegroup.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_closegroup" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jscloseGroup%>
    <%=newClear%>
    <script type="text/javascript">
        function closeGroup() {
            jscloseGroup();
            ReDlgDeptWith("", "/", "CCA");
        }

        function ReDlgDeptWith(objDlg, itemType, recpPayType) {
//            objdtMain.SetItem(1, "deptwith_flag", itemType);
//            objdtMain.AcceptText();
//            objdtMain.SetItem(1, "recppaytype_code", recpPayType);
//            objdtMain.AcceptText();
//            if (recpPayType == "DTO" || recpPayType == "WTO") {
//                Gcoop.SetLastFocus("deptslip_date_tdate_0");
//            } else if (recpPayType == "DCP") {
//                Gcoop.SetLastFocus("cheque_no_0");
//            } else {
//                Gcoop.SetLastFocus("deptslip_amt_0");
//            }
            postDeptWith();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server" Text=""></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_closegroup"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True">
    </dw:WebDataWindowControl>
    <div id="BtProcess" width="100%" align="center">
    <br />
        <input type="button" id="BottonProcess" onclick="closeGroup();" value="ประมวลผล" style="width:100px; height:50px;" />
    </div>
    
</asp:Content>
