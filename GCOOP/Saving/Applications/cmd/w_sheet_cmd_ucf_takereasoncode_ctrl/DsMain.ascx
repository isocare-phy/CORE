<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_takereasoncode_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 10px">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width:500px;">
            <tr>
                 <td width="15%">
                    <div>
                        <span>รายการเหตุผลการเบิก :</span>
                    </div>
                </td>
                <td width="25%">
                     <div>
                        <asp:TextBox ID="takereason_desc" runat="server" Style="text-align: left" ></asp:TextBox>
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