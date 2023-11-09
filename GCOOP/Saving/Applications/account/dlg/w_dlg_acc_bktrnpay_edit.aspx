<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_acc_bktrnpay_edit.aspx.cs" 
Inherits="Saving.Applications.account.dlg.w_dlg_acc_bktrnpay_edit" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>รายการรับชำระหนี้</title>
</head>
<%=jsPostDetail%>
<%=jsPostDwShareInsertRow%>
<%=jsPostDwLoanInsertRow%>
<%=jsPostShareFlag%>
<%=jsPostLoanFlag%>
<%=jsPostOtherFlag%>
<%=jsPostDwOtherInsertRow%>
<%=jsSave%>

<script type="text/javascript">
    function DwmainButtonClick(s, r, c, v) {
        if (c == "b_search") {
            Gcoop.OpenDlgIn("900", "600", "w_dlg_acc_search_member.aspx", "");
//            Gcoop.OpenDlgIn("475", "400", "w_dlg_template.aspx", "");
        }
    }

    function DwMainItemChanged(s, r, c, v) {
        if (c == "member_no") {
            s.SetItem(1, "member_no", v);
            s.AcceptText();
            if (v == "" || v == null) {
                alert("กรุณาระบุรหัสสมาชิก")
            }
            else {
                jsPostDetail();
            }
        }
        else if (c == "doc_tdate") {
            if (v == "" || v == null) {
                alert("กรุณากรอกข้อมูลวันที่")
            }
            else {
                s.SetItem(1, "doc_tdate", v);
                s.AcceptText();
                s.SetItem(1, "doc_date", Gcoop.ToEngDate(v));
                s.AcceptText();
            }
            return 0; 
        }

    }

    //เพิ่มแถว
    function B_DwShareInsert() {
        jsPostDwShareInsertRow();
    }
    //เพิ่มแถว
    function B_DwLoanInsert() {
        jsPostDwLoanInsertRow();
    }
    //เพิ่มแถว
    function B_DwOtherInsert() {
        jsPostDwOtherInsertRow();
    }


    function OnDwShareClick(s, r, c, v) {
        if (c == "operate_flag") {
            Gcoop.GetEl("HdRowShare").value = r + "";
            jsPostShareFlag();
        }
    }

    function OnDwLoanClick(s, r, c, v) {
        if (c == "operate_flag") {
            Gcoop.GetEl("HdRowLoan").value = r + "";
            jsPostLoanFlag();
        }
    }

    function OnDwOtherClick(s, r, c, v) {
        if (c == "operate_flag") {
            Gcoop.GetEl("HdRowOther").value = r + "";
            jsPostOtherFlag();
        }
    }

    function OnOkClick() {
        jsSave();
    }

    function DwShareButtonClick(s, r, c) {
        var Acc_no = objDwShare.GetItem(r, "paytrnbank_docno");
        var isConfirm = confirm("ต้องการลบข้อมูล " + Acc_no + " ใช่หรือไม่ ?");
        if (isConfirm) {
            Gcoop.GetEl("HdRowDeleteShare").value = r + "";
            jsPostDeleteDetail();
        }
    }

    //ฟังก์ชันในการปิด dialog
    function OnCloseDialog() {
        if (confirm("ยืนยันการออกจากหน้าจอ ")) {
            // window.close();
            window.parent.jsRetrieve();
            parent.RemoveIFrame();
//            postRefresh();
        }
    }

    function DwShareItemChanged(s, r, c, v) {
        s.SetItem(r,c, v);
        s.AcceptText();
        if (c == "moneytrn_amt") {
            SumTotal();
        }
    }

    function DwLoanItemChanged(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        if (c == "moneytrn_amt") {
            SumTotal();
        }
    }

    function DwOtherItemChanged(s, r, c, v) {
        s.SetItem(r, c, v);
        s.AcceptText();
        if (c == "moneytrn_amt") {
            SumTotal();
        }
    }

    function SumTotal() {
        var sumtotal = 0;
        sumtotal += SumDWCheck(objDwShare, 'moneytrn_amt', 'operate_flag');
        sumtotal += SumDWCheck(objDwLoan, 'moneytrn_amt', 'operate_flag');
        sumtotal += SumDWCheck(objDwOther, 'moneytrn_amt', 'operate_flag');
        objDwmain.SetItem(1, "paytrnbank_amt", sumtotal);
        objDwmain.AcceptText();
    }


    function SumDWCheck(s, c, checkbox) {
        var sum = 0;
        var count = s.RowCount();
        for (var i = 1; i <= count; i++) {
            var chk = Number( s.GetItem(i,checkbox));
            if (chk == 1) {
                sum += Number(s.GetItem(i,c));
            }
        }
        return sum;
    }

    function GetValueFromDlg(memberno) {
        objDwmain.SetItem(1, "member_no", memberno);
        objDwmain.AcceptText();
//        Gcoop.GetEl("Hfmember_no").value = memberno;
        jsPostDetail();
    }
</script>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
    <tr>
        <td>
            <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" >
            <dw:WebDataWindowControl ID="Dwmain" runat="server" DataWindowObject="d_ac_bktrnpayedit_new"
                LibraryList="~/DataWindow/account/acc_post_pay.pbl" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                ClientScriptable="True" ClientEventItemChanged="DwMainItemChanged" 
                    ClientEventButtonClicked = "DwmainButtonClick" 
                    style="top: 0px; left: 0px; width: 711px">
            </dw:WebDataWindowControl>
            </asp:Panel>
            </td>
    </tr>
    </table>
    <table style="width: 720px">
    <tr>
       <td>
       <span class="linkSpan" onclick="B_DwShareInsert()" 
       style="font-family: Tahoma; font-size: small; float: left; color: #0000CC;">เพิ่มแถว</span>
       </td>
       <td>
       <span class="linkSpan" onclick="B_DwLoanInsert()" 
       style="font-family: Tahoma; font-size: small; float: left; color: #0000CC;">เพิ่มแถว</span>
       </td>
    </tr>

    </table>

        <table>
          <tr>
        <td>
            <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Height="250px" Width="350px" ScrollBars="Vertical">
            <dw:WebDataWindowControl ID="DwShare" runat="server" DataWindowObject="d_ac_bktrnpayedit_detshare"
                LibraryList="~/DataWindow/account/acc_post_pay.pbl" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                ClientScriptable="True" ClientEventClicked="OnDwShareClick" ClientEventButtonClicked = "DwShareButtonClick"
                ClientEventItemChanged="DwShareItemChanged" >
            </dw:WebDataWindowControl>
            </asp:Panel>
         </td>
         <td>
            <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" Height="250px" Width="350px" ScrollBars="Vertical">
            <dw:WebDataWindowControl ID="DwLoan" runat="server" DataWindowObject="d_ac_bktrnpayedit_detloan"
                LibraryList="~/DataWindow/account/acc_post_pay.pbl" AutoRestoreContext="False"
                AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                ClientScriptable="True" ClientEventClicked="OnDwLoanClick" ClientEventButtonClicked = "DwLoanButtonClick"
                ClientEventItemChanged="DwLoanItemChanged" >
            </dw:WebDataWindowControl>
            </asp:Panel>
          </td>
   </tr>
   </table>
   <table>
   <tr>
          <td>
       <span class="linkSpan" onclick="B_DwOtherInsert()" 
       style="font-family: Tahoma; font-size: small; float: left; color: #0000CC;">เพิ่มแถว</span>
       </td>
       </tr>
        <tr>
        <td>

         <asp:Panel ID="Panel4" runat="server" BorderStyle="Ridge" Width="711px" ScrollBars="Vertical">
        <dw:WebDataWindowControl ID="DwOther" runat="server" DataWindowObject="d_ac_bktrnpayedit_detetc"
            LibraryList="~/DataWindow/account/acc_post_pay.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
            ClientScriptable="True" ClientEventClicked="OnDwOtherClick" ClientEventButtonClicked = "DwOtherButtonClick"
            ClientEventItemChanged="DwOtherItemChanged" >
        </dw:WebDataWindowControl>
        </asp:Panel>
        </td>
  </tr>
  <tr>
  <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
             <input id="B_save" type="button" value="บันทึก" onclick="OnOkClick()" />
             <input id="B_cancel" type="button" value="ยกเลิก" onclick="OnCloseDialog()" />
  </td>
  </tr>
        </table>
        <asp:HiddenField ID="HdRowShare" runat="server" />
        <asp:HiddenField ID="HdRowLoan" runat="server" />
        <asp:HiddenField ID="HdRowOther" runat="server" />
        <asp:HiddenField ID="HdRowDeleteShare" runat="server" />
        <asp:HiddenField ID="Hfmember_no" runat="server" />


    </div>
    </form>
</body>
</html>
