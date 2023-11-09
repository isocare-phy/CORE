<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="u_cm_hr_constant_step.ascx.cs" Inherits="Saving.Applications.hr.u_cm_hr_constant_step" %>
<%@ Register assembly="WebDataWindow" namespace="Sybase.DataWindow.Web" tagprefix="dw" %>
<style type="text/css">
    .style2
    {
        font-size: small;
        font-weight: bold;
    }
</style>
<% Page_LoadComplete(); %>

<script type="text/javascript">
   
    function OnButtonClick(sender, row, name) {
        if (name == "b_del") {
            var level = objDwMain.GetItem(row, "level");
            var step = objDwMain.GetItem(row, "step");

            //detail += " : " + objDwMain.GetItem(row, "level");

            if (confirm("คุณต้องการลบรายการระดับ: " + level + " ขั้น: " + step + " ใช่หรือไม่ ?")) {
                objDwMain.DeleteRow(row);
            }
        }
        return 0;
    }

  
    function OnUpdate() {
        if (confirm("ยืนยันการบันทึกข้อมูลทั้งหมด?")) {
            objDwMain.AcceptText();
            objDwMain.Update();
            
        }
    }

    function OnInsert() {
        objDwMain.InsertRow(objDwMain.RowCount() + 1);
    }
</script>
<div style="height: 18px; vertical-align: top">
   
        <span class="style2"
        onclick="OnUpdate()">บันทึกข้อมูล</span>
</div>
<dw:WebDataWindowControl ID="DwMain" runat="server" BorderStyle="Solid" BorderWidth="0px"
    ClientScriptable="True" DataWindowObject="dw_hr_step" LibraryList="~/DataWindow/hr/hr_constant.pbl"
    Width="560px" ClientEventButtonClicked="OnButtonClick" Height="350px">
</dw:WebDataWindowControl>
<span class="style2"
    onclick="OnInsert()">เพิ่มแถว</span> 