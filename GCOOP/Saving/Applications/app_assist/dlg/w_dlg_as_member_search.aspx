<%@ Page  Language="C#"  AutoEventWireup="true" 
CodeBehind="w_dlg_as_member_search.aspx.cs" Inherits="Saving.Applications.app_assist.dlg.w_dlg_as_member_search" %>


<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ค้นหาสมาชิก</title>
    <%=MemberNoSearch%>
    <script type="text/javascript">
        function selectRow(sender, rowNumber, objectName) {
            var memberNo = objDwList.GetItem(rowNumber, "member_no");
            window.opener.GetMemberNoFromDlg(memberNo);
            window.close();

        }

        function OnDwMainButtonClicked(sender, rowNumber, buttonName) {

            if (buttonName == "b_search") {

                objDwMain.AcceptText();
                MemberNoSearch();
            }
        }
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_kt_member_search_criteria"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" ClientEventButtonClicked="OnDwMainButtonClicked" ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="hidden_search" runat="server" />
    <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="d_kt_member_search_memno_list_new"
        LibraryList="~/DataWindow/app_assist/kt_50bath.pbl" 
        RowsPerPage="14" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" 
        ClientScriptable="True" ClientEventClicked="selectRow">
        <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
            <BarStyle HorizontalAlign="Left" />
            <NumericNavigator FirstLastVisible="True" />
        </PageNavigationBarSettings>
    </dw:WebDataWindowControl>
    </form>
</body>
</html>