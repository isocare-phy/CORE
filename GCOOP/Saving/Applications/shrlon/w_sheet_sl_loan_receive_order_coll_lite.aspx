<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_loan_receive_order_coll_lite.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_receive_order_coll_lite" %>

<%@ Register Src="w_sheet_sl_loan_receive_order_coll_ctrl/DsList.ascx" TagName="DsList"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <uc1:DsList ID="dsList" runat="server" />
</asp:Content>
