<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_loan_requestment_lite.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_requestment_lite" %>

<%@ Register Src="w_sheet_sl_loan_requestment_lite_ctrl/DsMain.ascx" TagName="DsMain"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function OnDsMainItemChanged(s, r, c, v) {
        if (c == "member_no") {
            PostMemberNo();
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsMain ID="dsMain" runat="server" />
</asp:Content>
