<%@ Page Title="" Language="C#" MasterPageFile="~/Frame.Master" AutoEventWireup="true"
    CodeBehind="w_sheet_sl_loan_requestment_spec.aspx.cs" Inherits="Saving.Applications.shrlon.w_sheet_sl_loan_requestment_spec"
    ValidateRequest="false" %>

<%@ Register Assembly="WebDataWindow" Namespace="Sybase.DataWindow.Web" TagPrefix="dw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%=initJavaScript %>
    <%=cancelRequest  %>
    <%=collInitP %>
    <%=collCondition%>
    <%=jsPostMember %>
    <%=jsPostColl  %>
    <%=jsPostClear%>
    <%=loadDwCollR%>
    <%=openNew%>
    <%=openOldDocNo %>
    <%=resendStr%>
    <%=resetColl %>
    <%=refresh%>
    <%=refreshDW %>
    <%=regenLoanClear  %>
    <%=resumLoanClear %>
    <%=setData  %>
    <%=setDWOthClr  %>
    <%=setLoanType  %>
    <%=setOldData %>
    <%=setOthClr%>
    <%=visibleChange%>
    <%=showBranch %>

    <script type="text/javascript">
        function Validate() {
            return confirm("ยืนยันการบันทึกข้อมูล");
        }
        function btnAddRowColl_Click(sender, row, name)
        {
            var rowCount = objdw_coll.RowCount;
            objdw_coll.InsertRow(rowCount);
        }
        function CloseDLG()
        {
        }
        function GetValueFromDlgCollmast(collRefNo)
        {
              var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", collRefNo );
            objdw_coll.AcceptText();
            PrePareColl(rowNumber, "ref_collno", collRefNo);
        }
        function GetValueFromDlg(memberno)
        {
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            var colunmName = Gcoop.GetEl("HdColumnName").value;
            if(colunmName =="b_searchc"){
                objdw_coll.SetItem(rowNumber, "ref_collno", memberno );
                objdw_coll.AcceptText();
                PrePareColl(rowNumber, "ref_collno", memberno);
            }else if (colunmName =="b_search"){
                objdw_main.SetItem(1, "member_no", memberno );
                objdw_main.AcceptText();
                PrePareData(1, "member_no", memberno);
            }   
        }
        function GetValueAccID(accID)
        {
            objdw_main.SetItem(1, "expense_accid", accID );
            objdw_main.AcceptText();   
        }
        function GetValueLoanRequest (docNo)
        {
            objdw_main.SetItem(1, "loanrequest_docno", docNo );
            objdw_main.AcceptText();
            openOldDocNo();
        }
        function GetValueOtherClr()
        {
            setOthClr();
        }
        function GetValueResumloanClear()
        {
            resumLoanClear();
        }
        function MenubarNew(){
            window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_spec.aspx";
        }
        function MenubarOpen(){
            openNew();
            Gcoop.OpenDlg('800', '600', 'w_dlg_sl_loanrequest_search.aspx', '');
        }
        function OnInsert(dwname)
        {
            if( dwname == "10")
            {
                objdw_coll.InsertRow(objdw_coll.RowCount() +1 );
            }else if ( dwname == "20")
            {
                objdw_otherclr.InsertRow(objdw_otherclr.RowCount() +1 );
            }
        }
        function OldDocNo(docno)
        {
           objdw_main.SetItem(1,"loanrequest_docno", docno);
           objdw_main.AcceptText ();
           setOldData();
        }
        function OnDwMainClick(sender, rowNumber, objectName)
        {
            Gcoop.GetEl("HdReturn").value = rowNumber + "";
            if ((objectName == "loanrcvfix_flag")||(objectName == "clearloan_flag")||(objectName == "otherclr_flag")||(objectName == "custompayment_flag"))
            {
                if(objectName == "loanrcvfix_flag"){
                    Gcoop.CheckDw(sender, rowNumber, objectName, "loanrcvfix_flag", 1, 0 );
                     refresh();   
                }
                 if(objectName == "clearloan_flag"){
                    Gcoop.CheckDw(sender, rowNumber, objectName, "clearloan_flag", 1, 0 );
                     refresh();
                }
                 if(objectName == "otherclr_flag"){
                    Gcoop.CheckDw(sender, rowNumber, objectName, "otherclr_flag", 1, 0 );
                     refresh();
                }
                if(objectName == "custompayment_flag"){
                    Gcoop.CheckDw(sender, rowNumber, objectName, "custompayment_flag", 1, 0 );
                     refresh();
                }
            }else if(objectName == "buyshare_flag"){
                    Gcoop.CheckDw(sender, rowNumber, objectName, "buyshare_flag", 1, 0 );
                    sender.SetItem(1 , "buyshare_amt",  0);
                    refresh();
                     var clearStatus =  objdw_main.GetItem(rowNumber, objectName);
                   PrePareCK(rowNumber, objectName ,  clearStatus  );
           } else { 
                if (objectName == "b_othclr"){
                     Gcoop.OpenDlg('770', '270', 'w_dlg_sl_loanrequest_otherclr.aspx', '');
//                     Gcoop.OpenIFrame(770, 270, 'w_dlg_sl_loanrequest_otherclr.aspx', '');
                }else if (objectName == "b_mthpay"){
                    var memberNo = objdw_main.GetItem( 1, "member_no");
                    if(( memberNo != "")&&( memberNo != null)&&( memberNo !="00000000")){
                        var income = objdw_main.GetItem(1, "incomemonth_other");
                        var paymonth = objdw_main.GetItem(1, "paymonth_other");
                        Gcoop.OpenIFrame(500, 400, "w_dlg_sl_loanrequest_monthpay.aspx", "?income="+income+ "&paymonth="+paymonth);
                        return 0;
//                        Gcoop.OpenDlg('500', '400', 'w_dlg_sl_loanrequest_monthpay.aspx', '?income='+income+ '&paymonth='+paymonth);
                    }else{
                        alert("ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                    }
                
                }else if (objectName == "b_cancel"){
                    var loanreqDocNo = objdw_main.GetItem( 1, "loanrequest_docno");
                    if (( loanreqDocNo != "") && (loanreqDocNo != null ) && ( loanreqDocNo!= "Auto")){
                        cancelRequest();
                    }else{
                        alert("ไม่สามารถยกเลิกรายการได้ กรุณาตรวจสอบข้อมูลใหม่อีกครั้ง");
                    }
                   
                }else if (objectName == "b_search"){
                 Gcoop.GetEl("HdColumnName").value = objectName;
                     Gcoop.OpenDlg('600', '600', 'w_dlg_sl_member_search.aspx', '');
                }else if (objectName == "b_inttab"){
                    //ของ กสท. ไม่แสดงค่าตัวนี้
                    //Gcoop.OpenDlg('600', '400', 'w_dlg_sl_loanrequest_intratespc.aspx', '');
                }else if(objectName == 'b_chgaccid'){
                    //เปลี่ยนแปลงเลขที่บัญชี
                    var memberNoVal = objdw_main.GetItem(1, "member_no");
                    if (( memberNoVal  != null) &&(memberNoVal != "")){
                        Gcoop.OpenDlg(620,250, "w_dlg_show_accid.aspx", "?member="+memberNoVal);
                    }
                }
            }  
        }
        function PrePareCK(rowNumber, columnName, newValue) 
        {
                var newV = Gcoop.Trim(newValue);
                Gcoop.GetEl("HdColumnName").value = columnName;
                jsPostMember();
        }
        function OnDwClearClick(sender, rowNumber, objectName)
        {
            if(objectName=="clear_status"){
               Gcoop.CheckDw(sender, rowNumber, objectName, "clear_status", 1, 0 );
               var clearStatus =  objdw_clear.GetItem(rowNumber, objectName);
               PrePareClear(rowNumber, objectName ,  clearStatus  );
            }else if ( objectName == "b_detail"){
                var loanContractNo = objdw_clear.GetItem( rowNumber, "loancontract_no");
                if ((loanContractNo!= null) && ( loanContractNo != "")){
                    setData();
                    var contractNo = objdw_clear.GetItem(rowNumber , "loancontract_no");
                    Gcoop.OpenDlg(500, 400, 'w_dlg_sl_loanrequest_cleardet.aspx', '?contractNo='+contractNo );
//                    Gcoop.OpenIFrame(500, 400, 'w_dlg_sl_loanrequest_cleardet.aspx', '?contractNo='+contractNo );
                }else{
                    alert ( "ไม่สามารถแสดงรายการได้ กรุณากรอกเลขทะเบียนก่อน");
                }
                
            }else if ( objectName == "b_genclr"){
                regenLoanClear();
            }
        }
        function OnDwCollButtonClicked(sender, rowNumber, buttonName)
        {
            var collTypeCode = objdw_coll.GetItem(rowNumber, "loancolltype_code");
            Gcoop.GetEl("HdReturn").value = rowNumber + "";
            Gcoop.GetEl("HdRowNumber").value = rowNumber + ""; 
            if (buttonName =="b_delrow")
            {
                objdw_coll.DeleteRow (rowNumber);
            }else if(buttonName=="b_detail"){
                var collNo = objdw_coll.GetItem(1, "ref_collno");
                if(( collNo != "")&&(collNo != null )){
                     setData();
                    var refCollNo = objdw_coll.GetItem( rowNumber , "ref_collno");
                    var collType = objdw_coll.GetItem( rowNumber , "loancolltype_code");
                    var requestDate = objdw_main.GetItem( rowNumber , "loanrequest_tdate");
//                    alert (requestDate);
                    Gcoop.OpenDlg('600', '450', 'w_dlg_sl_loanrequest_coll.aspx', "?refCollNo="+refCollNo + "&collType="+collType + "&date="+requestDate);
//                    Gcoop.OpenIFrame(600, 450, "w_dlg_sl_loanrequest_coll.aspx", "?refCollNo="+refCollNo + "&collType="+collType + "&date="+requestDate);
                    return ;
                }else{
                    alert ( "ไม่สามารถแสดงรายการได้ กรุณากรอกเลขที่อ้างอิงก่อน");
                }
            }else if((buttonName=="b_searchc") &&(collTypeCode =='01')){
                // ค้นหาคนค้ำประกัน
                Gcoop.GetEl("HdColumnName").value = buttonName;
                Gcoop.OpenDlg('600', '600', 'w_dlg_sl_member_search.aspx', '');
            }else if((buttonName=="b_searchc") &&(collTypeCode =='03')){  
                // ค้นหาบัญชีเงินฝาก 
                Gcoop.OpenDlg('600', '450', 'w_dlg_dp_account_search.aspx', '');
            }else if((buttonName=="b_searchc") &&(collTypeCode =='04')){ 
                //ค้นหาหลักทรัพย์ค้ำประกัน 
                Gcoop.OpenDlg('600', '450', 'w_dlg_sl_collmaster_search_req.aspx', '');
            }else if ( buttonName == "b_recoll"){
                collInitP();
            }else if (buttonName == "b_condition"){
                collCondition();
           }else if (buttonName == "b_addrow"){
                objdw_coll.InsertRow(objdw_coll.RowCount() +1 );
            }
        }
        function OnDwOtherClrButtonClicked(sender, rowNumber, buttonName)
        {
            if (buttonName =="b_delete"){
                Gcoop.GetEl("HdReturn").value = "7";  // กำหนด เพื่อส่งไปยัง SheetLoadComplete
                objdw_otherclr.DeleteRow (rowNumber);
            }else if(buttonName =="b_addrow"){
                objdw_otherclr.InsertRow(objdw_otherclr.RowCount() +1 );
            } 
        }
         function PrePareBranch(rowNumber, columnName, newValue)
        {
            objdw_main.SetItem(rowNumber, columnName, newValue);
            objdw_main.AcceptText();
            Gcoop.GetEl("HdColumnName").value = columnName;
            showBranch();
        }
        
        function PrePareClear(rowNumber, columnName, newValue)
        {
            objdw_clear.SetItem(rowNumber , columnName , newValue );
            objdw_clear.AcceptText();
            Gcoop.GetEl("HdColumnName").value = columnName ;
            jsPostClear();
        }
        
        function PrePareColl(rowNumber, columnName, newValue)
        {
            objdw_coll.SetItem(rowNumber, columnName, newValue);
            objdw_coll.AcceptText();
            Gcoop.GetEl("HdColumnName").value = columnName ;
            jsPostColl();
        }
        function PrePareData(rowNumber, columnName, newValue) 
        {
            var newV = Gcoop.Trim(newValue);
            if(newV != ""){
                objdw_main.SetItem(rowNumber, columnName, newV);
                objdw_main.AcceptText();
                Gcoop.GetEl("HdColumnName").value = columnName;
                jsPostMember();
            }
        }
         function ItemDataWindowChange(sender, rowNumber, columnName, newValue) {
            if ((columnName == "loantype_code")||(columnName == "loantype_code_1")) {
                if((newValue == "10")||(newValue == "11")){
                    objdw_main.SetItem(rowNumber, columnName, newValue);
                    objdw_main.AcceptText();
                    setLoanType();
                }else{
                    PrePareData(rowNumber, columnName, newValue);
                }   
            }else if (columnName == "member_no") {
                PrePareData(rowNumber, columnName, newValue);
            }else if (columnName == "expense_code"){
                PrePareData (rowNumber, columnName , newValue );        
            }else if (columnName == "expense_bank"){
             PrePareBranch(rowNumber, columnName , newValue);
//                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "expense_bank_1"){
             PrePareBranch(rowNumber, columnName , newValue);
//                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "expense_branch"){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "expense_branch_1"){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "expense_accid"){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "loanreqregis_amt"){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "loanrequest_amt"){
                PrePareData (rowNumber, columnName , newValue );
            }else if((columnName =="loanpayment_type")||(columnName =="period_payamt")){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName =="period_payment"){
                PrePareData (rowNumber, columnName , newValue );
            }else if(columnName == "incomemonth_other"){
                PrePareData (rowNumber, columnName , newValue );
            }else if(columnName == "paymonth_other"){
                PrePareData (rowNumber, columnName , newValue );
            }else if (columnName == "loanrequest_tdate"){
                if(newValue ==""){
                    refresh();
                }else {
                    objdw_main.SetItem( rowNumber, "loanrequest_date", Gcoop.ToEngDate(newValue));
                    objdw_main.AcceptText();
                    PrePareData (rowNumber, columnName , newValue );    
                }
                
            }else if(columnName == "loanrcvfix_tdate"){
                if(newValue ==""){
                    refresh();
                }else {
                    objdw_main.SetItem( rowNumber, "loanrcvfix_date", Gcoop.ToEngDate(newValue));
                    objdw_main.AcceptText();
                    PrePareData (rowNumber, columnName , newValue );    
                }
//            }else if (columnName == "expense_bank"){
//                PrePareBranch(rowNumber, columnName , newValue);
            }else if(columnName == "startkeep_tdate"){
                if(newValue ==""){
                    refresh();
                }else {
                    objdw_main.SetItem( rowNumber, "startkeep_date", Gcoop.ToEngDate(newValue));
                    objdw_main.AcceptText();
                    PrePareData (rowNumber, columnName , newValue );    
                }
            }else if ( columnName == "paytoorder_desc_1"){
                PrePareData (rowNumber, columnName , newValue );
            }else if ( columnName == "paytoorder_desc"){
                PrePareData (rowNumber, columnName , newValue );
            }else if ( columnName == "loanobjective_code"){
                PrePareData (rowNumber, columnName , newValue );
            }else if ( columnName == "int_continttype"){
                PrePareData (rowNumber, columnName , newValue );
            }else if ( columnName == "inspayment_amt"){
                PrePareData (rowNumber, columnName , newValue );
            }else if ( columnName == "agencyclr_amt"){
                PrePareData (rowNumber, columnName , newValue );
             }else if ( columnName == "salarybal_amt"){
                PrePareData (rowNumber, columnName , newValue );
            }
        }
        function ItemDwcollChanged(sender, rowNumber, columnName, newValue){
            Gcoop.GetEl("HdRowNumber").value = rowNumber + "";  
            if (columnName == "ref_collno") {
                if((newValue != "")&&(newValue != null)){
                     PrePareColl(rowNumber, columnName, newValue);
                }else{
                    objdw_coll.Reset(0);
                }
            }else if (columnName == "loancolltype_code"){
                if (newValue =="02"){
                    PrePareColl(rowNumber, columnName, newValue);
                }
            }else if (columnName == "use_amt") {
                PrePareColl(rowNumber, columnName, newValue);
            }else if (columnName == "coll_percent") {
                PrePareColl(rowNumber, columnName, newValue);
            }  
        }
        function ItemChangeDwOtherClr(sender, rowNumber, columnName, newValue)
        { 
            if (columnName=="clrother_amt")
            {
                objdw_otherclr.SetItem(rowNumber, "clrother_amt", newValue );
                objdw_otherclr.AcceptText();
                 setDWOthClr();
            }
        }
        function LoadDWColl(){
            resendStr();
        }
        function NewAccountNo(dept_no){
            var rowNumber = Gcoop.GetEl("HdRowNumber").value;
            objdw_coll.SetItem(rowNumber, "ref_collno", dept_no );
            objdw_coll.AcceptText();
            PrePareColl(rowNumber, "ref_collno", dept_no);
        }
        
        function SheetLoadComplete(){
            var returnVal = Gcoop.GetEl("HdReturn").value;
            var columnVal = Gcoop.GetEl("HdColumnName").value;
            var msgVal = Gcoop.GetEl("HdMsg").value;
            var msgWarning = Gcoop.GetEl( "HdMsgWarning").value;
            var memberNoVal = Gcoop.GetEl("HdMemberNo").value;
           if( msgVal != ""){
                Gcoop.GetEl("HdMsg").value = "";
                var rowCount = objdw_message.RowCount()+1;
//                alert (rowCount);
                for(var i = 1; i <  rowCount ; i++)
                {
//                    alert ("88888");
                    msgVal = objdw_message.GetItem(i, "msgtext")+"";
                    msgWarning = objdw_message.GetItem(i, "msgicon")+"";
                    var splitmsg = msgVal.split('|') ;
                    var msg = "";
                    for(var j=0 ; j<splitmsg.length ;j++){
                    msg +=splitmsg[j]+"\n";
                    }
                    alert (msg );
                     if ( msgWarning == "warning"){
                    //กรณีสลับคนค้ำประกัน
                      if(confirm('\n ท่านต้องการกรอกคนค้ำคนนี้หรือไม่ ถ้าต้องการ ตอบตกลง (OK) \n ถ้าไม่ต้องการ ตอบยกเลิก (Cancel)')){
                        Gcoop.GetEl("HdReturn").value = "";
                        Gcoop.GetEl("HdColumnName").value = columnVal ;
                        Gcoop.GetEl("HdMsgWarning").value = "";
                        jsPostColl();
                      }else{
                        Gcoop.GetEl("HdReturn").value = "";
                        Gcoop.GetEl("HdColumnName").value = "";
                        Gcoop.GetEl("HdMsgWarning").value = "";
                      }
                    }
                }
                
            }
            if(returnVal==8 && columnVal=="checklnrequest"){
                if(confirm('\nท่านต้องการทำรายการต่อหรือไม่  ถ้าต้องการทำรายการต่อตอบ OK ถ้าไม่ตอบ CANCEL')){
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = "";
                    resendStr();
                }else{
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = "";
//                    openNew();
                    window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_spec.aspx";
                }
            }else if( returnVal == 8 && columnVal  == "setmemberinfo"){
                if (confirm ('\n มีรายการใบคำขอกู้เก่า ถ้าต้องการแสดงกด OK ถ้าไม่ต้องการกด Cancel') ) //ปาล์มแก้ไข
                {
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = "";
                    resendStr();
                }else
                { 
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = "";
                    Gcoop.OpenDlg(400,200, "w_dlg_sl_loanrequest_duplicate.aspx", "?member="+memberNoVal);
//                    Gcoop.OpenIFrame(400,200, "w_dlg_sl_loanrequest_duplicate.aspx", "?member="+memberNoVal);
                    return 0;
                }
            }else if(  returnVal == 8 && columnVal  == "genbaseloancredit"){
                Gcoop.GetEl("HdReturn").value = "";
                Gcoop.GetEl("HdColumnName").value = "";
                Gcoop.OpenDlg(720,200, "w_dlg_sl_loanrequest_loanrightchoose.aspx","" );
//                Gcoop.OpenIFrame(720,200, "w_dlg_sl_loanrequest_loanrightchoose.aspx","" );
            }else if ((returnVal == 8)&&(columnVal == "setcolldetail") ){
                if(confirm (msgVal +'\nต้องการทำรายการต่อหรือไม่ ถ้าต้องการทำรายการต่อกด OK ถ้าต้องกานดึงข้อมูลเก่ากด Cancel'))
                {
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = columnVal ;
                    jsPostColl();
                }else{
                    Gcoop.GetEl("HdReturn").value = "";
                    Gcoop.GetEl("HdColumnName").value = "";
                    resetColl();
                }
            }else if (returnVal =="9"){
                window.open("w_sheet_sl_loan_requestment_emer.aspx"  ,"emer"  );
            }else if (returnVal =="7"){
                //กรณีลบรายการหักอื่นๆ 
                Gcoop.GetEl("HdReturn").value = "";
                var rowCount = objdw_otherclr.RowCount()
                if( rowCount == 0 )
                {
                    objdw_main.SetItem(1, "otherclr_flag", 0);
                }
                setDWOthClr();
           }else if (returnVal =="10"){
            //กรณีเลือกเปิดสัญญา ต่างประเภทกัน
             Gcoop.GetEl("HdReturn").value = "";
//                openNew();
             window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_spec.aspx";
            }else if (returnVal =="11"){
            //กรณีบันทึกข้อมูล
                Gcoop.GetEl("HdReturn").value = "";
//                openNew();
                alert ("บันทึกข้อมูลเรียบร้อยแล้ว");
                window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_spec.aspx";
            }else if (returnVal =="12"){
                //กรณีไม่สามารถทำรายการได้
                Gcoop.GetEl("HdReturn").value = "";
                alert ("ไม่สามารถเข้าใช้งานได้ กรุณาติดต่อผู้ดูแลระบบ");
//                openNew();
                window.location = Gcoop.GetUrl() + "Applications/shrlon/w_sheet_sl_loan_requestment_spec.aspx";
            }else if (returnVal =="15"){
               //กรณีแสดงเลขที่บัญชี
                Gcoop.GetEl("HdReturn").value = "";
                var memberNoVal = Gcoop.GetEl("HdMemberNo").value ;
                Gcoop.OpenDlg(620,250, "w_dlg_show_accid.aspx", "?member="+memberNoVal);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlace" runat="server">
    <asp:Literal ID="LtServerMessage" runat="server"></asp:Literal>
    <table style="width: 100%;" border="0">
        <tr>
            <td valign="top" colspan="2">
                <dw:WebDataWindowControl ID="dw_main" runat="server" DataWindowObject="d_sl_loanrequest_master_spec"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventItemChanged="ItemDataWindowChange" ClientEventClicked="OnDwMainClick"
                    ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <br />
                <asp:Label ID="Label3" runat="server" Text="หลักประกัน" Font-Bold="True"></asp:Label>
                <%--<span class="linkSpan" style="cursor: pointer;" onclick="OnInsert('10');">เพิ่มแถว</span>
                <br />--%>
                <dw:WebDataWindowControl ID="dw_coll" runat="server" DataWindowObject="d_sl_loanrequest_collateral"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventItemChanged="ItemDwcollChanged" ClientEventButtonClicked="OnDwCollButtonClicked"
                    ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="Label1" runat="server" Text="รายการเรียกเก็บ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_keeping" runat="server" DataWindowObject="d_sl_loanrequest_paymonth"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True">
                </dw:WebDataWindowControl>
            </td>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" Text="รายการหักกลบ" Font-Bold="True"></asp:Label>
                <dw:WebDataWindowControl ID="dw_clear" runat="server" DataWindowObject="d_sl_loanrequest_clear"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientFormatting="True" ClientEventClicked="OnDwClearClick">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <dw:WebDataWindowControl ID="dw_reqloop" runat="server" DataWindowObject="d_sl_loanrequest_loop"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    ClientEventItemChanged="ItemDwcollChanged" ClientEventButtonClicked="OnDwCollButtonClicked"
                    ClientFormatting="True">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td valign="top" colspan="2">
                <asp:Label ID="Label5" runat="server" Text="รายการหักอื่นๆ" Font-Bold="True"></asp:Label>
               <%-- <br />
                <span class="linkSpan" style="cursor: pointer; border:solid 1px white;" onclick="OnInsert('20');">เพิ่มแถว</span>
                <br />--%>
                <dw:WebDataWindowControl ID="dw_otherclr" runat="server" DataWindowObject="d_sl_loanrequest_otherclr"
                    LibraryList="~/DataWindow/Shrlon/sl_loan_requestment.pbl" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientFormatting="True"
                    ClientScriptable="True" ClientEventItemChanged="ItemChangeDwOtherClr" ClientEventButtonClicked="OnDwOtherClrButtonClicked"
                    BorderStyle="solid" BorderColor="white">
                </dw:WebDataWindowControl>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dw:WebDataWindowControl ID="dw_message" runat="server" AutoRestoreContext="False"
                    AutoRestoreDataCache="True" AutoSaveDataCacheAfterRetrieve="True" ClientScriptable="True"
                    DataWindowObject="d_ln_message" LibraryList="~/DataWindow/shrlon/sl_loan_requestment.pbl">
                </dw:WebDataWindowControl>
            </td>
        </tr>
    </table>
    <asp:Literal ID="LtXmlKeeping" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlReqloop" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlLoanDetail" runat="server" Visible="False"></asp:Literal>
    <asp:Literal ID="LtXmlOtherlr" runat="server" Visible="False"></asp:Literal>
    <asp:HiddenField ID="HdReturn" runat="server" />
    <asp:HiddenField ID="HdColumnName" runat="server" />
    <asp:HiddenField ID="HdMemberNo" runat="server" />
    <asp:HiddenField ID="HdMsg" runat="server" />
    <asp:HiddenField ID="HdXml" runat="server" />
    <asp:HiddenField ID="HdMsgWarning" runat="server" />
    <asp:HiddenField ID="HdRowNumber" runat="server" />
</asp:Content>
