<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_add65year.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_add65year" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<%--<%@ Register TagPrefix="Controls" Namespace="Tittle" Assembly="Saving" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsRefresh%>
    <%=jsCalculate %>
    <script type="text/javascript">
        function PostRefresh() {
            jsRefresh();
        }
        function OnCriClick(s, r, c) {
            if (c == "b_calculate") {
                jsCalculate();
            }
        }
        function OnCriItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "calculate_tdate") {
//                //s.SetItem(r, "calculate_date", Gcoop.ToEngDate(v));
//                s.AcceptText();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <%--<asp:Panel ID="Panel1" runat="server" BackColor="#CCCCFF" Height="221px" Width="543px">--%>
        
        <div align="center">
            <dw:WebDataWindowControl ID="DwCri" runat="server" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
                DataWindowObject="d_cri_addmember65" ClientScriptable="True" AutoRestoreContext="false"
                AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                ClientEventButtonClicked="OnCriClick" ClientEventItemChanged="OnCriItemChange">
            </dw:WebDataWindowControl>

            <asp:Button ID="Add_65" runat="server" Text="ดึงข้อมูล" OnClick="Add_65_Click" OnClientClick="PostRefresh()"
                Height="53px" Width="115px" Enabled="False" Visible="False" />

            
        </div>
    <%--</asp:Panel>--%>
            <asp:HiddenField ID="HdMemberNo" runat="server" />
            <asp:HiddenField ID="HdCalculateDate" runat="server" />
            <asp:HiddenField ID="HdCheckOldMem" runat="server" />
            <asp:HiddenField ID="HdMaxYear" runat="server" />
</asp:Content>
