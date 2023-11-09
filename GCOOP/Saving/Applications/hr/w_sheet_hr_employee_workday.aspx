<%@ Page Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true" CodeBehind="w_sheet_hr_employee_workday.aspx.cs"
    Inherits="Saving.Applications.w_sheet_hr_employee_workday" Title="การลงเวลาเข้า - ออกงาน - นอกเวลา(Over time)"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%=initJavaScript%>
   <%=postGetMember%>
   <%=postAddRow %>
   <%=postDeleteRow %>
   <%=postNewClear%>
   <%=postShowDetail %>
   <%=postSearchGetMember%>
   <%=postDifferTime%>

   
    <style type="text/css">
        .style5
        {
            font-size: small;
            font-weight: bold;
        }
        .style6
        {
            font-size: small;
        }
    </style>

    <script type="text/javascript">
        function Validate()
        {
            return confirm("ยืนยันการบันทึกข้อมูล?");
        }
        
        function MenubarOpen() 
        {
            Gcoop.OpenDlg('590','600','w_dlg_hr_master_search.aspx','');
        }
        
        function GetValueEmplid(emplid)
        {
            var str_temp = window.location.toString();
            var str_arr = str_temp.split("?", 2);
            objDwMain.SetItem(1, "emplid", Gcoop.Trim(emplid));
            objDwMain.AcceptText();
            postGetMember();
        }

        function OnDwDetailButtonClick(s,r,c)
        {
            if (c == "b_del") {
                var seq_no = "";
                seq_no = objDwDetail.GetItem(1, "seq_no");
                if (seq_no == "" || seq_no == null || seq_no == "Auto") {
                    alert("ยังไม่มีข้อมูลไม่สามารถลบรายการได้");
                }
                else {
                    if (confirm("คุณต้องการลบรายการ " + seq_no + " ใช่หรือไม่?")) {
                        postDeleteRow();
                    }
                }
            }
            return 0;
        }
        
        function OnButtonClicked(s, row, oName)
        {
            if(oName == "b_search")
            {
                MenubarOpen();
            }
            return 0;
        }
        
         function MenubarNew()
         {
            if(confirm("ยืนยันการล้างข้อมูลบนหน้าจอ"))
            {
                postNewClear();
            }
        }
        
        function DwDetailAddRow()
        {
            var emplid = objDwMain.GetItem(1,"emplid");
            if (emplid == "Auto" || emplid == null)
            {
                alert("ไม่พบรหัสมาชิก กรุณากรอกรหัสสมาชิกก่อน");
            }
            else
            {
                postAddRow(); 
            }
        }

        function OnDwDetailListClick(sender, rowNumber, objectName) {
            if (objectName == "type_ot") {
                objDwDetail.AcceptText();
                postDifferTime();
            }
            return 0;
        }

        function OnListClick(sender, rowNumber, objectName) {
            var seq_no = objDwList.GetItem(rowNumber, "seq_no");
            if(seq_no != "Auto") {
                
                objDwDetail.SetItem(1, "seq_no", Gcoop.Trim(seq_no));
                objDwDetail.AcceptText();
                postShowDetail();
            }
            return 0;
        }

        
        function OnItemChangeMain(s, r, c, v)
        {
           if(c== "emplcode")
           {
               objDwMain.SetItem(r,c,Gcoop.StringFormat(v,"00000"));
               objDwMain.AcceptText();
               postSearchGetMember();
           }
           
           else if(c == "entry_tdate")
           {
                objDwMain.SetItem(1, "entry_tdate", v );
                objDwMain.AcceptText();
                objDwMain.SetItem(1, "entry_date", Gcoop.ToEngDate(v));
                objDwMain.AcceptText();
            }

           return 0;
        }
        
        function OnDwDetailItemChange(s,r,c,v)
        {
            if (c == "move_tdate")
            {
                objDwDetail.SetItem(1, "move_tdate", v);
                objDwDetail.AcceptText();
                objDwDetail.SetItem(1, "move_date", Gcoop.ToEngDate(v));
                objDwDetail.AcceptText();
            }

            else if (c == "t_in") {
                objDwDetail.SetItem(r, "t_in", v);
                objDwDetail.AcceptText();
                t_in = objDwDetail.GetItem(r, "t_in");
                if (t_in.length != 6) {
                    alert("กรุณากรอกข้อมูลเวลาเริ่มต้นในรูปแบบ HH:MM:SS");
                }
            }
            else if (c == "t_out") {
                objDwDetail.SetItem(r, "t_out", v);
                objDwDetail.AcceptText();
                t_out = objDwDetail.GetItem(r, "t_out");
                if (t_out.length != 6) {
                    alert("กรุณากรอกข้อมูลเวลาสิ้นสุดในรูปแบบ HH:MM:SS");
                }
            }

            else if (c == "ot_in") {
                objDwDetail.SetItem(r, "ot_in", v);
                objDwDetail.AcceptText();
                ot_in1 = objDwDetail.GetItem(r, "ot_in");
                if (ot_in1.length != 6) {
                    alert("กรุณากรอกข้อมูลเวลาเริ่มต้นในรูปแบบ HH:MM:SS");
                }        
            }
            else if (c == "ot_out") {
                objDwDetail.SetItem(r, "ot_out", v);
                objDwDetail.AcceptText();
                ot_out2 = objDwDetail.GetItem(r, "ot_out");
                if (ot_out2.length != 6) {
                    alert("กรุณากรอกข้อมูลเวลาสิ้นสุดในรูปแบบ HH:MM:SS");
                }
            }
            return 0;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <br />
    <table style="width: 100%;">
        <tr>
            <td class="style5" colspan="2">
                ข้อมูลการทำรายการ
            </td>
        </tr>
        <tr>
            <td colspan="2" valign="top">
                <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_emplworkday_main"
                    LibraryList="~/DataWindow/hr/hr_employee_workday.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" ClientEventItemChanged="OnItemChangeMain" AutoRestoreContext="False"
                    ClientEventButtonClicked="OnButtonClicked" ClientScriptable="True" 
                    ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style5">
                ข้อมูลการปรับอัตราเงินเดือน
            </td>
            <td>
                <span class="style5">รายละเอียดข้อมูลการปรับอัตราเงินเดือน</span>&nbsp;
            </td>
        </tr>
        <tr>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwList" runat="server" DataWindowObject="dw_hr_emplworkday_list"
                    LibraryList="~/DataWindow/hr/hr_employee_workday.pbl" AutoSaveDataCacheAfterRetrieve="True"
                    AutoRestoreDataCache="True" AutoRestoreContext="False" ClientScriptable="True"
                    ClientEventClicked="OnListClick" ClientFormatting="True">
                </dw:WebDataWindowControl>
                <span class="linkSpan" onclick="DwDetailAddRow()" style="font-family: Tahoma; font-size: small;
                    float: left; font-weight: 700; color: #003399;">เพิ่มแถว</span>
                    <asp:HiddenField ID="Hd_leavtotalhour" runat="server" /></td>
            <td valign="top">
                <dw:WebDataWindowControl ID="DwDetail" runat="server" DataWindowObject="dw_hr_emplworkday_detail"
                    LibraryList="~/DataWindow/hr/hr_employee_workday.pbl" Width="470px" Height="350px"
                    AutoSaveDataCacheAfterRetrieve="True" HorizontalScrollBar="Auto" AutoRestoreDataCache="True"
                    AutoRestoreContext="False" ClientScriptable="True" ClientFormatting="True" 
                    ClientEventButtonClicked="OnDwDetailButtonClick" 
                    ClientEventItemChanged="OnDwDetailItemChange" 
                    ClientEventClicked="OnDwDetailListClick">
                </dw:WebDataWindowControl>

            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="style6">
                หมายเหตุ : กรุณากรอกข้อมูลเวลาในรูปแบบ HH:MM:SS
                </td>
        </tr>
    </table>
    </asp:Content>

