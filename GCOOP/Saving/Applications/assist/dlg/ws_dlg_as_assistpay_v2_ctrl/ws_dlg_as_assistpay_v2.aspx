<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="ws_dlg_as_assistpay_v2.aspx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl.ws_dlg_as_assistpay_v2" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsPayto.ascx" TagName="DsPayto" TagPrefix="uc2" %>
<%@ Register Src="DsLoan.ascx" TagName="DsLoan" TagPrefix="uc3" %>
<%@ Register src="DsBank.ascx" tagname="DsBank" tagprefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsPayto = new DataSourceTool;
        var dsLoan = new DataSourceTool;
        var dsBank = new DataSourceTool;
        var ChkRow;
        function Validate() {
        }
        function chkNumber(ele) {
            var vchar = String.fromCharCode(event.keyCode);
            if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
            ele.onKeyPress = vchar;
        }
        function OnDsMainClicked(s, r, c) {
        }
        function OnDsLoanClicked(s, r, c, v) {
            if (c == "b_delloan") {
                dsLoan.SetRowFocus(r);
                if (confirm("ลบรายการหัก แถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRowLoan();
                }
            }
        }
        function OnDsPaytoClicked(s, r, c, v) {
           
           if (c == "b_delpayto") {
                dsPayto.SetRowFocus(r);
                if (confirm("ลบรายการจ่าย แถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRowPayto();
                }
            } else if (c == "b_searchbank") {
                var memno = dsMain.GetItem(0, "member_no");
                var moneytype_code = dsPayto.GetItem(r, "moneytype_code");
                var expense_bank = dsPayto.GetItem(r, "expense_bank");
                var expense_branch = dsPayto.GetItem(r, "expense_branch"); 
                var expense_accid = dsPayto.GetItem(r, "expense_accid");
                var name =dsPayto.GetItem(r, "gain_name");
                var surname = dsPayto.GetItem(r, "gain_surname");
                
                var prename = dsPayto.GetItem(r, "prename_desc");
                ChkRow = r;
                Gcoop.OpenDlg2("500", "350", "w_dlg_ass_deptaccountno_search_v2.aspx", "?memno=" + memno + "|" + moneytype_code + "|" + r + "|" + expense_bank + "|" + expense_branch + "|" + expense_accid+"|" + name + "|" + surname + "|" + prename);
            }
        }

        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "assist_amt") {
                var approveamt = dsMain.GetItem(0, "approve_amt");
                var assistamt = dsMain.GetItem(0, "assist_amt");
                if (assistamt > approveamt) {
                    alert('ยอดอนุมัติมากกว่ายอดจ่าย!!');
                    dsMain.SetItem(0, "assist_amt", approveamt);
                }
            }
            else if (c == "req_date") {
            
                PostCheckDate();
            }
        }
        function OnDsPaytoItemChanged(s, r, c, v) {
            if (c == "assist_amt") {
                var sumpayto_amt = 0;
                for (var ii = 0; ii < dsPayto.GetRowCount(); ii++) {
                    sumpayto_amt = sumpayto_amt + dsPayto.GetItem(ii, "assist_amt");
                }
                dsMain.SetItem(0, "assist_amt", sumpayto_amt);
                if (sumpayto_amt > dsMain.GetItem(0, "approve_amt")) {
                    alert('ตรวจสอบยอดเงินรายการจ่าย มียอดมากว่ายอดอนุมัติ');
                    for (var ii = 0; ii < dsPayto.GetRowCount(); ii++) {
                        dsPayto.SetItem(ii, "assist_amt", 0);
                    }
                    dsMain.SetItem(0, "assist_amt", 0);
                    return;
                } else {
                    Calamount();
                }
            } else if (c == "moneytype_code") {
                dsPayto.SetItem(r, "payto_detail", "||||");
                PostSetDataSave();
                if (v == "fee") {
                    PostGetFee();
                }
            }
        }


        function OnDsLoanItemChanged(s, r, c, v) {
            if (c == "assist_amt") {
                var sumloan_amt = 0;
                for (var ii = 0; ii < dsLoan.GetRowCount(); ii++) {
                    if (dsLoan.GetItem(ii, "assist_amt") < 0) {
                        alert('ตรวจสอบยอดเงินรายการหักชำระ');
                        dsLoan.SetItem(ii, "assist_amt", 0);
                        return;
                    }
                    sumloan_amt = sumloan_amt + dsLoan.GetItem(ii, "assist_amt");
                }
                if (sumloan_amt > dsMain.GetItem(0, "approve_amt")) {
                    alert('ยอดเงินที่อนุมัติน้อยกว่ายอดเงินรายการหักชำระ กรุณาตรวจสอบ');
                    return;
                }
                dsMain.SetItem(0, "pay_amt", sumloan_amt);
                //dsMain.SetItem(0, "assist_amt", dsMain.GetItem(0, "approve_amt") - sumloan_amt);
                Calamount();
            } else if (c == "payback_type") {
                if (v == "LON") {
                    dsLoan.SetItem(r, "loan_detail", "หักชำระหนี้");
                } else if (v == "ETC") {
                    dsLoan.SetItem(r, "loan_detail", "หักอื่นๆ");
                }
            }
        }
        function Calamount() {
            var approve_amt = dsMain.GetItem(0, "approve_amt");
            var pay_amt = dsMain.GetItem(0, "pay_amt"); // sum ใน OnDsLoanItemChanged มาแล้ว
            var sumpayto_amt = dsMain.GetItem(0, "assist_amt"); // sum ใน OnDsPaytoItemChanged มาแล้ว
            var net_amt = dsMain.GetItem(0, "approve_amt");
            net_amt = net_amt - (pay_amt + sumpayto_amt);
            if (net_amt < 0) {
                alert('ยอดจ่ายน้อยกว่า 0 ! กรุณาตรวจสอบ');
                dsMain.SetItem(0, "pay_amt", 0);
                dsMain.SetItem(0, "assist_amt", 0);
                for (ii = 0; ii < dsPayto.GetRowCount(); ii++) {
                    dsPayto.SetItem(ii, "assist_amt", 0);
                }
                for (ii = 0; ii < dsLoan.GetRowCount(); ii++) {
                    dsLoan.SetItem(ii, "assist_amt", 0);
                }
                return;
            }
        }




        function OnClickNewRowPayto() {
            PostNewRowPayto();
        }
        function OnClickNewRowLoan() {
            PostNewRowLoan();
        }
        function GetValueFromDlg(bank_code, branch_code, expense_accid, pay_name, tofrom_accid) {
            var show_text = "";
            show_text = bank_code + '|' + branch_code + '|' + expense_accid + '|' + pay_name + '|' + tofrom_accid;
            //alert(show_text);
            dsPayto.SetItem(ChkRow, "payto_detail", show_text);
            PostSetDataSave();
        }

        function SavePay() {
            for (ii = 0; ii < dsPayto.GetRowCount(); ii++) {
                if (dsPayto.GetItem(ii, "assist_amt") <= 0) {
                    alert('ตรวจสอบรายการจ่าย');
                    return;
                }
            }
            for (ii = 0; ii < dsLoan.GetRowCount(); ii++) {
                if (dsLoan.GetItem(ii, "assist_amt") <= 0) {
                    alert('ตรวจสอบรายการหัก');
                    return;
                }
            }
            if (dsMain.GetItem(0, "approve_amt") != dsMain.GetItem(0, "assist_amt") + dsMain.GetItem(0, "pay_amt")) {
                alert('ตรวจสอบยอดรับสุทธิ์');
                return;
            }
            if (confirm("ยืนยันการจ่ายสวัสดิการ")) {
                PostSave();           
            }  
        }
        function GetShowData() {
            alert('บันทึกข้อมูลสำเร็จ');
            parent.GetRetrivedata();
            parent.RemoveIFrame();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <br />
    <div>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <asp:Label ID="lbCurrentAssist" runat="server" Text="Label" Style="margin-left: 40px;"></asp:Label>
        <input type="button" value="บันทึก" id="btnSave" onclick="SavePay()" style="margin-left: 550px;" />
        <input type="button" value="ยกเลิก" onclick="PostCancel()" style="margin-left: 2px;" />
    </div>
    <div align="center">
        <uc1:DsMain ID="dsMain" runat="server" />

        <uc4:DsBank ID="dsBank" runat="server" />

        <br />
        <div id="insertPayto" class="NewRowLink" onclick="OnClickNewRowPayto()" style="display:block; margin-left:680px; margin-bottom:-20px;" >เพิ่มแถว</div>
        <uc2:DsPayto ID="dsPayto" runat="server" />
        <br />
        <br />
        <div id="insertLoan" class="NewRowLink" onclick="OnClickNewRowLoan()" style="display:block; margin-left:680px; margin-bottom:-20px;" >เพิ่มแถว</div>
        <uc3:DsLoan ID="dsLoan" runat="server" />
    </div>
    <asp:HiddenField ID="HdIndex" runat="server" />
    <asp:HiddenField ID="Hdbank" runat="server" />
    <asp:HiddenField ID="Hdbankbranch" runat="server" />
    <asp:HiddenField ID="Hdbankaccid" runat="server" />
    <asp:HiddenField ID="Hdtofrom" runat="server" />
</asp:Content>
