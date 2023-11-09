<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_constant_fold.aspx.cs" Inherits="Saving.Applications.arc.w_sheet_constant_fold" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
<script type="text/javascript">
    function ItemType_Click(sender, rowNumber, objectName) {
        //alert("constatnt change JS");
        //alert("ROW = " + rowNumber + "|" + "OBJ =" + objectName)
        Gcoop.GetEl("HdrowItemtype").value = rowNumber;
        postBranchshow();
    }

    function OnAddRowi(cmds) {
        if (cmds == "Foldadd") {
            Gcoop.OpenDlg(475, 550, "w_dlg_itemtype_folder.aspx");
        }
        
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
<asp:Literal ID="LtAlert" runat="server"></asp:Literal>

                 <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Height="1000px" Width="770px">
<dw:WebDataWindowControl ID="DwFolder" runat="server" DataWindowObject="dw_hr_iyemtype_folder"
                    LibraryList="~/DataWindow/arc/hr_constant.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="DwFolder_Change"
                    ClientEventClicked="DwFolder_Click">
                </dw:WebDataWindowControl>
                </asp:Panel>
                 <span  onclick="OnAddRowi('Foldadd')" style="font-size: small; color: #808080; cursor: pointer; ">
                    แก้ข้อมูล</span> 
                
                 <br />
                 
                 <asp:HiddenField ID="HdrowItemtype" runat="server" />
                 <asp:HiddenField ID="HdrowDomain" runat="server" />
                 <asp:HiddenField ID="HdrowGroup" runat="server" />
                 <asp:HiddenField ID="HdItemType" runat="server" />
                 <asp:HiddenField ID="HdDomain" runat="server" />
                 <asp:HiddenField ID="HdGroup" runat="server" />

                 <asp:HiddenField ID="Hddomain_id" runat = "server" />
                 <asp:HiddenField ID="Hddomain_name" runat = "server" />
                

</asp:Content>