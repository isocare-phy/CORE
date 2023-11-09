<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_constant_getting.aspx.cs" Inherits="Saving.Applications.arc.w_sheet_constant_getting" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<%=initJavaScript%>
 <%=postShowDomain%>
 <%=postShowGroup%>
 <%=postShowSubGroup%>
 <%=postBranchshow%>
<script type="text/javascript">
    function ItemType_Click(sender, rowNumber, objectName) {
        //alert("constatnt change JS");
        //alert("ROW = " + rowNumber + "|" + "OBJ =" + objectName)
        Gcoop.GetEl("HdrowItemtype").value = rowNumber;
        postBranchshow();
    }

    function OnAddRowi(cmds) {
        if (cmds == "Itemtype") {
            Gcoop.OpenDlg(375, 550, "w_dlg_itemtype_master_list.aspx");


        }
        else if (cmds == "Domain") {
            var itemtype = Gcoop.GetEl("HdItemType").value;
            //alert(itemtype);
            Gcoop.OpenDlg(600, 500, "w_dlg_itemtype_secondary.aspx", "?itemtype_id=" + itemtype);
        }
        else { }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
<asp:Literal ID="LtAlert" runat="server"></asp:Literal>

<dw:WebDataWindowControl ID="dw_ItemType" runat="server" DataWindowObject="dw_cmd_itemtype_master"
                    LibraryList="~/DataWindow/arc/hr_constant.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="ItemType_Change"
                    ClientEventClicked="ItemType_Click">
                </dw:WebDataWindowControl>
                <span  onclick="OnAddRowi('Itemtype')" style="font-size: small; color: #808080; cursor: pointer; ">
                    แก้ข้อมูล</span> 
                 <br />

                 <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="300px" Width="770px">
<dw:WebDataWindowControl ID="dw_Domain" runat="server" DataWindowObject="dw_hr_itemtype_secondary"
                    LibraryList="~/DataWindow/arc/hr_constant.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="Domain_Change"
                    ClientEventClicked="Domain_Click">
                </dw:WebDataWindowControl>
                 <span  onclick="OnAddRowi('Domain')" style="font-size: small; color: #808080; cursor: pointer; ">
                    แก้ข้อมูล</span> 
                 </asp:Panel>
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
