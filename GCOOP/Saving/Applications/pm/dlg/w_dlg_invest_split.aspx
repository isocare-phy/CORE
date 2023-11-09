<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_invest_split.aspx.cs"
    Inherits="Saving.Applications.pm.dlg.w_dlg_invest_split" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%=jsPostInsertRow%>
    <%=jsPostDeleteRow%>
    <%=jsPostSave%>
    <%=jsPostBlank%>
    <script type="text/javascript">
        function OnOkClick() {
            if (confirm("ยืนยันการบันทึกข้อมูล")) {
                var alert_message = "";
                var regist_no = "";
                var investment_docno = "";
                var unit_amt = "";
                var money_amount = 0;
                var unit_amt = 0;
                var flag = false;
                var row = objDwMain.RowCount();

                for (var i = 1; i <= row; i++) {
                    regist_no = objDwMain.GetItem(i, "regist_no");
                    investment_docno = objDwMain.GetItem(i, "investment_docno");
                    money_amount += objDwMain.GetItem(i, "money_amount") * 1;
                    unit_amt += objDwMain.GetItem(i, "unit_amt");

                    if ((regist_no == "" || regist_no == null || investment_docno == "" || investment_docno == null) && !flag) {
                        alert_message += "กรุณาระบุข้อมูลให้ครบถ้วน\n";
                        flag = true;
                    }
                }

                if (money_amount != objDwHead.GetItem(1,"money_amout")) {
                    alert_message += "กรุณาตรวจสอบยอดเงิน\n";
                }
                if (unit_amt != objDwHead.GetItem(1, "unit_amt")) {
                    alert_message += "กรุณาตรวจสอบจำนวนหน่วยลงทุน\n";
                }

                if (alert_message != "") {
                    alert(alert_message);
                }
                else {
                    jsPostSave();
                }
            }
        }

        function InsertDwMain() {
            jsPostInsertRow();
        }

        function OnDwMainButtonClicked(sender, row, bName) {
            Gcoop.GetEl("Hdrow").value = row + "";
            jsPostDeleteRow();
        }

        function OnCloseDialog() {
            if (confirm("ยืนยันการออกจากหน้าจอ ")) {
                parent.RemoveIFrame();
            }
        }

        function DialogLoadComplete() {
            var fin = Gcoop.GetEl("Hdfin").value;
            if (fin == "true") {
                parent.Refresh();
                parent.RemoveIFrame();
            }
        }

        function OnDwMainItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            if (col == "money_amount" || col == "unit_amt") {
                jsPostBlank();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width: 100%;">
            <tr>
                <td valign="top">
                    <dw:WebDataWindowControl ID="DwHead" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientFormatting="True"
                        ClientValidation="False" DataWindowObject="d_invest_split_head" LibraryList="~/DataWindow/pm/pm_investment.pbl">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
                        ClientFormatting="True" DataWindowObject="d_invest_split" LibraryList="~/DataWindow/pm/pm_investment.pbl"
                        ClientEventButtonClicked="OnDwMainButtonClicked" Height="250px" Width="530px"
                        ClientEventItemChanged="OnDwMainItemChanged">
                    </dw:WebDataWindowControl>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    &nbsp;<input id="B_add" type="button" value="เพิ่มแถว" onclick="InsertDwMain()" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="B_save" type="button" value="ตกลง" onclick="OnOkClick()" />
                    <input id="B_cancel" type="button" value="ยกเลิก" onclick="OnCloseDialog()" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="Hdrow" runat="server" />
        <asp:HiddenField ID="Hdfin" runat="server" />
    </div>
    </form>
</body>
</html>
