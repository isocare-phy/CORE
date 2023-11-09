<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_sl_EstimateCredit_by_bank.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_EstimateCredit_by_bank" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=jsEstimate%>
<script type="text/javascript">
    function DialogLoadComplete() {
        var chkStatus = Gcoop.GetEl("HfChkStatus").value;
        if (chkStatus == "1") {
            try {
                Gcoop.GetEl("HfChkStatus").value = "";
                if (confirm("ต้องการปิดหน้าจอนี้ ")) {
                    parent.CloseDLG();
                }

            } catch (Err) {
                alert("Error Dlg");
                //window.close();
                parent.RemoveIFrame();
            }
            //            window.opener.CloseDLG();
            //            window.close();
        }
    }
    function OnCloseClick() {
        //        parent.removeIFrame();
        //        alert(".........");
        //closeWebDialog();
        parent.RemoveIFrame();
    }
    function ChangedEstimate(s, r, c, v) {

        if (c == "loanrequest_amt" || c == "period") {        
            objdw_estimate.SetItem(r, c, v);
            objdw_estimate.AcceptText();
            var loanrequest_amt = objdw_estimate.GetItem(r, "loanrequest_amt");
            var period = objdw_estimate.GetItem(r, "period");
            if (loanrequest_amt != null || loanrequest_amt != "" || period == "" || period != null) {
                jsEstimate();
            }
           
        }
     }
    </script>
    <style type="text/css">
        .newStyle1
        {
            font-family: Tahoma;
            font-size: 12px;
            color: #008080;
            float: right;
        }
        .style1
        {
            width: 151px;
        }
        .newStyle2
        {
            float: left;
            color: Red;
            font-weight: bold;
            font-family: tahoma;
            font-size: 12px;
        }
        .newStyle3
        {
            font-family: tahoma;
            font-size: 12px;
            font-weight: bold;
            float: left;
        }
        .newStyle4
        {
            font-family: tahoma;
            font-size: 12px;
            color: #008080;
            float: none;
            clip: rect(auto, 15px, auto, 15px);
        }
        .newStyle5
        {
            font-family: tahoma;
            font-size: 20px;
            background-color: Green;
            color: Yellow;
            font-weight: bold;
            right: 15px;
            left: 15px;
        }
         .newStyle6
        {
            float: left;
            color: Red;
            font-weight: bold;
            font-family: tahoma;
            font-size: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table>
        <%--<tr>
            <td>
                  <table style="width: 100%;">
                  <tr>
                        <td class="style1">
                            <asp:Label ID="Label5" runat="server" Text="เปอร์เซ็นเงินเดือน" CssClass="newStyle4"></asp:Label>
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label3" runat="server" Text="เงินเดือน ::" CssClass="newStyle1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox4" Width="90px" runat="server" CssClass="newStyle3"></asp:TextBox>
                        </td>
                        <td>
                         <asp:TextBox ID="TextBox6" Width="90px" runat="server" CssClass="newStyle3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:TextBox ID="TextBox1" Width="50px" runat="server" CssClass="newStyle5"></asp:TextBox>
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label1" runat="server" Text="% เงินเดือนคงเหลือ ::" CssClass="newStyle1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" Width="90" runat="server" CssClass="newStyle3"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="Button1" runat="server" Text="คำนวน" OnClick="Button1_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label4" runat="server" Text="ยอดหักจากสหกรณ์ ::" CssClass="newStyle1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox5" Width="90px" runat="server" CssClass="newStyle3"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;
                        </td>
                        <td class="style1">
                            <asp:Label ID="Label2" runat="server" Text="เงินเดือนคงเหลือ ::" CssClass="newStyle1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox3" Width="90px" runat="server" CssClass="newStyle6"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_head" runat="server" DataWindowObject="d_sl_loanrequest_monthpay"
                    LibraryList="~/DataWindow/shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    Style="top: 0px; left: 0px" >
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                <dw:WebDataWindowControl ID="dw_detail" runat="server" DataWindowObject="d_sl_loanrequest_monthpaydet"
                    LibraryList="~/DataWindow/shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    Style="top: 0px; left: 0px">
                </dw:WebDataWindowControl>
            </td>
        </tr>--%>
        <tr>
            <td>


                <dw:WebDataWindowControl ID="dw_estimate" runat="server" DataWindowObject="d_sl_estimatecredit_by_bank"
                    LibraryList="~/DataWindow/shrlon/sl_loan_requestment_cen.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="ChangedEstimate" 
                    Style="top: 0px; left: 0px">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
</asp:Content>
