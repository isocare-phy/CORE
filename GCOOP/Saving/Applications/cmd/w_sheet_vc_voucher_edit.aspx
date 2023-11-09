<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_vc_voucher_edit.aspx.cs" Inherits="Saving.Applications.account.w_sheet_vc_voucher_edit1" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%  Page_LoadComplete(); %>
    <style type="text/css">
        .style1
        {
            width: 193px;
            font-size: small;
        }
    </style>

    <script type="text/javascript">
        {

            //ฟังก์ชันการ get ค่าจาก dw_list แล้วส่ง voucher_no ไปให้กับ dw_main และ dw_detail
            function Click_dw_list(sender, rowNumber, objectName) {
                var voucher_no = objdw_list.GetItem(rowNumber, "voucher_no");
                window.location = "?voucher_no=" + voucher_no;

            }
           function mypopup()
             {
               mywindow = window.open("w_dlg_vc_voucher_edit.aspx", "mywindow",
               "location=1,status=1,scrollbars=1,width=1000,height=500");
               mywindow.moveTo(100,100);
            } 


        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <p>
        <input id="Button1" type="button" value="เพิ่ม Voucher ใหม่" 
            onclick="mypopup()" /><br />
        <table style="width: 100%;">
            <tr>
                <td class="style1" valign="top">
                    ประจำวันที่<br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txt_vcdate" runat="server" AutoPostBack="True" OnTextChanged="txt_vcdate_TextChanged"></asp:TextBox>
                    <asp:HiddenField ID="hidden_search" runat="server" />
                    <br />
                    <br />
                </td> 
                <td valign="top">
                    รายละเอียด Voucher<br />
                    <dw:WebDataWindowControl ID="dw_main" runat="server" ClientScriptable="True" DataWindowObject="d_vc_vcedit_vchead"
                        LibraryList="~/DataWindow/vcvcedit.pbl">
                    </dw:WebDataWindowControl>
                    <br />
                </td>
            </tr>
            <tr>
                <td class="style1" valign="top">
                    รายการ Voucher<br />
                    <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_vc_vcedit_vclist"
                        LibraryList="~/DataWindow/vcvcedit.pbl" ClientScriptable="True" ClientEventClicked="Click_dw_list">
                    </dw:WebDataWindowControl>
                </td>
                <td valign="top">
                    รายการ Voucher<br />
                    <dw:WebDataWindowControl ID="dw_detail" runat="server" ClientScriptable="True" DataWindowObject="d_vc_vcedit_vcdetail"
                        LibraryList="~/DataWindow/vcvcedit.pbl" Width="500px">
                    </dw:WebDataWindowControl>
                    <br />
                </td>
            </tr>
        </table>
    </p>
</asp:Content>
