<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_as_loan_benefit_list.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_loan_benefit_list1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%=postFuncSearch%>
    <%=postCapitalYear%>
    <%=postMemberNo%>
    <%=postChildCardPerson%>
   <title></title>
<script type="text/javascript">

    function DwMainClick(sender, rowNumber, objectName) {
        var assist_docno = objdw_detail.GetItem(rowNumber, "assist_docno");
        var capital_year = objdw_detail.GetItem(rowNumber, "capital_year");
        var member_no = objdw_detail.GetItem(rowNumber, "member_no");

        window.opener.GetValueFromDlgList(assist_docno, capital_year, member_no);
        window.close();
    }

    function DwMemButtonClick(s, row, oName) {
        if (oName == "btn_search") {
            objdw_main.AcceptText();
            postFuncSearch();
        }
    }
    function DwmainItemChange(sender, rowNumber, columnName, newValue) {
        if (columnName == "capital_year") {
            sender.SetItem(rowNumber, columnName, newValue);
            sender.AcceptText(); 
        } else if (columnName == "member_no") {
            objdw_main.SetItem(rowNumber, "member_no", newValue);
            objdw_detail.AcceptText();
        }
        return 0;
    }
    function GetValueFromDlgList(assist_docno, capital_year, member_no) {
        Gcoop.GetEl("HfMemNo").value = member_no;
        Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
        Gcoop.GetEl("HfCapitalYear").value = capital_year;
        alert("aaa");
        postRetrieveDwMain();
    }          
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"> </asp:Literal>
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfSearchValue" runat="server" />
     <table>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_main" runat="server" ClientScriptable="True"
                        DataWindowObject="d_as_year_memno" HorizontalScrollBar="NoneAndClip" 
                        LibraryList="~/DataWindow/app_assist/as_capital.pbl" UseCurrentCulture="True"
                        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwmainItemChange" AutoRestoreContext="False">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_as_reqloan_benefit_list"
        LibraryList="~/DataWindow/app_assist/as_capital.pbl"  RowsPerPage="16" ClientScriptable="True"
        AutoRestoreContext="false" AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventClicked="DwMainClick">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Center" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings> 
    </dw:WebDataWindowControl>
    </form>
</body>
</html>
