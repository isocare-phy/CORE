<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsSearch.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_cancel_leave_ctrl.DsSearch" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView">
            <tr>
                <td width="10%">
                    <div>
                        <span>วันที่ :</span>
                    </div>
                </td>
                <td width="20%">
                    <div>
                        <asp:TextBox ID="leave_date" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>
                <%--  
                <td width="5%">
                    <div>
                        <span>ถึง</span>
                    </div>
                </td>
                <td>
                </td>
                
                <td width="20%">
                    <div>
                        <asp:TextBox ID="leave_date" runat="server" placeholder="วัน/เดือน/ปี"></asp:TextBox>
                    </div>
                </td>
                --%>
                <td>
                    <div>
                        <asp:Button ID="b_search" runat="server" Text="ค้นหา" Style="background-color: #DDDDDD;">
                        </asp:Button>
                    </div>
                </td>
        </table>
    </EditItemTemplate>
</asp:FormView>
