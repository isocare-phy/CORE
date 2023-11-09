<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_hr_tax.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_tax_ctrl.w_sheet_hr_tax" %>
<%@ Register Src="DsTaxcal.ascx" TagName="DsTaxcal" TagPrefix="uc1" %>
<%@ Register Src="DsTaxsum.ascx" TagName="DsTaxsum" TagPrefix="uc1" %>
<%@ Register Src="DsTax.ascx" TagName="DsTax" TagPrefix="uc1" %>
<%@ Register Src="DsDiscount.ascx" TagName="DsDiscount" TagPrefix="uc1" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <style type="text/css">
      .style3
{
    float: left;
    width: 96%;
    height: 22px;
    margin: 2px 2px 0px 0px;
    line-height: 22px;
    text-align: right;
    vertical-align: middle;
    border: 1px solid #000000;
    background-color: rgb(211, 231, 255);
    font-family: Tahoma;
    font-size: 13px;
}
.style2
{
    width: 96%;
    height: 22px;
    float: left;
    font-family: Tahoma;
    font-size: 13px;
    border: 1px solid #000000;
    margin: 2px 2px 0px 0px;
    vertical-align: middle;
}
    </style>
    <script type="text/javascript">
      
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function SheetLoadComplete() {
        }
        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsMainItemChanged(s, r, c, v) {

            if (c == "emplcode") {

               
                PostProvinceId();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsMainClicked(s, r, c) {
         
            if (c == "taxcal") {

               
                PostTaxcal();
            }
        }

        //ประกาศฟังก์ชันสำหรับ event ItemChanged
        function OnDsDetailItemChanged(s, r, c, v) {

        }

        //ประกาศฟังก์ชันสำหรับ event Clicked
        function OnDsDetailClicked(s, r, c) {
            if (c == "taxcal") {
                dsDetail.SetRowFocus(r);
                PostDeleteRow();
            }
        }

        function OnClickInsertRow() {
            PostInsertRow();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table>
        <tr>
            <td width="100" >
                <div>
                    <span class="style3">ปี:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:TextBox ID="year" runat="server" class = "style2"></asp:TextBox>
                </div>
            </td>
           <%-- <td width="100">
                <div>
                    <span class="style3">เดือน:</span>
                </div>
            </td>
            <td>
                <div>
                    <asp:DropDownList ID="month" runat="server" class = "style2" Width = "100">
                        <asp:ListItem>มกราคม</asp:ListItem>
                        <asp:ListItem>กุมภาพันธ์</asp:ListItem>
                        <asp:ListItem>มีนาคม</asp:ListItem>
                        <asp:ListItem>เมษายน</asp:ListItem>
                        <asp:ListItem>พฤษภาคม</asp:ListItem>
                         <asp:ListItem>มิถุนายน</asp:ListItem>
                        <asp:ListItem>กรกฏาคม</asp:ListItem>
                        <asp:ListItem>สิงหาคม</asp:ListItem>
                        <asp:ListItem>กันยายน</asp:ListItem>
                        <asp:ListItem>ตุลาคม</asp:ListItem>
                         <asp:ListItem>พฤศจิกายน</asp:ListItem>
                        <asp:ListItem>ธันวาคม</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>--%>
            
        </tr>
    </table>
    <uc1:DsMain ID="dsMain" runat="server" />
    <br />
   <u> เงินได้พึงประเมิน</u>
    <uc1:DsTaxcal ID="dsTaxcal" runat="server" />
      <br />
     <u>ค่าใช้จ่ายเหมาจ่าย-ลดหย่อน</u>
     <uc1:DsDiscount ID="dsDiscount" runat="server" />
     <br />
      <u>คำนวนภาษี</u>
      <uc1:DsTax ID="dsTax" runat="server" />
       <uc1:DsTaxsum ID="dsTaxsum" runat="server" />
</asp:Content>
