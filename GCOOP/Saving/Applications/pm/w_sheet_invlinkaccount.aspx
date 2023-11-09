<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_invlinkaccount.aspx.cs" Inherits="Saving.Applications.pm.w_sheet_invlinkaccount" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostInsertRow%>
    <%=jsPostDelRow%>
    <script type="text/javascript">


        function Validate() {
            var IsConfirm = confirm("ยืนยันการบันทึกข้อมูล");

            if (!IsConfirm) {
                return false;
            }

            var invsource_code;
            var investtype_code;

            for (var i = 1; i <= objDwMain.RowCount(); i++) {
                invsource_code = objDwMain.GetItem(i, "invsource_code");
                investtype_code = objDwMain.GetItem(i, "investtype_code");
                if (investtype_code == "" || investtype_code == null || investtype_code == "" || investtype_code == null) {
                    alert("กรุณาระบุข้อมูลให้ครบถ้วน");
                    return false;
                }
            }

            return true;
        }

        function InsertRow() {
            jsPostInsertRow();
        }

        function OnDwMainButtonClicked(sender, row, bName) {
            if (confirm("ต้องการลบข้อมูลแถวที่ " + row + " ใช่หรือไม่")) {
                Gcoop.GetEl("Hdrow").value = row + "";
                jsPostDelRow();
            }
        }

        function OnDwFilterItemChanged(sender, row, col, value) {
            sender.SetItem(row, col, value);
            sender.AcceptText();
            jsPostEstSideChange();
        }

        function OnDwMainItemChanged(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <span onclick="InsertRow()" style="cursor: pointer;">
        <asp:Label ID="Label5" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>

    <input id="printBtn" type="button" value="พิมพ์หน้าจอ" onclick="printPage();"   />

    <asp:Panel ID="Panel1" runat="server" Height="400px" Width="750px" BorderStyle="Ridge">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_invlinkaccount"
            LibraryList="~/DataWindow/pm/pm_investment.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientFormatting="True" TabIndex="100" ClientEventButtonClicked="OnDwMainButtonClicked"
            Height="400px" Width="750px" RowsPerPage="13" ClientEventItemChanged="OnDwMainItemChanged">
            <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
                <BarStyle HorizontalAlign="Center" />
                <NumericNavigator FirstLastVisible="True" />
            </PageNavigationBarSettings>
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:HiddenField ID="Hdrow" runat="server" Value="" />
</asp:Content>
