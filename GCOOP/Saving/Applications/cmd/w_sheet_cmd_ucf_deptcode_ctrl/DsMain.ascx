<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_deptcode_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 10px">
    <EditItemTemplate>
        <table class="DataSourceFormView"  style="width:500px;">
            <tr>
                 <td width="10%">
                    <div>
                        <span>ชื่อแผนก :</span>
                    </div>
                </td>
                <td width="30%">
                     <div>
                        <asp:TextBox ID="dept_desc" runat="server" Style="text-align: left" ></asp:TextBox>
                    </div>
                </td> 
                <td width="10%">
                    <div>
                        <span>ตัวย่อ :</span>
                    </div>
                </td>
                <td width="15%">
                     <div>
                        <asp:TextBox ID="dept_abb" runat="server" Style="text-align: center" ></asp:TextBox>
                    </div>
                </td>             
            </tr>
        </table>
        <tr>
        <td colspan="5" style="text-align:center;">
                        <span style="color:Red;font-size:12px;">* หมายเหตุ กรอกข้อมูล แล้วกดปุ่ม Save เพื่อทำการบันทึกข้อมูล </span>
                  </td>
                  </tr>
    </EditItemTemplate>
</asp:FormView>