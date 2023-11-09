<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_add_investment_group.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_add_investment_group" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
     <%=jsBtDel %>
    <script type="text/JavaScript">
        function Validate() {
            objdw_main.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
//        function OnButtonClick(sender, row, name) {
//            if (name == "b_delete") {
//                var detail = "รหัสประเถท:" + objdw_main.GetItem(row, "group_code");
//                if (confirm("คุณต้องการลบ " + detail + " ใช่หรือไม่?")) {
//                    objdw_main.DeleteRow(row);
//                }
//            }
//            return 0;
//        }
        function OnButtonClick(sender, rowNumber, objectName) {
            if (objectName == "b_delete") {
                if (confirm("แน่ใจว่าต้องการลบแถว")) {
                    Gcoop.GetEl("HdMainRowDel").value = rowNumber;
                    jsBtDel();
                }
            }
        }
        function OnInsert() {
            objdw_main.InsertRow(objdw_main.RowCount() + 1);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hforg_memno" runat="server" />
    &nbsp;&nbsp; เพิ่มประเภทหน่วยงาน
     <br/>
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_investgroup"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
 <span class="linkSpan" onclick="OnInsert()" style="font-size: small;  color:Green;
    float:left">เพิ่มแถว</span> <span style="font-size: small; color: #808080;">(หมายเหตุ
        - หลังจาก เพิ่มแถว/ลบแถว  แล้วกดปุ่ม save อีกครั้ง )</span>  
        <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />

</asp:Content>
