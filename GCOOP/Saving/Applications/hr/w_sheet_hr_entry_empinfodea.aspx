<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_entry_empinfodea.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_entry_empinfodea" Title="ประวัติครอบครัว"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postGetMember%>
    <%=postAddRow %>
    <%=postDeleteRow %>
    <%=postNewClear%>
    <%=postShowDetail %>
    <%=postSearchGetMember%>
    <%=postRefresh%>
    <style type="text/css">
        .style7
        {
            font-size: small;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('780', '600', 'w_dlg_hr_master_search.aspx', '');
        }

        function GetValueEmplid(emplid) {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Gcoop.Trim(emplid));
            objDwMain.AcceptText();
            postGetMember();
        }

        function OnDwDetailButtonClick(s, r, c) {
            if (c == "b_del") {
                var seq_no = "";
                seq_no = objDwDetail.GetItem(1, "seq_no");
                if (confirm("คุณต้องการลบรายการ " + seq_no + " ใช่หรือไม่?")) {
                    postDeleteRow();
                }
            }
            return 0;
        }

        function OnButtonClicked(s, row, oName) {
            if (oName == "b_search") {
                MenubarOpen();
            }
            return 0;
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }

        function DwDetailAddRow() {
            var emplid = objDwMain.GetItem(1, "emplid");
            if (emplid == "Auto" || emplid == null) {
                alert("ไม่พบรหัสมาชิก กรุณากรอกรหัสสมาชิกก่อน");
            }
            else {
                postAddRow();
            }
        }

        function OnListClick(sender, rowNumber, objectName) {
            var seq_no = objDwDetail.GetItem(rowNumber, "seq_no");
            if (seq_no != "Auto") {
                objDwDetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
                objDwDetail.AcceptText();
                postShowDetail();
            }
            return 0;
        }

        function OnItemChangeMain(s, r, c, v) {
            if (c == "emplcode") {
                objDwMain.SetItem(r, c, Gcoop.StringFormat(v, "00000"));
                objDwMain.AcceptText();
                postSearchGetMember();
            }

            else if (c == "entry_tdate") {
                objDwMain.SetItem(1, "entry_tdate", v);
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "entry_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }
            return 0;
        }
        function OnItemChangeDetail(s, r, c, v) {
            if (c == "socisecu_tdate") {
                objDwDetail.SetItem(r, c, v);
                objDwDetail.AcceptText();
                Gcoop.GetEl("HSchk").value = "1";
                postRefresh();
            }
            else if (c == "socisein_tdate") {
                objDwDetail.SetItem(r, c, v);
                objDwDetail.AcceptText();
                Gcoop.GetEl("HSchk").value = "2";
                postRefresh();
            }
            else if (c == "sociseout_tdate") {
                objDwDetail.SetItem(r, c, v);
                objDwDetail.AcceptText();
                Gcoop.GetEl("HSchk").value = "3";
                postRefresh();
            }
            else if (c == "modisala_tdate") {
                objDwDetail.SetItem(r, c, v);
                objDwDetail.AcceptText();
                Gcoop.GetEl("HSchk").value = "4";
                postRefresh();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td>
                <span class="style7">ข้อมูลการทำรายการ</span>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_family_head"
                    LibraryList="~/DataWindow/hr/hr_entry_emp_family.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventButtonClicked="OnButtonClicked" ClientEventItemChanged="OnItemChangeMain">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp&nbsp&nbsp
            </td>
        </tr>
        <tr>
            <td>
                <span class="style7">กำหนดข้อมูลเฉพาะบุคคล</span>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_hr_emplinfodea"
                    LibraryList="~/DataWindow/hr/hr_master.pbl" Width="750px" Height="800px" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="true"
                    ClientEventButtonClicked="OnDwDetailButtonClick" ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:HiddenField ID="HSchk" runat="server" />
</asp:Content>
