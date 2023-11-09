<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptinvtslipout_autolot.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptinvtslipout_autolot" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostDel%>
    <%=jsPostFindShow%>
    <%=jspostDlgShow%>
    <%=jsPostInvtId%>
    <%=jsCheckBal%>
    <%=postTakereasonCode%>
    <script type="text/javascript">

        function Validate() {
            try {
                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                    var invt_id = getObjString(objDwDetail, i, "invt_id");
                    var invt_qty = getObjFloat(objDwDetail, i, "invt_qty");
                    var unit_price = getObjFloat(objDwDetail, i, "unit_price");
                    var invt_bal = getObjFloat(objDwDetail, i, "qty_bal");
                    if (invt_id == null || invt_id == "") {
                        alert("กรุณากรอกรหัสวัสดุให้ถูกต้อง");
                        return;
                    }
                    if (invt_qty <= 0) {
                        alert("กรุณากรอกจำนวนที่เบิกให้ถูกต้อง");
                        return;
                    }
                    if (unit_price <= 0) {
                        alert("กรุณากรอกราคาต่อหน่วยให้ถูกต้อง");
                        return;
                    }
                    if (invt_qty > invt_bal) {
                        alert("จำนวนที่เบิกเกินกว่าจำนวนที่คงเหลือ\r\nกรุณากรอกจำนวนที่เบิกให้ถูกต้อง");
                        return;
                    }
                }
                var branch_code = objDwMain.GetItem(1, "branch_code");
                if (branch_code == null || branch_code == "") {
                    alert("กรุณากรอกสาขาให้ถูกต้อง");
                    return;
                }
                var dept_code = objDwMain.GetItem(1, "dept_code");
                if (dept_code == null || dept_code == "") {
                    alert("กรุณากรอกแผนกให้ถูกต้อง");
                    return;
                }
                var takereason_code = objDwMain.GetItem(1, "takereason_code");
                if (takereason_code == null || takereason_code == "") {
                    alert("กรุณากรอกเหตุผลในการเบิกให้ถูกต้อง");
                    return;
                }
            }
            catch (Error) { }            
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnFindShow(invt_id) {
                Gcoop.GetEl("HinvtId").value = invt_id;
                jsPostFindShow();
            }

            function DlgFindShow(lot_id) {
                Gcoop.GetEl("HinvtId").value = lot_id;
                jspostDlgShow();
                
            }

        function OnDwDetailOnClick(s, r, c) {
            if (c == "b_invtidsearch") {
                Gcoop.GetEl("HdR").value = r;
                Gcoop.OpenDlg("750", "570", "w_dlg_cmd_ptinvtmast.aspx");

            }
            else if (c == "b_lotidsearch") {
                Gcoop.GetEl("HdR").value = r;
                var invt_id = objDwDetail.GetItem(r, "invt_id")
                Gcoop.OpenDlg("590", "570", "w_dlg_cmd_ptmetlmastlot.aspx", "?invt_id=" + invt_id);
            }
            else if (c == "b_del") {
                Gcoop.GetEl("HdR").value = r;
                jsPostDel();
                CalPriceDetail();
            }
        }

        function CalPriceDetail(row) {
            var invt_qty = 0, unit_price = 0, sumamt = 0;
            try {
                invt_qty = getObjFloat(objDwDetail, row, "invt_qty");
                if (invt_qty < 0) {
                    return alert("จำนวนที่กรอกไม่ถูกต้อง");
                }
                else {
                    unit_price = getObjFloat(objDwDetail, row, "unit_price");
                    sumamt = invt_qty * unit_price;
                    objDwDetail.SetItem(row, "sumprice", sumamt);
                }
            }
            catch (Error) { }
        }

        function postOnInsert() {
            objDwDetail.InsertRow(0);
        }

        function OnDwDetailItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            switch (c) {
                case "invt_id":
                    Gcoop.GetEl("HdR").value = r;
                    jsPostInvtId();
                    break;
                case "invt_qty":
                    CalPriceDetail(r);
                    jsCheckBal(r);
                    break;
                case "unit_price":
                    CalPriceDetail(r);
                    break;
            }
        }

        function OnDwMainItemChange(s, r, c, v) {
            if (c == "takereason_code") {
                objDwMain.SetItem(r, c, v);
                objDwMain.AcceptText();
                postTakereasonCode();
            }

        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_slipout_main"
    LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipout.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange">
    </dw:WebDataWindowControl>
    <div style="height: 18px; vertical-align: bottom; padding-left:30px;">
        <span class="linkSpan" onclick="postOnInsert()" style="font-size:medium; color:Red;float: left">เพิ่มแถว</span>
    </div>
    <br />
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_slipout_detail_autolot"
    LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipout.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventButtonClicked="OnDwDetailOnClick" ClientEventItemChanged="OnDwDetailItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HinvtId" runat="server" />
    
</asp:Content>

