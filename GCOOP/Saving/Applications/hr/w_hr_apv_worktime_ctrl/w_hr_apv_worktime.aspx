<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_hr_apv_worktime.aspx.cs" Inherits="Saving.Applications.hr.w_hr_apv_worktime_ctrl.w_hr_apv_worktime" %>
<%@ Register src="DsSearch.ascx" tagname="DsSearch" tagprefix="uc1" %>
<%@ Register src="DsMain.ascx" tagname="DsMain" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript">
     var dsSearch = new DataSourceTool;
     var dsMain = new DataSourceTool;

     function OnDsSearchClicked(s, r, c) {
         if (c == "b_search") {
             dsSearch.SetRowFocus(r);
             PostWORKTIME();
         }
     }

     function SheetLoadComplete() {

     }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
 <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />

    <div>
        <uc1:DsSearch ID="dsSearch" runat="server" />
    
    </div>

     <div>
         <uc2:DsMain ID="dsMain" runat="server" />
    </div>

    <asp:HiddenField ID="hdGetDate" runat="server" Value="" />
</asp:Content>
