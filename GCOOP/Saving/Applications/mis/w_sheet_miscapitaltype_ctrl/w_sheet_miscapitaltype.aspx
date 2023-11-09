<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_miscapitaltype.aspx.cs" Inherits="Saving.Applications.mis.w_sheet_miscapitaltype_ctrl.w_sheet_miscapitaltype" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ register src="DsList.ascx" tagname="DsList" tagprefix="uc1" %>
    <script type="text/javascript">
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
        function OnDsListItemChange(s, r, c, v) {

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<span class="NewRowLink" onclick="PostInsertRow()">เพิ่มแถว</span> &nbsp; &nbsp;
 <uc1:DsList ID="dsList" runat="server" />
</asp:Content>
