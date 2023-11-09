<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_regloan_year.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_regloan_year" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsPostMember %>
    <%=jsLoanreq_amt%>
    <%=jsLoanreq_dif%>
    <%=jsRefresh %>
    <%=jsSetloanreqAmt %>
    <%=jsPostMemberColl %>
    <%=openNew %>
    <%=setloanreq_year %>
    <%=getLoanrequest%>
    <%=setcollname%>
    <script type="text/javascript">
        function Validate() {
            var sharestk_value = Gcoop.ParseFloat(objdw_main.GetItem(1, "sharestk_value"));
            var loanreq_amt = Gcoop.ParseFloat(objdw_main.GetItem(1, "loanreq_amt"));
            var reqgrt_memno = objdw_main.GetItem(1, "reqgrt_memno");
            //  var loancontract_running = objdw_main.GetItem(1, "loancontract_running");
            //  loancontract_running = loancontract_running == null || loancontract_running == undefined ? "" : Gcoop.Trim(loancontract_running);
            reqgrt_memno = reqgrt_memno == null || reqgrt_memno == undefined ? "" : Gcoop.Trim(reqgrt_memno);
            if (sharestk_value < loanreq_amt && reqgrt_memno == "") {
                alert("ยอดขอกู้ มากกว่าทุนเรือนหุ้น  กรุณาใส่คนค้ำ");
                return false;
            }
            //           else if (loancontract_running == "") {
            //                alert("กรุณาใส่เลขที่สัญญา");
            //                return false;
            //            }
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarNew() {

            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
            openNew();
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('580', '590', 'w_dlg_regloan_year.aspx', '');

        }
        function GetValueLoanRequest(docNo) {

            objdw_main.SetItem(1, "loanrequest_docno", docNo);
            Gcoop.GetEl("Hloanrequest_docno").value = docNo;
            getLoanrequest();
        }
        function ItemDataWindowChange(s, r, c, v) {
            if (c == "member_no") {
                objdw_main.SetItem(r, "member_no", Gcoop.StringFormat(v, "000000"));
                Gcoop.GetEl("HdMemberNo").value = objdw_main.GetItem(r, "member_no");
                objdw_main.AcceptText();
                jsPostMember();
                return 0;
            }
            else if (c == "loancontract_running") {
                objdw_main.SetItem(r, "loancontract_running", v);
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month1_amt") {
                objdw_main.SetItem(r, "month1_amt", v);
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "zmonth2_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month1_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month2_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            } else if (c == "month3_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month2_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month3_amt", v);
                // }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month4_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month3_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month4_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month5_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month4_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month5_amt", v);
                // }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month6_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month5_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month6_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month7_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month6_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month7_amt", v);
                //   }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month8_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month7_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month8_amt", v);
                // }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month9_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month8_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month9_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month10_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month9_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month10_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month11_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month10_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month11_amt", v);
                // }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "month12_amt") {
                //                var month_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "month11_amt"));
                //                if (v <= month_amt) {
                objdw_main.SetItem(r, "month12_amt", v);
                //  }
                objdw_main.AcceptText();
                setloanreq_year();
                return 0;
            }
            else if (c == "loanreq_year") {
                objdw_main.SetItem(r, "loanreq_year", v);
                objdw_main.AcceptText();
                setloanreq_year();

            }
            else if (c == "reqgrt_memno") {
                //คนค้ำประกัน
                objdw_main.SetItem(r, "reqgrt_memno", Gcoop.StringFormat(v, "000000"));
                Gcoop.GetEl("Hdreqgrt_memno").value = objdw_main.GetItem(r, "reqgrt_memno");
                objdw_main.AcceptText();
                jsPostMemberColl();
            }
            else if (c == "loanreq_amt") {
                //  alert(c);
                var loancredit_amt = 0;
                var loanreq_amt = Gcoop.ParseFloat(v);
                var sharestk_value = 0;
                loancredit_amt = Gcoop.ParseFloat(objdw_main.GetItem(r, "loancredit_amt"));

                if (loancredit_amt < loanreq_amt) {
                    alert("ยอดขอกู้ เกินสิทธิการกู้ ");
                    objdw_main.SetItem(r, "loanreq_amt", loancredit_amt);
                    objdw_main.AcceptText();
                    Gcoop.GetEl("HFloanreq_amt").value = loancredit_amt;
                    jsLoanreq_amt();
                } else {

                    Gcoop.GetEl("HFloanreq_amt").value = loanreq_amt;
                    objdw_main.SetItem(r, "loanreq_amt", loanreq_amt);
                    objdw_main.AcceptText();
                    jsLoanreq_amt();
                    //  jsSetloanreqAmt();
                }
                return 0;
            }

            else if (c == "loanreq_dif") {
                //เพิ่ม/ลด
                // alert(c);
                objdw_main.SetItem(r, "loanreq_dif", v);
                objdw_main.AcceptText();
                var loanreq_type = objdw_main.GetItem(r, "loanreq_type");
                if (loanreq_type == 2) {
                    jsSetloanreqAmt();
                }
                //  jsLoanreq_dif();
                //  jsSetloanreqAmt();
                return 0;
            }
            else if (c == "loanreq_type") {
                //ประเภท
                // alert(c);
                objdw_main.SetItem(r, "loanreq_type", v);
                objdw_main.AcceptText();
                if (v == 1) {
                    objdw_main.SetItem(r, "loanreq_dif", 0);
                    objdw_main.AcceptText();
                    jsSetloanreqAmt();
                }
                else if (v == 2) {
                    jsSetloanreqAmt();
                }
                else if (v == 4) {
                    objdw_main.SetItem(r, "loanreq_dif", 0);
                    objdw_main.AcceptText();
                    jsSetloanreqAmt();
                }

                return 0;
            }
            else if (c == "loanreq_mthstart") {
                // alert(c);
                objdw_main.SetItem(r, "loanreq_mthstart", v);
                objdw_main.AcceptText();
                jsSetloanreqAmt();
                return 0;
            }

            if (Gcoop.GetEl("HdIsPostBack").value == "false") {
                SheetLoadComplete();
            }

        }
        function MainError(s, r, c, v) {
            return 1;
        }
        function OnDwMainClick(s, r, c) {

            if (c == "b_searchmem") {
                Gcoop.OpenDlg('630', '650', 'w_dlg_sl_member_search.aspx', '');
            }
            else if (c == "b_searchcoll") {
                Gcoop.OpenDlg('630', '650', 'w_dlg_sl_member_search_regloan.aspx', '');
            }

        }

        function GetValueMbColl(memberno_coll, membname_coll) {

            objdw_main.SetItem(1, "reqgrt_memno", memberno_coll);
            Gcoop.GetEl("Hdreqgrt_memno").value = memberno_coll;
            objdw_main.AcceptText();
            jsPostMemberColl();
            // jsPostMember();
        }
        function GetValueFromDlg(memberno) {

            objdw_main.SetItem(1, "member_no", memberno);
            objdw_main.AcceptText();
            Gcoop.GetEl("HdMemberNo").value = memberno;
            jsPostMember();
        }

        function SheetLoadComplete() {
            var msgtext = Gcoop.GetEl("Hmsgtext").value;
            var as_errtext = Gcoop.GetEl("Has_errtext").value;
            var collname = Gcoop.GetEl("HdMsg").value;
            if (as_errtext == "-1") {
                // alert("e");
                if (confirm('\n' + msgtext)) {

                    //setcollname();
                    objdw_main.SetItem(1, "coll_name", collname);
                    objdw_main.AcceptText();
                    Gcoop.GetEl("Has_errtext").value = "";
                }
            }
            else if (Gcoop.GetEl("HdIsPostBack").value != "true") {
                Gcoop.GetEl("HdIsPostBack").value = "true";
                Gcoop.SetLastFocus("member_no_0");
//                alert(Gcoop.GetEl("HdMemberNo").value);
                Gcoop.Focus();
                objdw_main.SetItem(1, "member_no", Gcoop.GetEl("HdMemberNo").value);


            }

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td valign="top" colspan="2">
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanyear_main"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemDataWindowChange" ClientEventClicked="OnDwMainClick"
                    ClientEventItemError="MainError">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr id="trOtherClr">
            <td valign="top" colspan="2">
                <dw:WebDataWindowControl ID="dw_oldreq" runat="server" DataWindowObject="d_sl_loanyear_oldreq"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <dw:WebDataWindowControl ID="dw_message" runat="server" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
        DataWindowObject="d_ln_message" LibraryList="~/DataWindow/shrlon/sl_loan_requestment.pbl">
    </dw:WebDataWindowControl>
    <asp:Literal ID="LtXmlKeeping" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlReqloop" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlLoanDetail" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlOtherlr" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtRunningNo" runat="server" Visible="False"></asp:Literal>
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HFloanreq_amt" runat="server" />
    <asp:HiddenField ID="Hdreqgrt_memno" runat="server" />
    <asp:HiddenField ID="Hloanrequest_docno" runat="server" />
    <asp:HiddenField ID="Has_errtext" runat="server" />
    <asp:HiddenField ID="HdReturn" runat="server" />
    <asp:HiddenField ID="Hmsgtext" runat="server" />
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:HiddenField ID="HdMsg" runat="server" />
</asp:Content>
