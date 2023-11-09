<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_reqinsapnew.aspx.cs"
    Inherits="Saving.Applications.app_assist.w_sheet_as_reqinsapnew" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript"></script>
    <%=initJavaScript%>
    <%=postMemberNo%>
    <script type="text/javascript">
        function GetValueFromDlg(memberno) {
            //number objdwcontrol.SetItem ( number row, string column, variant value )
            objDwMain.SetItem(1, "member_no", memberno);
            objDwMain.AcceptText();

            postMemberNo();

        }

        function MenubarOpen() {
            Gcoop.OpenDlg('580', '590', 'w_dlg_sl_member_search.aspx', '');
        }

        function OnDwMainClicked(s, r, c) {
            if (c == "b_1") {
                Gcoop.OpenDlg('580', '590', 'w_dlg_sl_member_search.aspx', '');
            }
            return 0;
        }

        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "member_no") {

                s.SetItem(r, c, v);
                s.AcceptText();
                postMemberNo();
            }
            else if (c == "insmemb_type") {
                s.SetItem(r, c, v);
                s.AcceptText();
                if (v != 1) {
                    s.SetItem(r, "person_card", "");
                    s.SetItem(r, "birth_tdate", "");
                     s.SetItem(r, "loan_amt", "");
                     s.SetItem(r, "share_amt", "");
                    
                     s.AcceptText();
                }
             }

            return 0;
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกสมัครใหม่");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_insureqnew"
        LibraryList="~/DataWindow/app_assist/as_reqinsapnew.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True" ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="OnDwMainClicked">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwLoan" runat="server" DataWindowObject="d_as_loandetail"
        LibraryList="~/DataWindow/app_assist/as_reqinsapnew.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientScriptable="True">
    </dw:WebDataWindowControl>
</asp:Content>
