<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_investment.aspx.cs"
    Inherits="Saving.Applications.mis.w_sheet_investment" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
            height: 656px;
        }
        .style2
        {
            width: 100%;
            height: 1839px;
        }
        .style5
        {
            width: 100%;
        }
        .style6
        {
            width: 154px;
        }
        .style18
        {
            width: 144px;
        }
        .style20
        {
            width: 138px;
        }
        .style25
        {
            height: 5px;
        }
        .style26
        {
            width: 148px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function validate(aaa, bbb, ccc, ddd, eee, fff) {
            var txt49 = document.getElementById(aaa).value;
            var txt56 = document.getElementById(bbb).value;
            var txt58 = document.getElementById(ccc).value;
            var txt59 = document.getElementById(ddd).value;
            var txt60 = document.getElementById(eee).value;
            var txt61 = document.getElementById(fff).value;
            if ((txt49 == "") || (txt56 == "") || (txt58 == "") || (txt59 == "") || (txt60 == "") || (txt61 == "")) {
                alert("กรุณากรอกข้อมูลให้ครบ");
                return false;
            }
        }

        function validate2(aaa, bbb, ccc, ddd) {
            var txt1 = document.getElementById(aaa).value;
            var txt2 = document.getElementById(bbb).value;
            var txt3 = document.getElementById(ccc).value;
            var txt4 = document.getElementById(ddd).value;

            if ((txt1 == "") || (txt2 == "") || (txt3 == "") || (txt4 == "")) {
                alert("กรุณากรอกข้อมูลให้ครบ");
                return false;
            }
        }

        function validate3(aaa, bbb) {
            var txt9 = document.getElementById(aaa).value;
            var txt20 = document.getElementById(bbb).value;

            if ((txt9 == "") || (txt10 == "")) {
                alert("กรุณากรอกข้อมูลให้ครบ");
                return false;
            }
        }

        function validate4(aaa, bbb, ccc, ddd) {
            var txt5 = document.getElementById(aaa).value;
            var txt6 = document.getElementById(bbb).value;
            var txt7 = document.getElementById(ccc).value;
            var txt8 = document.getElementById(ddd).value;

            if ((txt5 == "") || (txt6 == "") || (txt7 == "") || (txt8 == "")) {
                alert("กรุณากรอกข้อมูลให้ครบ");
                return false;
            }
        }

        function validate5(aaa, bbb, ccc, ddd, eee, fff, ggg, hhh, iii) {
            var txt11 = document.getElementById(aaa).value;
            var txt12 = document.getElementById(bbb).value;
            var txt13 = document.getElementById(ccc).value;
            var txt14 = document.getElementById(ddd).value;
            var txt33 = document.getElementById(eee).value;
            var txt34 = document.getElementById(fff).value;
            var txt35 = document.getElementById(ggg).value;
            var txt36 = document.getElementById(hhh).value;
            var txt37 = document.getElementById(iii).value;

            if ((txt11 == "") || (txt12 == "") || (txt13 == "") || (txt14 == "") || (txt33 == "") || (txt34 == "") || (txt35 == "") || (txt36 == "") || (txt37 == "")) {
                alert("กรุณากรอกข้อมูลให้ครบ");
                return false;
            }
        }

        function NumOnly() {
            if (event.keyCode < 48 || event.keyCode > 57) {
                event.returnValue = false;
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <table class="style1" style="width: 720px;">
        <tr>
            <td>
                <table class="style2">
                    <tr>
                        <td bgcolor="#E4E4E4" class="style25" colspan="2">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                            <asp:Button ID="Button1" runat="server" Text="พันธบัตร" OnClick="Button1_Click" />
                            <asp:Button ID="Button2" runat="server" Text="ตั๋วแลกเงิน" OnClick="Button2_Click" />
                            <asp:Button ID="Button3" runat="server" Text="กองทุนปิด" OnClick="Button3_Click" />
                            <asp:Button ID="Button4" runat="server" Text="ประกันชีวิต" OnClick="Button4_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor="#E4E4E4" class="style26" align="center" style="vertical-align: top">
                            <asp:Label ID="Label3" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ต้องการลงทุน"
                                Font-Underline="False"></asp:Label>
                            <br />
                            <asp:TextBox ID="TextBox49" runat="server" MaxLength="9" OnKeyPress="return NumOnly()"
                                Width="105px"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label4" runat="server" Font-Size="Medium" Text="อัตราดอกเบี้ยเพื่อคำนวณNPV "
                                Font-Underline="False"></asp:Label>
                            <br />
                            <asp:TextBox ID="TextBox56" runat="server" MaxLength="5" Width="59px"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label5" runat="server" Font-Size="Medium" Text="กรอกจำนวนผลิตภัณฑ์ที่ต้องการ"
                                Font-Underline="False"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="Label6" runat="server" Font-Size="Medium" Text="พันธบัตร"></asp:Label>
                            &nbsp;
                            <asp:TextBox ID="TextBox58" runat="server" Width="46px" MaxLength="1" OnKeyPress="return NumOnly()"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label7" runat="server" Font-Size="Medium" Text="ตั๋วแลกเงิน"></asp:Label>
                            <asp:TextBox ID="TextBox59" runat="server" Width="46px" MaxLength="1" OnKeyPress="return NumOnly()"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label8" runat="server" Font-Size="Medium" Text="กองทุนปิด"></asp:Label>
                            <asp:TextBox ID="TextBox60" runat="server" Width="46px" OnKeyPress="return NumOnly()"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label9" runat="server" Font-Size="Medium" Text="ประกันชีวิต"></asp:Label>
                            <asp:TextBox ID="TextBox61" runat="server" Width="46px" OnKeyPress="return NumOnly()"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="Button13" runat="server" Height="32px" Text="เริ่มคำนวณ" Width="104px"
                                OnClick="Button13_Click" OnClientClick="return validate('ctl00_ContentPlace_TextBox49','ctl00_ContentPlace_TextBox56','ctl00_ContentPlace_TextBox58','ctl00_ContentPlace_TextBox59','ctl00_ContentPlace_TextBox60','ctl00_ContentPlace_TextBox61')" />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                        <td bgcolor="#E4E4E4" style="vertical-align: top">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="จำนวนเงินที่เหลือ" Font-Size="Medium"
                                Visible="False"></asp:Label>
                            <asp:TextBox ID="TextBox57" runat="server" Visible="False" Width="99px"></asp:TextBox>
                            <asp:Label ID="Label2" runat="server" Text="บาท" Font-Size="Medium" Visible="False"></asp:Label>
                            <br />
                            <br />
                            <br />
                            <asp:MultiView ID="MultiView1" runat="server" Visible="False">
                                <asp:View ID="View1" runat="server">
                                    <asp:Label ID="Label10" runat="server" Font-Size="Medium" Text="พันธบัตร"></asp:Label>
                                    <br />
                                    <table class="style5">
                                        <tr>
                                            <td class="style6">
                                                <asp:Label ID="Label11" runat="server" Font-Size="Medium" Text="มูลค่าพันธบัตรหน่วยละ "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox1" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                <asp:Label ID="Label12" runat="server" Font-Size="Medium" Text="จำนวนหน่วยลงทุน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox2" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                <asp:Label ID="Label13" runat="server" Font-Size="Medium" Text="อายุพันธบัตร "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox3" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                <asp:Label ID="Label14" runat="server" Font-Size="Medium" Text="คิดดอกเบี้ยประจำทุก "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList1" runat="server">
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style6">
                                                &nbsp;<asp:Label ID="Label15" runat="server" Font-Size="Medium" Text="อัตราดอกเบี้ย "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="Button5" runat="server" Text="คำนวณ" OnClick="Button5_Click" OnClientClick="return validate2('ctl00_ContentPlace_TextBox1','ctl00_ContentPlace_TextBox2','ctl00_ContentPlace_TextBox3','ctl00_ContentPlace_TextBox4')" />
                                    <asp:Button ID="Button6" runat="server" Text="ลบข้อมูล" OnClick="Button6_Click" />
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label16" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ลงทุน "></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Label ID="Label17" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ได้รับ"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Label ID="Label18" runat="server" Font-Size="Medium" Text="NPV"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label19" runat="server" Font-Size="Medium" Text="IRR"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox62" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox63" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox64" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox65" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox66" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox70" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox74" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox78" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox67" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox71" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox75" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox79" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox68" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox72" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox76" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox80" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox69" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox73" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox77" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox81" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="View2" runat="server">
                                    <asp:Label ID="Label20" runat="server" Font-Size="Medium" Text="ตั๋วแลกเงิน"></asp:Label>
                                    <table class="style5">
                                        <tr>
                                            <td class="style18">
                                                <asp:Label ID="Label21" runat="server" Font-Size="Medium" Text="ราคาตั๋วแลกเงิน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox9" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style18">
                                                <asp:Label ID="Label22" runat="server" Font-Size="Medium" Text="อายุตั๋วแลกเงิน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList3" runat="server">
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style18">
                                                <asp:Label ID="Label23" runat="server" Font-Size="Medium" Text="อัตราดอกเบี้ย "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox10" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="Button7" runat="server" Text="คำนวณ" OnClick="Button7_Click" OnClientClick="return validate3('ctl00_ContentPlace_TextBox9','ctl00_ContentPlace_TextBox10')" />
                                    <asp:Button ID="Button8" runat="server" Text="ลบข้อมูล" OnClick="Button8_Click" />
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label24" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ลงทุน "></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ได้รับ"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Font-Size="Medium" Text="NPV"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label27" runat="server" Font-Size="Medium" Text="IRR"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox82" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox83" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox84" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox85" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox86" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox87" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox88" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox89" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox90" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox91" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox92" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox93" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox94" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox95" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox96" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox97" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox98" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox99" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox100" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox101" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="View3" runat="server">
                                    <asp:Label ID="Label28" runat="server" Font-Size="Medium" Text="กองทุนปิด"></asp:Label>
                                    <table class="style5">
                                        <tr>
                                            <td class="style20">
                                                <asp:Label ID="Label30" runat="server" Font-Size="Medium" Text="ราคาหน่วยลงทุน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox5" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style20">
                                                &nbsp;<asp:Label ID="Label31" runat="server" Font-Size="Medium" Text="จำนวนหน่วยลงทุน"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox6" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style20">
                                                <asp:Label ID="Label32" runat="server" Font-Size="Medium" Text="ระยะเวลาลงทุน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox7" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style20">
                                                &nbsp;<asp:Label ID="Label33" runat="server" Font-Size="Medium" Text="คิดดอกเบี้ยประจำทุก "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DropDownList2" runat="server">
                                                    <asp:ListItem>3</asp:ListItem>
                                                    <asp:ListItem>6</asp:ListItem>
                                                    <asp:ListItem>12</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style20">
                                                <asp:Label ID="Label34" runat="server" Font-Size="Medium" Text="อัตราดอกเบี้ย "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="Button9" runat="server" Text="คำนวณ" OnClick="Button9_Click" OnClientClick="return validate4('ctl00_ContentPlace_TextBox5','ctl00_ContentPlace_TextBox6','ctl00_ContentPlace_TextBox7','ctl00_ContentPlace_TextBox8')" />
                                    <asp:Button ID="Button10" runat="server" Text="ลบข้อมูล" OnClick="Button10_Click" />
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label35" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ลงทุน "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label36" runat="server" Font-Size="Medium" Text="จำนวนเงินที่ได้รับ"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label37" runat="server" Font-Size="Medium" Text="NPV"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label38" runat="server" Font-Size="Medium" Text="IRR"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox102" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox103" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox104" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox105" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox106" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox107" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox108" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox109" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox110" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox111" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox112" runat="server" Height="22px" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox113" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox114" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox115" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox116" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox117" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox118" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox119" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox120" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox121" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="View4" runat="server">
                                    <asp:Label ID="Label29" runat="server" Font-Size="Medium" Text="ประกันชีวิต"></asp:Label>
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                &nbsp;<asp:Label ID="Label39" runat="server" Font-Size="Medium" Text="ทุนประกัน"></asp:Label>
                                                <asp:TextBox ID="TextBox11" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                            <td>
                                                &nbsp;<asp:Label ID="Label40" runat="server" Font-Size="Medium" Text="ค่าเบี้ยประกันต่อปี"></asp:Label>
                                                <asp:TextBox ID="TextBox12" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label41" runat="server" Font-Size="Medium" Text="ระยะเวลาจ่ายเบี้ย"></asp:Label>
                                                <asp:TextBox ID="TextBox13" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label42" runat="server" Font-Size="Medium" Text="ะระยะเวลาเอาประกัน"></asp:Label>
                                                <asp:TextBox ID="TextBox14" runat="server" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="style5">
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="Label43" runat="server" Font-Size="Medium" Text="อัตราผลตอบแทนในการรับคืนเงินสด"></asp:Label>
                                                <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" Height="19px"
                                                    Width="45px">
                                                    <asp:ListItem>1</asp:ListItem>
                                                    <asp:ListItem>2</asp:ListItem>
                                                    <asp:ListItem>3</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label54" runat="server" Font-Size="Medium" Text="แบบที่ 1"></asp:Label>
                                                <asp:TextBox ID="TextBox30" runat="server" ReadOnly="True" Width="49px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label63" runat="server" Font-Size="Medium" Text="% ต่อปี"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label57" runat="server" Font-Size="Medium" Text="ทุกๆ"></asp:Label>
                                                <asp:TextBox ID="TextBox18" runat="server" Width="47px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label60" runat="server" Font-Size="Medium" Text="ปี"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label66" runat="server" Font-Size="Medium" Text="เป็นจำนวน"></asp:Label>
                                                <asp:TextBox ID="TextBox21" runat="server" Width="31px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label69" runat="server" Font-Size="Medium" Text="ครั้ง"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label72" runat="server" Font-Size="Medium" Text="ตั้งแต่ปีที่"></asp:Label>
                                                <asp:TextBox ID="TextBox26" runat="server" Width="30px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label75" runat="server" Font-Size="Medium" Text="ถึง"></asp:Label>
                                                <asp:TextBox ID="TextBox27" runat="server" Width="30px" Height="19px" ReadOnly="True"
                                                    OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label55" runat="server" Font-Size="Medium" Text="แบบที่ 2"></asp:Label>
                                                <asp:TextBox ID="TextBox16" runat="server" Width="48px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label64" runat="server" Font-Size="Medium" Text="% ต่อปี"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label58" runat="server" Font-Size="Medium" Text="ทุกๆ"></asp:Label>
                                                <asp:TextBox ID="TextBox19" runat="server" Width="54px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label61" runat="server" Font-Size="Medium" Text="ปี"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label67" runat="server" Font-Size="Medium" Text="เป็นจำนวน"></asp:Label>
                                                <asp:TextBox ID="TextBox22" runat="server" Width="28px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label70" runat="server" Font-Size="Medium" Text="ครั้ง"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label73" runat="server" Font-Size="Medium" Text="ตั้งแต่ปีที่"></asp:Label>
                                                <asp:TextBox ID="TextBox25" runat="server" Width="30px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label76" runat="server" Font-Size="Medium" Text="ถึง"></asp:Label>
                                                <asp:TextBox ID="TextBox28" runat="server" Width="30px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label56" runat="server" Font-Size="Medium" Text="แบบที่ 3"></asp:Label>
                                                <asp:TextBox ID="TextBox17" runat="server" Width="48px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label65" runat="server" Font-Size="Medium" Text="% ต่อปี"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label59" runat="server" Font-Size="Medium" Text="ทุกๆ"></asp:Label>
                                                <asp:TextBox ID="TextBox20" runat="server" Width="56px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label62" runat="server" Font-Size="Medium" Text="ปี"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label68" runat="server" Font-Size="Medium" Text="เป็นจำนวน"></asp:Label>
                                                <asp:TextBox ID="TextBox23" runat="server" Width="29px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label71" runat="server" Font-Size="Medium" Text="ครั้ง"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label74" runat="server" Font-Size="Medium" Text="ตั้งแต่ปีที่"></asp:Label>
                                                <asp:TextBox ID="TextBox24" runat="server" Width="30px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label77" runat="server" Font-Size="Medium" Text="ถึง"></asp:Label>
                                                <asp:TextBox ID="TextBox29" runat="server" Width="30px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label44" runat="server" Font-Size="Medium" Text="เงินก้อนสุดท้ายที่ได้รับ "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="RadioButton1" runat="server" AutoPostBack="True" Text="อัตราผลตอบแทน"
                                                    Font-Size="Medium" />
                                                &nbsp;<asp:TextBox ID="TextBox31" runat="server" Width="54px" ReadOnly="True" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:RadioButton ID="RadioButton2" runat="server" AutoPostBack="True" Text="เงินก้อน"
                                                    Font-Size="Medium" />
                                                <asp:TextBox ID="TextBox32" runat="server" ReadOnly="True" Width="108px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="style5">
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label45" runat="server" Font-Size="Medium" Text="เงินปันผลรายปี"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label46" runat="server" Font-Size="Medium" Text="ดอกเบี้ยเงินปันผล"></asp:Label>
                                                <asp:TextBox ID="TextBox33" runat="server" Width="25px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label53" runat="server" Font-Size="Medium" Text="% ต่อปี"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label47" runat="server" Font-Size="Medium" Text="ทุกๆ"></asp:Label>
                                                <asp:TextBox ID="TextBox34" runat="server" Width="35px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                &nbsp;
                                                <asp:Label ID="Label48" runat="server" Font-Size="Medium" Text="ปี"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label49" runat="server" Font-Size="Medium" Text="จำนวน"></asp:Label>
                                                <asp:TextBox ID="TextBox35" runat="server" Width="30px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                &nbsp;<asp:Label ID="Label50" runat="server" Font-Size="Medium" Text="ครั้ง"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label51" runat="server" Font-Size="Medium" Text="ตั้งแต่ปีที่"></asp:Label>
                                                <asp:TextBox ID="TextBox36" runat="server" Width="30px" Height="20px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                                <asp:Label ID="Label52" runat="server" Font-Size="Medium" Text="ถึง"></asp:Label>
                                                <asp:TextBox ID="TextBox37" runat="server" Width="30px" OnKeyPress="return NumOnly()"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="Button11" runat="server" Text="คำนวณ" OnClick="Button11_Click" OnClientClick="return validate5('ctl00_ContentPlace_TextBox11','ctl00_ContentPlace_TextBox12','ctl00_ContentPlace_TextBox13','ctl00_ContentPlace_TextBox14','ctl00_ContentPlace_TextBox33','ctl00_ContentPlace_TextBox34','ctl00_ContentPlace_TextBox35','ctl00_ContentPlace_TextBox36','ctl00_ContentPlace_TextBox37')" />
                                    <asp:Button ID="Button12" runat="server" Text="ลบข้อมูล" OnClick="Button12_Click" />
                                    <table class="style5">
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label78" runat="server" Font-Size="Medium" Text="ค่าเบี้ยประกัน"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label79" runat="server" Font-Size="Medium" Text="จำนวนเงินรวม"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label80" runat="server" Font-Size="Medium" Text="เงินปีสุดท้าย"></asp:Label>
                                            </td>
                                            <td>
                                                &nbsp;
                                                <asp:Label ID="Label81" runat="server" Font-Size="Medium" Text="NPV"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox122" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox46" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox48" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox47" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox126" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox127" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox128" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox129" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox130" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox131" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox132" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox133" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox134" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox135" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox136" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox137" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox138" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox139" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox140" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox141" runat="server" ReadOnly="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <br />
                                </asp:View>
                                <br />
                                <br />
                                <br />
                                <br />
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
