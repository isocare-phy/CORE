<%@ Page Title="" Language="C#" MasterPageFile="~/FrameDialog.Master" AutoEventWireup="true"
    CodeBehind="w_dlg_vc_trn_loan.aspx.cs" Inherits="Saving.Applications.account.dlg.w_dlg_vc_trn_loan_ctrl.w_dlg_vc_trn_loan" %>

<%@ Register Src="DsList.ascx" TagName="DsList" TagPrefix="uc1" %>
<%@ Register Src="DsSum.ascx" TagName="DsSum" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var dsList = new DataSourceTool();
        var dsSum = new DataSourceTool();
        var operate_flag_sum = 0;
        var payout_amt_sum = 0;
        window.$order = 1;
        function OnDsSumClicked(s, r, c) {
            if (c == "btn_ok") {
                if (confirm("ยืนยันการเลือกสัญญาเงินกู้")) {
                    var rowcount = dsList.GetRowCount();
                    var cash_type = "";
                    cash_type = "<%=cashType%>";
                    var slip_no = "";
                    for (var i = 0; i < rowcount; i++) {
                        var operate_flag = 0;
                        var operate_flag = dsList.GetItem(i, "operate_flag");
                        if (operate_flag == 1) {
                            if (i > 0 && slip_no != "") { slip_no += ","; }
                            slip_no += dsList.GetItem(i, "payoutslip_no");
                        }
                    }
                    parent.GetAccount(slip_no, cash_type);
                    parent.RemoveIFrame();
                }
            } else if (c == "btn_cancel") {
                if (confirm("ยืนยันการออกจากหน้าจอ ")) {
                    // window.close();
                    //            window.parent.postVoucherDate();
                    parent.RemoveIFrame();
                }
            }
        }

        function OnDsListItemChanged(s, r, c, v) {
            if (c == "operate_flag") {
                if (r >= 0) {
                    var rowcount = dsList.GetRowCount();
                    var sumtotal = 0, count = 0;
                    for (var i = 0; i < rowcount; i++) {
                        var flag = Number(dsList.GetItem(i, "operate_flag"));
                        if (flag == 1) {
                            count++;
                            sumtotal += Number(dsList.GetItem(i, "payout_amt"));
                        }
                    }
                    dsSum.SetItem(0, "operate_flag_sum", count);
                    dsSum.SetItem(0, "payment_sum", sumtotal);
                }
            }
        }

        function OnDsListClicked(s, r, c) {
            switch (c) {
                case "payoutslip_no":
                    SortbyColumn(dsList, c);
                    break;
                case "memb_name":
                    SortbyColumn(dsList, c);
                    break;
                case "shrlontype_code":
                    SortbyColumn(dsList, c);
                    break;
                case "loancontract_no":
                    SortbyColumn(dsList, c);
                    break;
                case "payout_amt":
                    SortbyColumn(dsList, c);
                    break;
                case "entry_id":
                    SortbyColumn(dsList, c);
                    break;
            }
        }

        function SortbyColumn(s, col) {

            var arr_col = ["operate_flag", "payoutslip_no", "memb_name", "shrlontype_code", "loancontract_no", "payout_amt", "entry_id"];
            window.$idx_col = find_idx_arr(arr_col, col);
            if (window.$idx_col == -1)
                return;
            var arr_tmp = Dwcopy2Arr(s, arr_col);
            arr_tmp.sort(SortFunction_ASC);

            //            if (window.$order == 1) { // 1 = asc  0 desc
            //                arr_tmp.sort(SortFunction_ASC);
            //                window.$order = 0;
            //            } else {
            //                arr_tmp.sort(SortFunction_DESC);
            //                window.$order = 1;
            //            }
            DataBindObj(s, arr_tmp, arr_col);
        }

        function DataBindObj(s, arr_tmp, arr_col) {
            for (var i = 0; i < s.GetRowCount(); i++) {
                for (var j = 0; j < arr_col.length; j++) {
                    s.SetItem(i, arr_col[j], arr_tmp[i][j]);
                }
            }
        }

        function Dwcopy2Arr(s, arr_col) {
            var arrdata = new Array();
            for (var i = 0; i < s.GetRowCount(); i++) {
                var tmp = new Array();
                for (var j = 0; j < arr_col.length; j++) {
                    tmp.push(s.GetItem(i, arr_col[j]));
                }
                arrdata.push(tmp);
            }
            return arrdata;
        }

        function find_idx_arr(arr, col) {
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] == col)
                    return i;
            }
            return -1;
        }

        function SortFunction_ASC(a, b) {
            var index;
            try {
                index = window.$idx_col;
            } catch (Error) {
                index = 0;
            }
            return a[index] > b[index];
        }

        function SortFunction_DESC(a, b) {
            var index;
            try {
                index = window.$idx_col;
            } catch (Error) {
                index = 0;
            }
            return a[index] < b[index];
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <script type="text/javascript">
        function Check_All() {
            var chk = $('#ctl00_ContentPlace_dsList_chk_all').is(':checked')
            var operate_flag;
            if (chk) {
                operate_flag = 1;
            } else {
                operate_flag = 0;
            }
            var count = dsList.GetRowCount();
            var sumtotal = 0;
            for (var i = 0; i < count; i++) {
                dsList.SetItem(i, "operate_flag", operate_flag);
                sumtotal += Number(dsList.GetItem(i, "payout_amt"));
            }
            if (!chk) {
                sumtotal = 0;
                count = 0;
            }
            dsSum.SetItem(0, "operate_flag_sum", count);
            dsSum.SetItem(0, "payment_sum", sumtotal);
        }
    </script>
    <uc1:DsList ID="dsList" runat="server" />
    <uc2:DsSum ID="dsSum" runat="server" />
</asp:Content>
