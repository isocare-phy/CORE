<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_hr_constant_ucfworktimecode.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_constant_ucfworktimecode_ctrl.ws_hr_constant_ucfworktimecode" %>
<%@ Register src="DsList.ascx" tagname="DsList" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    var dsList = new DataSourceTool();

    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล");
    }

    function OnDsListItemChanged(s, r, c, v) {
        if (c == "probitemtype") {
            PostProbitemtype();
        }
    }


    function SheetLoadComplete() {
    }

    function OnDsListClicked(s, r, c) {
        if (c == "b_del") {
            dsList.SetRowFocus(r);
            PostDelRow();
        }
    }

    function OnClickNewRow() {
        PostNewRow();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<div align="right" style="margin-right: 1px; width: 700px;">
<span class="NewRowLink" onclick="OnClickNewRow()">เพิ่มแถว</span>
</div>
<div>
    <uc1:DsList ID="dsList" runat="server" />
</div>
</asp:Content>
