<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_approve_status.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_approve_status" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=postSearchList %>
    <%=postApprove%>
    <%=postNotApprove%>
    <script type="text/javascript">

        function Validate() {
            objDwMain.AcceptText();
            var approve_status;
            for (var i = 1; i <= objDwMain.RowCount(); i++) {
                var req_status = getObjInt(objDwMain, i, "req_status");
                if (req_status == 1) {
                    approve_status = getObjInt(objDwMain, i, "approve_status");
//                    if (approve_status != 1) {
//                        alert("กรุณาระบุ สถานะการอนุมัติ");
//                        return;
//                    }
                }
            }
            return confirm("ยืนยันการบันทึกข้อมูล");

        }
        function DwTypeItemClicked(sender, rowNumber, buttonName) {
            if (buttonName == "btn_edit") {
                objDwType.AcceptText();
                postSearchList();
            }
        }
        function DwTypeItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "member_no") {
                postSearchList();
            }
        }
        function DwMainItemBClicked(s, r, c) {

            var rowTotal = objDwMain.RowCount();
            var req_status;
            if (c == "b_con") {
//                for (var i = 1; i <= rowTotal; i++) {
//                    req_status = objDwMain.GetItem(i, "req_status");
//                    if (req_status == 1) {
//                        objDwMain.SetItem(i, "approve_status", "1");
//                    }
                //                }
                objDwMain.AcceptText();
                postApprove();
            }
            else if (c == "b_can") {
                for (var i = 1; i <= rowTotal; i++) {
                    req_status = objDwMain.GetItem(i, "req_status");
                    if (req_status == 1) {
                        objDwMain.SetItem(i, "approve_status", "-1");
                    }
                }
            }
            else if (c == "b_not") {
//                for (var i = 1; i <= rowTotal; i++) {
//                    req_status = objDwMain.GetItem(i, "req_status");
//                    if (req_status == 1) {
//                        objDwMain.SetItem(i, "approve_status", "0");
//                    }
                //                }
                objDwMain.AcceptText();
                postNotApprove();
            }
        }
        function DwMainItemClicked(s, r, c) {
            if (r != 0 + '') {
                if (c == "approve_status" || c == "approve_tdate") {
                    var req_status = objDwMain.GetItem(r, "req_status");
                    if (req_status == 8 + '') {
                        objDwMain.SetItem(r, "req_status", "1");
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwType" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_approve_type" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwTypeItemClicked" ClientEventItemChanged="DwTypeItemChange">
    </dw:WebDataWindowControl>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Width="700px" Height="500px">
        <asp:CheckBox ID="ChkAll" runat="server" AutoPostBack="True" Text="อนุมัติทั้งหมด"
            OnCheckedChanged="CheckAllChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblRowCount" runat="server" Text="" Font-Bold="true"></asp:Label>
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
            DataWindowObject="d_as_approve" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            RowsPerPage="15" ClientEventButtonClicked="DwMainItemBClicked" ClientEventClicked="DwMainItemClicked">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
