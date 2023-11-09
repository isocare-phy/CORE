<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_slipins.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_slipins" Title="Untitled Page" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=jsIncSlipetc%>
    <%=jsAddNewRow %>

<script type="text/javascript">
//    function MenubarNew() {
//        if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
//            newClear();
//        }
    //    }
    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูลรับชำระ");
    }
    function MenubarOpen() {
        Gcoop.OpenDlg('580', '590', 'w_dlg_sl_member_search.aspx', '');
    }
    function OnDwMainClicked(s, r, c) {
        if (c == "b_search") {
            Gcoop.OpenDlg('580', '590', 'w_dlg_sl_member_search.aspx', '');
        }

    }
    function insertRow() {
        jsAddNewRow();
    }
    function GetValueFromDlg(memberno) {
        //number objdwcontrol.SetItem ( number row, string column, variant value )
        objdw_main.SetItem(1, "member_no", memberno);
        objdw_main.AcceptText();

        jsIncSlipetc();
    }
    function OnDwMainItemChanged(s, r, c, v) {
        if (c == "member_no") {
            s.SetItem(r, c, v);
            s.AcceptText();
            jsIncSlipetc();
        }
        return 0;
    }
  
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table width="100%">
        <tr>
            <td>
               
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_ins_slip_operate_main"
                    LibraryList="~/DataWindow/app_assist/as_slipins.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="OnDwMainItemChanged" 
                                        ClientEventButtonClicked="OnDwMainClicked">
                </dw:WebDataWindowControl>
            </td>
        </tr>
                <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_ins_slip_operate_etc"
                    LibraryList="~/DataWindow/app_assist/as_slipins.pbl" ClientScriptable="True"
                    AutoRestoreContext="False" AutoRestoreDataCache="True">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="insertRow()" style="font-size:small; color:Red; float:left">เพิ่มแถว</span>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfRowNumber" Value="1" runat="server" />
    <asp:HiddenField ID="HiddenFieldCode" runat="server" />
    <asp:HiddenField ID="Hfmember_no" runat="server" />
</asp:Content>
