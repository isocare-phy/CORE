<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_installloan.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_installloan" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsRetrieval%>
    <%=jsFind%>
    <%=jsCancel%>
    <%=jsInstall%>
    <%=ClearProcess%>
    <%=jsReadyKeep %>

    <script type="text/javascript">

        function OnDwMainButtonClicked(s, r, c) 
        {
            if (c == "b_search") {
                jsRetrieval();
            }
            else if (c == "b_install") {
                jsInstall();
            }
            else if (c == "b_find") {
                jsFind();
            }
            else if (c == "b_cancel") {
                jsCancel();
            }
            else if (c == "b_readykeep") {
                jsReadyKeep();
            }
        }

        function SheetLoadComplete() 
        {
            if (Gcoop.GetEl("HdRunProcess").value == "true") 
            {
                Gcoop.OpenProgressBar("ประมวลผล...", true, true, jsClear);
            }
        }

        function jsClear() {
            ClearProcess();
        }

        function OnDwmainItemChanged(s, r, c, v) 
        {
            if (c == "start_tdate") 
            {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(1, "start_date", Gcoop.ToEngDate(v));
                s.AcceptText();

            }
            else if (c == "end_tdate") 
            {
                s.SetItem(r, c, v);
                s.AcceptText();
                s.SetItem(1, "end_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            else if (c == "loantype_code") 
            {
                s.SetItem(r, c, v);
                s.AcceptText();
            }
            else if (c == "member_no") 
            {

                s.SetItem(r, c, v);
                s.AcceptText();
            }
        }

        function Validate() 
        {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"  LibraryList="~/DataWindow/app_assist/as_asn_installloan.pbl" 
        DataWindowObject="d_assist_sk_process_loan_cri"
       ClientFormatting="True"
        ClientEventButtonClicked="OnDwMainButtonClicked" Style="top: 0px; left: 0px">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="dw_data" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"  LibraryList="~/DataWindow/app_assist/as_asn_installloan.pbl" 
         DataWindowObject="d_assist_sk_install_loanlist_process"
       ClientFormatting="True" RowsPerPage="10">
         <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdRunProcess" runat="server" />
</asp:Content>
