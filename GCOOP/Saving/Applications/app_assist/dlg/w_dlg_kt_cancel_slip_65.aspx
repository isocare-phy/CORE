<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_kt_cancel_slip_65.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_kt_cancel_slip_65" %>

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
                //                var memb_name = objdw_detail.GetItem(rowNumber, "memb_name");
                //                var memb_surname = objdw_detail.GetItem(rowNumber, "memb_surname");
                var resign_date = objdw_detail.GetItem(rowNumber, "resign_date");
                try {
                    window.opener.GetValueFromDlg2(memberno);
                } catch (err) {
                    window.opener.GetValueFromDlg(memberno, resign_date);
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
    <style type="text/css">
        #form1
        {
            height: 264px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 530px;">
        <tr>
            <td>
                
                <asp:HiddenField ID="HfSearch" runat="server" />
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto">
                    <dw:WebDataWindowControl ID="dw_detail" runat="server" ClientEventClicked="selectRow"
                        DataWindowObject="d_dlg_cancel_slip_65" LibraryList="~/DataWindow/app_assist/kt_65years.pbl"
                        RowsPerPage="17" ClientScriptable="True">
                        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                            <BarStyle HorizontalAlign="Center" />
                            <NumericNavigator FirstLastVisible="True" />
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

