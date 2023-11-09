<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_hr_employee_workday.aspx.cs" Inherits="Saving.Applications.hr.w_sheet_hr_employee_workday_ctrl.w_sheet_hr_employee_workday" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ register src="DsList.ascx" tagname="DsList" tagprefix="uc1" %>
    <style type="text/css">
        .style3
        {
            float: left;
            width: 12%;
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
            width: 20%;
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
        var dsList = new DataSourceTool();
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function OnDsListItemChanged(s, r, c) {
           
            if (c == "check_in") {
                dsList.SetRowFocus(r);
               var checkin = dsList.GetItem(r, c);  //ให้รู้ว่ากดแถวไหนในลิส
               dsList.SetItem(r, c, checkin);  //PostDeleteRow(); ไว้ลบแถวที่เลือก
            }
        }
        function OnPostListDetail() {
            PostList();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <span class="style3">ประจำวันที่:</span>
    <asp:TextBox ID="checkin_date" runat="server" AutoPostBack="True" Style="text-align: center;" class="style2" onchange="OnPostListDetail()"></asp:TextBox>
    <br />
    <br />
    <asp:CheckBox ID="check_in" runat="server" AutoPostBack="True" Text="เลือกทั้งหมด" OnCheckedChanged="CheckAllChanged">
    </asp:CheckBox>
    <uc1:DsList ID="dsList" runat="server" />
    <asp:HiddenField ID="seq_no_old" runat="server" />
</asp:Content>
