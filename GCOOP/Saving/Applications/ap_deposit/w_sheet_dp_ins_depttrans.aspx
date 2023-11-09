<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_dp_ins_depttrans.aspx.cs"
    Inherits="Saving.Applications.ap_deposit.w_sheet_dp_ins_depttrans" Title="Untitled Page"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=postAccountNo%>
    <%=postDeptAmt%>
    <%=postNew%>
    <%=deleteDepttran%>
    
    <script type="text/javascript">
        function MenubarOpen() {
            Gcoop.OpenIFrame(900, 600, "w_dlg_dp_account_search.aspx", "");
        }

        function OnDwMainItemChanged(s, r, c, v) {
            if (c == "deptaccount_no") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postAccountNo();
            }
            else if (c == "deptitem_amt") {
                s.SetItem(r, c, v);
                s.AcceptText();
                postDeptAmt();

            }
            else if (c == "b_delete") {                
                s.SetItem(r, c, v);
                s.AcceptText();  
            }
        }
        function NewAccountNo(coopid, accNo) {
            objDwMain.SetItem(1, "deptaccount_no", Gcoop.Trim(accNo));
            objDwMain.AcceptText();
            postAccountNo();
        }
         function SheetLoadComplete() {
            Gcoop.SetLastFocus("deptaccount_no_0");
            Gcoop.Focus();
        }

        function Validate() {
            if (confirm("ยืนยันการบันทึกข้อมูล")) {
                
                return true;
            }
            return false;
        }

        function DelClick(sender, row, btnName) {
            if (btnName == "b_delete") {
                Gcoop.GetEl("HdRow").value = row;
                var isConfirm = confirm("ยืนยันการลบรายการ");
                if (isConfirm) {
                    deleteDepttran();
                }
                
            }
        }

        function OnDwMainClick(s, r, c) {
            //----------------------------
        }
           
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server" Text=""></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_dp_depttrans_insert_main"
        LibraryList="~/DataWindow/ap_deposit/dp_depttrans.pbl" ClientEventItemChanged="OnDwMainItemChanged"
        ClientEventClicked="OnDwMainClick" ClientFormatting="True">
    </dw:WebDataWindowControl>
     <br />
     <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Tahoma" Font-Size="12px"
        Text="ลบรายการ"></asp:Label>
    <br />    
     <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="d_dp_depttran_insert_detail"
        LibraryList="~/DataWindow/ap_deposit/dp_depttrans.pbl" AutoRestoreContext="False"
        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientEventButtonClicked = "DelClick" ClientScriptable="True"
        ClientFormatting="True">
     </dw:WebDataWindowControl>

    <asp:HiddenField ID="HSelect" runat="server" Value="01" />
    <asp:HiddenField ID="HdIsPostBack" runat="server" Value="false" />
    <asp:HiddenField ID="HdRowsel" runat="server" Value="false" />
    <asp:HiddenField ID="HfCoopid" runat="server" />
    <asp:HiddenField ID="HfCoopcontrol" runat="server" />   
    <asp:HiddenField ID="HdRow" runat="server" />  
</asp:Content>
