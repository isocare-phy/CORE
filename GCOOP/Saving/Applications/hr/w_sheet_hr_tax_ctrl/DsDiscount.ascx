<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDiscount.ascx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.DsDiscount" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table class="DataSourceFormView" style="width: 550px;">
            <tr>
                <td width="70%">
                    <div>
                        <span>ค่าใช้จ่ายเหมาจ่าย 40%(ไม่เกินคนละ 60,000.-บาท)</span>
                    </div>
                </td>
                <td  width="30%">
                    <div>
                       <asp:TextBox ID="A03" runat="server" style="text-align:right; background-color:#E7E7E7;" ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="200">
                    <div>
                        <span>ลดหย่อนส่วนตัว(30,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div>
                       <asp:TextBox ID="A04" runat="server" style="text-align:right; background-color:#E7E7E7; " ToolTip="#,##0.00" ></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนคู่สมรส(30,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A05" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
           
                <td width="100">
                    <div>
                        <span>ลดหย่อนบุตร</span>
                    </div>
                </td>
                <td width="80">
                    <div>
                       <asp:TextBox ID="A06" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
             <tr>
           
                <td width="100">
                    <div>
                        <span>ลดหย่อนการศึกษาบุตร</span>
                    </div>
                </td>
                <td width="80">
                    <div>
                       <asp:TextBox ID="A07" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนบุตรทุพลภาพ(60,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A21" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนเบี้ยประกัน(ไม่เกิน 100,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A08" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>เบี้ยประกันชีวิตแบบบำนาญ</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A22" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนเบี้ยบ้าน(100,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A09" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
             <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนเงินบริจาค(ไม่เกิน 10% ของที่เหลือ)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A10" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
             <tr>
                <td width="100">
                    <div>
                        <span>ลดหย่อนบิดา มารดา(คนละ 30,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A11" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
             <tr>
                <td width="100">
                    <div>
                        <span>ค่าซื้อหน่วยลงทุนในกองทุนรวม 15% (ไม่เกินคนละ 500,000.-บาท)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A12" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>ค่าซื้อหน่วยลงทุนเพื่อการเลี้ยงชีพ(ไม่เกินร้อยละ 15 ของเงินได้)</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A23" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
             <tr>
                <td width="100">
                    <div>
                        <span>เงินประกันสังคม</span>
                    </div>
                </td>
                <td  width="100">
                    <div >
                       <asp:TextBox ID="A13" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
             
             <tr>
                <td width="100">
                    <div>
                        <span>เงินสะสมกองทุนสำรองเลี้ยงชีพ</span>
                    </div>
                </td>
                <td width="100">
                    <div >
                       <asp:TextBox ID="A14" runat="server" style="text-align:right; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
           
            <tr>
                <td width="100">
                    <div>
                        <span>รวมเหมาจ่าย-ลดหย่อน</span>
                    </div>
                </td>
                <td width="100">
                    <div >
                       <asp:TextBox ID="COMPUTE_1" runat="server" style="text-align:right; background-color:#000000;color:#00FF00; " ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100">
                    <div>
                        <span>เงินได้สุทธิที่ต้องคำนวนภาษี ณ ที่จ่าย</span>
                    </div>
                </td>
                <td width="100">
                    <div >
                       <asp:TextBox ID="COMPUTE_2" runat="server" style="text-align:right; background-color:#000000; color:#00FF00;" ToolTip="#,##0.00"></asp:TextBox>
                       
                    </div>
                </td>
            </tr>
            
            
        </table>
    </EditItemTemplate>
</asp:FormView>