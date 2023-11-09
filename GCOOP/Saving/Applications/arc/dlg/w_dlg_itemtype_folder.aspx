<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="w_dlg_itemtype_folder.aspx.cs" Inherits="Saving.Applications.arc.w_dlg_itemtype_folder" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<html>
    <head id="Head1" runat="server">
    <%=postSaveData%>
    <%=postAddrow%>
    <%=postDelrow %>
    <title>ข้อมูลหมวดแฟ้ม</title>
    <script type="text/javascript">
        function OnButtonCloseClick() {
            //alert("Close");
            //postCloseIF();
            parent.RemoveIFrame();
        }
        function OnButtonSaveClick() {
            var isConfirm = confirm("ยืนยันการบันทึกข้อมูล");
            if (isConfirm) {
                //objDwMain.Acceptext();
                postSaveData();
            }
        }
        function OnInsert() {
            //alert("จะเพิ่มเเถวนะ");
            //objDwMain.InsertRow(0);
            postAddrow();
        }

        function DwMain_Click(sender, rowNumber, objectName) {
            if (objectName == "b_2") {
                Gcoop.GetEl("HD_Del").value = rowNumber;
                postDelrow();
             }
        }
    </script>
   
    </head>
   <body>
    <form id="form1" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
     <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Width="465px" Height="540px">
      <div align="center">   
      <div align="center">
           <dw:WebDataWindowControl ID="DwMain" runat="server" DataWindowObject="dw_hr_iyemtype_folder_ii"
                    LibraryList="~/DataWindow/arc/hr_constant.pbl" AutoRestoreContext="False" AutoRestoreDataCache="True"
                    AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True" ClientEventItemChanged="DwMain_Change"
                    ClientEventClicked="DwMain_Click">
                </dw:WebDataWindowControl>
       </div>
            <span  onclick="OnInsert()" style="font-size: small; color: #808080; cursor: pointer; ">
                เพิ่มแถว</span> 
            <input id="buttonSaveData" style="width: 80px;" type="button" value="บันทึก" onclick="OnButtonSaveClick()" />
                &nbsp;
            <input id="buttonCloseIFrame" type="button" value="ปิดหน้าจอ" onclick="OnButtonCloseClick()"
                style="width: 80px;" /> 
       </div>
       </asp:Panel>
    </form>
     <asp:HiddenField ID="HD_Del" runat="server" />
</body>
</html>