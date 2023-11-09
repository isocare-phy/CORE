<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="ws_hr_worktime_imp.aspx.cs" Inherits="Saving.Applications.hr.ws_hr_worktime_imp_ctrl.ws_hr_worktime_imp" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">
       var dsMain = new DataSourceTool;
      
       function OnDsMainClicked(s, r, c) {
           if (c == "b_process") {
               var isConfirm = confirm("ยืนยันการ Import ข้อมูล");
               if (isConfirm) {
                   JsPostProcess();
               }
           } else if (c == "b_delete") {
               var isConfirm = confirm("ยืนยันการลบข้อมูลที่ Import");
               if (isConfirm) {
                   JsPostDelete();
               }
           }
       }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <table class="DataSourceFormView">
         <tr>
            <td style="width:180px;">
                <span>File Path : </span>
            </td>
            <td colspan="2">
                <asp:FileUpload ID="txtInput" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <span>*** หมายเหตุ : </span>
            </td>
            <td style="width:180px;">
                <div style="font-size:16px;background-color:Yellow"><a href="../../../filecommon/Sample_hr_Excal.xlsx">ดาวน์โหลดไฟล์ตัวอย่าง</a></div>
            </td>
            <td>
                **** ไฟล์ที่จะ Import จะต้องเป็นไฟล์ .xlsx เท่านั้น
            </td>
        </tr>
    </table>
    <%--<table class="DataSourceFormView">
        <tr>
            <td style="width: 150px;">
                <span>วันที่ : </span>
            </td>
            <td>
                <asp:TextBox ID="entry_date" runat="server" Width="100px" Style="text-align: center;"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <span>ประเภทรายการ : </span>
            </td>
            <td>
                <asp:DropDownList ID="type_code" runat="server" Width="244px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <span>File Path : </span>
            </td>
            <td>
                <asp:FileUpload ID="txtInput" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <span>*** หมายเหตุ : </span>
            </td>
            <td>
                ไฟล์ที่จะ Import จะต้องเป็นไฟล์ .xlsx เท่านั้น
            </td>
        </tr>
        <tr>
            <td>
                <span>ลบข้อมูลที่ Import : </span>
            </td>
            <td>
                <asp:Button ID="b_delete" runat="server" Text="ลบข้อมูลที่ Import" Style="width: 244px;"
                    OnClientClick="return fnConfirmDelete()" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <a href="../../../filecommon/Sample_Deposit_Excal.xlsx">ไฟล์ตัวอย่าง</a>
            </td>
        </tr>
    </table>--%>
</asp:Content>
