<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_request_scholarship.aspx.cs" Inherits="Saving.Applications.Assis.w_sheet_as_reqscholarship" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/JavaScript">
        function MenubarOpen() {
            opendlg('570', '590', 'w_dlg_sl_member_search.aspx', '')
        }
        function GetValueFromDlg(strvalue) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            window.location = str_arr[0] + "?strvalue=" + strvalue;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table style="width: 100%;">
        <tr align =left >
            <td>
                <dw:WebDataWindowControl ID="DwMain" runat="server" 
                    DataWindowObject="d_as_information_member" 
                    LibraryList="~/DataWindow/app_assist/as_information.pbl">
                </dw:WebDataWindowControl>
           </td>
        </tr>
        <tr align =left >
            <td>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" 
                    DataWindowObject="d_as_reqscholarship" 
                    LibraryList="~/DataWindow/app_assist/as_scholarship.pbl">
                </dw:WebDataWindowControl> 
            </td>
        </tr>
    </table>
</asp:Content>
