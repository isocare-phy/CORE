<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_appinsresign.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_appinsresign" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=MemberNo%>
    <%=GetStatus%>
    <%=newClear%>
    <script type="text/javascript">
        function Clickb_search(sender, row, column) {
            if (column == "member_no") {
//                alert(objdw_search.GetItem(row, "member_no"));
                Gcoop.GetEl("HMember_no").value = objdw_search.GetItem(row, "member_no");
                MemberNo();
            }

        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function statuschange(s, r, c, v) {
            if (c == "reqresign_status") {

                objdw_search.SetItem(r, c, v);
                objdw_search.AcceptText();
                Gcoop.GetEl("Hstatus").value = v;
                Gcoop.GetEl("hrow").value = r + "";
                // alert(v);
                GetStatus();
            }
        }
        function MenubarNew() {
           
                newClear();            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HMember_no" runat="server" />
    <asp:HiddenField ID="Hstatus" runat="server" />
    <asp:HiddenField ID="hrow" runat="server" />
    <asp:HiddenField ID="Hlist" runat="server" />
    <asp:HiddenField ID="hidden_search" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
        รายการ
        <dw:WebDataWindowControl ID="dw_search" runat="server" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            DataWindowObject="d_ins_approve_listnew" LibraryList="~/DataWindow/app_assist/as_appinsresign.pbl"
            ClientEventClicked="Clickb_search" ClientEventItemChanged="statuschange">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        รายละเอียด
        <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_ins_approve_detail"
            LibraryList="~/DataWindow/app_assist/as_appinsresign.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventClicked="selectRow">
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
