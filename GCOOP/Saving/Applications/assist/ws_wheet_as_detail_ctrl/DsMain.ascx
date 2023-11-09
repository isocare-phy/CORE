<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_detail_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <span class="TitleSpan">ข้อมูลสมาชิก</span>
        <table class="DataSourceFormView" style="width: 770px;">
           <tr>
                <td width="13%">
                    <div>
                        <span>เลขสมาชิก:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:TextBox ID="member_no" runat="server" Style="width: 100px;text-align:center"></asp:TextBox>
                        <asp:Button ID="b_search" runat="server" Text="..." Style="width: 25px;" />
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td width="56%">
                    <div>
                        <asp:TextBox ID="fullname" runat="server" ReadOnly="true" BackColor="#DCDCDC"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ประเภทสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="membtype_desc" runat="server" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>สังกัด:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="fullgroup_desc" runat="server" ReadOnly="true" BackColor="#DCDCDC"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <span>ประเภทสวัสดิการ:</span>
                    </div>
                </td>
                <td colspan="2">
                    <div>
                        <asp:DropDownList ID="assisttype_code" runat="server"></asp:DropDownList>
                    </div>
                </td>                
            </tr>            
        </table>
    </EditItemTemplate>
</asp:FormView>
