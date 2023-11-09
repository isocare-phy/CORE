<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_info.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_info" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
     <table style="width: 100%;">
        <tr>
            <td align="left" valign="top">
                <asp:Panel ID="Panel1" runat="server" Width="730" >
                    <dw:WebDataWindowControl ID="DwMain" runat="server" Height="70px" 
                        Width="730px" DataWindowObject="d_as_information_member" 
                        LibraryList="~/DataWindow/app_assist/as_information.pbl" 
                        BorderStyle="None" BorderWidth="1px" HorizontalScrollBar="None" 
                        VerticalScrollBar="None">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td> 
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" Width="730" ScrollBars="Vertical"  >
                    <dw:WebDataWindowControl ID="DwMain_loan" runat="server" 
                        DataWindowObject = "d_as_information_loan" 
                        LibraryList="~/DataWindow/app_assist/as_information.pbl" Height="150px" 
                        Width="730px" BorderStyle="None" BorderWidth="1px" 
                        HorizontalScrollBar="None" VerticalScrollBar="Fixed" BackColor="White">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td> 
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel3" runat="server" Width="730" ScrollBars="Vertical"  >
                    <dw:WebDataWindowControl ID="DwMain_dept" runat="server" 
                        DataWindowObject = "d_as_information_dept" 
                        LibraryList="~/DataWindow/app_assist/as_information.pbl" Height="150px" 
                        Width="730px" BorderStyle="None" BorderWidth="1px" 
                        HorizontalScrollBar="None" VerticalScrollBar="Fixed" BackColor="White">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td> 
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel4" runat="server" Width="730" ScrollBars="Vertical" >
                    <dw:WebDataWindowControl ID="Dwdetail" runat="server" 
                        DataWindowObject = "d_udw_assist_detail" 
                        LibraryList="~/DataWindow/app_assist/as_information.pbl" Height="150px" 
                        Width="730px" BorderStyle="None" BorderWidth="1px" 
                        HorizontalScrollBar="None" VerticalScrollBar="Fixed" BackColor="White">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td> 
        </tr>
    </table>
</asp:Content>
