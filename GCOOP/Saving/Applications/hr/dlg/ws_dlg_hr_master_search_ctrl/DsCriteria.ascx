<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsCriteria.ascx.cs"
    Inherits="Saving.Applications.hr.dlg.ws_dlg_hr_master_search_ctrl.DsCriteria" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 560px;">
            <tr>
                <td width="12%">
                    <div>
                        <span>ลำดับที่:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:TextBox ID="emp_no" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>รหัสเจ้าหน้าที่:</span>
                    </div>
                </td>
                <td width="18%">
                    <div>
                        <asp:TextBox ID="salary_id" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td width="15%">
                    <div>
                        <span>ตำแหน่ง:</span>
                    </div>
                </td>
                <td width="22%">
                    <div>
                        <asp:DropDownList ID="pos_code" runat="server">
                        </asp:DropDownList>                        
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ชื่อ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_name" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>นามสกุล:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="emp_surname" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>งาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="deptgrp_code" runat="server">
                        </asp:DropDownList> 
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>
