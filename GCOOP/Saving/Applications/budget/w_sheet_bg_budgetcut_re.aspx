<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_bg_budgetcut_re.aspx.cs" Inherits="Saving.Applications.budget.w_sheet_bg_budgetcut_re" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initProgress%>
    <%=DeleteRow%>
    <%=FilterSortSeq%>
    <%=ProcessCutPay%>
    <%=getSortseq%>
    <script type="text/javascript">
        function CheckAll(obj) {
            var theForm = obj.form;
            if (obj.checked) {
                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                    objDwDetail.SetItem(i, "post_flag", 1);
                }
            }
            else {
                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                    objDwDetail.SetItem(i, "post_flag", 0);
                }
            }
        }

        function OnDwMainButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "cb_ok") {
//                if (Gcoop.GetEl("HdCheckAccId").value == "true") {
//                    Gcoop.OpenIFrame(430, 450, "w_dlg_bg_budgettype_noaccid.aspx", "?xmlAccId=" + Gcoop.GetEl("HdXmlFromDlg").value);
//                    //alert(Gcoop.GetEl("HdXmlFromDlg").value);

//                }
//                else {
                    initProgress();
               // }
            }
        }

        function OnDwMainItemChanged(sender, rowNumber, colunmName, newValue) {
            objDwMain.SetItem(rowNumber, colunmName, newValue);
            objDwMain.AcceptText();
            return 0;
        }

        function OnDwDetailItemChanged(sender, rowNumber, columnName, newValue) {
            if (columnName == "budget_seq_no") {
                Gcoop.GetEl("HdRowDetail").value = rowNumber + "";
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                FilterSortSeq();
            }
        }

        function OnDwDetailClicked(sender, rowNumber, objectName) {
            if (objectName == "post_flag") {
                Gcoop.CheckDw(sender, rowNumber, objectName, "post_flag", 1, 0);
                objDwDetail.AcceptText();
            }

        }

        function OnClickDeleteRow() {
            if (objDwDetail.RowCount() > 0) {
                var postFlag = 0;
                var check = false;
                for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                    postFlag = objDwDetail.GetItem(i, "post_flag") + "";
                    if (postFlag == "1") {
                        check = true;
                        break;
                    }
                    else {
                        check = false;
                    }
                }
                if (check == true) {
                    DeleteRow();
                }
                else {
                    alert("กรุณาเลือกรายการ ที่จะทำการลบข้อมูลก่อน");
                }
            }
            else {
                alert("ไม่มีข้อมูล ไม่สามารถลบข้อมูลได้");
            }
        }

        function Validate() {
            var seqNo = 0;
            var sortSeq = 0;
            var postFlag = 0;
            var check = false;
            var txtErr = "";
            for (var i = 1; i <= objDwDetail.RowCount(); i++) {
                postFlag = objDwDetail.GetItem(i, "post_flag") + "";
                try {
                    seqNo = objDwDetail.GetItem(i, "budget_seq_no");
                }
                catch (err) { seqNo = 0 };
                try {
                    sortSeq = objDwDetail.GetItem(i, "sort_seq");
                }
                catch (err) { sortSeq = 0 };

                if (postFlag == "1") {
                    if (seqNo != null && seqNo != 0 && sortSeq != null && sortSeq != 0) {
                        check = true;
                    }
                    else {
                        check = false;
                        break;
                    }
                }
                else {
                    if (seqNo != null && seqNo != 0 && sortSeq != null && sortSeq != 0) {
                        check = false;
                        txtErr = "กรุณาเลือกรายการ ที่จะทำการบันทึกข้อมูลก่อน";
                        break;
                    }
                }
            }
            if (check == true) {
                return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");
            }
            else if (txtErr == "") {
                alert("กรุณาเลือกหมวด และประเภทงบประมาณก่อน");
            }
            else {
                alert(txtErr);
            }
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdValueDlg").value == "true") {
                Gcoop.GetEl("HdValueDlg").value = "false"
                ProcessCutPay();
            }
        }

        function GetValueYesORNo(check) {
            Gcoop.GetEl("HdValueDlg").value = check;
            if (check == "true") {
                ProcessCutPay();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:HiddenField ID="HFseqNo" runat="server" />
    <asp:Label ID="Label1" runat="server" Text="ตัดจ่ายงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetadjust"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
   
    <asp:HiddenField ID="HdRowDetail" runat="server" />
    <asp:HiddenField ID="HdValueDlg" runat="server" Value="false" />
    <asp:HiddenField ID="HdCheckAccId" runat="server" Value="false" />
    <asp:HiddenField ID="HdXmlFromDlg" runat="server" />
</asp:Content>
