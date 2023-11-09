<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_durtgrpcode_ctrl.DsList" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<asp:Panel ID="Panel" Height="500px" runat="server" ScrollBars="Vertical" 
    Width="620px" >
    <table class="DataSourceRepeater"  style="width:600px;">
     <tr>
            <th width="3%">
               รหัสหมวดครุภัณฑ์
            </th>
            <th width="15%">
                หมวดครุภัณฑ์
            </th>
             <th width="3%">
                ตัวย่อ
            </th>

             <th width="3%">
                ค่าเสื่อม%
            </th>
             <th width="2%"></th>
             <th width="2%"></th>
            
        </tr>
        <asp:Repeater ID="Repeater1" runat="server" >
            <ItemTemplate>
                <tr>
                    <td width="3%">
                        <asp:TextBox ID="durtgrp_code" runat="server" Style="text-align: center;" ReadOnly=true></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="durtgrp_desc" runat="server" Style="text-align: left;" ReadOnly=true></asp:TextBox>
                    </td>
                    <td width="3%">
                        <asp:TextBox ID="durtgrp_abb" runat="server" Style="text-align: center;" ReadOnly=true></asp:TextBox>
                    </td>
                     <td width="3%">
                        <asp:TextBox ID="devalue_percent" runat="server" ToolTip="#,##0.00" Style="text-align: center;" ReadOnly=true></asp:TextBox>
                    </td>
                   <td width="2%">
                    <asp:Button ID="b_edit" runat="server" style="background-color:#FFFF33;font-size:15px;" Height="100%" Width="100%" Text="แก้ไข" />
                </td>
                    <td width="2%" >
                    <asp:Button ID="b_del" runat="server" style="background-color:#FF3300;font-size:15px;" Height="100%" Width="100%" Text="ลบ" />
                </td>
                   
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</asp:Panel>