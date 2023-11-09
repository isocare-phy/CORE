<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucf_invtlitemcode_ctrl.DsList" %>

<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<link id="css2" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<asp:Panel ID="Panel" runat="server" 
    Width="734px" >
    <table class="DataSourceRepeater"  style="width:650px;">
     <tr>
            <th width="8%">
               รหัสรายการเคลื่อนไหว
            </th>
            <th width="15%">
               รายละเอียดการเคลื่อนไหว
            </th>
             <th width="5%">
               ฝั่งรายงาน
            </th>
             
             <th width="2%"></th>
             <th width="2%"></th>
            
        </tr>
        <asp:Repeater ID="Repeater1" runat="server" >
            <ItemTemplate>
                <tr>
                    <td width="8%">
                        <asp:TextBox ID="item_code" runat="server" Style="text-align: center;" ReadOnly=true></asp:TextBox>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="item_des" runat="server" Style="text-align: left;" ></asp:TextBox>
                    </td>
                    <td width="5%">
                        <%--<asp:DropDownList ID="sign_flag" runat="server">
                       <asp:ListItem Value="1">เข้า</asp:ListItem>
                        <asp:ListItem Value="-1">ออก</asp:ListItem>
                    </asp:DropDownList>--%>
                     <asp:TextBox ID="sign_flag" runat="server" Style="text-align: center;" ></asp:TextBox>
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