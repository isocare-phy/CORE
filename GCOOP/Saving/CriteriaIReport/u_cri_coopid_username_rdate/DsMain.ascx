<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.CriteriaIReport.u_cri_coopid_username_rdate.DsMain" %>
<link id="css1" runat="server" href="../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<!-- <center>
    <asp:Label ID="Label1" runat="server" Font-Bold="true" Font-Size="Medium">รายงานกำหนดสิทธ์การเข้าใช้งานหน้าจอของระบบต่างๆ </asp:Label>
</center> -->
<br />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit" >
    <EditItemTemplate>
        <table class="iReportDataSourceFormView" >
            <tr>
                <td>
                    <div>
                        <span>สาขา:</span>
                    </div>
                </td>   
                <td>
                    <div>
                        <asp:DropDownList ID="coop_id" runat="server" ></asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ตั้งแต่วันที่:</span>
                    </div>
                </td>   
                <td>
                    <div>
                        <asp:TextBox ID="start_date" runat="server" ></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>ถึงวันที่:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="end_date" runat="server"></asp:TextBox>
                    </div>
                </td>
               </tr>
            <tr>
            <td> 
                    <div>
                        <span>รหัสผู้ใช้งาน:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="user_name" runat="server"></asp:TextBox>
                    </div>
                </td>
                <td></td>
            </tr>
        </table>
    </EditItemTemplate>
</asp:FormView>