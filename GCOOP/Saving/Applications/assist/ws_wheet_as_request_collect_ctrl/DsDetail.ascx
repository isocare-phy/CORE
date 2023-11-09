<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsDetail.ascx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_request_collect_ctrl.DsDetail" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <span class="TitleSpan">รายละเอียดใบคำขอ</span>
        <table class="DataSourceFormView" style="width: 758px;">
           <tr>
                <td width="15%">
                    <div>
                        <span>เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="17%">
                    <div>
                        <asp:TextBox ID="assist_docno" runat="server" Style="text-align:center" BackColor="#DCDCDC"></asp:TextBox>
                      </div>
                </td>
                <td width="17%">
                    <div>
                        <span>วันที่ยื่นใบคำขอ:</span>
                    </div>
                </td>
                <td width="17%">
                    <div>
                        <asp:TextBox ID="req_date" runat="server" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td width="13%">
                    <div>
                        <span>สถานะ:</span>
                    </div>
                </td>
                <td width="15%">
                    <div><span style="text-align:center;background-color:#DCDCDC">ลาออก</span>
                        <%--<asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" BackColor="#DCDCDC"></asp:TextBox>--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>เริ่มเข้าเป็นสมาชิกเมื่อ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="member_date" runat="server" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>ขาดสมาชิกภาพเมื่อ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="resign_date" runat="server"   Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เหตุเพราะ:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="resigncause_desc" runat="server" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
            </tr>  
            <tr>
                <td>
                    <div>
                        <span>อายุการเป็นสมาชิก:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="work_age" runat="server" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span>เดือนสะสม:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="monthcollect" runat="server"  ToolTip="#,##0"  Style="text-align:right"></asp:TextBox>
                    </div>
                </td>   
                <td>
                    <div>
                        <span>มูลค่าหุ้นซื้อเพิ่ม:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sharespx" runat="server"  ToolTip="#,##0.00" ReadOnly="true" BackColor="#DCDCDC" Style="text-align:right"></asp:TextBox>
                    </div>
                </td>               
            </tr>       
            <tr>
                <td>
                    <div>
                        <span  style="height:40px;text-align:right">มูลค่าหุ้นสะสมรวม:</span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="sum_sharestk" runat="server" ToolTip="#,##0.00" ReadOnly="true" BackColor="#DCDCDC"  Style="height:40px;font-size:14px;text-align:right"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div>
                        <span  style="height:40px;text-align:right">มูลค่าหุ้นสะสม ไม่รวมซื้อหุ้นเพิ่ม:</span>
                    </div>
                </td>
                <td colspan="3">
                    <div>
                        <asp:TextBox ID="sharestk_amt" runat="server" ToolTip="#,##0.00" 
                            BackColor="Black" ForeColor="#CCFF33"  
                            Style="width:400px;height:40px;font-size:18px;text-align:right; "></asp:TextBox>
                    </div>
                </td>                
            </tr>           
        </table>
    </EditItemTemplate>
</asp:FormView>
