<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_req_contadj.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_req_contadj" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostMembNo%>
    <%=jsPostLoancontractNoChange%>
    <%=jsPostBack%>
    <%=jsPostColl%>
    <%=jsPostCollAmt%>
    <script type="text/JavaScript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function MenubarOpen() {
            Gcoop.OpenIFrame("630", "350", "w_dlg_member_search_new.aspx", "");
        }
        function OnDwMainBclick(s, r, c) {
            if (c == "b_1") {
                Gcoop.OpenIFrame("630", "350", "w_dlg_member_search_new.aspx", "");
            }
        }
        function ReceiveMemberNo(member_no, memb_name) {
            objDwMain.SetItem(1, "member_no", member_no);
            objDwMain.AcceptText();
            jsPostMembNo();
        }
        function OnDwDetailBClick(s, r, c) {
            if (c == "b_contract") {
                Gcoop.OpenIFrame("600", "590", "w_dlg_member_search_loancontractno.aspx", "")
            }
        }
        function PostLoancontractNo(member_no, loancontract_no) {
            objDwDetail.SetItem(1, "loancontract_no", loancontract_no);
            objDwDetail.AcceptText();
            jsPostLoancontractNoChange();
        }
        //---------------------
        function OnDwMainChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "member_no") {
                jsPostMembNo();
            }
        }
        function OnDwDetailChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "loancontract_no") {
                jsPostLoancontractNoChange();
            }
        }
        function OnDwSpcInsertRow() {
            row = objDwSpc.InsertRow(objDwSpc.RowCount() + 1);
        }
        function OnDwCollInsertRow() {
            row = objDwColl.InsertRow(objDwColl.RowCount() + 1);
        }
        function OnDwSpcBclick(s, r, c) {
            if (c == "b_del") {
                objDwSpc.DeleteRow(r);
            }
        }
        function OnDwIntSpcChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "int_continttype") {
                jsPostBack();
            }
        }
        function OnDwIntChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "int_continttype") {
                jsPostBack();
            }
        }
        function OnDwCollBClick(s, r, c) {
            if (c == "b_del") {
                objDwColl.DeleteRow(r);
            }
            if (c == "b_search") {
                colltype = s.GetItem(r, "loancolltype_code");
                member_no = objDwDetail.GetItem(1, "member_no");
                if (colltype == "02") {
                    Gcoop.OpenIFrame("550", "350", "w_dlg_loan_dept.aspx", "?member_no=" + member_no + "&row=" + r);
                }
                else if (colltype == "03") {
                    Gcoop.OpenIFrame("550", "350", "w_dlg_loan_dept2.aspx", "?member_no=" + member_no + "&row=" + r);
                }
                else if (colltype == "04") {
                    Gcoop.OpenIFrame("550", "350", "w_dlg_loan_share.aspx", "?member_no=" + member_no + "&row=" + r);
                }
            }
        }
        function ReciveDept(deptaccount_no, deptaccount_name, prncbal, row) {
            objDwColl.SetItem(row, "ref_collno", deptaccount_no);
            objDwColl.SetItem(row, "description", "ผู้ถือตั๋วสัญญา:" + deptaccount_name + " - ยอด:");
            Gcoop.GetEl("HdCollRow").value = row;
            Gcoop.GetEl("HdCollAmt").value = prncbal;
            jsPostCollAmt();
        }
        function ReciveDept2(deptaccount_no, deptaccount_name, prncbal, row) {
            objDwColl.SetItem(row, "ref_collno", deptaccount_no);
            objDwColl.SetItem(row, "description", "ชื่อบัญชี:" + deptaccount_name + " - ยอด:");
            Gcoop.GetEl("HdCollRow").value = row;
            Gcoop.GetEl("HdCollAmt").value = prncbal;
            jsPostCollAmt();
        }
        function ReciveShare(sharecert_no, shareno_start, shareno_end, share_amt, row) {
            objDwColl.SetItem(row, "ref_collno", sharecert_no);
            objDwColl.SetItem(row, "description", "หมายเลขหุ้นตั้งแต่:" + shareno_start + " ถึง:" + shareno_end + " จำนวน:");
            Gcoop.GetEl("HdCollRow").value = row;
            Gcoop.GetEl("HdCollAmt").value = share_amt;
            jsPostCollAmt();
        }
        function OnDwCollChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "loancolltype_code") {
                Gcoop.GetEl("HdCollRow").value = r;
                jsPostColl();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%; border: 2px; border-color: Black;">
<%--        <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_lcwin_memdet"
                    LibraryList="~/DataWindow/loan/loan.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" ClientEventItemChanged="OnDwMainChange" TabIndex="1"
                    ClientEventButtonClicked="OnDwMainBclick">
                </dw:WebDataWindowControl>
                <hr />
            </td>
        </tr>
        <tr>--%>
            <td colspan="2">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_lcsrv_req_contadj"
                    LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" TabIndex="200" ClientEventItemChanged="OnDwDetailChange" ClientEventButtonClicked="OnDwDetailBClick">
                </dw:WebDataWindowControl>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="DwInt" runat="server" DataWindowObject="d_lcsrv_req_contadj_contint"
                    LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" TabIndex="300" ClientEventItemChanged="OnDwIntChange">
                </dw:WebDataWindowControl>
                <dw:WebDataWindowControl ID="DwPreriod" runat="server" DataWindowObject="d_lcsrv_req_contadj_payment"
                    LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" TabIndex="400">
                </dw:WebDataWindowControl>
            </td>
            <td align="left" valign="top">
                <div>
                    <span onclick="OnDwSpcInsertRow()" style="cursor: pointer;">
                        <asp:Label ID="Label1" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                            Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
                    <dw:WebDataWindowControl ID="DwSpc" runat="server" DataWindowObject="d_lcsrv_req_contadj_contintspc"
                        LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                        AutoRestoreContext="False" TabIndex="500" ClientEventButtonClicked="OnDwSpcBclick"
                        ClientEventItemChanged="OnDwIntSpcChange">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
                <span onclick="OnDwCollInsertRow()" style="cursor: pointer;">
                    <asp:Label ID="Label6" runat="server" Text="เพิ่มแถว" Font-Bold="False" Font-Names="Tahoma"
                        Font-Size="14px" Font-Underline="True" ForeColor="Blue" /></span>
                <dw:WebDataWindowControl ID="DwColl" runat="server" DataWindowObject="d_lcsrv_req_contadj_collateral"
                    LibraryList="~/DataWindow/investment/loan.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" TabIndex="600" ClientEventButtonClicked="OnDwCollBClick"
                    ClientEventItemChanged="OnDwCollChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    <asp:HiddenField ID="HdCollRow" runat="server" Value="" />
    <asp:HiddenField ID="HdCollAmt" runat="server" Value="" />
</asp:Content>
