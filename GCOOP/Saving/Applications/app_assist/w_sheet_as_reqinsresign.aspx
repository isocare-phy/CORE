<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_reqinsresign.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_reqinsresign" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%=initJavaScript%>
    <%=jsGetmember%>
    <%=postGetins%>
    <script type="text/javascript">

        function DWITemCHANGE(sender, row, colum, value) {
            if (colum == "member_no") {
                objDwMain.SetItem(row, colum, value);
                objDwMain.AcceptText();
                Gcoop.GetEl("hfmember").value = value;
              //  alert(value);
                postGetins();
            }
        }
        function DwMainButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }
        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMain.SetItem(1, "member_no", memberno);
            objDwMain.SetItem(1, "memb_name", memb_name);
            objDwMain.SetItem(1, "memb_surname", memb_surname);
            Gcoop.GetEl("hfmember").value = memberno;
//            objDwMain.SetItem(1, "membgroup_desc", membgroup_desc);
//            objDwMain.SetItem(1, "membgroup_code", membgroup_code);
//            var project_id = objDwMain.GetItem(1, "project_id");
//            var member_no = objDwMain.GetItem(1, "member_no");
//            var course_id = objDwMain.GetItem(1, "course_id");
//            if (project_id != null && member_no != null && course_id != null) {
                postGetins();
//            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hlist" runat="server" />
    <asp:HiddenField ID="hfmember" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ins_mainresign"
            LibraryList="~/DataWindow/app_assist/as_reqinsresign.pbl" ClientEventButtonClicked="DwMainButtonClick"
            ClientEventItemChanged="DWITemCHANGE" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
       <%-- รายละเอียดการลาออก--%>
        <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_sl_appsrg_detial"
            LibraryList="~/DataWindow/app_assist/as_reqinsresign.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventClicked="selectRow" ClientFormatting="True" Visible="false">
        </dw:WebDataWindowControl>
    </asp:Panel>
</asp:Content>
