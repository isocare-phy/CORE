<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_scholarship.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_scholarship" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postGetMemberDetail %>
    <script type="text/javascript">
        function DwMemItemChange(s, r, c, v) {
            if (columnName == "member_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postGetMemberDetail();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <div>
        <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_membmaster_2" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange"
            TabIndex="100">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <div>
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_reqmaster_2" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <div>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_as_reqschool_mu" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventItemChanged="DwSchoolItemChange" ClientEventButtonClicked="DwSchoolButtonClick">
        </dw:WebDataWindowControl>
    </div>
</asp:Content>
