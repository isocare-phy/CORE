<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_give_child_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="10%">
                    ผู้สมรสชื่อ :
                </td>
                <td width="40%">
                        <asp:TextBox ID="f_name" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td width="10%">
                    ตำแหน่ง :
                </td>
                <td width="40%">
                        <asp:TextBox ID="occupation" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    สังกัด :
                </td>
                <td colspan ="3">
                    <div>
                        <asp:TextBox ID="empgroup" runat="server" Style="text-align: center"></asp:TextBox>
                        <div>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    ชื่อสำนักงาน :
                </td>
                <td width="40%">
                        <asp:TextBox ID="office" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td width="10%">
                    ที่ตั้ง :
                </td>
                <td width="40%">
                        <asp:TextBox ID="location" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    จดทะเบียนสมรสที่เขต :
                </td>
                <td width="40%">
                        <asp:TextBox ID="regis_amphur" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td width="10%">
                    เลขทะเบียน :
                </td>
                <td width="40%">
                        <asp:TextBox ID="regis_no" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="10%">
                    วันที่จดทะเบียน :
                </td>
                <td width="40%">
                        <asp:TextBox ID="regis_date" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td width="10%">
                 
                </td>
                <td width="40%">
                    
                </td>
            </tr>
            <tr>
                <td colspan = "2">
                    ภาพถ่ายสูติบัตร :
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="page" runat="server" Style="text-align: center"></asp:TextBox>
                        <div>
                </td>
                <td>
                    ฉบับ
                </td>
            </tr>
            <tr>
                <td colspan = "2">
                    หลักฐานอื่นๆ :
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="remark" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                        <div>
                </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td colspan = "2">
                    ขอรับเงินช่วยเหลือ :
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="approve_money" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                        <div>
                </td>
                <td>
                    บาท
                </td>
            </tr>
            <tr>
                <td colspan = "2">
                    ตั้งแต่เดือน :
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="month" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                    <div>
                </td>
                <td>
                    
                </td>
            </tr>
            <tr>
                <td colspan = "2">
                    บุตรอายุไม่เกิน 18 ปี และยังไม่บรรลุนิติภาวะโดยการสมรส จำนวน :
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="desc" runat="server" Style="text-align: center" ></asp:TextBox>
                        <div>
                </td>
                <td>
                    คน
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
