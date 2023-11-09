<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsList.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_ucfassisttype_ctrl.DsList" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<table class="DataSourceRepeater">
    <tr>
        <th width="7%">
            รหัส
        </th>
        <th>
            คำอธิบาย
        </th>
        <th width="10%">
            แยกประเภทสมาชิก
        </th> 
        <th width="10%">
            ทุนต่อเนื่อง
        </th>   
        <th width="9%">
            คำนวณในรอบ
        </th> 
        <th width="15%">
            ตัวคำนวณ
        </th>    
        <th width="10%">
            เช็คครอบครัว
        </th>
        <th width="15%">
            กลุ่มสวัสดิการ
        </th>    
        <th width="5%">
            ลบ!
        </th>
    </tr>
    <asp:Repeater ID="Repeater1" runat="server">
        <ItemTemplate>
            <tr>
                <td>
                    <asp:TextBox ID="ASSISTTYPE_CODE" runat="server" Style="text-align: center;" MaxLength="2" ></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="ASSISTTYPE_DESC" runat="server"></asp:TextBox>
                </td>  
                <td align="center">
                    <asp:CheckBox ID="membtype_flag" runat="server" ></asp:CheckBox>
                </td> 
                <td align="center">
                    <asp:CheckBox ID="STM_FLAG" runat="server" ></asp:CheckBox>
                </td> 
                <td>
                    <asp:DropDownList ID="process_flag" runat="server">
                        <asp:ListItem Value="0">ไม่ระบุ</asp:ListItem>
                        <asp:ListItem Value="1">เดือน</asp:ListItem>
                        <asp:ListItem Value="2">ปี</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="calculate_flag" runat="server">
                        <asp:ListItem Value="1">เกรดเฉลี่ย</asp:ListItem>
                        <asp:ListItem Value="2">อายุ</asp:ListItem>
                        <asp:ListItem Value="3">อายุการเป็นสมาชิก</asp:ListItem>
                        <asp:ListItem Value="4">เงินเดือน</asp:ListItem>
                        <asp:ListItem Value="5">ค่าเสียหาย</asp:ListItem>
                        <asp:ListItem Value="6">ตามประเภทการจ่าย</asp:ListItem>
                    </asp:DropDownList>
                </td>  
                <td>
                    <asp:DropDownList ID="family_flag" runat="server">
                        <asp:ListItem Value="0">ไม่เช็ค</asp:ListItem>
                        <asp:ListItem Value="1">เช็ค</asp:ListItem>
                    </asp:DropDownList>
                </td>  
                <td>
                    <asp:DropDownList ID="ASSISTTYPE_GROUP" runat="server"></asp:DropDownList>
                </td>           
                <td>
                    <asp:Button ID="b_del" runat="server" Text="ลบ" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
