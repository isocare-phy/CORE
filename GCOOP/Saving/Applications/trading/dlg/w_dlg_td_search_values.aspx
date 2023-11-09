<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_td_search_values.aspx.cs" 
Inherits="Saving.Applications.trading.dlg.w_dlg_td_search_values" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%=jsSearch%>
    <%=jsSend %>
    <script type="text/javascript">
        function OnDwCriButtonClicked(s, r, c) {
            if (c == "b_search") {
                jsSearch();
            }
        }

        function DwDetailClick(s, r, c) {
//            if (r > 0) {
//                var debt_no = objDwDetail.GetItem(r, "debt_no");
//                parent.GetDlgDebtNo(debt_no);
//                parent.RemoveIFrame();
            //            }
//            if (objectName == "select_flag") {
//                Gcoop.CheckDw(s, r, c, "select_flag", 1, 0);
//                s.AcceptText();
//            }
        }

        function SendResult() {
            jsSend();
        }

        function DialogLoadComplete() {
            if (Gcoop.GetEl("HdEnd").value == "true") {
                try {
                    parent.GetDlgValue(Gcoop.GetEl("HdResult").value, Gcoop.GetEl("HdProductCode").value);
                    alert(Gcoop.GetEl("HdEnd").value);
                    parent.RemoveIFrame();
                    alert(Gcoop.GetEl("HdEnd").value);
                }
                catch (err) {
                    alert(err);
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Label1" runat="server" Text="ค้นหา"></asp:Label>
        <dw:WebDataWindowControl ID="DwCri" runat="server" DataWindowObject="d_td_search_values_cri"
            LibraryList="~/DataWindow/trading/dlg_td_search_values.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventButtonClicked="OnDwCriButtonClicked" ClientFormatting="True" TabIndex="1">
        </dw:WebDataWindowControl>
        <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_td_search_values"
            LibraryList="~/DataWindow/trading/dlg_td_search_values.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" ClientEventClicked="DwDetailClick">
        </dw:WebDataWindowControl>
        <div align="center">
            <input type="button" id="send" value="ตกลง" onclick="SendResult();"/>
        </div>        
    </div>
    <asp:HiddenField ID="HdEnd" runat="server" />
    <asp:HiddenField ID="HdResult" runat="server" />
    <asp:HiddenField ID="HdProductCode" runat="server" />
    <asp:HiddenField ID="HdSlipType" runat="server" />
    <asp:HiddenField ID="HdDebtNo" runat="server" />
    </form>
</body>
</html>
