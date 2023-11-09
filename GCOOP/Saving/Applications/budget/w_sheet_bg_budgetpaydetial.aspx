<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_bg_budgetpaydetial.aspx.cs"
    Inherits="Saving.Applications.budget.w_sheet_bg_budgetpaydetial" Title="Untitled Page" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript%>
    <%=initPayDetail%>
    <script type="text/javascript">
        function OnDwMainButtonClicked(sender, rowNumber, buttonName){
            if(buttonName == "cb_ok"){
                initPayDetail();
            }
        }
        
        function OnDwMainItemChanged(sender, rowNumber, colunmName, newValue){
            objDwMain.SetItem(rowNumber, colunmName, newValue);
            objDwMain.AcceptText();
            return 0;
        }
        
        function Validate(){
            return confirm("ต้องการบันทึกข้อมูล ใช่หรือไม่?");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <asp:Label ID="Label1" runat="server" Text="ตัดจ่ายงบประมาณ" Font-Bold="True" Font-Names="Tahoma"
        Font-Size="16px" Font-Underline="True" />
    <br />
    <br />
    <dw:WebDataWindowControl ID="DwMain" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
        AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_bg_budgetpaydetail_main"
        LibraryList="~/DataWindow/budget/budget.pbl" ClientFormatting="True" ClientEventButtonClicked="OnDwMainButtonClicked"
        ClientEventItemChanged="OnDwMainItemChanged">
    </dw:WebDataWindowControl>
    <table>
        <tr>
            <td>
                <table width="700px">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="รายละเอียด" Font-Bold="True" Font-Names="Tahoma"
                                Font-Size="14px" Font-Underline="True" />
                        </td>
                        <td align="right">
                            <span onclick="OnClickDeleteRow()" style="cursor: pointer;">
                                <asp:Label ID="LbDel2" runat="server" Text="ลบแถว" Font-Bold="False" Font-Names="Tahoma"
                                    Font-Size="14px" Font-Underline="True" ForeColor="Red" /></span>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" Height="400px" BorderStyle="Ridge">
                    <dw:WebDataWindowControl ID="DwDetail" runat="server" AutoRestoreContext="False"
                        AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                        DataWindowObject="d_bg_budgetpaydetail_detail" LibraryList="~/DataWindow/budget/budget.pbl"
                        ClientFormatting="True" Height="400px" ClientEventItemChanged="OnDwDetailItemChanged"
                        ClientEventClicked="OnDwDetailClicked">
                    </dw:WebDataWindowControl>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
