<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_add_intreceivedate.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_add_intreceivedate" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=postAccountNO%>

<%=RunProcess%>
    <script type="text/javascript">
        function MenubarNew() {
            if (confirm("ยืนยันการล้างหน้าจอ")) {
                window.location = "";
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function DwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "account_no") {
                postAccountNO();
            }
        }

        function Button1_onclick() {
            RunProcess();
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_account_list.aspx","");
        }
        function ChooseAcc(account_no) {
            objDwMain.SetItem(1, "account_no", account_no);
            objDwMain.AcceptText();
            postAccountNO();
        }
        function OnDwDetailChange(s,r,c,v) {
            s.SetItem(r, c, v);
            s.AcceptText();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_due_date_condition2" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" TabIndex="1"
        ClientEventItemChanged="DwMainItemChange" >
    </dw:WebDataWindowControl>
    <br />
    <input id="Button1" type="button" value="จัดทำข้อมูล" onclick="return Button1_onclick()" />
    <br />
    <%--<asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal" Height="400" Width="700">--%>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/pm/pm_investment.pbl"
        DataWindowObject="d_account_due_date3" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" TabIndex="80"
        ClientEventItemChanged="OnDwDetailChange">
    </dw:WebDataWindowControl>
 <%--   </asp:Panel>--%>
</asp:Content>
