<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_assist_child_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="15%">
                    <div>
                        <span>ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td width="30%">
                    <div>
                        <asp:TextBox ID="assist_detail" runat="server" Style="text-align: center"></asp:TextBox>
                     <div>
                </td>
                <td width="10%">
                    <div>
                        <span>อายุ:</span>
                    </div>
                </td>
                <td width="10%">
                    <asp:TextBox ID="assist_son" runat="server" Style="text-align: center"></asp:TextBox>
                </td>
                <td width="6%">
                    <div>
                        <span>ปี</span>
                    </div>
                </td>
                <td width="12%">
                    <div>
                        <span>ชั้นการศึกษา :</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:DropDownList ID="education_code" runat="server">
                        </asp:DropDownList>
                     <div>
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <div>
                        <span>ชื่อสถานศึกษา :</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:TextBox ID="assist_posit" runat="server" Style="text-align: center"></asp:TextBox>
                     <div>
                </td>
                <td>
                    <div>
                        <span>ภาคเรียน :</span>
                    </div>
                </td>
                <td >
                    
                        <div>
                        <asp:TextBox ID="assist_month" runat="server" 
                        Style="text-align: center" placeholder="ภาคเรียน/ปี"></asp:TextBox>
                     </div>
                     
                </td>
                <td>
                    <div>
                        <span>วันที่ :</span>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <asp:TextBox ID="assist_date" runat="server" Style="text-align: center"></asp:TextBox>
                     </div>
                </td>
            </tr>
            <tr>
                <td >
                    <div>
                        <span>จำนวนเงิน :</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:TextBox ID="assist_amt" runat="server" Style="text-align: right" ToolTip="#,##0.00"></asp:TextBox>
                     <div>
                </td>
                <td >
                    <div>
                        <span>เอกสาร :</span>
                    </div>
                </td>
                <td >
                    <div>
                        <asp:TextBox ID="assist_paper" runat="server" Style="text-align: center"></asp:TextBox>
                     <div>
                </td>
                <td >
                    <div>
                        <span>ฉบับ</span>
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
                <td >
                    <div>
                        <span>บาท</span>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
