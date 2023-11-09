<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_camels_new.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_camels_new" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 706px;
        }
        .style2
        {
            height: 313px;
        }
        .style3
        {
            height: 123px;
        }
        .style4
        {
            height: 313px;
            width: 44%;
        }
        .style5
        {
            width: 44%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table class="style9">
        <tr>
            <td width="80%">
                <dw:WebDataWindowControl ID="dw_cri" runat="server" DataWindowObject="d_mis_cri_monthyear"
                    LibraryList="~/DataWindow/mis/criteria.pbl">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <asp:Button ID="B_retrieve" runat="server" Text="แสดงข้อมูล" />
            </td>
        </tr>
    </table>
    </br>
    <table class="style1">
        <tr>
            <td width="50%" style="vertical-align: top">
                <dw:WebDataWindowControl ID="WebDataWindowControl1" runat="server" DataWindowObject="d_mis_camels1"
                    LibraryList="~/DataWindow/mis/camels.pbl">
                </dw:WebDataWindowControl>
            </td>
            <td class="style2" style="vertical-align: top">
                <dw:WebDataWindowControl ID="WebDataWindowControl2" runat="server" DataWindowObject="d_mis_camels2"
                    LibraryList="~/DataWindow/mis/camels.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style3" colspan="2" style="vertical-align: top">
                <dw:WebDataWindowControl ID="WebDataWindowControl3" runat="server" DataWindowObject="d_mis_camels3"
                    LibraryList="~/DataWindow/mis/camels.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style5">
                </br>
                <dw:WebDataWindowControl ID="WebDataWindowControl4" runat="server" DataWindowObject="d_mis_camels4"
                    LibraryList="~/DataWindow/mis/camels.pbl">
                </dw:WebDataWindowControl>
            </td>
            <td style="vertical-align: top">
                </br>
                <dw:WebDataWindowControl ID="WebDataWindowControl5" runat="server" DataWindowObject="d_mis_camels5"
                    LibraryList="~/DataWindow/mis/camels.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
