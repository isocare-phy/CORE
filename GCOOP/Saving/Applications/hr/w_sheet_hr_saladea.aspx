<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_saladea.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_saladea" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=getNameId%>
    <style type="text/css">
        .style3
        {
            width: 198px;
        }
        .style4
        {
            width: 47px;
        }
    </style>

    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function ItemChangedMain(s, r, c, v) {
            if (c == "empid") {
                objDwMain.SetItem(1, c, v);
                objDwMain.AcceptText();
                getNameId();
            }
        }
        
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" Text="เลือกพนักงาน" Font-Bold="True" Font-Names="tahoma"
                    Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true" DataWindowObject="dw_hr_main"
                    HorizontalScrollBar="Auto" LibraryList="~/DataWindow/hr/hr_payroll.pbl" Width="750px"
                    Height="80px" ClientEventItemChanged="ItemChangedMain">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="รายละเอียดรายรับ" Font-Bold="True" Font-Names="tahoma"
                    Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true"
                    DataWindowObject="dw_hr_payroll_set" LibraryList="~/DataWindow/hr/hr_payroll.pbl"
                    Width="780px">
                </dw:WebDataWindowControl>
                
            </td>
        </tr>
        <tr>
            <td>
                
                <dw:WebDataWindowControl ID="DwTranacc" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true"
                    DataWindowObject="dw_hr_payroll_set_tran_acc" LibraryList="~/DataWindow/hr/hr_payroll.pbl"
                    Width="780px">
                </dw:WebDataWindowControl>
                
            </td>
        </tr>
    </table>
</asp:Content>
