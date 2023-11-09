<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_arc_master_setting.aspx.cs" Inherits="Saving.Applications.arc.w_sheet_hr_arc_master_setting" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%=initJavaScript%>
    <%=postAddRow %>
    <%=postDeleteRow %>
    <%=postShowDetail %>
    <%=postSearchGetMember%>
    <%=postgetname%>
    <%=postgetfield%>
    <%=postNewClear%>
    <%=postGetDocument%>
    <%=postRetrieveSetgroup %>
    <%=postRetrieveSetgroupDetailDD %>

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

        function GetMemDetFromDlg(memberno) {
            Gcoop.GetEl("Hfmember_no").value = memberno;
            postSearchGetMember();
            objDwDetail.AcceptText();
        }
        //////////////////////////////////////////////////////
        function OnButtonClicked(s, r, c) {
            if (c == "b_process") {
                postGetDocument();
            }
            return 0;
        }
        //////////////////////////////////////////////////////
        function OnDwDetailButtonClick(s, r, c) {
            if (c == "b_del") {
                if (confirm("ยืนยันการลบข้อมูล")) {
                    postDeleteRow();
                }
            }
            else if (c == "b_search") {
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
            Gcoop.GetEl("HRow").value = rowNumber;
            postShowDetail();
            return 0;
        }

        function OnItemChangeMain(s, r, c, v) {
            if (c == "arc_master_paper_type") {
                objDwMain.SetItem(1, "arc_master_paper_type", v);
            }
            else if (c == "year") {
                objDwMain.SetItem(1, "year", v);
            }
            else if (c == "month") {
                objDwMain.SetItem(1, "month", v);
            }
            else if (c == "domain_id") {
                objDwMain.SetItem(1, "domain_id", v);
                postRetrieveSetgroup();
            }
            else if (c == "group_id") {
                objDwMain.SetItem(1, "group_id", v);
            }
            objDwMain.AcceptText();
            return 0;
        }

        function OnDwDetailChanged(s, r, c, v) {

            if (c == "domain_id") {
                objDwDetail.SetItem(1, "domain_id", v);
                postRetrieveSetgroupDetailDD();
            }
            else if (c == "paper_type") {
                objDwDetail.SetItem(1, "paper_type", v);
                objDwDetail.AcceptText();
                postgetfield();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="HRow" runat="server" />
    <asp:HiddenField ID="Hfmember_no" runat="server" />
    <table style="width: 100%;">
     
        <%--
        <tr>
            <td valign="top" class="style6">
                &nbsp;
            </td>
            <td valign="top" class="style6">
                &nbsp;
            </td>
        </tr>
        --%>
        <tr>
            <td valign="top" class="style6">
                <b>รายการหนังสือ</b>
            </td>
            <td valign="top" class="style6">
                <b>ทะเบียนหนังสือออก</b>
            </td>
        </tr>
        <tr>
            <td class="style5" valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="hr_arc_master_list"
                    LibraryList="~/DataWindow/arc/hr_arc_master.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True" RowsPerPage="10"
                    ClientEventClicked="OnListClick" Width="200px">
               <PageNavigationBarSettings Position="Top" Visible="True" NavigatorType="Numeric">
               <BarStyle HorizontalAlign="Center" />
               <NumericNavigator FirstLastVisible="True" />
               </PageNavigationBarSettings>
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="hr_arc_master_detail_for_set"
                    LibraryList="~/DataWindow/arc/hr_arc_master.pbl" Width="550px" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="true"
                    ClientEventButtonClicked="OnDwDetailButtonClick" ClientFormatting="True" ClientEventItemChanged="OnDwDetailChanged">
                </dw:WebDataWindowControl>
            </td>
        </tr>

        <tr>
          <td class="style5" valign="top">
                <dw:WebDataWindowControl ID="WebDataWindowControl1" runat="server" DataWindowObject="hr_arc_master_list"
                    LibraryList="~/DataWindow/arc/hr_arc_master.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True" Visible="false"
                    ClientEventClicked="OnListClick" Width="200px">
                </dw:WebDataWindowControl>
            </td>
            <td colspan="2">
       
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_arc_master_head_setting"
                    LibraryList="~/DataWindow/arc/hr_arc_master.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventButtonClicked="OnButtonClicked" ClientEventItemChanged="OnItemChangeMain"
                    ClientFormatting="True">
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
    <asp:HiddenField ID="HSqlTemp" runat="server" Value="0" Visible="False" />
</asp:Content>
