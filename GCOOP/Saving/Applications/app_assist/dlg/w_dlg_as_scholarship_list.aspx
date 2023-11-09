<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_as_scholarship_list.aspx.cs"
    Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_scholarship_list" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%=postFilter %>
    <%=postDelete %>
    <title></title>
    <script type="text/javascript">

        function DwMainClick(sender, rowNumber, objectName) {
            if (objectName == "assist_docno") {
                var assist_docno = objDwMain.GetItem(rowNumber, "assist_docno");
                var capital_year = objDwMain.GetItem(rowNumber, "capital_year");
                var member_no = objDwMain.GetItem(rowNumber, "member_no");
                window.opener.GetValueFromDlgList(assist_docno, capital_year, member_no);
                window.close();
            }
        }

        function DwCriButtonClick(sender, rowNumber, buttonName) {
            objDwCri.AcceptText();
            postFilter();
        }

        function DwMainButtonClick(sender, rowNumber, buttonName) {
            objDwMain.AcceptText();
            var member_no = objDwMain.GetItem(rowNumber, "member_no");
            if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ " + member_no)) {
                var assist_docno = objDwMain.GetItem(rowNumber, "assist_docno");
                Gcoop.GetEl("HfRow").value = assist_docno;
                postDelete();
            }
        }

        function DwCriItemChang(sender, rowNumber, columnName, newValue) {
            objDwCri.SetItem(rowNumber, columnName, newValue);
            objDwCri.AcceptText();
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <div>
        <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="dw_cri_level_school"
            LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
            AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
            ClientFormatting="True" ClientEventButtonClicked="DwCriButtonClick" ClientEventItemChanged="DwCriItemChang">
        </dw:WebDataWindowControl>
        <br />
        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="400px">
            <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_as_scholarship_list"
                LibraryList="~/DataWindow/app_assist/as_capital.pbl" ClientScriptable="True"
                AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
                ClientFormatting="True" ClientEventClicked="DwMainClick" ClientEventButtonClicked="DwMainButtonClick">
            </dw:WebDataWindowControl>
        </asp:Panel>
        <asp:HiddenField ID="HfRow" runat="server" />
    </div>
    </form>
</body>
</html>
