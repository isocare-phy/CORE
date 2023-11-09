<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpsubcode_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 28px">
    <EditItemTemplate>
        <table class="DataSourceFormView"  style="width:700px;">
            <tr>
                 <td width="20%">
                    <div>
                        <span>หมวดครุภัณฑ์ :</span>
                    </div>
                </td>
                <td width="30%">
                     <div>
                        <asp:DropDownList ID="durtgrp_code" runat="server" Style="text-align: left" ></asp:DropDownList>
                    </div>
                </td>
                </tr> 
                <tr>
                <td width="10%">
                    <div>
                        <span>หมวดย่อยครุภัณฑ์ :</span>
                    </div>
                </td>
                <td width="15%">
                     <div>
                        <asp:TextBox ID="durtgrpsub_desc" runat="server" Style="text-align: left" ></asp:TextBox>
                    </div>
                </td>  
                <td width="10%">
                    <div>
                        <span>ตัวย่อ :</span>
                    </div>
                </td>
                <td width="15%">
                     <div>
                        <asp:TextBox ID="durtgrpsub_abb" runat="server" Style="text-align: center" ></asp:TextBox>
                    </div>
                </td>    
                <td width="12%">
                    <div>
                        <span>ค่าเสื่อม (%) :</span>
                    </div>
                </td>
                <td width="15%">
                     <div>
                        <asp:TextBox ID="devalue_percent" runat="server" ToolTip="#,##0.00" Style="text-align: center" ></asp:TextBox>
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