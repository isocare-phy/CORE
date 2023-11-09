<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_vc_voucher_tks.aspx.cs" Inherits="Saving.Applications.account.w_sheet_vc_voucher_tks" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--  protected String postSelectList;
        protected String postVoucherDate;
        protected String postW_dlg_Click;
        protected String postNewClear;
        protected String postSearchVoucher;--%>
    <%=initJavaScript%>
    <%=postSelectList%>
    <%=postVoucherDate%>
    <%=postW_dlg_Click%>
    <%=postNewClear%>
    <%=postSearchVoucher%>
 <%--   <%=postPrint %>--%>
    <%=postAccidDtail%>
    <%= postInsertDetail %>
    <%= postDeleteDetail %>
    <script type="text/javascript">

        function OnDwfindItemChange(s, r, c, v) {
            if (c == "voucher_no") {
                s.SetItem(1, "voucher_no", v);
                s.AcceptText();
                Gcoop.GetEl("vcno").value = v;
                postSearchVoucher();
            }
        }


        // ฟังก์ชันการเพิ่มแถวข้อมูล
        function Insert_ucf_accsection() {

            postInsertDetail();

        }

        // ฟังก์ชันการลบข้อมูล
        function Delete_ucf_accsection(sender, row, bName) {
            if (bName == "b_del") {
                var account_id = objDw_detail.GetItem(row, "account_id");

                if (account_id == "" || account_id == null) {
                    Gcoop.GetEl("Hd_row").value = row + "";
                    postDeleteDetail();
                } else {
                    var isConfirm = confirm("ต้องการลบข้อมูล " + account_id + "ใช่หรือไม่ ?");
                    if (isConfirm) {
                        Gcoop.GetEl("Hd_row").value = row + "";
                        postDeleteDetail();
                    }
                }
            }
                        return 0;


        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }

        function OnDwListClick(s, r, c, v) {
            //            alert(Hd_row);
            if (c == "account_id") {
                ////                var vcNo = "";
                ////                var acc_id = "";
                ////                vcNo = objDw_list.GetItem(r, "voucher_no");
                ////                acc_id = objDw_list.GetItem(r, "account_id");
                Gcoop.GetEl("Hd_row").value = r + "";

                postAccidDtail();

            }
        }

        // opendialog
        function B_NewVc_Click() {
            var vcDate = "";
            vcDate = objDw_date.GetItem(1, "voucher_tdate"); // ส่งชื่อฟิวส์วันที่ไทยเข้าไป


            if (vcDate == "" || vcDate == null) {
                alert("กรุณากรอกวันที่ Voucher")
            }
            else {
                var B_status = "New";
                //Gcoop.OpenDlg(950,550,"w_dlg_vc_voucher_edit.aspx", "?vcDate="+ vcDate +"&B_status=" + B_status);
                Gcoop.OpenIFrame(900, 600, "w_dlg_vc_voucher_edit.aspx", "?vcDate=" + vcDate + "&B_status=" + B_status);

            }
        }

        // Print ใบ voucher จากหน้าลงรายวัน
        function Dw_mainButtonclick(sender, rowNumber, buttonName) {
            //if (buttonName == "b_print") {

//            postPrint();
            // }
        }


        function OnDWDateItemChange(s, r, c, v)//เปลี่ยนวันที่
        {
            if (c == "voucher_tdate") {
                if (v == "" || v == null) {
                    alert("กรุณากรอกข้อมูลวันที่ Voucher")
                }
                else {
                    s.SetItem(1, "voucher_tdate", v);
                    s.AcceptText();
                    s.SetItem(1, "voucher_date", Gcoop.ToEngDate(v));
                    s.AcceptText();
                    postVoucherDate();
                }
            }
            return 0;
        }


        function OnDwDateClicked(s, row, c) {
            if (c == "b_retrieve") {
                postSelectList();
            }
            return 0;
        }

        function Validate() {
            var alertstr = "";
            var voucher_date = "";
            voucher_date = objDw_date.GetItem(1, "voucher_tdate");
            if (voucher_date == "" || voucher_date == null) {
                alertstr = alertstr + "_กรุณากรอกวันที่ Voucher \n";
            }

            if (alertstr == "") {
                return true;
            }
            else {
                alert(alertstr);
                return false;
            }
        }


        function W_dlg_Click(vc_no) {
            Gcoop.GetEl("HdVoucherNo").value = vc_no;
            postW_dlg_Click();
        }
    
   
    

    </script>
    <style type="text/css">
        .style3
        {
            font-size: small;
        }
        .style4
        {
            font-size: small;
            font-weight: bold;
        }
        .style5
        {
            width: 177px;
        }
        .style6
        {
            width: 177px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <p>
        <span class="style3">
            <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
            <table style="width: 100%;">
                <tr>
                    <td class="style5">
                        <b>ประจำวันที่</b>
                    </td>
                    <td class="style6">
                        รายละเอียด <%--<span class="linkSpan" id="print2" runat="server" onclick="postPrint()"
                            style="font-family: Tahoma; font-size: medium; float: right; color: #0000CC;">Print</span>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        <asp:Panel ID="Panel3" runat="server" Height="100px" Width="200px" BorderStyle="Ridge">
                            <span class="style3">
                                <dw:WebDataWindowControl ID="Dw_date" runat="server" DataWindowObject="d_vc_vcdate_gl"
                                    LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwDateClicked"
                                    ClientScriptable="True" ClientEventItemChanged="OnDWDateItemChange" ClientFormatting="True">
                                </dw:WebDataWindowControl>
                        </asp:Panel>
                    </td>
                    <td rowspan="2" valign="top">
                        <asp:Panel ID="Panel4" runat="server" BorderStyle="Ridge" Height="100px">
                            <span class="style3">
                                <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_vc_vchead"
                                    LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                                    ClientScriptable="True" ClientEventClicked="Dw_mainClick" ClientEventButtonClicked="Dw_mainButtonclick">
                                </dw:WebDataWindowControl>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        <span class="style3">
   <%--                         <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Height="30px" Width="100px">
                                <dw:WebDataWindowControl ID="Dw_find" runat="server" DataWindowObject="d_vcno_find"
                                    LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventItemChanged="OnDwfindItemChange"
                                    ClientFormatting="True" ClientScriptable="True">
                                </dw:WebDataWindowControl>
                            </asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                    <td class="style6" valign="top">
                        รหัสบัญชี
                    </td>
                    <td>
                        <b>รายละเอียด</b><span class="linkSpan" onclick="Insert_ucf_accsection()" style="font-family: Tahoma;
                            font-size: small; float: right; color: #0000CC;">เพิ่มแถว</span>
                    </td>
                    </td>
                </tr>
                <tr>
                    <td class="style5" valign="top">
                        <span class="style3">
                            <asp:Panel ID="Panel2" runat="server" Height="300px" ScrollBars="Vertical" BorderStyle="Ridge">
                                <span class="style3">
                                    <dw:WebDataWindowControl ID="Dw_list" runat="server" DataWindowObject="d_vc_list_accid"
                                        LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                                        ClientEventClicked="OnDwListClick" Width="30px">
                                    </dw:WebDataWindowControl>
                            </asp:Panel>
                    </td>
                    <td>
                        <span class="style3">
                            <asp:Panel ID="Panel5" runat="server" BorderStyle="Ridge" Height="300px " ScrollBars="Vertical">
                                <span class="style3">
                                    <dw:WebDataWindowControl ID="Dw_detail" runat="server" DataWindowObject="d_acc_add_detail_accid"
                                        LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                                        ClientScriptable="True" ClientEventButtonClicked="Delete_ucf_accsection">
                                    </dw:WebDataWindowControl>
                            </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        <span class="style3">
                            <asp:Panel ID="Panel7" runat="server" Height="60px" Width="100px">
                                <span class="style3">
                      <%--              <table width="100%">
                                        <tr>
                                            <td align="left" style="font-size: 14px;">
                                                <span class="style4">เงินสดยกมา</span><span class="style3"> </span>
                                            </td>
                                            <td align="right" style="font-size: 14px;">
                                                <b>&nbsp;<span class="style3"><asp:Label ID="lbl_moneybg" runat="server" Text="0.00"
                                                    ForeColor="#006600"></asp:Label>
                                                </span></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="font-size: 14px;" width="100px">
                                                <span class="style4">เงินสดยกไป</span><span class="style3"> </span>
                                            </td>
                                            <td align="right" class="style4">
                                                <asp:Label ID="lbl_moneyfw" runat="server" Text="0.00" ForeColor="Maroon"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>--%>
                            </asp:Panel>
                    </td>
                    <td valign="top">
                        <span class="style3">
 <%--                           <asp:Panel ID="Panel6" runat="server" Height="30px">
                                <span class="style3">
                                    <dw:WebDataWindowControl ID="Dw_footer" runat="server" DataWindowObject="d_vc_vcedit_vcdetail_tail"
                                        LibraryList="~/DataWindow/account/vc_voucher_edit.pbl" AutoRestoreContext="False"
                                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                                        ClientScriptable="True">
                                    </dw:WebDataWindowControl>
                            </asp:Panel>--%>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HdRowListClick" runat="server" />
            <asp:HiddenField ID="Hdrow" runat="server" />
            <asp:HiddenField ID="HdVoucherNo" runat="server" Value="" />
            <asp:HiddenField ID="Hdvcdate" runat="server" />
            <asp:HiddenField ID="vcno" runat="server" />
            <asp:HiddenField ID="HdButton" runat="server" />
            <asp:HiddenField ID="HSqlTemp" runat="server" />
            <asp:HiddenField ID="HdVoucherDate" runat="server" Value="" />
            <asp:HiddenField ID="HdBranchId" runat="server" />
            <asp:HiddenField ID="HdOpenIFrame" runat="server" Value="False" />
            <asp:HiddenField ID="Hd_row" runat="server" Value="False" />
            <asp:HiddenField ID="Hd_row2" runat="server" Value="False" />
    </p>
</asp:Content>
