<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_public_funds_edit.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_public_funds_edit" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeHost %>
    <%=postChangeAge%>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain %>
    <%=postRetrieveBankBranch %>
    <%=postGetMemberDetail %>
    <%=postCopyAddress%>
    <%=postMemProvince%>
    <%=postMemDistrict%>
    <%=postMemTambol%>
    <%=postMainProvince%>
    <%=postMainDistrict%>
    <%=postMainTambol%>
    <%=postGetMoney%>
    <%=postPopupReport%>
    <%=postDepptacount%>
    <%=postToFromAccid%>
    <script type="text/javascript">

        function Validate() {
            var child_age = Gcoop.GetEl("checkage").value;

            if (child_age <= 4.00) {
                return confirm("ยืนยันการบันทึกข้อมูล");
            } else {
                alert("ไม่สามารถบันทึกได้เนื่องจากยื่นใบคำขอ นานเกินระยะเวลา 120 วัน");
            }
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(640, 600, "w_dlg_as_public_funds.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function DwDetailClick(sender, rowNumber, objectName) {
            if (objectName == "home_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "home_flag", 1, 0);
                postChangeHost();
            }
        }

        //        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code, memb_addr, addr_group, soi, mooban, road, tambol, district_code, province_code, postcode) {
        //            objDwMem.SetItem(1, "member_no", memberno);
        //            objDwMem.SetItem(1, "addr_no", memb_addr);
        //            Gcoop.GetEl("HfMemberNo").value = memberno;
        //            postRetreiveDwMem();
        //        }
        function GetValueFromDlg(memberno) {
            objDwMem.SetItem(1, "member_no", memberno);
            //            objDwMem.SetItem(1, "memb_addr", memb_addr);
            Gcoop.GetEl("HfMemberNo").value = memberno;
            postRetreiveDwMem();
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            //objDwMem.SetItem(1, "member_no", memberno);
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;
            postRetrieveDwMain();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "tambol_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainTambol();
            } else if (columnName == "amphur_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainDistrict()
            } else if (columnName == "province_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMainProvince();
            }
            //            else if (columnName == "status_was") {
            //                sender.SetItem(rowNumber, columnName, newValue);
            //                sender.AcceptText();
            //                postGetMoney();
            //            }
            else if (columnName == "req_tdate") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                postChangeAge();
            } else if (columnName == "approve_tdate") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                postChangeAge();
            }
            else if (columnName == "moneytype_code") {
                objDwMain.SetItem(rowNumber, columnName, newValue);
                objDwMain.AcceptText();
                //if (newValue == "05") {
                //postDepptacount();
                //}
                postToFromAccid();

            }
            return 0;
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            if (columnName == "member_no") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                Gcoop.GetEl("HfFocus").value = "1";
                postGetMemberDetail();
            } else if (columnName == "tambol_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMemTambol();
            } else if (columnName == "amphur_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMemDistrict()
            } else if (columnName == "province_code") {
                sender.SetItem(rowNumber, columnName, newValue);
                sender.AcceptText();
                postMemProvince();
            }
            return 0;
        }
        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }
        function MenubarNew() {
            window.location = "";
            return 0;
        }
        function OnDwMainButtonClicked(s, r, c) {
            if (c == "b_copy") {
                postCopyAddress();
            }
            else if (c == "b_dept_search") {
                var member_no = objDwMem.GetItem(1, "member_no");
                Gcoop.OpenIFrame(580, 500, "w_dlg_as_deptaccount_no.aspx", "?member_no=" + member_no);
            }
        }
        function GetDeptAccountNo(deptaccount_no) {
            Gcoop.GetEl("Hfdeptaccount_no").value = deptaccount_no;
            postDepptacount();
        }
        function OnClickLinkNext() {
            postPopupReport();
        }
        function SheetLoadComplete() {
            var next = Gcoop.GetEl("HfFocus").value;
            if (next == "1") {
                Gcoop.SetLastFocus("addr_no_0");
                Gcoop.Focus();
            }
        }

        $(function () {
            $('input[name="approve_tdate_0"]').click(function () {
                $('input[name="approve_tdate_0"]').val("")
            })

            $('input[name="req_tdate_0"]').click(function () {
                $('input[name="req_tdate_0"]').val("")
            })
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
    <%--<table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer" onclick="OnClickLinkNext();">พิมพ์ใบสำคัญจ่าย
                    </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>--%>
    <div>
        <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_membmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange"
            TabIndex="100">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <div>
        <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_public_funds.pbl"
            DataWindowObject="d_public_funds_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
            AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked="OnDwMainButtonClicked"
            ClientFormatting="True" ClientEventItemChanged="DwMainItemChange" TabIndex="250">
        </dw:WebDataWindowControl>
    </div>
    <br />
    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HdDuplicate" runat="server" />
    <asp:HiddenField ID="HdNameDuplicate" runat="server" />
    <asp:HiddenField ID="HdActionStatus" runat="server" />
    <asp:HiddenField ID="HdOpenIFrame" runat="server" />
    <asp:HiddenField ID="Hdposttovc_flag" runat="server" />
    <asp:HiddenField ID="Hfdeptaccount_no" runat="server" />
    <asp:HiddenField ID="HfFocus" runat="server" />
    <asp:HiddenField ID="checkage" runat="server" />
</asp:Content>
