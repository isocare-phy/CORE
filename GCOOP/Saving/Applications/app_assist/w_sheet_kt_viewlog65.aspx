<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_kt_viewlog65.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_viewlog65" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%=postMemberNo%>
    <%=postChangeAssist %>
    <%=postChangeAmt %>

    <script type="text/javascript">

//        function MenubarOpen() {
//            Gcoop.OpenDlg(780, 500, "w_dlg_kt_cancel_slip_65.aspx", "");
//        }

        function GetMemberNoFromDlg(memberNo) {
            objDwMain.SetItem(1, "member_no", memberNo);
            objDwMain.AcceptText();
            postMemberNo();
        }

        function GetValueFromDlg(deptmem_id) {
            objDwMain.SetItem(1, "member_no", deptmem_id);
            objDwMain.AcceptText();
            postMemberNo();
        }


        function MenubarNew() {
            window.location = Gcoop.GetUrl() + "Applications/app_assist/w_sheet_kt_viewlog.aspx";
        }

        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "mbmembmaster_member_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postMemberNo();
            }

            return 0;
        }

        function SheetLoadComplete() {

            if (Gcoop.GetEl("HdSaveAccept").value == "true") {
                var deptAccountNo = Gcoop.GetEl("HdAccoutNo").value;
                var deptPassBookNo = Gcoop.GetEl("HdPassBookNo").value;
                Gcoop.OpenIFrame(450, 150, "w_dlg_kt_printfirstpage.aspx", "?deptAccountNo=" + deptAccountNo + "&deptPassBookNo=" + deptPassBookNo);
            }
        }

        //        function DwSelectItemChange(sender, rowNumber, columnName, newValue) {

        //            if (columnName == "assisttype_code") {
        //                objDwSelect.SetItem(rowNumber, columnName, newValue);
        //                objDwSelect.AcceptText();
        //                //alert(newValue);
        //                postChangeAssist();
        //            }
        //        }
        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMain.SetItem(1, "mbmembmaster_member_no", memberno);
            Gcoop.GetEl("HfMemberNo").value = memberno;
            postMemberNo();
        }
        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(780, 550, "w_dlg_kt_cancel_slip_65.aspx", "");
            }
        }

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Panel ID="Panel1" runat="server" Width="680px">

        <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_kt_asnslipsearch"
            LibraryList="~/DataWindow/app_assist/kt_65years.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="DwMemButtonClick"  ClientFormatting="True" 
            TabIndex="1"  >
        </dw:WebDataWindowControl>

        <dw:WebDataWindowControl ID="DwSlip" runat="server" DataWindowObject="d_kt_asnslippayout"
            LibraryList="~/DataWindow/app_assist/kt_65years.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventItemChanged="OnDwMainItemChanged" ClientEventButtonClicked="DwMemButtonClick"  ClientFormatting="True" 
            TabIndex="1"  >
        </dw:WebDataWindowControl>
    </asp:Panel>
    <br />
<%--    <asp:HiddenField ID="HdDayPassCheq" runat="server" />
    <asp:HiddenField ID="HdDwGainCurrentRow" runat="server" />
    <asp:HiddenField ID="HdIsInsertCheque" runat="server" />
    <asp:HiddenField ID="HdDwChequeRow" runat="server" />
    <asp:HiddenField ID="HdCheckApvAlert" runat="server" />
    <asp:HiddenField ID="HdProcessId" runat="server" />
    <asp:HiddenField ID="HdAvpCode" runat="server" />
    <asp:HiddenField ID="HdItemType" runat="server" />
    <asp:HiddenField ID="HdAvpAmt" runat="server" />
    <asp:HiddenField ID="HdSaveAccept" runat="server" />
    <asp:HiddenField ID="HdAccoutNo" runat="server" />
    <asp:HiddenField ID="HdPassBookNo" runat="server" />
    <asp:HiddenField ID="HdDueDate" runat="server" />--%>
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="checkMem" runat="server" />
    
</asp:Content>