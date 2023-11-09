<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_werfare_drawhosp.aspx.cs"
    Inherits="Saving.Applications.hr.w_sheet_hr_werfare_drawhosp" Title="ประวัติการเบิกค่ารักษาพยาบาล"
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
        .style5
        {
            width: 95px;
        }
        .style6
        {
            font-size: small;
        }
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

        function OnDwDetailButtonClick(s, r, c) 
        {
            if (c == "b_del")
             {
                var seq_no = "";
                seq_no = objDwDetail.GetItem(1, "emp_seq");
                    if (confirm("คุณต้องการลบรายการ " + seq_no + " ใช่หรือไม่?")) 
                    {
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

            var seq_no = objDwList.GetItem(rowNumber, "emp_seq");
          
            if (seq_no != "<Auto>") {
                objDwDetail.SetItem(1, "emp_seq", Gcoop.Trim(seq_no));
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
           if (c == "commit_tdate") {
               objDwDetail.SetItem(1, "commit_tdate", v);
               objDwDetail.AcceptText();
               objDwDetail.SetItem(1, "commit_date", Gcoop.ToEngDate(v));
               objDwDetail.AcceptText();
           }
           else if (c == "appv_tdate") {
               objDwDetail.SetItem(1, "appv_tdate", v);
               objDwDetail.AcceptText();
               objDwDetail.SetItem(1, "appv_date", Gcoop.ToEngDate(v));
               objDwDetail.AcceptText();
           }
           else if (c == "paid_tdate") {
               objDwDetail.SetItem(1, "paid_tdate", v);
               objDwDetail.AcceptText();
               objDwDetail.SetItem(1, "paid_date", Gcoop.ToEngDate(v));
               objDwDetail.AcceptText();
           }
            
            return 0;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td colspan="2">
                <span class="style7">ข้อมูลการทำรายการ</span>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_werfare_drawhosp_head"
                    LibraryList="~/DataWindow/hr/hr_werfare.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventButtonClicked="OnButtonClicked" ClientEventItemChanged="OnItemChangeMain">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top" class="style6">
                &nbsp;</td>
            <td valign="top" class="style6">
                &nbsp;</td>
        </tr>
        <tr>
            <td valign="top" class="style6">
                <b>ข้อมูลการเบิกค่าสวัสดิการ</b>
            </td>
            <td valign="top" class="style6">
                <b>รายละเอียดการเบิกค่าสวัสดิการเจ้าหน้าที่</b>
            </td>
        </tr>
        <tr>
            <td class="style5" valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="dw_hr_welfare_employee_list"
                    LibraryList="~/DataWindow/hr/hr_werfare.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventClicked="OnListClick" Width="200px">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span>
            </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_hr_welfare_employee_detail"
                    LibraryList="~/DataWindow/hr/hr_werfare.pbl" Width="550px" Height="550px"
                    AutoSaveDataCacheAfterRetrieve="True" AutoRestoreDataCache="True" AutoRestoreContext="False"
                    ClientScriptable="true" ClientEventButtonClicked="OnDwDetailButtonClick" ClientFormatting="True"
                    ClientEventItemChanged="OnItemChangeDetail">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style5">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;
</asp:Content>
