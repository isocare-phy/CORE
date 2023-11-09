<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_cancel_regloan_year.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_cancel_regloan_year" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=jsPostMember %>
    
    <%=openNew %>
 
    <%=getLoanrequest%>
    <%=initlist %>
    <script type="text/javascript">
        function Validate() {
            
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarNew() {

            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
            openNew();
        }
        function MenubarOpen() {
            Gcoop.OpenDlg('580', '590', 'w_dlg_regloan_year.aspx', '');

        }
        function GetValueLoanRequest(docNo) {

            objdw_main.SetItem(1, "loanrequest_docno", docNo);
            Gcoop.GetEl("Hloanrequest_docno").value = docNo;
            getLoanrequest();
        }
        function ItemDataWindowChange(s, r, c, v) {
            if (c == "member_no") {
                objdw_main.SetItem(r, "member_no", Gcoop.StringFormat(v, "000000"));
                Gcoop.GetEl("HdMemberNo").value = objdw_main.GetItem(r, "member_no");
                objdw_main.AcceptText();
                jsPostMember();
            }
           
            if (Gcoop.GetEl("HdIsPostBack").value == "false") {
                SheetLoadComplete();
            }

        }
        function MainError(s, r, c, v) {
            return 1;
        }
        function OnDwMainClick(s, r, c) {

            if (c == "b_searchmem") {
                Gcoop.OpenDlg('630', '650', 'w_dlg_sl_member_search.aspx', '');
            }
            else if (c == "b_searchcoll") {
                Gcoop.OpenDlg('630', '650', 'w_dlg_sl_member_search_regloan.aspx', '');
            }

        }

        function GetValueMbColl(memberno_coll, membname_coll) {

            objdw_main.SetItem(1, "reqgrt_memno", memberno_coll);
            Gcoop.GetEl("Hdreqgrt_memno").value = memberno_coll;
            objdw_main.AcceptText();
            jsPostMemberColl();
            // jsPostMember();
        }
        function GetValueFromDlg(memberno) {

            objdw_main.SetItem(1, "member_no", memberno);
            objdw_main.AcceptText();
            Gcoop.GetEl("HdMemberNo").value = memberno;
            jsPostMember();
        }

        function SheetLoadComplete() {
            if (Gcoop.GetEl("HdIsPostBack").value != "true") {

                Gcoop.SetLastFocus("member_no_0");
                Gcoop.Focus();
                objdw_main.SetItem(r, "member_no", Gcoop.GetEl("HdMemberNo").value);
            }
        }
        function dw_list_Click(sender, rowNumber, objectName) {

            Gcoop.GetEl("Hfloancontract_no").value = objdw_list.GetItem(rowNumber, "loanrequest_docno");

            initlist();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:TextBox ID="TextBox2" runat="server" Height="31px" Width="159px" Visible="false"></asp:TextBox>
     <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_sl_loanyear_listbymem"
        LibraryList="~/DataWindow/shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientFormatting="True"
        ClientEventClicked="dw_list_Click" Visible="false">
    </dw:WebDataWindowControl>
    <table style="width: 100%;">
        <tr>
            <td valign="top" colspan="2">
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanyear_cancel"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemDataWindowChange" ClientEventClicked="OnDwMainClick"
                    ClientEventItemError="MainError">
                </dw:WebDataWindowControl>
            </td>
        </tr>
       
    </table>
    <asp:Literal ID="LtXmlKeeping" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlReqloop" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlLoanDetail" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlOtherlr" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtRunningNo" runat="server" Visible="False"></asp:Literal>
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HFloanreq_amt" runat="server" />
    <asp:HiddenField ID="Hdreqgrt_memno" runat="server" />
    <asp:HiddenField ID="Hloanrequest_docno" runat="server" />
     <asp:HiddenField ID="Hfloancontract_no" runat="server" />
      <asp:HiddenField ID="HfisCalInt" runat="server" />

     
</asp:Content>
