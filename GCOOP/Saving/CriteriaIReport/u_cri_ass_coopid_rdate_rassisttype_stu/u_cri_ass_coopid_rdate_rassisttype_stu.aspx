﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Report.Master" AutoEventWireup="true"
    CodeBehind="u_cri_ass_coopid_rdate_rassisttype_stu.aspx.cs" Inherits="Saving.CriteriaIReport.u_cri_ass_coopid_rdate_rassisttype_stu.u_cri_ass_coopid_rdate_rassisttype_stu" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <center>
        <asp:Label ID="ReportName" runat="server" Text="ชื่อรายงาน" Enabled="False" EnableTheming="False"
            Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Size="Large"
            Font-Underline="False"></asp:Label></center>
    <uc1:DsMain ID="dsMain" runat="server" />
</asp:Content>
