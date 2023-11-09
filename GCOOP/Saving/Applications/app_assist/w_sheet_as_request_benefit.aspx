<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_request_benefit.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_request_benefit" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<table style="width: 100%;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwMain" runat="server" 
                    DataWindowObject="d_as_information_member" 
                    LibraryList="~/DataWindow/app_assist/as_information.pbl">
                
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" 
                    DataWindowObject="d_as_reqcouple_death" 
                    LibraryList="~/DataWindow/app_assist/as_benefit.pbl">
                </dw:WebDataWindowControl> 
            </td>
        </tr>
    </table>
</asp:Content>
