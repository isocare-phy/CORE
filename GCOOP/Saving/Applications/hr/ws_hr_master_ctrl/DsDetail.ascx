<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_master_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="FormStyle">
            <tr>
                <td colspan="6">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>ข้อมูลทั่วไป</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    ชื่อ(Eng):
                </td>
                <td>
                    <asp:TextBox ID="emp_ename" runat="server"></asp:TextBox>
                </td>
                <td>
                    นามสกุล(Eng):
                </td>
                <td>
                    <asp:TextBox ID="emp_esurname" runat="server"></asp:TextBox>
                </td>

                
                <td colspan="5" rowspan="5">
                <center>
                    <asp:Image ID="Img_emp_profile" runat="server"  style="width:55%; height:55%;" />
                    </center>
                </td>
            </tr>
            <tr>
                <td>
                    ชื่อเล่น:
                </td>
                <td>
                    <asp:TextBox ID="emp_nickname" runat="server"></asp:TextBox>
                </td>
                <td>
                    วันเกิด:
                </td>
                <td>
                    <asp:TextBox ID="birth_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    เลขบัตรประชาชน:
                </td>
                <td>
                    <asp:TextBox ID="id_card" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    สัญชาติ:
                </td>
                <td>
                    <asp:TextBox ID="nation" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    ศาสนา:
                </td>
                <td>
                    <asp:TextBox ID="religion" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    กรุ๊ปเลือด:
                </td>
                <td>
                    <asp:DropDownList ID="bloodtype_code" runat="server" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    น้ำหนัก:
                </td>
                <td>
                    <asp:TextBox ID="weight" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    ส่วนสูง:
                </td>
                <td>
                    <asp:TextBox ID="height" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>

            <tr>
            <tr>
            <td colspan="2">
                <asp:Button ID="show" runat="server" Text="แสดงที่อยู่ทะเบียนบ้าน" Width="130px" />
            </td>
            </tr>
            </tr>

            <tr>
                <td colspan="6">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>ที่อยู่</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    ที่อยู่ปัจจุบัน:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="adn_no" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td width="14%">
                    จังหวัด:
                </td>
                <td width="20%">
                    <asp:DropDownList ID="adn_province" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="13%">
                    อำเภอ/เขต:
                </td>
                <td width="20%">
                    <asp:DropDownList ID="adn_amphur" runat="server">
                    </asp:DropDownList>
                </td>
                <td width="14%">
                    ตำบลแขวง:
                </td>
                <td width="19%">
                    <asp:DropDownList ID="adn_tambol" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    รหัสไปรษณีย์:
                </td>
                <td>
                    <asp:TextBox ID="adn_postcode" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    โทรศัพท์:
                </td>
                <td>
                    <asp:TextBox ID="adn_tel" runat="server"></asp:TextBox>
                </td>
                <td>
                    E-mail:
                </td>
                <td>
                    <asp:TextBox ID="adn_email" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    ทะเบียนบ้าน:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="adr_no" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    จังหวัด:
                </td>
                <td>
                    <asp:DropDownList ID="adr_province" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    อำเภอ/เขต:
                </td>
                <td>
                    <asp:DropDownList ID="adr_amphur" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    ตำบลแขวง:
                </td>
                <td>
                    <asp:DropDownList ID="adr_tambol" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    รหัสไปรษณีย์:
                </td>
                <td>
                    <asp:TextBox ID="adr_postcode" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <span style="font-size: 12px;"><font color="#cc0000"><u><strong>ข้อมูลเจ้าหน้าที่</strong></u></font></span>
                </td>
            </tr>
            <tr>
                <td>
                    สถานะการทำงาน:
                </td>
                <td>
                    <asp:DropDownList ID="emp_status" runat="server">
                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                        <asp:ListItem Value="1">ปกติ</asp:ListItem>
                        <asp:ListItem Value="2">ลาออก</asp:ListItem>
                        <asp:ListItem Value="3">พักงาน</asp:ListItem>
                        <asp:ListItem Value="4">ต่อสัญญา</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    วันที่เริ่มทำงาน:
                </td>
                <td>
                    <asp:TextBox ID="work_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    วันที่บรรจุ:
                </td>
                <td>
                    <asp:TextBox ID="contain_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    วันที่ลาออก:
                </td>
                <td>
                    <asp:TextBox ID="resign_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    ทำงานวันสุดท้าย:
                </td>
                <td>
                    <asp:TextBox ID="term_date" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
                <td>
                    อ้างอิงสมาชิก:
                </td>
                <td>
                    <asp:TextBox ID="ref_membno" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <%-- <tr>
                <td>
                    เหตุผลการลาออก:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="TextBox32" runat="server"></asp:TextBox>
                </td>
            </tr>--%>
        </table>
    </EditItemTemplate>
</asp:FormView>
