<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_sheet_as_ucfassist.aspx.cs" Inherits="Saving.Applications.assist.ws_sheet_as_ucfassist_ctrl.ws_sheet_as_ucfassist" %>
<%@ Register Src="DsAssisttype.ascx" TagName="DsAssisttype" TagPrefix="uc1" %>
<%@ Register Src="DsUcfasstype.ascx" TagName="DsUcfasstype" TagPrefix="uc2" %>
<%@ Register Src="DsUcfasspaytype.ascx" TagName="DsUcfasspaytype" TagPrefix="uc3" %>
<%--<%@ Register Src="DsUcfasstypedet.ascx" TagName="DsUcfasstypedet" TagPrefix="uc4" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
        var dsAssisttype = new DataSourceTool();
        var dsUcfasstype = new DataSourceTool();
        var dsUcfasspaytype = new DataSourceTool();
        //var dsUcfasstypedet = new DataSourceTool();

        function OnDsAssisttypeItemChanged(s, r, c, v) {
            PostSendAssCode();
        }
        function OnClickNewRow() {
            Gcoop.OpenIFrame2("520", "270", "ws_dlg_sl_addassisttype.aspx", "");
        }
        function OnClickDeleteRow() {
            if (confirm("ยืนยันการลบประเภทสวัสดิการ!!!") == true) {
                PostDelRowTap1();
            }            
        }
        function OnClickNewRowTap2() {
            PostNewRowTap2();
        }
        function OnClickNewRowTap3() {
            PostNewRowTap3();
        }
        function OnDsUcfasstypeClicked(s, r, c) {
            if (c == "b_del") {
                dsUcfasstype.SetRowFocus(r);
                if (confirm("ลบข้อมูลแถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRowTap1();
                }
            }
        }
        function OnDsUcfasspaytypeClicked(s, r, c) {
            if (c == "b_del") {
                dsUcfasspaytype.SetRowFocus(r);
                if (confirm("ลบข้อมูลแถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRowTap2();
                }
            }
        }
//        function OnDsUcfasstypedetClicked(s, r, c) {
//            if (c == "b_del") {
//                dsUcfasstypedet.SetRowFocus(r);
//                if (confirm("ลบข้อมูลแถวที่ " + (r + 1) + " ?") == true) {
//                    PostDelRowTap3();
//                }
//            }
//        }

        function OnDsUcfasstypeItemChanged(s, r, c, v) {
            if (c == "stm_flag") {
                if (v == "1") {
                    dsUcfasstype.GetElement(r, "process_flag").disabled = false;
                } else {
                    dsUcfasstype.GetElement(r, "process_flag").disabled = true;
                }
            }
        }

//        function OnDsUcfasstypedetItemChanged(s, r, c, v) {
//            if (c == "member_type") {
//                dsUcfasstypedet.SetItem(r, "member_type", v);
//            } else if (c == "assisttype_pay") {
//                dsUcfasstypedet.SetItem(r, "assisttype_pay", v);
//            } else if (c == "num_pay") {
//                if (v > 1) {
//                    dsUcfasstypedet.GetElement(r, 'first_payamt').disabled = false;
//                    dsUcfasstypedet.GetElement(r, 'first_payamt').style.background = '#FFFFFF';
//                    dsUcfasstypedet.GetElement(r, 'next_payamt').disabled = false;
//                    dsUcfasstypedet.GetElement(r, 'next_payamt').style.background = '#FFFFFF'
//                    dsUcfasstypedet.GetElement(r, 'first_typepay').disabled = false;
//                    dsUcfasstypedet.GetElement(r, 'first_typepay').style.background = '#FFFFFF'
//                    dsUcfasstypedet.GetElement(r, 'next_typepay').disabled = false;
//                    dsUcfasstypedet.GetElement(r, 'next_typepay').style.background = '#FFFFFF'
//                    dsUcfasstypedet.GetElement(r, 'max_nextpayamt').disabled = false;
//                    dsUcfasstypedet.GetElement(r, 'max_nextpayamt').style.background = '#FFFFFF'
//                } else if (v = 1) {
//                    dsUcfasstypedet.SetItem(r, "next_payamt", '0.00');
//                    dsUcfasstypedet.GetElement(r, 'next_payamt').disabled = true;
//                    dsUcfasstypedet.GetElement(r, 'next_payamt').style.background = '#CCCCCC'
//                    dsUcfasstypedet.GetElement(r, 'next_typepay').disabled = true;
//                    dsUcfasstypedet.GetElement(r, 'next_typepay').style.background = '#CCCCCC'
//                    dsUcfasstypedet.GetElement(r, 'max_nextpayamt').disabled = true;
//                    dsUcfasstypedet.GetElement(r, 'max_nextpayamt').style.background = '#CCCCCC'
//                }
//            } 
//            else if (c == "first_typepay") {
//                if (v = 2) {
//                    var chk_fpay = dsUcfasstypedet.GetItem(r, "first_payamt");
//                    if (chk_fpay > '100') {
//                        dsUcfasstypedet.SetItem(r, "first_payamt", '100');
//                    }
//                }
//            }
//        }

        function GetValueFromDlg(assisttype_code) {
            dsAssisttype.SetItem(0, "assisttype_code", assisttype_code);
            PostSendAssCode();
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }


    </script>
    <script type="text/javascript">
        Number.prototype.round = function (p) {
            p = p || 10;
            return parseFloat(this.toFixed(p));
        };

        $(function () {
            //alert($("#tabs").tabs()); //ชื่อฟิวส์
            
            var tabIndex = Gcoop.GetEl("hdTabIndex").value; // Gcoop.ParseInt($("#<%=hdTabIndex.ClientID%>").val());
            $("#tabs").tabs({
                active: tabIndex,
                activate: function (event, ui) {
                    $("#<%=hdTabIndex.ClientID%>").val(ui.newTab.index() + "");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">  
   <span class="NewRowLink" onclick="OnClickNewRow()">เพิ่มประเภทสวัสดิการ</span>   
<span class="NewRowLink" style="float:right;" onclick="OnClickDeleteRow()">ลบประเภทสวัสดิการ</span>   

   <uc1:DsAssisttype ID="dsAssisttype" runat="server" />
    <br />
    <div id="tabs">
        <ul style="font-size:12px;">
    
            <li><a href="#tabs-1">ประเภทสวัสดิการ</a></li>
            <li><a href="#tabs-2">ประเภทการจ่ายสวัสดิการ</a></li>
            <%--<li><a href="#tabs-3">เงื่อนไขการจ่ายสวัสดิการ</a></li>--%>            
        </ul>
        <div id="tabs-1"> 
            <uc2:DsUcfasstype ID="dsUcfasstype" runat="server" />          
        </div>
        <div id="tabs-2">
            <span class="NewRowLink" onclick="OnClickNewRowTap2()">เพิ่มแถว</span> 
            <uc3:DsUcfasspaytype ID="dsUcfasspaytype" runat="server" />            
        </div>
        <%--<div id="tabs-3">
            <span class="NewRowLink" onclick="OnClickNewRowTap3()">เพิ่มแถว</span> 
            <uc4:DsUcfasstypedet ID="dsUcfasstypedet" runat="server" />            
        </div>--%>
    </div>      
    <br />
    <asp:HiddenField ID="hdTabIndex" Value="0" runat="server" />
    <asp:HiddenField ID="hdSaveChk_GPA" runat="server" />
    <asp:HiddenField ID="hdInertRow" runat="server" />
    <asp:HiddenField ID="hdCalDay" runat="server" />
    <asp:HiddenField ID="hdActMemno" runat="server" />
</asp:Content>



