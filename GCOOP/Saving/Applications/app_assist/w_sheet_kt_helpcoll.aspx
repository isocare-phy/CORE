<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_kt_helpcoll.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_helpcoll" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postInitMemb %>
    <%=postmoneytype_code %>
    <%=postbank_code %>
    <script type="text/javascript">
        function DwMemItemChange(s, r, c, v) {
            switch (c) {
                case "member_no":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    postInitMemb();
                    break;
            }
        }

        function DwMainItemChange(s, r, c, v) {
            switch (c) {
                case "moneytype_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    postmoneytype_code();
                    break;
                case "asnslippayout_bank_code":
                    s.SetItem(r, c, v);
                    s.AcceptText();
                    postbank_code();
                    break;
            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_kt_member_search.aspx", "");

        }

        function MenubarNew() {

            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                window.location = "";
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", memberno);
            postInitMemb();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>

    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/kt_50bath.pbl"
        DataWindowObject="d_kt_helpcoll_member" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" 
        ClientEventItemChanged="DwMemItemChange" >
    </dw:WebDataWindowControl>
   <%-- <br />--%>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/kt_50bath.pbl"
        DataWindowObject="d_kt_helpcoll_master" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange" >
    </dw:WebDataWindowControl>
    <%--<br />--%>    
    <dw:WebDataWindowControl ID="DwColl" runat="server" LibraryList="~/DataWindow/app_assist/kt_50bath.pbl"
        DataWindowObject="d_kt_coll_loan" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>
    <%--<br />--%>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/kt_50bath.pbl"
        DataWindowObject="d_kt_helpcoll_pay" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailItemChange" >
    </dw:WebDataWindowControl>
    

</asp:Content>
