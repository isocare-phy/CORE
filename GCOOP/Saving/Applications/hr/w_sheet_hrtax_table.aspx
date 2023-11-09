<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_hrtax_table.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hrtax_table"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=newRecord%>
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
        function Validate(){
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
       
        function OnUpdate() {
            if (confirm("ยืนยันการบันทึกข้อมูลทั้งหมด?")) {
                objDwMain.Update();
            }
        }

        function OnInsert() {
            objDwMain.InsertRow(objDwMain.RowCount() + 1);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div style="height: 18px; vertical-align: top">
    <span class="linkSpan" onclick="OnUpdate()" style="font-size: small; color: Green;
        float: right">บันทึกข้อมูล</span> <span class="linkSpan" onclick="OnInsert()" style="font-size: small;
            color: Red; float: left">เพิ่มแถว</span>
</div>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_taxrate_table"
        LibraryList="~/DataWindow/hr/hr_payroll.pbl" ClientScriptable="True" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnButtonClick"
        Height="250px" Width="800px" >
    </dw:WebDataWindowControl>
   
</asp:Content>
