<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_payout.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_payout_benefit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table style="width: 100%;">
        <tr>
            <td valign=top>
                <asp:Panel ID="Panel3" runat="server">
                     
                </asp:Panel>
            </td>
            <td  valign=top>
                <asp:Label ID="Label1" runat="server" Text="ระบุการจ่ายรายบุคคล :"></asp:Label>
                <input id="Button1" align="bottom" type="button" value="จ่ายเงิน ..." /></td>
        </tr> 
        <tr>
            <td valign=top  >
                 <asp:Panel ID="Panel1" runat="server">
                    <dw:WebDataWindowControl ID="DwMain" runat="server" 
                        DataWindowObject="d_as_payassist_list" 
                        LibraryList="~/DataWindow/app_assist/as_payout.pbl">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td>
                <asp:Panel ID="Panel4" runat="server">
                      <dw:WebDataWindowControl ID="Dw_Detail" runat="server" 
                        DataWindowObject="d_as_payassist_detail" 
                        LibraryList="~/DataWindow/app_assist/as_payout.pbl">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr> 
    </table>
</asp:Content>
