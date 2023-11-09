<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.assist.dlg.ws_dlg_as_assistpay_v2_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />
<asp:FormView ID="FormView2" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
        <table border="0">
            <br />
        </table>
        <table class="DataSourceFormView">
            <tr>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ทะเบียน:</span>
                    </div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="member_no" runat="server" Style="text-align: center; background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ชื่อ-สกุล:</span>
                    </div>
                </td>
                <td width="27%">
                    <asp:TextBox ID="full_name" runat="server" ReadOnly="true" Style="background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">วันที่จ่าย:</span></div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="REQ_DATE" runat="server" Style="text-align: center;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 11px;">สังกัด :</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="membgroup_desc" runat="server" Style="width:407px;background-color: #CCCCCC;"></asp:TextBox>
                </td>
                <td width="13%">
                    <div>
                        <span style="font-size: 11px;">ประเภทสมาชิก :</span>
                    </div>
                </td>
                <td width="17%">
                    <asp:TextBox ID="member_type" runat="server" Style="text-align: center; background-color: #CCCCCC"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table class="DataSourceFormView" border="0">
           <tr>
                <td colspan="4">
                    <strong style="font-size: 12px;">รายละเอียดสัญญา</strong>
                </td>
            </tr>
            <tr>
                <td width="20%">
                    <div>
                        <span style="font-size: 11px;">เลขที่ใบคำขอ:</span>
                    </div>
                </td>
                <td width="35%">
                    <asp:TextBox ID="ASSIST_DOCNO" runat="server" Style="text-align: center; background-color:Lime; width: 140px;"></asp:TextBox>
                </td>
                <td width="20%">  
                    <div>
                        <span style="font-size:12px;font-weight:bold;">ยอดอนุมัติ :</span>
                    </div>
                </td>  
                <td width="25%">  
                    <asp:TextBox ID="APPROVE_AMT" runat="server" Style="background-color: Black;text-align:right;font-weight:bold;" 
                            ToolTip="#,##0.00" ForeColor="#CCFF33" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="font-size: 11px;">ประเภทสวัสดิการ :</span>
                    </div>
                </td>
                <td>
                  <asp:TextBox ID="assisttype_desc" runat="server" Style="margin-left: 1px;" 
                        BackColor="#CCCCCC">
                    </asp:TextBox>
                </td> 
                <td>  
                    <div>
                        <span style="font-size:12px;font-weight:bold;">ยอดหักชำระ :</span>
                    </div>
                </td>  
                <td>  
                    <asp:TextBox ID="pay_amt" runat="server" Style="background-color: Black;text-align:right;font-weight:bold;" 
                            ToolTip="#,##0.00" ForeColor="Red" ReadOnly="true"></asp:TextBox>
                </td>  
            </tr>
            <tr>    
                <td>
                    <div>
                        <span style="font-size: 11px;">ประเภทการจ่ายสวัสดิการ :</span>
                    </div>
                </td>
                <td>
                     <asp:TextBox ID="assistpay_desc" runat="server" Style="margin-left: 1px;"
                        BackColor="#CCCCCC">
                    </asp:TextBox>
                </td> 
                <td>                    
                    <div>
                        <span style="font-size:12px;font-weight:bold;">ยอดจ่ายสุทธิ์ :</span>
                    </div>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="assist_amt" runat="server" Style="background-color: Black;text-align:right;font-weight:bold;" ToolTip="#,##0.00" ForeColor="#CCFF33"   ></asp:TextBox>
                </td>   
             </tr>            
        </table>
    </EditItemTemplate>
</asp:FormView>
