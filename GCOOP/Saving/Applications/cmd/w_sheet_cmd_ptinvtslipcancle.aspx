<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ptinvtslipcancle.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ptinvtslipcancle" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=jsPostRefSlipNo%>
    <%=jsPostRefSlipnoDlg%>
    <%=jsPostSearchInvslip%>
    <script type="text/javascript">

        function Validate() {    
            return confirm("ยืนยันการบันทึกข้อมูล รายการยกเลิก");
        }

        function OnFindShow(invt_id) {
                Gcoop.GetEl("HinvtId").value = invt_id;
                jsPostFindShow();
            }

        function DlgFindShow(RefslipNo) {
            Gcoop.GetEl("HdRefSlipno").value = RefslipNo;
            jsPostRefSlipnoDlg();      
        }

        function OnDwMainItemChange(s, r, c, v) {
            s.SetItem(r, c, v);
            s.AcceptText();
            if (c == "ref_slipno") {
                Gcoop.GetEl("HdRefSlipno").value = v;
                jsPostRefSlipNo();
            }
            if (c == "slip_tdate") {
                objDwMain.SetItem(1, "slip_date", v);
                Gcoop.GetEl("HdDateStart").value = v;
            }
            if (c == "buy_tdate") {
                objDwMain.SetItem(1, "buy_date", v);
                Gcoop.GetEl("HdDateEnd").value = v;
            }
        }

        function OnDwMainClick(s, r, c) {
            if (c == "b_search") {
                jsPostSearchInvslip();
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_ptinvtcancleslip_main"
    LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipcancle.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventItemChanged="OnDwMainItemChange" ClientEventButtonClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_ptinvtcancleslip_detail"
    LibraryList="~/DataWindow/Cmd/cmd_ptinvtslipcancle.pbl" Width="750px" ClientScriptable="True"
    AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
    ClientFormatting="True" ClientEventButtonClicked="OnDwDetailOnClick" ClientEventItemChanged="OnDwDetailItemChange">
    </dw:WebDataWindowControl>
    <asp:HiddenField ID="HdR" runat="server" />
    <asp:HiddenField ID="HinvtId" runat="server" />
    <asp:HiddenField ID="HdRefSlipno" runat="server" />
    <asp:HiddenField ID="HdDateStart" runat="server" />
    <asp:HiddenField ID="HdDateEnd" runat="server" />
</asp:Content>

