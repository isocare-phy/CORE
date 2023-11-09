<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_modify50bath.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_modify50bath" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMembNo%>
    <%=jsButtonPayClick %>
    <%=jsButtonDelClick %>
    <script type="text/javascript">

        function MenubarNew() {
            window.location = Gcoop.GetUrl() + "Applications/app_assist/w_sheet_kt_modify50bath.aspx";
        }

        function OnDwMainItemChanged(sender, rowNumber, columnName, newValue) {
            switch (columnName) {
                case "member_no":
                    sender.SetItem(rowNumber, columnName, newValue);
                    sender.AcceptText();
                    jsPostMembNo();
                    break;
            }
        }

        function DwSlipButtonClick(sender, row, columnName) {
            if (columnName == "b_pay") {
                jsButtonPayClick();
            }
            else if (columnName == "b_arrear") {
                jsButtonDelClick();
            }
        }


        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" Width="680px">

        <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_viewlog_paymain"
            LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventItemChanged="OnDwMainItemChanged"
            ClientFormatting="True">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwSlip" runat="server" DataWindowObject="d_modify_asn_cri"
            LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="DwSlipButtonClick"
            ClientFormatting="True" TabIndex="1">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
</asp:Content>
