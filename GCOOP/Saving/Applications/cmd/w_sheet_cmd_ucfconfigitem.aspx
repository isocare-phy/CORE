<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" 
CodeBehind="w_sheet_cmd_ucfconfigitem.aspx.cs" Inherits="Saving.Applications.cmd.w_sheet_cmd_ucfconfigitem" %>
<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript">

        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }

        function OnDwMainClick(s, r, c) {
            switch (c) {
                case "year_flag":
                    SetFormatItem(r);
                    break;
                case "branch_flag":
                    SetFormatItem(r);
                    break;
                case "dept_flag":
                    SetFormatItem(r);
                    break;
                case "group_flag":
                    SetFormatItem(r);
                    break;
                case "number_flag":
                    SetFormatItem(r);
                    break;                               
            }
        }

        function SetFormatItem(rowNum) {
            if (rowNum == "1") {
                setFormatDurtRegit(rowNum);
            }
            else if (rowNum == "2") {
                setFormatInvNo(rowNum);
            }
            else if (rowNum == "3") {
                setFormatDurNo(rowNum);
            }
        }

        function setFormatInvNo(rowNum) {
            var yearFlag = 0, brachFlag = 0, deptFlag = 0, groupFlag = 0, numFlag = 0;
            var formattext = "";

            /*yearFlag = objDwMain.GetItem(rowNum, "year_flag");
            brachFlag = objDwMain.GetItem(rowNum, "branch_flag");
            deptFlag = objDwMain.GetItem(rowNum, "dept_flag");*/
            groupFlag = objDwMain.GetItem(rowNum, "group_flag");
            numFlag = objDwMain.GetItem(rowNum, "number_flag");
            /*if (yearFlag == "1") {
                formattext += "YY";
            }
            if (brachFlag == "1") {
                formattext += "XXX-";
            }
            if (deptFlag == "1") {
                formattext += "XX-";
            }*/
            if (groupFlag == "1") {
                formattext += "XX";
            }
            if (numFlag == "1") {
                formattext += "RRRRRR"
            }
            objDwMain.SetItem(rowNum, "configitem_format", formattext);
            objDwMain.AcceptText();
        }

        function setFormatDurNo(rowNum) {
            var yearFlag = 0, brachFlag = 0, deptFlag = 0, groupFlag = 0, numFlag = 0;
            var formattext = "";

            /*yearFlag = objDwMain.GetItem(rowNum, "year_flag");
            brachFlag = objDwMain.GetItem(rowNum, "branch_flag");
            deptFlag = objDwMain.GetItem(rowNum, "dept_flag");*/
            groupFlag = objDwMain.GetItem(rowNum, "group_flag");
            numFlag = objDwMain.GetItem(rowNum, "number_flag");
            /*if (yearFlag == "1") {
            formattext += "YY";
            }
            if (brachFlag == "1") {
            formattext += "XXX-";
            }
            if (deptFlag == "1") {
            formattext += "XX-";
            }*/
            if (groupFlag == "1") {
                formattext += "XX";
            }
            if (numFlag == "1") {
                formattext += "RRRR"
            }
            objDwMain.SetItem(rowNum, "configitem_format", formattext);
            objDwMain.AcceptText();
        }

        function setFormatDurtRegit(rowNum) {
            var yearFlag = 0, brachFlag = 0, deptFlag = 0, groupFlag = 0, numFlag = 0;
            var formattext = "";
            
            yearFlag = objDwMain.GetItem(rowNum, "year_flag");
            brachFlag = objDwMain.GetItem(rowNum, "branch_flag");
            deptFlag = objDwMain.GetItem(rowNum, "dept_flag");
            groupFlag = objDwMain.GetItem(rowNum, "group_flag");
            numFlag = objDwMain.GetItem(rowNum, "number_flag");
            if (yearFlag == "1") {
                formattext += "YY";
            }
            if (brachFlag == "1") {
                formattext += "XXX-";
            }
            if (deptFlag == "1") {
                formattext += "XX-";
            }
            if (groupFlag == "1") {
                formattext += "XX-";
            }
            if (numFlag == "1") {
                formattext += "RRRR"
            }
            objDwMain.SetItem(rowNum, "configitem_format", formattext);
            objDwMain.AcceptText();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="d_main_configitem"
        LibraryList="~/DataWindow/Cmd/cmd_ucfconfigitem.pbl" Width="750px" ClientScriptable="True"
        AutoRestoreContext="False" AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True"
        ClientFormatting="True" ClientEventClicked="OnDwMainClick">
    </dw:WebDataWindowControl>
</asp:Content>
