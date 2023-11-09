<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_cmd_group_list.aspx.cs"
    Inherits="Saving.Applications.cmd.dlg.w_dlg_cmd_group_list" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%=postSaveData%>
    <%=postSetVar%>
    <title>ข้อมูลกลุ่ม</title>
    <script type="text/javascript">
        function OnButtonCloseClick() {
            window.close();
        }

        function OnButtonSaveClick() {
            var isConfirm = confirm("ยืนยันการบันทึกข้อมูล");
            if (isConfirm) {
                objdw_list.AcceptText();
                postSaveData();
            }
        }

        function OnInsert() {
            objdw_list.InsertRow(0);
            postSetVar();
        }

        function DialogLoadComplete() {
         
        }
  
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div>
        <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="dw_cmd_group_master"
            LibraryList="~/DataWindow/Cmd/cm_constant_config.pbl" RowsPerPage="14" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
        <span onclick="OnInsert()" style="font-size: small; color: #808080; cursor: pointer;">
            เพิ่มแถว</span>
    </div>
    <div align="center">
        <input id="buttonSaveData" style="width: 80px;" type="button" value="บันทึก" onclick="OnButtonSaveClick()" />
        &nbsp;
        <input id="buttonCloseIFrame" type="button" value="ปิดหน้าจอ" onclick="OnButtonCloseClick()"
            style="width: 80px;" />
    </div>
    <asp:HiddenField ID="HdItemtype_id" runat="server" />
    <asp:HiddenField ID="HdDomain_id" runat="server" />
    </form>
</body>
</html>
