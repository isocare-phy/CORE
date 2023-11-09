<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_slipdea.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_slipdea" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <dw:WebDataWindowControl ID="DwMain" runat="server" 
        DataWindowObject="dw_hr_slipdea" LibraryList="~/DataWindow/hr/hr_payroll.pbl">
    </dw:WebDataWindowControl>
</asp:Content>
