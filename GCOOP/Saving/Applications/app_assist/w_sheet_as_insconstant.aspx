<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_as_insconstant.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_as_insconstant" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <%=initJavaScript%>
 <%=jsinsertrow%>
 <%=GetStatus%>

    <script type="text/javascript">
      function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        function MenubarNew() {
            window.location = Gcoop.GetUrl() + "Applications/app_assist/w_sheet_as_insconstant.aspx";
        }


        function OnInsert() {

            jsinsertrow();

        }
        function Changeinstype_code(s, r, c, v) {
            if (c == "instype_code") {                
                objdw_search.SetItem(r, c, v);
                objdw_search.AcceptText();        
                Gcoop.GetEl("Hstatus").value = v;
                Gcoop.GetEl("hrow").value = r + "";
//              alert(Gcoop.GetEl("Hstatus").value);
                // alert(v);
              GetStatus();
            } 

        }
        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <asp:HiddenField ID="Hlist" runat="server" Value="" />
    <asp:HiddenField ID="hidden_search" runat="server" />
    <asp:HiddenField ID="Hstatus" runat="server" Value="" />
    <asp:HiddenField ID="hrow" runat="server" />
    <asp:HiddenField ID="Hvtypecode" runat="server" Value="" />
    <asp:HiddenField ID="HdStatusRow" runat="server" Value="" />
    <asp:Panel ID="Panel1" runat="server">
    รายละเอียดประเภทประกันชีวิต
        <dw:WebDataWindowControl ID="dw_search" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" DataWindowObject="d_sk_typeprakun"
            LibraryList="~/DataWindow/app_assist/as_insconstant.pbl" ClientEventButtonClicked="Clickb_search"
             ClientEventItemChanged="Changeinstype_code">
        </dw:WebDataWindowControl>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
    ระดับวงเงินเอาประกัน
        <dw:WebDataWindowControl ID="dw_list" runat="server" DataWindowObject="d_sk_levelprakun"
            LibraryList="~/DataWindow/app_assist/as_insconstant.pbl" AutoRestoreContext="False"
            AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
            ClientEventClicked="selectRow">
        </dw:WebDataWindowControl>

       <%-- <span class="linkSpan" onclick="OnUpdate()" style="font-size: small; color:Green;
                float: right">บันทึกข้อมูล</span>  --%>
       <span class="linkSpan" onclick="OnInsert()" style="font-size: small; color:Red;
                float: left">เพิ่มแถว</span>
    </asp:Panel>
    
</asp:Content>