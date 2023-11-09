<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="ws_wheet_as_detail.aspx.cs" Inherits="Saving.Applications.assist.ws_wheet_as_detail_ctrl.ws_wheet_as_detail" %>
<%@ Register Src="DsMain.ascx" TagName="DsMain" TagPrefix="uc1" %>
<%@ Register Src="DsContmaster.ascx" TagName="DsContmaster" TagPrefix="uc2" %>
<%@ Register Src="DsStatement.ascx" TagName="DsStatement" TagPrefix="uc3" %>
<%@ Register Src="DsPayout.ascx" TagName="DsPayout" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    //ประกาศตัวแปร ควรอยู่บริเวณบนสุดใน tag <script>
    var dsMain = new DataSourceTool();
    function OnDsMainClicked(s, r, c) {
        if (c == "b_search") {
            Gcoop.OpenIFrame2(650, 600, 'w_dlg_sl_member_search_tks.aspx', '')
        }
    }
    function GetMembNoFromDlg(memberno) {
        dsMain.SetItem(0, "member_no", memberno.trim());
        PostRetrivememno();
    }
    function OnDsMainItemChanged(s, r, c, v) {
        if (c == "member_no") {
            PostRetrivememno();
        } else if (c == "assisttype_code") {
            PostRetriveDetail();
        }
    }
    function OnDsContItemChanged(s, r, c, v) {
        if (c == "asscontract_no") {
            PostRetrivefromDDW();
        }
    }  
    function Validate() {
        return confirm("ยืนยันการบันทึกข้อมูล");
    }
    </script>
    <script type="text/javascript">
        Number.prototype.round = function (p) {
            p = p || 10;
            return parseFloat(this.toFixed(p));
        };

        $(function () {
            //alert($("#tabs").tabs()); //ชื่อฟิวส์

            var tabIndex = Gcoop.GetEl("hdTabIndex").value; // Gcoop.ParseInt($("#<%=hdTabIndex.ClientID%>").val());
            $("#tabs").tabs({
                active: tabIndex,
                activate: function (event, ui) {
                    $("#<%=hdTabIndex.ClientID%>").val(ui.newTab.index() + "");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <uc1:DsMain ID="dsMain" runat="server" />
    <uc2:DsContmaster ID="dsContmaster" runat="server" />
     <br />
    <div id="tabs">
        <ul style="font-size:12px;">    
            <li><a href="#tabs-1">การจ่ายสวัสดิการ</a></li>
            <li><a href="#tabs-2">Statement</a></li>
         </ul>
        <div id="tabs-1"> 
            <uc4:DsPayout ID="dsPayout" runat="server" />          
        </div>
        <div id="tabs-2">
            <uc3:DsStatement ID="dsStatement" runat="server" />         
        </div>
    </div>              
    <asp:HiddenField ID="hdTabIndex" Value="0" runat="server" />
</asp:Content>
