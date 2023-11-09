<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_mssysbal.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_mssysbal_ctrl.w_sheet_mssysbal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ register src="DsList.ascx" tagname="DsList" tagprefix="uc1" %>
    <%@ register src="DsMain.ascx" tagname="DsMain" tagprefix="uc2" %>
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsList = new DataSourceTool;

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDsListClicked(s, r, c) {
            if (c == "b_del") {
                dsList.SetRowFocus(r); //ให้รู้ว่ากดแถวไหนในลิส
                PostDeleteRow(); //PostDeleteRow(); ไว้ลบแถวที่เลือก
            }
        }

        function OnDsMainClicked(s, r, c) {
            if (c == "btn_search") {
                postsearch();
            } else if (c == "btn_process") {
                postprocess();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <%--  <span class="NewRowLink" onclick="PostInsertRow()">เพิ่มแถว</span>--%>
    <uc2:DsMain ID="dsMain" runat="server" />
    <uc1:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="seq_no_old" runat="server" />
</asp:Content>
