<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_ln_intfixchg.aspx.cs" Inherits="Saving.Applications.investment.w_sheet_ln_intfixchg" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postNewClear %>
    <%=postInit%>
    
    <script type="text/javascript">
        function OnDwMainButtonClick(s, r, c) {
            if (c == "b_init") {
                var loantype_code = objDw_main.GetItem(1, "loantype_code");
                if (loantype_code == "" || loantype_code == null) {
                    alert("กรุณาเลือกประเภทเงินกู้");
                }
                else {
                    var new_intrate = objDw_main.GetItem(1, "new_intrate");
                    if (new_intrate == "" || new_intrate == null) {
                        alert("กรุณากรอกอัตราดอกเบี้ยเงินกู้ที่ต้องการเปลี่ยน");
                    }
                    else {
                        objDw_main.AcceptText();
                        postInit();
                    }
                }
            }
            return 0;
        }

        function OnDwMainItemChange(s, r, c, v) {
            if (c == "contbefore_tdate") {
                objDw_main.SetItem(1, "contbefore_tdate", v);
                objDw_main.AcceptText();
                objDw_main.SetItem(1, "contbefore_date", Gcoop.ToEngDate(v));
                objDw_main.AcceptText();
            }
            else if (c == "intfixchg_tdate") {
                objDw_main.SetItem(1, "intfixchg_tdate", v);
                objDw_main.AcceptText();
                objDw_main.SetItem(1, "intfixchg_date", Gcoop.ToEngDate(v));
                objDw_main.AcceptText();
            }
        }

        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postNewClear();
            }
        }

        function Validate() {
            objDw_main.AcceptText();
            objDw_detail.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

     

        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td colspan="3">
                <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True" ClientScriptable="True"
                    DataWindowObject="d_lcsrv_intfixchg_option" LibraryList="~/DataWindow/investment/int_intfixchg.pbl"
                    ClientEventButtonClicked="OnDwMainButtonClick" ClientEventItemChanged="OnDwMainItemChange">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dw:WebDataWindowControl ID="Dw_detail" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" DataWindowObject="d_lcsrv_intfixchg_detail" LibraryList="~/DataWindow/investment/int_intfixchg.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <br />
    </asp:Content>
