<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_transfertoofficedea.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_transfertoofficedea" Title="ประวัติการเลื่อนตำแหน่ง"
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
    <style type="text/css">
        .style3
        {
            width: 198px;
        }
        .style4
        {
            width: 47px;
        }
    </style>
    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg('590', '600', 'w_dlg_hr_master_search.aspx', '');
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
                if (seq_no == "" || seq_no == null || seq_no == "Auto") {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else {
                    if (confirm("คุณต้องการลบรายการ " + seq_no + " ใช่หรือไม่?")) {
                        postDeleteRow();
                    }
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
            var seq_no = objDwList.GetItem(rowNumber, "seq_no");
            Gcoop.GetEl("HfRow").value = rowNumber;
            //if (seq_no != "Auto") {
            objDwDetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
            objDwDetail.AcceptText();
            postShowDetail();
            //}
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

        function OnDwDetailItemChange(s, r, c, v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();
            if (c == "move_tdate") {
                objDwDetail.SetItem(1, "move_tdate", v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1, "move_date", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
            }
            else if (c == "start_tdate") {
                objDwDetail.SetItem(1, "start_tdate", v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1, "start_date", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
            }
//            else if (c == "step_new" || c == "level_new" || c == "account_new") {
//                //alert("55555");
//                var step_new = objDwDetail.GetItem(r, "step_new");
//                var level_new = objDwDetail.GetItem(r, "level_new");
//                var account_new = objDwDetail.GetItem(r, "account_new");
//                //alert(step_new + level_new + account_new);
//                if (step_new != null && level_new != null && account_new != null) {
//                    //alert("111");
//                    postStepSalary();
//                }
//            }
            //            else if (c == "level_new") {
            //                alert("55555");
            //                objDwDetail.SetItem(1, "level_new", v);
            //                objDwDetail.AcceptText();
            //                postStepSalary();
            //            }
            //            else if (c == "account_new") {
            //                alert("55555");
            //                objDwDetail.SetItem(1, "account_new", v);
            //                objDwDetail.AcceptText();
            //                postStepSalary();
            //            }
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td colspan="3">
                &nbsp; &nbsp; &nbsp;
                <asp:Label ID="Label1" runat="server" Text="ข้อมูลการทำรายการ" Font-Bold="True" Font-Names="tahoma"
                    Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_transfertooffice_head"
                    LibraryList="~/DataWindow/hr/hr_transfertoofficedea.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientEventItemChanged="OnItemChangeMain"
                    ClientScriptable="True" ClientEventButtonClicked="OnButtonClicked" ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <table style="width: 100%;">
        <tr>
            <td class="style3" valign="top">
                &nbsp;
                <asp:Label ID="Label2" runat="server" Text="ข้อมูลการเลื่อนตำแหน่งเจ้าหน้าที่" Font-Bold="True"
                    Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="dw_hr_transfertooffice_list"
                    LibraryList="~/DataWindow/hr/hr_transfertoofficedea.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventClicked="OnListClick" ClientFormatting="True">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span>
                <br />
                <br />
                <br />
            </td>
            <td class="style4">
                &nbsp;
            </td>
            <td valign="top">
                &nbsp;
                <asp:Label ID="Label3" runat="server" Text="รายละเอียดการเลื่อนตำแหน่งเจ้าหน้าที่"
                    Font-Bold="True" Font-Names="tahoma" Font-Size="12px"></asp:Label>
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_hr_transfertooffice_detail"
                    LibraryList="~/DataWindow/hr/hr_transfertoofficedea.pbl" Width="500px" Height="500px"
                    AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
                    ClientScriptable="true" ClientFormatting="True" ClientEventItemChanged="OnDwDetailItemChange"
                    ClientEventButtonClicked="OnDwDetailButtonClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style3">
                &nbsp;
            </td>
            <td class="style4">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfRow" runat="server" />
</asp:Content>
