<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_ucf_loancolluse.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_ucf_loancolluse" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostTypeChange%>
    <%=jsPostDelete%>
    <script type="text/javascript">

        function Validate() { //function เช็คค่าข้อมูลก่อน save
            var isconfirm = confirm("ยืนยันการบันทึกข้อมูล ?");

            if (!isconfirm) {
                return false;
            }
            var loantype_code = objDwHead.GetItem(1, "loantype_code");
            var loancolltype_code = objDwHead.GetItem(1, "loancolltype_code");
            var collmasttype_code = objDwHead.GetItem(1, "collmasttype_code");
            var coll_percent = objDwMain.GetItem(1, "coll_percent");

            if (loantype_code != null && loancolltype_code != null && collmasttype_code != null && coll_percent != null) {
                return true;
            }
            else {
                confirm("กรุณาระบุข้อมูลให้ครบถ้วน");
                return false;
            }
        }

        function OnDwHeadItemChange(sender, row, col, Value) {
            sender.SetItem(row, col, Value);
            sender.AcceptText();
            var loantype_code = objDwHead.GetItem(1, "loantype_code");
            var loancolltype_code = objDwHead.GetItem(1, "loancolltype_code");
            var collmasttype_code = objDwHead.GetItem(1, "collmasttype_code");
            if (loantype_code != null && loancolltype_code != null && collmasttype_code != null) {
                jsPostTypeChange();
            }
        }

        function DwPayInExpButtonClicked(sender, row, bName) {
            if (bName == "b_del") {
                var loantype_code = objDwHead.GetItem(1, "loantype_code");
                var loancolltype_code = objDwHead.GetItem(1, "loancolltype_code");
                var collmasttype_code = objDwHead.GetItem(1, "collmasttype_code");
                if (loantype_code != null && loancolltype_code != null && collmasttype_code != null) {
                    var isconfirm = confirm("ต้องการลบข้อมูลหรือไม่ ?");
                    if (isconfirm) {
                        jsPostDelete();
                    }
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;">
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwHead" runat="server" DataWindowObject="dw_lc_lccfloantypecolluse_head"
                    LibraryList="~/DataWindow/investment/loan_ucf.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" ClientEventItemChanged="OnDwHeadItemChange">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_lc_lccfloantypecolluse"
                    LibraryList="~/DataWindow/investment/loan_ucf.pbl" ClientScriptable="True" ClientEvents="true"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    AutoRestoreContext="False" ClientEventButtonClicked="DwPayInExpButtonClicked">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="Hd_status" runat="server" />
</asp:Content>
