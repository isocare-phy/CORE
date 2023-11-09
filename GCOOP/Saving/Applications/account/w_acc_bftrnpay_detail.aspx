<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_acc_bftrnpay_detail.aspx.cs" Inherits="Saving.Applications.account.w_acc_bftrnpay_detail" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <%=initJavaScript%>
    <%=jsPostDetail%>
    <%=jsPostDeleteDetail%>
    <%=jsRetrieve%>

    <script type="text/javascript">
    function OnDwHeadBuntonClick(s, r, c, v) {
            var trnDate = "";
            trnDate = objDw_Head.GetItem(1, "trnpay_tdate"); // ส่งชื่อฟิวส์วันที่ไทยเข้าไป
            if (trnDate == "" || trnDate == null) {
                alert("กรุณากรอกข้อมูลวันที่โอนรับชำระ")
            }
            else {
                if (c == "b_new") {
                    Gcoop.OpenIFrame(900, 600, "w_dlg_acc_bktrnpay_edit.aspx", "?trnDate=" + trnDate + "&payType=" + c); 
                }
            }
        }

    function OnDw_mainClick(s, r, c) {
        if (c == "paytrnbank_docno") {
            var Acno = "";
            Acno =  objDw_main.GetItem(r, "paytrnbank_docno");
            Gcoop.GetEl("HdAcNo").value = Gcoop.Trim(Acno);
            Gcoop.GetEl("HdRowMainClick").value = r + "";
            jsPostDetail();
            }
    }

    function OnDwMainClickButton(s, r, c) {
        if (c == "b_edit") {
            var Acno = "";
            Acno = objDw_main.GetItem(r, "paytrnbank_docno");
            var trnDate = "";
            trnDate = objDw_Head.GetItem(1, "trnpay_tdate"); // ส่งชื่อฟิวส์วันที่ไทยเข้าไป
            if (trnDate == "" || trnDate == null) {
                alert("กรุณากรอกข้อมูลวันที่โอนรับชำระ")
            }
            else {
                Gcoop.OpenIFrame(900, 600, "w_dlg_acc_bktrnpay_edit.aspx", "?trnDate=" + trnDate + "&payType=" + c + "&Acno=" + Acno);
            }
        }

        else if (c == "b_del") {
            var Acc_no = objDw_main.GetItem(r, "paytrnbank_docno");
            var isConfirm = confirm("ต้องการลบข้อมูล " + Acc_no + " ใช่หรือไม่ ?");
            if (isConfirm) {
                Gcoop.GetEl("Hdrow").value = r + "";
                jsPostDeleteDetail();
            }
        }
    }

    function OnDwHeadItemChange(s, r, c, v) {
        if (c == "trnpay_tdate") {
            if (v == "" || v == null) {
                alert("กรุณากรอกข้อมูลวันที่รายการ")
            }
            else {
                s.SetItem(1, "trnpay_tdate", v);
                s.AcceptText();
                s.SetItem(1, "trnpay_date", Gcoop.ToEngDate(v));
                s.AcceptText();
                jsRetrieve();
            }
        }
        return 0;
    }
        
    </script>

    <style type="text/css">
        .style2
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
  <p>
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <table style="width: 100%; font-size: small;">
        <tr>
                <td class="style2">
                    รายการโอนชำระหนี้
                </td>

            </tr>
            <tr>
            <td valign="top">
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="Ridge" >
                        <dw:WebDataWindowControl ID="Dw_Head" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_acc_trnpay" LibraryList="~/DataWindow/account/acc_post_pay.pbl"
                        ClientEventButtonClicked="OnDwHeadBuntonClick"  ClientFormatting="True" 
                        ClientEventItemChanged="OnDwHeadItemChange">    
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
                </tr>
                <tr>
                <td valign="top">
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Ridge" Height="250px" Width="750px" ScrollBars="Auto" >
                        <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_ac_bktrnpay_list_new" LibraryList="~/DataWindow/account/acc_post_pay.pbl"
                        ClientEventClicked="OnDw_mainClick" ClientEventButtonClicked="OnDwMainClickButton" 
                        ClientFormatting="True">
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
                </tr>
                <tr>
                <td valign="top">
                    <asp:Panel ID="Panel2" runat="server" BorderStyle="Ridge" Height="150px" Width="750px" ScrollBars="Auto" >
                        <dw:WebDataWindowControl ID="Dw_Detail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" 
                        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_ac_bktrnpay_detail" LibraryList="~/DataWindow/account/acc_post_pay.pbl"
                        ClientEventButtonClicked="Delete_DwDetailClick" 
                        ClientFormatting="True" >
                        </dw:WebDataWindowControl>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="HdAcNo" runat="server" />
                    <asp:HiddenField ID="HdIsFinished" runat="server" />
                    <asp:HiddenField ID="HdBotid" runat="server" />
                </td>
                <td valign="top">
                    <br />
                    <asp:HiddenField ID="HdRowDetail" runat="server" />
                    <asp:HiddenField ID="Hdrow" runat="server" />
                    <asp:HiddenField ID="Hdrow_mas" runat="server" />
                    <asp:HiddenField ID="HdRowMainClick" runat="server" />
                    <br />
                </td>
            </tr>
        </table>
        <br />
    </asp:Content>

