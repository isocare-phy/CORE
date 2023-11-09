<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_weight_analyze.aspx.cs"
    Inherits="Saving.Applications.mis.w_sheet_weight_analyze" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table width="100%">
        <tr>
            <td colspan="2" valign="top" height="317px" style="border-color: #000000; border-style: double;">
                <asp:Button ID="Button1" runat="server" Text="ต้นทุนจ่าย" OnClick="Button1_Click" />
                <asp:Button ID="Button2" runat="server" Text="ต้นทุนรับ" OnClick="Button2_Click" />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="True" Font-Size="Medium"
                    ForeColor="Red"></asp:Label>
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <asp:Panel ID="Panel1" runat="server" Height="291px" ScrollBars="Horizontal" Width="730px">
                            <dw:WebDataWindowControl ID="dw_cr" runat="server" DataWindowObject="d_mis_cost_analyze_detail"
                                LibraryList="~/DataWindow/mis/weight_analyze.pbl">
                            </dw:WebDataWindowControl>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <asp:Panel ID="Panel2" runat="server" Height="291px" ScrollBars="Horizontal" Width="730px">
                            <dw:WebDataWindowControl ID="dw_dr" runat="server" DataWindowObject="d_mis_cost_analyze_detail"
                                LibraryList="~/DataWindow/mis/weight_analyze.pbl">
                            </dw:WebDataWindowControl>
                        </asp:Panel>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
        <tr>
            <td style="border-style: double; border-color: #000000">
                <dw:WebDataWindowControl ID="dw_pastyear" runat="server" Height="100%" DataWindowObject="d_mis_cost_analyze"
                    LibraryList="~/DataWindow/mis/weight_analyze.pbl" Width="100%">
                </dw:WebDataWindowControl>
            </td>
            <td style="border-style: double; border-color: #000000">
                &nbsp;
                <dw:WebDataWindowControl ID="dw_weight" runat="server" Height="100%" DataWindowObject="d_mis_cost_analyze_weight"
                    LibraryList="~/DataWindow/mis/weight_analyze.pbl" Width="100%">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
