<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_kt_member_65.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_kt_member_65" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%=refresh  %>
    <%=search  %>
    <title>ค้นหาสมาชิก</title>

    <script type="text/javascript">
        function selectRow(sender, rowNumber, objectName) {
            if (objectName != "datawindow") {
                var memberno = objdw_detail.GetItem(rowNumber, "member_no");
                var pre_name = objdw_detail.GetItem(rowNumber, "prename_desc");
                var memb_name = objdw_detail.GetItem(rowNumber, "memb_name");
                var memb_surname = objdw_detail.GetItem(rowNumber, "memb_surname");
                var membgroup_desc = objdw_detail.GetItem(rowNumber, "membgroup_desc");
                var membgroup_code = objdw_detail.GetItem(rowNumber, "membgroup_code");
                try {
                    window.opener.GetValueFromDlg2(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code);
                } catch (err) {
                    window.opener.GetValueFromDlg(memberno);
                }
                window.close();
            }
        }

        function ItemChangedDwData(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                var memberNoT = Gcoop.Trim(newValue);
                var memberNo = Gcoop.StringFormat(memberNoT, "00000000");
                objdw_data.SetItem(1, "member_no", memberNo);
                objdw_data.AcceptText();
                refresh();
            }
        }
        function OnSearch() {
            search();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    รายการค้นหา
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 530px;">
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_data" runat="server" ClientScriptable="True" DataWindowObject="d_dlg_kt_membsrch_criteria"
                    LibraryList="~/DataWindow/app_assist/kt_65years.pbl" ClientEventItemChanged="ItemChangedDwData">
                </dw:WebDataWindowControl>
            </td>
            <td>
                <%--<asp:Button ID="cb_find" runat="server" Text="ค้นหา" Height="80px" Width="55px" OnClick="cb_find_Click" />--%>
                <input id="btnSearch" type="button" value="ค้นหา" onclick="OnSearch(this)" style="width: 55px;
                    height: 53px;" />
            </td>
        </tr>
    </table>
    รายละเอียด
    <asp:HiddenField ID="HfSearch" runat="server" />
    <dw:WebDataWindowControl ID="dw_detail" runat="server" ClientEventClicked="selectRow"
        DataWindowObject="d_dlg_kt_membsrch_list_memno2" LibraryList="~/DataWindow/app_assist/kt_50bath.pbl"
        RowsPerPage="17" ClientScriptable="True">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    </form>
</body>
</html>

