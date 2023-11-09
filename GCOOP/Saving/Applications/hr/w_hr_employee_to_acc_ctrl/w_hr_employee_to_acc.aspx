<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_hr_employee_to_acc.aspx.cs" Inherits="Saving.Applications.hr.w_hr_employee_to_acc_ctrl.w_hr_employee_to_acc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    function Post_Salary() {
        Post_Salary();
    }
    function Post_Update() {
        Post_Update();
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">

<asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table class="DataSourceFormView" style="width: 500px">
    <tr>
    <td colspan="2">
                <div>
                    <span>เลือกปี:</span></div>
            </td>
            <td colspan="2">
                <asp:TextBox ID="year" runat="server" Style="text-align: center;"></asp:TextBox>
            </td>
            <td width="30%">
                <input type="button" value="ตั้งเงินเดือนรายปี" style="width: 120px" onclick="Post_Salary()" />
            </td>
    </tr>
        <tr>
        <td colspan="2">
                <div>
                    </div>
            </td>
          <td colspan="2">
                
            </td>
            <td width="30%">
                <input type="button" value="update ข้อมูล emp" style="width: 120px" onclick="Post_Update()" />
            </td>
            
        </tr>
    </table>

</asp:Content>
