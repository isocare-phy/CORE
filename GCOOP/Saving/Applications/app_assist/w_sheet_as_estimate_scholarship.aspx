<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_estimate_scholarship.aspx.cs"
    Inherits="Saving.Applications.app_assist.w_sheet_as_estimate_scholarship" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=getCal%>
    <%=getInsert %>
    <%=postChangeLevel %>
    <script type="text/javascript">
        // function เรียก  dlg  ขึ้นมา ซึ่ง Button  ชื่อ b_search อยู่ใน datawindow

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                newClear();
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function DwCriItemChange(sender, rowNumber, columnName, newValue) {
            objDwCri.SetItem(rowNumber, columnName, newValue);
            objDwCri.AcceptText();
            postChangeLevel();
        }

        function DwCriButtonClick(sender, rowNumber, buttonName) {
            //objDwCri.SetItem(rowNumber, columnName, newValue);
            objDwCri.AcceptText();
            postChangeLevel();
        }

        function Calestimate() {
            objDwMain.AcceptText();
            getCal();
        }
        function Insertdelserise() {
            objDwMain.AcceptText();
            getInsert();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HidProduct_id" runat="server" />
    <asp:HiddenField ID="Hproduct_id" runat="server" />
    <asp:HiddenField ID="Hproduct_name" runat="server" />
    <asp:HiddenField ID="Hparner_id" runat="server" />
    <asp:HiddenField ID="Hcb" runat="server" />
    <asp:HiddenField ID="Hitemtype_id" runat="server" />
    <asp:HiddenField ID="Hcheck" runat="server" />
    <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="dw_cri_level_school"
        LibraryList="~/DataWindow/app_assist/as_estimate_scholarship.pbl" Width="750px"
        ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientEventButtonClicked="DwCriButtonClick">
    </dw:WebDataWindowControl>
    <%--<dw:WebDataWindowControl ID="WebDataWindowControl1" runat="server" DataWindowObject="dw_head_text"
        LibraryList="~/DataWindow/app_assist/as_estimate_scholarship.pbl" Width="750px"
        ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
    </dw:WebDataWindowControl>--%>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="730px" Height="300px">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_as_estimate"
            LibraryList="~/DataWindow/app_assist/as_estimate_scholarship.pbl" Width="750px"
            ClientScriptable="True" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
    <input id="b_cal" style="width: 150px; height: 35px; color: White; font-size: small;
        font-weight: bold; background-color: Highlight;" type="button" onclick="Calestimate()"
        value="ประมาณ" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <input id="b_insert" style="width: 150px; height: 35px; color: White; font-size: small;
        font-weight: bold; background-color: Highlight;" type="button" onclick="Insertdelserise()"
        value="จ่ายจริง" />
</asp:Content>
