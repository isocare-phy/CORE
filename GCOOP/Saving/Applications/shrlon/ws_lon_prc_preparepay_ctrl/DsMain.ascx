<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DsMain.ascx.cs" Inherits="Saving.Applications.shrlon.ws_lon_prc_preparepay_ctrl.DsMain" %>
<link id="css1" runat="server" href="../../../JsCss/DataSourceTool.css" rel="stylesheet"
    type="text/css" />

<asp:FormView ID="FormView1" runat="server" DefaultMode="Edit">
    <EditItemTemplate>
    <div style="font-size:12px">
        <b><u>ประมวลเตรียมข้อมูลชำระหนี้จากระบบต่างๆ</u></b>
    </div>
        <table class="DataSourceFormView">
            <tr>
                <td width="25%">
                    <div>
                        <span>ปีบัญชี : </span>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <asp:TextBox ID="bizz_period" runat="server" style="text-align:center"></asp:TextBox>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <span>วันที่ทำรายการ : </span>
                    </div>
                </td>
                <td width="25%">
                    <div>
                        <asp:TextBox ID="prepare_date" runat="server" style="text-align:center"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <span>วิธีการคำนวณ : </span>
                    </div>
                </td>
                <td>
                    <div>
                    <asp:DropDownList ID="caltype_code" runat="server" Style="text-align: left;">
                        <asp:ListItem Value="" Text="กรุณาเลือกวิธีการคำนวณ"></asp:ListItem>
                        <asp:ListItem Value="PRN" Text="หักชำระต้น"></asp:ListItem>
                        <asp:ListItem Value="INT" Text="หักชำระดอกเบี้ย"></asp:ListItem>
                        <asp:ListItem Value="ALL" Text="หักชำระต้นและดอกเบี้ย"></asp:ListItem>
                       
                    </asp:DropDownList>
                        <%--<asp:TextBox ID="sacc_date" runat="server" style="text-align:center"></asp:TextBox>--%>
                    </div>
                </td>
                <td>
                    <div>
                        <span>วันที่คิดดอกเบี้ย/วันที่ชำระเงินกู้ : </span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="calintto_date" runat="server" style="text-align:center"></asp:TextBox>
                    </div>
                </td>
            </tr>

            <tr>
                <td>
                    <div>
                        <span>ระบบที่รับชำระ :</span>
                    </div>
                </td>
                <td>
                    <div>
                    <asp:DropDownList ID="preparetype_code" runat="server" Style="text-align: left;">
                        <asp:ListItem Value="" Text="กรุณาเลือกระบบที่รับชำระ" />
                        <asp:ListItem Value="DIV" Selected="True" Text="หักจากปันผลเฉลี่ยคืน" />
                    </asp:DropDownList>
                        
                    </div>
                </td>
                
            </tr>
            <tr>
                <td>
                    <div>
                        <span style="text-align:left; background-color:#ff99cc;"> <asp:CheckBox id="prepareclr_flag" runat="server"/> เคลียร์ข้อมูลทำรายการ</span>
                    </div>
                </td>
                 <td>
                    <div>
                        <span style="text-align:left; background-color:#ff99cc;"> <asp:CheckBox id="preparelon_flag" runat="server"/> ประมวลเงินเตรียมชำระหนี้</span>
                    </div>
                </td>
               
            </tr>
            
        </table>
       
        <br />
       

        <div style="font-size:12px">
            <b><u>ช่วงข้อมูลที่ต้องการประมวลผล</u></b>
        </div>
        <table class="DataSourceFormView">
            <tr>
                <td width="20%">
                    <div>
                        <span>ช่วงข้อมูล : </span>
                    </div>
                </td>
                <td>
                    <div>
                        <asp:DropDownList ID="proc_type" runat="server" style="width:200px" AppendDataBoundItems="True">
                            <asp:listitem text="ทั้งหมด" Value="1"></asp:listitem>
                            <asp:listitem text="ตามประเภทสมาชิก" Value="20"></asp:listitem>
                            <asp:listitem text="ตามสังกัด" Value="40"></asp:listitem>
                            <asp:listitem text="ตามเลขสมาชิก" Value="60"></asp:listitem>
                        </asp:DropDownList>
                    </div>
                </td>
                
            </tr>
            <tr>
                <td style="vertical-align:top">
                    <div>
                        <asp:TextBox ID="proc_label" runat="server" style="color:Black; background-color:#C5DCC2; text-align:right" Enabled="False"></asp:TextBox>
                    </div>  
                </td>
                <td>
                    <div>
                        <asp:TextBox ID="proc_text" runat="server" TextMode="MultiLine" style="height:80px" Enabled="False"></asp:TextBox>
                    </div>  
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <div style="color:#9A9A9A; font-size:11px">
                        ใส่ข้อมูลหรือช่วงข้อมูลที่ต้องการประมวลผลและคั่นด้วยเครื่องหมาย - หรือ , เช่น 11010, 11000 - 11020
                    </div>
                </td>
            </tr>
        </table>
           
    </EditItemTemplate>
</asp:FormView>
