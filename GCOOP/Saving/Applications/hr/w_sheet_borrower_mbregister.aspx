<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_borrower_mbregister.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_borrower_mbregister" %>



<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%= initJavaScript %>  
<%= postAddDistance%> 
<%= postEditDistance %> 
<%= postSaveEdit %>
<%= postDelete %> 
<%= postShowItemMain%>   
<%= postSelectItemMain%>




    <script type="text/javascript">


        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function DwItemButtonselect(sender, rowNumber, buttonName) {
            if (buttonName == "b_select") {

                var borrower_borrowerstatus = "";
                borrower_borrowerstatus = objDwListselect.GetItem(rowNumber, "borrower_borrowerstatus");
                postSelectItemMain();
                }
                return 0;
            }

        

        function DwItemButtonsearchclick(sender, rowNumber, buttonName) {
            if (buttonName == "b_searchclick") {

                var bor_membernumber = "";
                var borrower_borrowerstatus = "";
                bor_membernumber = objDwMain.GetItem(rowNumber, "bor_membernumber");
                borrower_borrowerstatus = objDwMain.GetItem(rowNumber, "borrower_borrowerstatus");
                
                if (confirm("ต้องการค้นหาเลขที่ " + bor_membernumber + " ใช่หรือไม่ ")) {
                    postShowItemMain();
                }
                return 0;
            }

        }


        
        function DwItemButtonClickbdelete(sender, rowNumber, buttonName) {

            if (buttonName == "b_delete") {
                var member_mbnumber = "";
                member_mbnumber = objDwListMemnew.GetItem(rowNumber, "member_mbnumber");
                if (confirm("คุณต้องการลบรายการ " + member_mbnumber + " ใช่หรือไม่?")) {
                    postDelete();
                }
                return 0;
            }
        }
        

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:Literal ID="LtAlert" runat="server"></asp:Literal>
   <table style="text-align="left">  

        <tr>
            <td align="left" style="width: 500px" text-align="left" >
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="borrower_memberregister"
                    LibraryList="~/DataWindow/hr/borrower_member.pbl" AutoRestoreContext="False" Width="750px"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemChanged">
                </dw:WebDataWindowControl>
            </td>
           </tr>
            <tr>
            <td align="left" style="width: 500px" text-align="left" >
                <dw:WebDataWindowControl ID="DwListselect" runat="server" DataWindowObject="borrower_memberregister_list"
                    LibraryList="~/DataWindow/hr/borrower_member.pbl" AutoRestoreContext="False" Width="600"  
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemChanged" ClientEventButtonClicked="DwItemButtonselect">
                </dw:WebDataWindowControl>
            </td>
           <tr>
            <td align="left" style="width: 500px" text-align="left" >
                <dw:WebDataWindowControl ID="DwListMemnew" runat="server" DataWindowObject="borrower_bor"
                    LibraryList="~/DataWindow/hr/borrower_member.pbl" AutoRestoreContext="False" Width="600"  
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemChanged" ClientEventButtonClicked="DwItemButtonsearchclick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfMemberNo" runat="server" />
   
<%--  ---%>
           
</asp:Content>
