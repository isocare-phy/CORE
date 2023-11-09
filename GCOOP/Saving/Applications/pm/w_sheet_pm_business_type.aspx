<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_pm_business_type.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_pm_business_type" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
        <%=jsBtDel %>
    <script type="text/JavaScript">
        function Validate() {
            objdw_main.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
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
    &nbsp;&nbsp;เพิ่มประเภทธุรกิจ
     <br/>
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_industry"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl>
  <span class="linkSpan" onclick="OnInsert()" style="font-size: small;  color:Green;
    float:Left">เพิ่มแถว</span> <span style="font-size: small; color: #808080;">(หมายเหตุ
        - หลังจาก เพิ่มแถว/ลบแถว  แล้วกดปุ่ม save อีกครั้ง )</span>  
        <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />
        <asp:HiddenField ID="HdCode" runat="server" Value="" />
       
</asp:Content>
