<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_approve.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_approve_scholarship" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<table style="width: 100%;">
    <tr>
        <td>
            <asp:Panel ID="Panel1" runat="server" Width="700" ScrollBars="Auto">
            <dw:WebDataWindowControl ID="DwMain" runat="server" 
                DataWindowObject="d_as_apvassist_list" 
                LibraryList="~/DataWindow/app_assist/as_apv.pbl" Height="500px" Width="700px" >
            </dw:WebDataWindowControl>
            </asp:Panel>
        </td> 
    </tr> 
    <tr> 
        <td>
            <asp:Panel ID="Panel2" runat="server" Width="730px" ScrollBars="Auto">
            <dw:WebDataWindowControl ID="DwDetail" runat="server" 
                DataWindowObject="d_as_apvassist_detail" 
                LibraryList="~/DataWindow/app_assist/as_apv.pbl" Height="170px" Width="730px" >
            </dw:WebDataWindowControl>
            </asp:Panel>
        </td> 
    </tr> 
</table>
</asp:Content>

