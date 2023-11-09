<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.hr.ws_hr_changework_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet" type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class ="DataSourceFormView" style="width: 700px;">
            <tr>
                <td>
                    <div>
                        <span>เลขที่คำสั่ง :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="order_docno" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>วันที่มาเริ่มทำงาน :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="start_date" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
              </tr>
            <tr>
                <td>
                    <div>
                        <span>วันที่มีคำสั่ง:</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="order_date" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>กลุ่มงานเดิม :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="deptgrp_desc" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ตำแหน่งเดิม :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="pos_desc" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>กลุ่มงานใหม่ :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:DropDownList ID="deptgrp_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ตำแหน่งใหม่ :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:DropDownList ID="pos_code" runat="server">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ขั้นเงินเดือน :</span>
                    </div>
                </td>
                <td colspan ="5">
                    <div>
                        <asp:TextBox ID="xxxxxxxxxxxxx" runat="server"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td width ="15%">
                    <div>
                        <span>อัตราเงินเดือนเดิม :</span>
                    </div>
                </td>
                <td width ="15%">
                    <div>
                        <asp:TextBox ID="old_salary_amt" runat="server" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td width ="5%">
                    <div>
                        <span>บาท</span>
                    </div>
                </td>
                <td width ="15%">
                    <div>
                        <span>อัตราเงินเดือนใหม่ :</span>
                    </div>
                </td>
                <td width ="15%">
                    <div>
                        <asp:TextBox ID="salary_amt" runat="server" ToolTip="#,##0.00"></asp:TextBox>
                    </div>
                </td>
                <td width ="5%">
                    <div>
                        <span>บาท</span>
                    </div>
                </td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>