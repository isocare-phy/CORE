<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_loanreq.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_loanreq" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMemberNo%>
    <%=jsPostLoantypeCode%>
    <%=jsPostDel%>
    <%=jsPostContIntType%>
    <%=jsPostInsert%>
    <%=jsPostClearStatus%>
    <%=jsPostRefColl%>
    <%=jsPostCollAmt%>
    <%=jsPostCalPayMent%>
    <%=jsPostBack%>
    <%=jsPostLoanRequest%>
    <%=jsPostperiodinstallment%>
    <%=jsPostPeriodPayment%>
    <%=jsPostOpenlnreq%>
    <script type="text/JavaScript">
        function Validate() {
            var lnreq_status = Gcoop.GetEl("Hdlnreq_status").value;
            if (lnreq_status == "8") {
                return confirm("ยืนยันการบันทึกข้อมูล");
            }
            else {
                confirm("ไม่สามารถแก้ไขข้อมูลได้");
            }
        }
        function OnDwMainButtonClicked(sender, row, bName) {
            Gcoop.OpenIFrame("630", "450", "w_dlg_member_search_new.aspx", "");
        }
        function ReceiveMemberNo(member_no, memb_name) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMemberNo();
        }
        function PostMemberNo(member_no) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMemberNo();
        }
        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            jsPostMemberNo();
        }
        function OnDwDetailChange(s, r, c, v) {
            objDwDetail.SetItem(r, c, v);
            objDwDetail.AcceptText();
            if (c == "loantype_code") {
                jsPostLoantypeCode();
            }
            else if (c == "loanrequest_amt") {
                jsPostLoanRequest();
            }
            else if (c == "period_installment") {
                jsPostperiodinstallment();
            }
            else if (c == "period_payment") {
                jsPostPeriodPayment();
            }
            else if (c == "int_continttype") {
                Gcoop.GetEl("HdType").value = v;
            }
        }
        function DwCollInsertRow() {
            row = objDwColl.InsertRow(objDwColl.RowCount() + 1);
        }
        function DwClrothInsertRow() {
            row = objDwClroth.InsertRow(objDwClroth.RowCount() + 1);
        }
        //
        function OnDwIntSpcBClick(s, r, c) {
            if (c == "b_del") {
                if (confirm("ยืนยันการลบแถว")) {
                    Gcoop.GetEl("HdRow").value = r;
                    jsPostDel();
                }
            }
        }
        function DwIntSpcInsertRow() {
            jsPostInsert();
        }

        //DwClr
        function OnDwClrChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "clear_status") {
                Gcoop.GetEl("HdClrRow").value = r;
                jsPostClearStatus();
            }
        }
        //DwColl
        function OnDwCollChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            Gcoop.GetEl("HdCollRow").value = r;

            if (c == "loancolltype_code") {
                objDwColl.SetItem(r, "ref_collno", "");
                objDwColl.SetItem(r, "description", "");
            }
            else if (c == "ref_collno") {
                jsPostRefColl();
            }
        }
        function OnDwCollBClick(s, r, c) {
            if (c == "b_1") {
                colltype = s.GetItem(r, "loancolltype_code");
                member_no = objDwMain.GetItem(1, "member_no");
            }
        }
        function OnDwIntSpcChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "int_continttype") {
                jsPostBack();
            }
        }

        function MenubarOpen() {
            Gcoop.OpenIFrame("660", "350", "w_dlg_open_loanreq.aspx", "");
        }
        function GetLoanReq(loanrequest_docno, member_no, loanrequest_status) {
            Gcoop.GetEl("Hdlnreq_Docno").value = loanrequest_docno;
            Gcoop.GetEl("Hdlnreq_status").value = loanrequest_status + "";
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostOpenlnreq();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_reqloan_memdet"
        LibraryList="~/DataWindow/investment/ln_req_loan.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwMainChange"
        TabIndex="1" ClientEventButtonClicked="OnDwMainButtonClicked">
    </dw:WebDataWindowControl>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_lcsrv_loanreq"
        LibraryList="~/DataWindow/investment/ln_req_loan.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwDetailChange"
        ClientEventButtonClicked="OnDwDetailBClick" TabIndex="100">
    </dw:WebDataWindowControl>
    <br />
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                <asp:Label ID="Label1" runat="server" Text="รายการหักชำระหนี้" Font-Size="Small"
                    Font-Underline="True" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="DwClr" runat="server" DataWindowObject="d_lcwin_reqloan_clrln"
                    LibraryList="~/DataWindow/investment/ln_req_loan.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwClrChange"
                    TabIndex="400">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" Text="รายการหักชำระอื่นๆ" Font-Size="Small"
                    Font-Underline="True" Font-Bold="True"></asp:Label>
                <asp:Label ID="Label5" runat="server" Text="เพิ่มแถว" Font-Size="Smaller" Font-Underline="True"
                    onclick="DwClrothInsertRow();" ForeColor="Blue"></asp:Label>
                <dw:WebDataWindowControl ID="DwClroth" runat="server" DataWindowObject="d_lcwin_reqloan_clrother"
                    LibraryList="~/DataWindow/investment/ln_req_loan.pbl" ClientScriptable="True"
                    ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
                    ClientFormatting="True" AutoRestoreContext="False" TabIndex="700">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    <asp:Label ID="Label3" runat="server" Text="รายละเอียดหลักค้ำประกัน" Font-Size="Small"
        Font-Underline="True" Font-Bold="True"></asp:Label>
    <asp:Label ID="Label4" runat="server" Text="เพิ่มแถว" Font-Size="Smaller" Font-Underline="True"
        onclick="DwCollInsertRow();" ForeColor="Blue"></asp:Label>
    <dw:WebDataWindowControl ID="DwColl" runat="server" DataWindowObject="d_lcwin_reqloan_coll"
        LibraryList="~/DataWindow/investment/ln_req_loan.pbl" ClientScriptable="True"
        ClientEvents="true" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" AutoRestoreContext="False" ClientEventItemChanged="OnDwCollChange"
        TabIndex="1000" ClientEventButtonClicked="OnDwCollBClick">
    </dw:WebDataWindowControl>
    <br />
    <br />
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                <div align="left">
                    <asp:Label ID="Label7" runat="server" Text="เพิ่มแถว" Font-Size="Smaller" Font-Underline="True"
                        onclick="DwIntSpcInsertRow();" ForeColor="Blue"></asp:Label>
                </div>
                <dw:WebDataWindowControl ID="DwIntSpc" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientValidation="False" DataWindowObject="d_lcwin_reqloan_contintspc" LibraryList="~/DataWindow/investment/ln_req_loan.pbl"
                    ClientEventButtonClicked="OnDwIntSpcBClick" ClientEventItemChanged="OnDwIntSpcChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HdXml" runat="server" Value="" />
    <asp:HiddenField ID="HdType" runat="server" Value="" />
    <asp:HiddenField ID="HdRow" runat="server" Value="" />
    <asp:HiddenField ID="HdClrRow" runat="server" Value="" />
    <asp:HiddenField ID="HdCollRow" runat="server" Value="" />
    <asp:HiddenField ID="HdCollAmt" runat="server" Value="" />
    <asp:HiddenField ID="Hdlnreq_Docno" runat="server" Value="" />
    <asp:HiddenField ID="Hdlnreq_status" runat="server" Value="8" />
</asp:Content>
