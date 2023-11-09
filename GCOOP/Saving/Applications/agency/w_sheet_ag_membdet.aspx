<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_membdet.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_membdet" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
        .style3
        {
            color: #0000CC;
            font-size: small;
        }
    </style>
    <%=initJavaScript %>
    <%=postInitMembdet%>
    <%=postInitMembdet_detail%>
    <%=postNewClear%>
    <%=postSearchGroupCode%>
    <%=postSelectRow%>
    <script type="text/javascript">
        function OnDwSearchItemChange(s, r, c, v) {

            var choice = objDw_main.GetItem(1, "choice");
            var group = null;
            if (choice == null || choice == "") {
                alert("กรุณาเลือกเงื่อนไขการดึงข้อมูล");
            }
            else if (choice == "1") {
                objDw_search.SetItem(1, "membgroup_code", Gcoop.StringFormat(v, "0000"));
                objDw_search.AcceptText();
                group = objDw_search.GetItem(1, "membgroup_code");
                Gcoop.GetEl("HdGroup").value = group;
                if (objDw_search.GetItem(1, "membgroup_code") != null || objDw_search.GetItem(1, "membgroup_code") != "") {
                    postSearchGroupCode();
                }
            }
            else if (choice == "2") {
                objDw_search.SetItem(1, "agentgrp_code", Gcoop.StringFormat(v, "0000"));
                objDw_search.AcceptText();
                group = objDw_search.GetItem(1, "agentgrp_code");
                Gcoop.GetEl("HdGroup").value = group;
                if (objDw_search.GetItem(1, "agentgrp_code") != null || objDw_search.GetItem(1, "agentgrp_code") != "") {
                    postSearchGroupCode();
                }

            }
        }

        function OnDwDetailButtonClick(s, r, b) {
            if (b == "b_picture") {
                var agentrequest_no = null;
                var seq_no = null;
                agentrequest_no = objDw_detail.GetItem(r, "agentrequest_no");
                seq_no = objDw_detail.GetItem(r, "seq_no");
                if (agentrequest_no == null) {
                    alert("ไม่พบข้อมูลลำดับการคีย์");
                } else if (seq_no == null) {
                    alert("ไม่พบข้อมูลลำดับตัวแทน")
                } else {
                    Gcoop.OpenIFrame(640, 505, "w_dlg_ag_membdet_picture_popup.aspx", "?agentrequest_no=" + agentrequest_no + "&seq_no=" + seq_no);
                }
            }
        }
        function OnDwMainButtonClick(s, r, b) {
            var choice = null;
            if (b == "b_search") {
                choice = objDw_main.GetItem(1, "choice");
                if (choice == null || choice == "") {
                    alert("กรุณาเลือกเงื่อนไขการดึงข้อมูลก่อน");
                }
                else {
                    postInitMembdet();
                }
            }

        }
        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }

        function Validate() {

        }

        function OnDwListClick(s, r, c) {
            var choice = objDw_main.GetItem(1, "choice");
            var group = null;

            if (choice == null || choice == "") {
                alert("กรุณาเลือกเงื่อนไขการดึงข้อมูล");
            }
            else if (choice == "1") {
                group = objDw_list.GetItem(r, "membgroup_code");
                Gcoop.GetEl("HdGroup").value = group;
                Gcoop.GetEl("HdRow").value = r + "";
                postSelectRow();
               // postInitMembdet_detail();
            }
            else if (choice == "2") {
                group = objDw_list.GetItem(r, "agentgrp_code");
                Gcoop.GetEl("HdGroup").value = group;
                postInitMembdet_detail();
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td class="style1" colspan="2">
                เงื่อนไขการดึงข้อมูลรายละเอียดลูกหนี้ตัวแทน
            </td>
        </tr>
        <tr>
            <td align="center" align="left">
                <dw:WebDataWindowControl ID="Dw_main" runat="server" DataWindowObject="d_agentsrv_membdet_main"
                    LibraryList="~/DataWindow/agency/egat_ag_membdet.pbl" Width="400px" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventButtonClicked="OnDwMainButtonClick">
                </dw:WebDataWindowControl>
            </td>
            <td align="center">
                <span class="style3">จำนวนสมาชิก</span>&nbsp;&nbsp;
                <asp:Label ID="lbl_count" runat="server" Font-Bold="True" ForeColor="Red" Text="0"></asp:Label>
                &nbsp;&nbsp; <span class="style3">คน</span>
            </td>
        </tr>
        <tr>
            <td class="style1">
                &nbsp;
            </td>
            <td class="style1">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style1">
                ค้นหาข้อมูลสังกัด
            </td>
            <td class="style1">
                รายละเอียดลูกหนี้ตัวแทน
            </td>
        </tr>
        <tr>
            <td class="style1" valign="top">
                <asp:Panel ID="Panel3" runat="server">
                    <dw:WebDataWindowControl ID="Dw_search" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        ClientValidation="False" DataWindowObject="d_agentsrv_search_grp" LibraryList="~/DataWindow/agency/egat_ag_membdet.pbl"
                        ClientEventItemChanged="OnDwSearchItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
            <td class="style1" valign="top" rowspan="2">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Width="400px" Height="335px">
                    <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" DataWindowObject="d_agentsrv_membdet_detail" LibraryList="~/DataWindow/agency/egat_ag_membdet.pbl"
                        ClientValidation="False" ClientEventButtonClicked="OnDwDetailButtonClick">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1" valign="top">
                <asp:Panel ID="Panel2" runat="server" Height="275px">
                    ช่วงข้อมูลสังกัด
                    <dw:WebDataWindowControl ID="Dw_list" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_agentsrv_membdetgrp_list"
                        LibraryList="~/DataWindow/agency/egat_ag_membdet.pbl" ClientValidation="False"
                        ClientEventClicked="OnDwListClick" RowsPerPage="8" Height="255px">
                        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:HiddenField ID="HdGroup" runat="server" />
                <asp:HiddenField ID="HdRow" runat="server" />
            </td>
            <td valign="top">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
