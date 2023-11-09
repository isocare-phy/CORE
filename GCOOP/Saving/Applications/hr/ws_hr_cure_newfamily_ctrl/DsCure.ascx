<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsCure.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_cure_newfamily_ctrl.DsCure" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td>
                    <div>
                        <span>ชื่อผู้ป่วย :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_name" runat="server" ></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เกี่ยวข้องเป็น :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:DropDownList ID="assist_state" runat="server">
                            <asp:ListItem Value="00" Text=""></asp:ListItem>
                            <asp:ListItem Value="01" Text="ตนเอง"></asp:ListItem>
                            <asp:ListItem Value="02" Text="บิดา"></asp:ListItem>
                            <asp:ListItem Value="03" Text="มารดา"></asp:ListItem>
                            <asp:ListItem Value="04" Text="คู่สมรส"></asp:ListItem>
                            <asp:ListItem Value="05" Text="บุตร"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>โรงพยาบาล :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_hosname" runat="server" ></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ประเภทผู้ป่วย :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:DropDownList ID="assist_posit" runat="server">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="ผู้ป่วยนอก" Text="ผู้ป่วยนอก"></asp:ListItem>
                            <asp:ListItem Value="ผู้ป่วยใน" Text="ผู้ป่วยใน"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ใบเสร็จรับเงินเลขที่ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_amp" runat="server"></asp:TextBox>
                    </div>
                </td>
                 <td>
                    <div>
                        <span>วันที่ตรวจรักษา :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_sdate" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
               
                <td>
                    <div>
                        <span>จำนวนเงินที่จ่ายจริง :</span>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>จำนวนเงินที่เบิกได้ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="assist_minamt" runat="server" Style="text-align: right;" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td >
                    <div>
                        <span>รวมยอดเบิก :</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:TextBox ID="assist_halfamt" runat="server" Style="text-align: right; background-color: #DDDDDD;" ToolTip="#,##0.00" ReadOnly="True"></asp:TextBox>
                     <div>
                </td> 
                <td colspan="2">
                    <div>
                        <span>บาท</span>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>