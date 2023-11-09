<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_salamas.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_salamas" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=getNameId%>
    <%=getProcess%>
    <%=getTranfer%>
    <%=getCheckprocess%>

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
        function ChangedMain(s, r, c, v) {
            if (c == "empid") {
                objDwMain.SetItem(1, c, v);
                objDwMain.AcceptText();
                getNameId();
                
            }
        }
        function Click_Process(s, r, c) {
            if (c == "b_process") {
                var year = objdw_process.GetItem(1, "year_pay");
                var month = objdw_process.GetItem(1, "month_pay");
                alert("ประมวลผลเงินเดือนประจำเดือน    " + month + "   ปี    " + year);
                getCheckprocess();
            }
            else if (c == "b_old") 
            {
                getTranfer();
            }
        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
     <tr>
            <td>
               
                <dw:WebDataWindowControl ID="dw_process" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true" DataWindowObject="dw_hr_salary_main"
                   LibraryList="~/DataWindow/hr/hr_payroll.pbl" Width="750px"
                    Height="110px" ClientEventButtonClicked="Click_Process">
                </dw:WebDataWindowControl>
        
                <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true" DataWindowObject="dw_hr_main"
                   LibraryList="~/DataWindow/hr/hr_payroll.pbl" Width="750px"
                    Height="40px" ClientEventItemChanged="ChangedMain">
                </dw:WebDataWindowControl>
                
                <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="true"
                    DataWindowObject="dw_hr_salary_tax" LibraryList="~/DataWindow/hr/hr_payroll.pbl"
                    Width="780px">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    
</asp:Content>
