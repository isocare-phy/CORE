﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_wheet_as_ucfassisttypegroup.aspx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_ucfassisttypegroup_ctrl.ws_wheet_as_ucfassisttypegroup" %>
<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
    <script type="text/javascript">

        //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
        var dsList = new DataSourceTool();

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsListItemChanged(s, r, c, v) {

        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsListClicked(s, r, c) {
            if (c == "b_del") {
                dsList.SetRowFocus(r);
                if (confirm("ลบข้อมูลแถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRow();
                }
            }
        }
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
            //            for (var i = 0; i < dsList.GetRowCount() - 1; i++) {
            //                var chk_code = dsList.GetItem(i, "ASSISTTYPE_CODE");
            //                var chk_desc = dsList.GetItem(i, "ASSISTTYPE_GROUPDESC");
            //                if (chk_code == "") {
            //                    alert("กรุณากรอกรหัสประเภทสวัสดิการ");
            //                    dsList.SetItem(i, "ASSISTTYPE_CODE", "");
            //                    break;
            //                } else if (chk_desc = "") {
            //                    alert("กรุณากรอกคำอธิบายสวัสดิการ");
            //                    dsList.SetItem(i, "ASSISTTYPE_GROUPDESC", "");
            //                    break;
            //                }
            //            }                                        
        }

        function SheetLoadComplete() {
        }

        function OnClickNewRow() {
            PostNewRow();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <%--<span class="NewRowLink" onclick="OnClickNewRow()">เพิ่มแถว</span>--%>
    <uc1:DsList ID="dsList" runat="server" />
</asp:Content>

