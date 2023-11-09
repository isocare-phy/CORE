<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_as_member_dead_accident.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_member_dead_accident" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postRefresh %>
    <%=postChangeAmt %>
    <%=postRetreiveDwMem %>
    <%=postRetrieveDwMain%>
    <%=postGetMemberDetail %>
    <%=postRetrieveBankBranch %>
    <%=postDelete%>
    <%=postChangeAge%>
    <%=postPopupReport%>

    <script type="text/javascript">

        function Validate() {
           
                return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(630, 550, "w_dlg_as_memberdead_acc_list.aspx", "");
        }

        function DwMemButtonClick(sender, rowNumber, buttonName) {
            if (buttonName == "b_search") {
                Gcoop.OpenDlg(610, 550, "w_dlg_sl_member_search.aspx", "");
            }
        }

        function DwDetailButtonClicked(sender, rowNumber, buttonName) {
            if (buttonName == "b_del") {
                objDwMain.AcceptText();
                var member_no = objDwMem.GetItem(rowNumber, "member_no");
                if (confirm("ยืนยันการลบข้อมูลของเลขสมาชิกที่ " + member_no)) {
                    postDelete();
                }
            }

        }

        function GetValueFromDlg(memberno, pre_name, memb_name, memb_surname, membgroup_desc, membgroup_code) {
            objDwMem.SetItem(1, "member_no", member_no);
            Gcoop.GetEl("HfMemberNo").value = memberno;

            postRetreiveDwMem();
        }

        function GetValueFromDlgList(assist_docno, capital_year, member_no) {
            Gcoop.GetEl("HfMemNo").value = member_no;
            Gcoop.GetEl("HfAssistDocNo").value = assist_docno;
            Gcoop.GetEl("HfCapitalYear").value = capital_year;
            postRetrieveDwMain();
        }

        function DwMainItemChange(sender, rowNumber, columnName, newValue) {
            objDwMain.SetItem(rowNumber, columnName, newValue);
            objDwMain.AcceptText();
            if (columnName == "assist_amt") {
                //objDwSchool.SetItem(1, "assist_amt", newValue);
                postRefresh();
            }
            else if (columnName == "expense_bank") {
                objDwMain.AcceptText();
                postRetrieveBankBranch();
            }
            else if (columnName == "req_tdate") {
                objDwMain.AcceptText();
                Gcoop.GetEl("HdReqDate").value = newValue;
                postChangeAge();

            }
        }

        function DwMemItemChange(sender, rowNumber, columnName, newValue) {
            objDwMem.SetItem(rowNumber, columnName, newValue);
            objDwMem.AcceptText();
            if (columnName == "member_no") {
                newValue = Gcoop.StringFormat(newValue, "00000000");
                Gcoop.GetEl("HfMemberNo").value = newValue;

                postGetMemberDetail();
            }
        }

        function DwDetailItemChange(sender, rowNumber, columnName, newValue) {
          
            if (columnName == "member_dead_tdate") {
                objDwDetail.SetItem(rowNumber, columnName, newValue);
                objDwDetail.AcceptText();
                Gcoop.GetEl("hdate1").value = newValue;
                    
                postChangeAmt();
            }
        }

        function Alert() {
            alert("ไม่มีเลขที่สมาชิกนี้");
        }

        function MenubarNew() {
            window.location = "";
            Gcoop.SetLastFocus("member_no_0");
        }
        function SheetLoadComplete() {
            Gcoop.SetLastFocus("member_no_0");
            Gcoop.Focus();
        }
        function OnClickLinkNext() {
            postPopupReport();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"> </asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>

    <table style="width: 100%;">
        <tr>
            <td align="left">
                <span style="cursor: pointer" onclick="OnClickLinkNext();">พิมพ์ใบสำคัญจ่าย
                    </span>
            </td>
            <td align="right">
            </td>
        </tr>
    </table>

    <div>
    <dw:WebDataWindowControl ID="DwMem" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmember" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventButtonClicked="DwMemButtonClick" ClientEventItemChanged="DwMemItemChange">
    </dw:WebDataWindowControl> 
    </div>

    <br />

    <div>
    <dw:WebDataWindowControl ID="DwMain" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmaster" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwMainItemChange">
    </dw:WebDataWindowControl>
    </div>

    <br />

     <div>
    <dw:WebDataWindowControl ID="DwDetail" runat="server" LibraryList="~/DataWindow/app_assist/as_capital.pbl"
        DataWindowObject="d_as_reqmem_dead_accident" ClientScriptable="True" AutoRestoreContext="false"
        AutoRestoreDataCache="true" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
        ClientEventItemChanged="DwDetailItemChange" ClientEventButtonClicked="DwDetailButtonClicked">
    </dw:WebDataWindowControl>
     </div>

    <asp:HiddenField ID="HfMemNo" runat="server" />
    <asp:HiddenField ID="HfAssistDocNo" runat="server" />
    <asp:HiddenField ID="HfCapitalYear" runat="server" />
    <asp:HiddenField ID="HfMemberNo" runat="server" />
    <asp:HiddenField ID="HfReqSts" runat="server" />
    <asp:HiddenField ID="HfDate" runat="server" />
    <asp:HiddenField ID="hdate1" runat="server" />
    <asp:HiddenField ID="HdReqDate" runat="server" />
   <asp:HiddenField ID="HdOpenIFrame" runat="server" /> 
</asp:Content>
