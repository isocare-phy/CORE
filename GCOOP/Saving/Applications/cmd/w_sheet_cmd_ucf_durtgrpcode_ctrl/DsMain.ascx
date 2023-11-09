<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" 
    style="margin-right: 10px">
    <EditItemTemplate>
        <table class="DataSourceFormView"  style="width:750px;">
            <tr>
                 <td width="15%">
                    <div>
                        <span>หมวดครุภัณฑ์ :</span>
                    </div>
                </td>
                <td width="20%">
                     <div>
                        <asp:TextBox ID="durtgrp_desc" runat="server" Style="text-align: left" ></asp:TextBox>
                    </div>
                </td> 
                <td width="10%">
                    <div>
                        <span>ตัวย่อ :</span>
                    </div>
                </td>
                <td width="10%">
                     <div>
                        <asp:TextBox ID="durtgrp_abb" runat="server" Style="text-align: center" ></asp:TextBox>
                    </div>
                </td>    
                <td width="10%">
                    <div>
                        <span>ค่าเสื่อม :</span>
                    </div>
                </td>
                <td width="10%">
                    <asp:TextBox ID="devalue_percent" runat="server" ToolTip="#,##0.00"  style="text-align:center;" ></asp:TextBox>
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