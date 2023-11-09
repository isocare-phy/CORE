<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_suppliesgroup_ctrl.DsList" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<asp:Panel ID="Panel" runat="server" 
    Width="620px" >
    <table class="DataSourceRepeater"  style="width:500px;">
     <tr>
            <th width="3%">
               รหัสหมวดพัสดุ
            </th>
            <th width="15%">
               รายละเอียดหมวดพัสดุ
            </th>
             
             <th width="2%"></th>
             <th width="2%"></th>
            
        </tr>
        <asp:Repeater ID="Repeater1" runat="server" >
            <ItemTemplate>
                <tr>
                    <td width="3%">
                        <asp:TextBox ID="invtgrp_code" runat="server" Style="text-align: center;" ReadOnly=true></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="invtgrp_desc" runat="server" Style="text-align: left;" ReadOnly=true></asp:TextBox>
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