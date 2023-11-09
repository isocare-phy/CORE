<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_member_proposedregister.aspx.cs" 
Inherits="Saving.Applications.hr.w_sheet_hr_member_proposedregister" ValidateRequest="false" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%= initJavaScript %>  
<%= postAddDistance%>
<%= postEditDistance %> 
<%= postSaveEdit %>
<%= postDelete %>
<%= postRefresh%>
<%= postShowItemMain%>  
<%= postDeleteRow%>
<%= postSearchItemMain %>


    <script type="text/javascript">


        function Validate() {
            objDwMain.AcceptText();
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function MenubarOpen() {
            Gcoop.OpenDlg(780, 600, "w_dlg_hr_member_proposed_search.aspx", "");
        }
        function GetValueEmplid(seq_empid) {
            objDwMain.SetItem(1,"seq_empid", Gcoop.Trim(seq_empid));
            objDwMain.AcceptText();
            postSearchItemMain();
        }



        function OnButtonClicked(s, row, oName) {
            if (oName == "b_search") {
                MenubarOpen();
            }
            else if (oName == "b_del") {
                var seq_empid = "";
               
                seq_empid = objDwMain.GetItem(1, "seq_empid");
               
                objDwMain.SetItem(1, "seq_empid", seq_empid);
                objDwMain.AcceptText();
                Gcoop.GetEl("HdfEmplid").value = seq_empid;
                if (seq_empid == "" || seq_empid == null || seq_empid == "Auto") {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else {
                    if (confirm("คุณต้องการลบรายการ " + seq_empid + " ใช่หรือไม่?")) {
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }

       
        function MenubarNew() {
            if (confirm("ยืนยันการล้างข้อมูลบนหน้าจอ")) {
                postRefresh();
            }
            return 0;
        }
        



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
   <table style="text-align="left">  
        <tr align="left">
            <td valign="top">
                <div id="tab1" style="visibility: visible; position: absolute;">
                    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="hr_member_reqproposedregister"
                        LibraryList="~/DataWindow/hr/hr.pbl" ClientScriptable="True" Height="440px"
                        Width="850px" ClientEventButtonClicked="OnButtonClicked" AutoSaveDataCacheAfterRetrieve="True"
                        AutoRestoreDataCache="True" AutoRestoreContext="False" ClientEventItemChanged="OnItemChangeMain"
                        ClientEventClicked="OnDwMainClick" ClientFormatting="True">
                    </dw:WebDataWindowControl>
                </div>
            </td>
        </tr>

    </table>

    <asp:HiddenField ID="HdfEmplid" Value="" runat="server" />
    <asp:Panel ID="Panel1" runat="server">
    </asp:Panel>
<%--            function DwItemButtonsearchclick(sender, rowNumber, buttonName) {
            if (buttonName == "b_searchclick") {
                var mb_membernumber = "";
                mb_membernumber = objDwMain.GetItem(rowNumber, "mb_membernumber");
                if (confirm("ต้องการค้นหาเลขที่ " + mb_membernumber + " ใช่หรือไม่ ")) {
                    postShowItemMain();
                }
                return 0;
            }

        }--%>
</asp:Content>
