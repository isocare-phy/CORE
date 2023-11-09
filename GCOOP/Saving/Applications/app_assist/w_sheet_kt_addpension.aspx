<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_kt_addpension.aspx.cs" Inherits="Saving.Applications.app_assist.w_sheet_kt_addpension" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=postDate%>
    <%=postDate2%>
    <%=jsRefresh%>
    <script type="text/javascript">
        function PostRefresh() {
            jsRefresh();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    &nbsp;
    <asp:Panel runat="server" BackColor="#CCCCFF" Height="221px" Width="543px">
        <br />
        <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
        <br />
        <div align="center">
            <asp:Label ID="Label1" runat="server" Text="กดปุ่ม ดึงข้อมูล เพื่อดึงข้อมูลสมาชิกสวัสดิการ"
                ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Button ID="Add_PS" runat="server" Text="ดึงข้อมูล" OnClick="Add_PS_Click" OnClientClick="PostRefresh()" Height="53px"
                Width="115px" />
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="* หมายเหตุ ข้อมูลสมาชิกมีจำนวนมากจึงทำให้การคำนวณใช้เวลานาน"
                ForeColor="Red"></asp:Label>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdate" runat="server" />
    <asp:HiddenField ID="hdate2" runat="server" />
    
</asp:Content>
