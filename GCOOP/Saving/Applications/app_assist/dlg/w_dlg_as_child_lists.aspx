<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_as_child_lists.aspx.cs"
    Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_child_lists" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=postInsertRow %>
    <%=postDeleteRow %>
    <%=postSaveList %>
    <%=postFilterScholarship %>
    <title></title>
    <script type="text/javascript">

        function DwMainClick(sender, rowNumber, objectName) {
            Gcoop.CheckDw(sender, rowNumber, objectName, "select_flag", 1, 0);
            objDwMain.AcceptText();
        }

        function DwMainbuttonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_save") {
                //objDwMain.Update();
                postSaveList();
            }
            else if (buttonName == "b_select") {
                var child_prename = objDwMain.GetItem(rowNumber, "child_praname");
                var child_name = objDwMain.GetItem(rowNumber, "child_name");
                var child_surname = objDwMain.GetItem(rowNumber, "child_surname");
                var child_sex = objDwMain.GetItem(rowNumber, "child_sex");
                var level_school = objDwMain.GetItem(rowNumber, "level_school");
                var scholarship_level = objDwMain.GetItem(rowNumber, "scholarship_level");
                window.opener.GetValueFromDlgChildList(child_prename, child_name, child_surname, child_sex, level_school, scholarship_level);
                window.close();
            }
        }

        function DwMainItemChang(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            if (columnName == "child_praname") {
                var childprename_code = objDwMain.GetItem(rowNumber, "child_praname");
                if (childprename_code == "37" || childprename_code == "01") {
                    objDwMain.SetItem(rowNumber, "child_sex", "M");
                }
                else {
                    objDwMain.SetItem(rowNumber, "child_sex", "F");
                }
            }
            else if (columnName == "level_school") {
                objDwMain.AcceptText();
                postFilterScholarship();
            }
        }

        function OnClickInsertRow() {
            postInsertRow();
        }

        function OnClickDeleteRow() {
            if (objDwMain.RowCount() > 0) {
                var currentRow = Gcoop.GetEl("HdMainRow").value;
                var confirmText = "ยืนยันการลบแถวที่ " + currentRow;
                if (confirm(confirmText)) {
                    postDeleteRow();
                }
            } else {
                alert("ยังไม่มีการเพิ่มแถวสำหรับรายการ");
            }
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
        <span onclick="OnClickInsertRow()" style="cursor: pointer;">
            <asp:Label ID="LbInsert2" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                Font-Size="14px" Font-Underline="True" ForeColor="#006600" /></span> &nbsp;&nbsp;
        <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
            <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_child_lists"
            LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventClicked="DwMainClick" ClientEventButtonClicked="DwMainbuttonClick"
            ClientEventItemChanged="DwMainItemChang">
        </dw:WebDataWindowControl>
    </div>
    <asp:HiddenField ID="HdListRow" runat="server" />
    <asp:HiddenField ID="HdMainRow" runat="server" />
    </form>
</body>
</html>
