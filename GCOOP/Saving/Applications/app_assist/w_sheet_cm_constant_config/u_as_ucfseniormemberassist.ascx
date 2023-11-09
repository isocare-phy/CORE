<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="u_as_ucfseniormemberassist.ascx.cs" 
Inherits="Saving.Applications.app_assist.u_as_ucfseniormemberassist" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<script type="text/javascript">
    function OnButtonClick(sender, row, name) {
        if (name == "b_delete") {
            var detail = objdw_main.GetItem(row, "envdesc");

            if (confirm("คุณต้องการลบรายการ " + detail + " ใช่หรือไม่?")) {
                objdw_main.DeleteRow(row);
            }
        }
        return 0;
    }

    function MenubarSave() {
        if (confirm("ยืนยันการบันทึกข้อมูลทั้งหมด?")) {

            var a = objdw_main.GetItem(1, "coop_id");

            for (var j = 0; j <= objdw_main.RowCount(); j++) 
            {

                objdw_main.SetItem(j + 1, "coop_id", a);

            }
            objdw_main.AcceptText();
            objdw_main.Update();
        }
    }

    function OnInsert() {
        objdw_main.InsertRow(objdw_main.RowCount() + 1);
    }

</script>
<br />
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
<dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_as_ucf_senior_member_assist"
    LibraryList="~/DataWindow/app_assist/as_asnucf.pbl" ClientScriptable="True" OnBeginUpdate="dw_main_BeginUpdate"
    OnEndUpdate="dw_main_EndUpdate" BorderStyle="None" ClientEventButtonClicked="OnButtonClick">
</dw:WebDataWindowControl>

<%--<span class="linkSpan" onclick="OnInsert()">เพิ่มแถว</span>--%>