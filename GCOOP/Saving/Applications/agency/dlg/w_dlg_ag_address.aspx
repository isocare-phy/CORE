<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_ag_address.aspx.cs"
    Inherits="Saving.Applications.agency.dlg.w_dlg_ag_address" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<%=postSetDistrict %>
<%=postSetpostcode%>
<script type="text/javascript">
    function OnDwMainClick(s, r, c) {
        if (c == "district_code") {
            var province_code = objDw_main.GetItem(1, "province_code");
            if (province_code == null || province_code == "") {
                alert("กรุณาเลือกจังหวัดก่อน");
            }
        }

    }
    function OnDwMainItemChange(s, r, c, v) {
        if (c == "memb_addr") {
            objDw_main.SetItem(r, "memb_addr",v);
            objDw_main.AcceptText();
        }
        else if (c == "mooban") {
            objDw_main.SetItem(r, "mooban",v);
            objDw_main.AcceptText();
        }
        else if (c == "soi") {
            objDw_main.SetItem(r, "soi", v);
            objDw_main.AcceptText();
        }
        else if (c == "road") {
            objDw_main.SetItem(r, "road", v);
            objDw_main.AcceptText();
        }
        else if (c == "tambol") {
            objDw_main.SetItem(r, "tambol", v);
            objDw_main.AcceptText();
        }
        else if (c == "district_code") {
            objDw_main.SetItem(r, "district_code", v);
            objDw_main.AcceptText();
            postSetpostcode();
        }
        else if (c == "province_code") {
            objDw_main.SetItem(r, "province_code", v);
            objDw_main.AcceptText();
            postSetDistrict();
        }
        else if (c == "postcode") {
            objDw_main.SetItem(r, "postcode", v);
            objDw_main.AcceptText();
        }
        return 0;
    }


    function OnCloseDialog() {
        var memb_addr = null;
        var mooban = null;
        var soi = null;
        var road = null;
        var tambol = null;
        var district_code = null;
        var province_code = null;
        var postcode = null;

        memb_addr = objDw_main.GetItem(1, "memb_addr");
        mooban = objDw_main.GetItem(1, "mooban");
        soi = objDw_main.GetItem(1, "soi");
        road = objDw_main.GetItem(1, "road");
        tambol = objDw_main.GetItem(1, "tambol");
        district_code = objDw_main.GetItem(1, "district_code");
        province_code = objDw_main.GetItem(1, "province_code");
        postcode = objDw_main.GetItem(1, "postcode");
        window.parent.SetAddress(memb_addr, mooban, soi, road, tambol, district_code, province_code, postcode);
        parent.RemoveIFrame();
    }
</script>
<body>
    <form id="form1" runat="server">
    <div>
        <dw:WebDataWindowControl ID="Dw_main" runat="server" AutoRestoreContext="False" AutoRestoreDataCache="True"
            AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientValidation="False"
            DataWindowObject="d_agentsrv_address" LibraryList="~/DataWindow/agency/egat_ag_regagmemb.pbl"
            ClientEventItemChanged="OnDwMainItemChange" 
            ClientEventClicked="OnDwMainClick">
        </dw:WebDataWindowControl>
    </div>
    </form>
    <p>
        <input id="B_save" type="button" value="บันทึก/ปิดหน้าจอ" onclick="OnCloseDialog()" /></p>
</body>
</html>
