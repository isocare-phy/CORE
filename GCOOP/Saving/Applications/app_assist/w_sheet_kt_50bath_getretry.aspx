<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_50bath_getretry.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_50bath_getretry" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postGetRetry%>
    <%=postRefresh %>
    
    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenDlg(610, 550, "w_dlg_dp_account_search.aspx", "");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "select_option") {
                postRefresh();
            }
        }

        function MenubarNew() {
            //  window.location = Gcoop.GetUrl() + "Applications/ap_deposit/w_sheet_dp_deptedit.aspx";
        }

        function NewAccountNo(member_no) {
            objDwMain.SetItem(1, "member_no", Gcoop.Trim(member_no));
            objDwMain.AcceptText();
            postAccountNo();
        }

        function OnDwMainItemClicked(s, r, c) {
            switch (c) {
                case "b_get":
                    if (confirm("ต้องการทำรายการใช่หรือไม่ ?"))
                        postGetRetry();
                    break;
               
            }
        }

        function Validate() {
            //return confirm("ต้องเปลี่ยนแปลงสถานะการรอบัญชีใช่หรือไม่");
            alert("ใช้ปุ่มเปลี่ยนสถามนะคนเกษียณ");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server" Text=""></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_getretry"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientFormatting="True" ClientEventButtonClicked="OnDwMainItemClicked">
    </dw:WebDataWindowControl>
</asp:Content>
