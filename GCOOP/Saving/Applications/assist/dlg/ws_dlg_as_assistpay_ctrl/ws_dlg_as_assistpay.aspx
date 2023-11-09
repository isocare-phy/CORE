<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="ws_dlg_as_assistpay.aspx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_ctrl.ws_dlg_as_assistpay" %>

<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsGain.ascx" TagName="dsGain" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsMain = new DataSourceTool;
        var dsGain = new DataSourceTool;

        function Validate() {
        }

        function chkNumber(ele) {
            var vchar = String.fromCharCode(event.keyCode);
            if ((vchar < '0' || vchar > '9') && (vchar != '.')) return false;
            ele.onKeyPress = vchar;
        }
        function OnDsMainItemChanged(s, r, c, v) {
            if (c == "expense_bank") {
                PostBank();
            } else if (c == "expense_branch") {
                dsMain.SetItem(0, "send_system", '');
                dsMain.GetElement(0, "send_system").style.background = "#CCCCCC";
                dsMain.GetElement(0, "send_system").disabled = true;
                PostChkfee();
            } else if (c == "moneytype_code") {
                dsMain.SetItem(0, "moneytype_code", v);
                PostMoneygroup();
            } else if (c == "REQ_DATE") {
                dsMain.SetItem(0, "req_date", v);
            } else if (c == "send_system") {
                if (v == "DEP") {
                    PostChktrn();
                    dsMain.GetElement(0, "deptaccount_no").style.background = "#FFFFCC";
                    dsMain.GetElement(0, "deptaccount_no").disabled = false;
                }
            } else if (c == "assist_amt") {
                var approveamt = dsMain.GetItem(0, "approve_amt");
                var assistamt = dsMain.GetItem(0, "assist_amt");
                if (assistamt > approveamt) {
                    alert('ยอดอนุมัติมากกว่ายอดจ่าย!!');
                    dsMain.SetItem(0, "assist_amt", approveamt);
                }
            } else if (c == "lon_amt") {
                var approveamt = dsMain.GetItem(0, "approve_amt");
                var assistamt = approveamt - v
                //dsMain.SetItem(0, "assist_amt",assistamt);
                if (v > approveamt) {
                    alert('ยอดหักชำระหนี้มากกว่ายอดอนุมัติ!!');
                    dsMain.SetItem(0, "assist_amt", approveamt);
                    dsMain.SetItem(0, "lon_amt", 0);
                } else {
                    Calamount();
                }
            } else if (c == "chkloan") {
                if (v == 1) {
                    //dsMain.GetElement(0, "lon_amt").style.background = "#FFFFCC";
                    dsMain.GetElement(0, "lon_amt").style.background = "#FFFFFF";
                    dsMain.GetElement(0, "lon_amt").disabled = false;
                }
                else {
                    var approveamt = dsMain.GetItem(0, "approve_amt");
                    dsMain.SetItem(0, "assist_amt", approveamt);
                    dsMain.SetItem(0, "lon_amt", 0);
                    dsMain.GetElement(0, "lon_amt").style.background = "#EEEEEE";
                    dsMain.GetElement(0, "lon_amt").disabled = true;
                }
            } else if (c == "fee") {
                Calamount();
            } else if (c == "chkgain") {
                if (v == 1) {
                    //document.getElementById("NewRowLink").disabled = true;
                    for (var ii = 0; ii < dsGain.GetRowCount(); ii++) {
                        dsGain.GetElement(ii, "gain_fullname").style.background = "#FFFFFF";
                        dsGain.GetElement(ii, "gain_fullname").disabled = false;
                        dsGain.GetElement(ii, "assist_amt").style.background = "#FFFFFF";
                        dsGain.GetElement(ii, "assist_amt").disabled = false;
                    }
                } else {
                    for (var ii = 0; ii < dsGain.GetRowCount(); ii++) {
                        dsGain.GetElement(ii, "gain_fullname").style.background = "#EEEEEE";
                        dsGain.GetElement(ii, "gain_fullname").disabled = true;
                        dsGain.SetItem(ii, "assist_amt", 0);
                        dsGain.GetElement(ii, "assist_amt").style.background = "#EEEEEE";
                        dsGain.GetElement(ii, "assist_amt").disabled = true;
                    }
                }
            }
        }
        function Calamount() {
            var sumgain_amt = 0;
            var net_amt = dsMain.GetItem(0, "approve_amt");
            var approve_amt = dsMain.GetItem(0, "approve_amt");
            var fee = dsMain.GetItem(0, "fee");
            var lon_amt = dsMain.GetItem(0, "lon_amt");
            for (var ii = 0; ii < dsGain.GetRowCount(); ii++) {
                sumgain_amt = sumgain_amt + dsGain.GetItem(ii, "assist_amt");
            }
            net_amt = net_amt - (fee + lon_amt + sumgain_amt);
            if (net_amt < 0) {
                alert('ยอดจ่ายน้อยกว่า 0 ! กรุณาตรวจสอบ');
                for (ii = 0; ii < dsGain.GetRowCount(); ii++) {
                    dsGain.SetItem(ii, "assist_amt", 0);
                    dsMain.SetItem(0, "lon_amt", 0);
                    dsMain.SetItem(0, "assist_amt", approve_amt - fee);
                }
                return;
            }
            dsMain.SetItem(0, "assist_amt", net_amt);  
        }
        function OnDsMainClicked(s, r, c) {
            if (c == "b_searchdeptno") {
                if (dsMain.GetItem(0, "moneytype_code") == "TRN" && dsMain.GetItem(0, "send_system") == "DEP") {
                    var memno = dsMain.GetItem(0, "member_no");
                    Gcoop.OpenDlg2("500", "350", "w_dlg_ass_deptaccountno_search.aspx", "?memno=" + memno);
                } else {
                    alert('ปุ่มค้นหาเลขบัญชีใช้กรณีโอนเงินฝากสหกรณ์เท่านั้น!!');
                }
            }
        }
        function OnDsGainClicked(s, r, c, v) {
            if (c == "b_del") {
                dsGain.SetRowFocus(r);
                if (confirm("ลบข้อมูลผู้รับผลประโยชน์แถวที่ " + (r + 1) + " ?") == true) {
                    PostDelRow();
                }
            }
        }
        function OnDsGainItemChanged(s, r, c, v) {
            if (c == "assist_amt") {
                var sumgain_amt;
                for (var ii = 0; ii < dsGain.GetRowCount(); ii++) {
                    if (dsGain.GetItem(ii, "assist_amt") < 0) {
                        alert('ตรวจสอบยอดเงินจ่ายให้ผู้รับผลประโยชน์');
                        for (ii = 0; i < dsGain.GetRowCount(); ii++) {
                            dsGain.GetItem(ii, "assist_amt") = 0;
                        }
                        return;
                    }
                    sumgain_amt = sumgain_amt + dsGain.GetItem(ii, "assist_amt");
                }
                if (sumgain_amt > dsMain.GetItem(0, "approve_amt")) {
                    alert('ยอดเงินที่อนุมัติน้อยกว่ายอดเงินที่ให้ผู้รับผลประโชยน์ กรุณาตรวจสอบ');
                    return;
                }
                Calamount();
            }
        }

        function OnClickNewRow() {
            PostNewRow();
        }
        function GetDeptNoFromDlg(deptno) {
             dsMain.SetItem(0, "deptaccount_no", deptno.trim());
        }
        function DialogLoadComplete() {

        }

        function SavePay() {
            var alertstr = "";
            var money = dsMain.GetItem(0, "moneytype_code");
            var tofrom_accid = dsMain.GetItem(0, "tofrom_accid");
            if(money == ""){
                alertstr = "- กรุณาเลือกประเภทเงิน";
            }
            if (money != "CSH" && money != "TRN") {
                var expense_bank = dsMain.GetItem(0, "expense_bank");
                var expense_branch = dsMain.GetItem(0, "expense_branch");
                var deptno = dsMain.GetItem(0, "deptaccount_no");
                if (expense_bank == "" || expense_bank == null) { alertstr = alertstr + "- กรุณาเลือกธนาคาร\n"; }
                if (expense_branch == "" || expense_branch == null) { alertstr = alertstr + "- กรุณาเลือกสาขาธนาคาร\n"; }
                //if (deptno == "" || deptno == null) { alertstr = alertstr + "- กรุณากรอกเลขที่บัญชี\n"; }
            }
            if (tofrom_accid == "" || tofrom_accid == null) { alertstr = alertstr + "- กรุณาเลือกรหัสบัญชี\n"; }
            if (alertstr == "") {
                if (dsMain.GetItem(0, "chkloan") == "1") {
                    if (dsMain.GetItem(0, "assist_amt") > 0) {
                        alert("ตรวจสอบยอดที่ให้ผู้รับผลประโยชน์");
                        return;
                    }
                }
                if (confirm("ยืนยันการจ่ายสวัสดิการ")) {
                    PostSave();           
                }  
            } else {
                alert(alertstr);
                return false;
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
    </div>
    <div align="center" style="margin-top:-20px">
        <div id="insertRow" class="NewRowLink" onclick="OnClickNewRow()" style="display:block; margin-left:650px;" >เพิ่มแถว</div>
        <uc2:DsGain ID="dsGain" runat="server" />
    </div>
    <asp:HiddenField ID="HdIndex" runat="server" />
</asp:Content>
