<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_ag_receivegroupmem.aspx.cs"
    Inherits="Saving.Applications.agency.w_sheet_ag_receivegroupmem" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-size: small;
            font-weight: bold;
        }
        .style2
        {
        }
    </style>
    <%=initJavaScript%>
    <%=postInitReceiveGroupMem%>
    <%=postSetBranch%>
    <%=postNewClear%>
    <%=postFilterMembCode %>
    <%=postRefresh %>
    <%=PostsearchMembNo %>


    <script type="text/javascript">
        //    function SetBranch(BranchID)
        //    {
        //        Gcoop.GetEl("HdBranchId").value = BranchID;
        //        postSetBranch();
        //    }
        function CheckAll(obj) {
            var theForm = obj.form;
            var i;
            if (obj.checked) {
                for (i = 1; i <= objDw_detail.RowCount(); i++) {
                    objDw_detail.SetItem(i, "operate_flag", 1);
                    var receiveAmt = objDw_detail.GetItem(i, "receive_amt");
                    var recv_amt = objDw_detail.GetItem(i, "recv_amt");
                    var addrecv_amt = objDw_detail.GetItem(i, "addrecv_amt");
                    var ret_all_amt = objDw_detail.GetItem(i, "ret_all_amt");
                    var adj_all_amt = objDw_detail.GetItem(i, "adj_all_amt");
                    var cancel_all_amt = objDw_detail.GetItem(i, "cancel_all_amt");
                    objDw_detail.SetItem(i, "payment_amt", receiveAmt - (recv_amt + addrecv_amt + ret_all_amt + adj_all_amt + cancel_all_amt));
                }
            }
            else if (!obj.checked) {
                for (i = 1; i <= objDw_detail.RowCount(); i++) {
                    objDw_detail.SetItem(i, "operate_flag", 0);
                    objDw_detail.SetItem(i, "payment_amt", 0);
                }
            }
        }
        
        function searchMembNo() {
            PostsearchMembNo();
        }

        function OnDwDetailClick(s, r, c) {
            if (c == "operate_flag") {
                Gcoop.CheckDw(s, r, c, "operate_flag", 1, 0);
                if (objDw_detail.GetItem(r, "operate_flag") == 1) {
                    var receiveAmt = objDw_detail.GetItem(r, "receive_amt");
                    var recv_amt = objDw_detail.GetItem(r, "recv_amt");
                    var addrecv_amt = objDw_detail.GetItem(r, "addrecv_amt");
                    var ret_all_amt = objDw_detail.GetItem(r, "ret_all_amt");
                    var adj_all_amt = objDw_detail.GetItem(r, "adj_all_amt");
                    var cancel_all_amt = objDw_detail.GetItem(r, "cancel_all_amt");
                    objDw_detail.SetItem(r, "payment_amt", receiveAmt - (recv_amt + addrecv_amt + ret_all_amt + adj_all_amt + cancel_all_amt));
                }
                else {
                    objDw_detail.SetItem(r, "payment_amt", 0);
                }
            }
            return 0;
        }

        function OnDwMainButtonClick(s, r, c) {
            if (c == "b_search") {
                var recv_tday = objDw_main.GetItem(1, "recv_tday");
                var recv_period = objDw_main.GetItem(1, "recv_period");
                var agentgrp_code = objDw_main.GetItem(1, "agentgrp_code");
                var membgrp_code_begin = objDw_main.GetItem(1, "membgrp_code_begin");
                var membgrp_code_end = objDw_main.GetItem(1, "membgrp_code_end");
                var expense_code = objDw_main.GetItem(1, "expense_code");
                var tofromacc_id = objDw_main.GetItem(1, "tofromacc_id");
                if (recv_tday == null || recv_tday == "") {
                    alert("กรุณากรอกวันที่โอน");
                }
                if (recv_period == null || recv_period == "") {
                    alert("กรุณาเลือกงวด");
                }
                if ((agentgrp_code == null || agentgrp_code == "") && ((membgrp_code_begin == null || membgrp_code_begin == "") || (membgrp_code_end == null || membgrp_code_end == ""))) {
                    alert("กรุณาเลือกสังกัดตัวแทนหรือสมาชิก");
                }

                if (recv_tday != null && recv_period != null && expense_code != null && tofromacc_id != null) {
                    if (agentgrp_code == null && membgrp_code_begin != null && membgrp_code_end != null) {
                        postInitReceiveGroupMem();
                    }
                    else if (agentgrp_code != null && membgrp_code_begin == null && membgrp_code_end == null) {
                        postInitReceiveGroupMem();
                    }
                    else if (agentgrp_code != null && membgrp_code_begin != null && membgrp_code_end != null) {
                        postInitReceiveGroupMem();
                    }
                }
            }
            return 0;
        }

        function OnDwMainItemChange(s, r, c, v) {
            objDw_main.SetItem(1, c, v);
            if (c == "recv_tday") {
                objDw_main.SetItem(1, "recv_tday", v);
                objDw_main.AcceptText();
                objDw_main.SetItem(1, "recv_day", Gcoop.ToEngDate(v));
                objDw_main.AcceptText();
            }
            else if (c == "expense_bank") {
                objDw_main.SetItem(1, "expense_bank", v);
                objDw_main.AcceptText();
                postSetBranch();
            }
            else if (c == "expense_branch") {
                objDw_main.SetItem(1, "expense_branch", v);
                objDw_main.AcceptText();
            }
            else if (c == "agentgrp_code") {
                objDw_main.SetItem(1, "agentgrp_code", v);
                objDw_main.AcceptText();
                var agentgrp_code = objDw_main.GetItem(1, "agentgrp_code");
                Gcoop.GetEl("HdAgentCode").value = agentgrp_code;
                postFilterMembCode();
            }
            else if (c == "agentgrp_code_1") {
                objDw_main.AcceptText();
                postRefresh();
            }
            else if (c == "membgrp_code_begin") {
                objDw_main.AcceptText();
                postRefresh();
            }
            else if (c == "membgrp_code_end") {
                objDw_main.AcceptText();
                postRefresh();
            }
            return 0;
        }

        function OnDwDetailItemChange(s, r, c, v) {
            if (c == "recv_amt") {
                objDw_detail.SetItem(r, "recv_amt", v);
                objDw_detail.AcceptText();
            }
            return 0;
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }

        function Validate() {
            objDw_main.AcceptText();
            objDw_detail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function SaveComplete() {
            postNewClear();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdSaveRecGroupMem").value == "true") {
                Gcoop.OpenProgressBar("โอนเงินชำระลูกหนี้ตัวแทนสังกัด(ทะเบียน)", true, false, SaveComplete);
            }
        }
    
   
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style1">
                เงื่อนไขการดึงข้อมูล
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Panel ID="Panel1" runat="server" Width="550px">
                    <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                        AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                        DataWindowObject="d_agentsrv_initreceivegroup_mainmem" LibraryList="~/DataWindow/agency/agent.pbl"
                        ClientEventButtonClicked="OnDwMainButtonClick" ClientEventItemChanged="OnDwMainItemChange">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td class="style1">
                รายการทะเบียนลูกหนี้ตัวแทน
            </td>
        </tr>
        <tr>
            <td class="style1">
                <input type="checkbox" name="checkall" onclick="CheckAll(this)" />
                <asp:Label ID="Label1" runat="server" Text="เลือกทั้งหมด"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ค้นหาทะเบียน
                <asp:TextBox ID="txt_memberno" runat="server"></asp:TextBox>
                &nbsp;
                <asp:Button ID="B_search" runat="server" OnClick="B_search_Click" Text="ค้นหา" UseSubmitBehavior="False" OnClientClick="searchMembNo()" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Width="720px">
                    <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        ClientScriptable="True" DataWindowObject="d_agentsrv_initreceivegroup_memdetail"
                        LibraryList="~/DataWindow/agency/agent.pbl" ClientEventClicked="OnDwDetailClick"
                        ClientEventItemChanged="OnDwDetailItemChange" RowsPerPage="12">
                        <PageNavigationBarSettings NavigatorType="NumericWithQuickGo" Visible="True">
                        </PageNavigationBarSettings>
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="HdCurrentrow" runat="server" />
                <asp:HiddenField ID="HdSaveRecGroupMem" runat="server" />
                <asp:HiddenField ID="HdBranchId" runat="server" />
                <asp:HiddenField ID="HdAgentCode" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
