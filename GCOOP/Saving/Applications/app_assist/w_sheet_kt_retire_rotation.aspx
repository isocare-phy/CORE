<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kt_retire_rotation.aspx.cs" 
Inherits="Saving.Applications.app_assist.w_sheet_kt_retire_rotation" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jspostProcess %>
    <%=jspostRetrieve %>
    <%=ClearProcess %>
    <%=jspostaccid_flag %>
    <%=postmoneytype_code %>
    <script type="text/javascript">
        function OnCwCriButtonClick(s, r, c) {
            switch (c) {
                case "b_retrieve":
                    jspostRetrieve();
                    break;
                case "b_proc":
                    jspostProcess();
                    break;
            }
        }

        function OnCwCriButtonChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "accid_flag":
                    jspostaccid_flag();
                    break;
                case "moneytype_code":
                    postmoneytype_code();
                    break;
            }
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdRunProcess").value == "true") {
                Gcoop.OpenProgressBar("ประมวลผล...", true, true, jsClear);
            }
        }

        function jsClear() {
            ClearProcess();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server" Text=""></asp:Literal>

    <dw:WebDataWindowControl ID="DwCri" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kp_asn_cls_option"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" 
        ClientFormatting="True" ClientEventButtonClicked="OnCwCriButtonClick" ClientEventItemChanged="OnCwCriButtonChanged">
    </dw:WebDataWindowControl>

    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kp_asn_cls_info"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>

    <asp:HiddenField ID="HdRunProcess" runat="server" />
</asp:Content>
