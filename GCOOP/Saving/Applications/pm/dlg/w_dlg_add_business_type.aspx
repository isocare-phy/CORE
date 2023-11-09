<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_add_business_type.aspx.cs" Inherits="Saving.Applications.pm.dlg.w_dlg_add_business_type" %>


<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%=jsBtDel%>
<%=JsSaveBusiness%>
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
//        function OnDwMainClick(s, r, c, v) {
//            Gcoop.GetEl("HdRow").value = r;
//        }
        function Save() {
            JsSaveBusiness();
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
            objdw_main.InsertRow(0);
        }

        function DialogLoadComplete() {
            if (Gcoop.GetEl("HdResult").value == "True") {
//                parent.ReTurnOfInvSource(Gcoop.GetEl("HdValue").value)
                parent.RemoveIFrame();
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hforg_memno" runat="server" />
    &nbsp;&nbsp;เพิ่มประเภทธุรกิจ
    <br/>
    <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_industry"
        LibraryList="~/DataWindow/pm/pm_investment.pbl" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventButtonClicked="OnButtonClick" ClientEventItemChanged="DwMemItemChange" 
        ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
<span class="linkSpan" onclick="OnInsert()" style="font-size: small;  color:Green;
    float:Left">เพิ่มแถว</span> <span style="font-size: small; color: #808080;">(หมายเหตุ
        - หลังจาก เพิ่มแถว/ลบแถว  แล้วกดปุ่ม save อีกครั้ง )</span>  
        <input type="button" value="ตกลง" onclick="Save();" />
        <input type="button" value="ปิดหน้าจอ" onclick="parent.RemoveIFrame();" />
        <asp:HiddenField ID="HdResult" runat="server" Value="" />
        <asp:HiddenField ID="HdRow" runat="server" Value="" />
        <asp:HiddenField ID="HdValue" runat="server" Value="" />
        <asp:HiddenField ID="HdMainRowDel" runat="server" Value="" />
    </div>
    </form>
</body>
</html>
