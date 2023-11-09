
<!--<style>
.table11_1 table {
	width:100%;
	margin:15px 0;
	border:0;
}
.table11_1 th {
	background-color:#93DAFF;
	color:#000000
}
.table11_1,.table11_1 th,.table11_1 td {
	font-size:0.95em;
	text-align:;
	padding:4px;
	border-collapse:collapse;
}
.table11_1 th,.table11_1 td {
	border: 1px solid #6fcdfe;
	border-width:1px 0 1px 0;
	border:2px inset #ffffff;
}
.table11_1 tr {
	border: 1px solid #ffffff;
}
.table11_1 tr:nth-child(odd){
	background-color:#dbf2fe;
}
.table11_1 tr:nth-child(even){
	background-color:#ffffff;
}

</style>-->



<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');
/*

//select * from amappstatus where application='sms';
//select * from amsecwinsgroup where application='sms';
//select * from amsecwins where application='sms';
//select * from amsecpermiss where application='sms';
//select * from amsecuseapps where application='sms';
//select * from smsconfig;
truncate table smsconfig ;
truncate table smsnewsmessage ;
truncate table smspatternconfig ;
truncate table smstransaction ;
*/
/*

$sql="CREATE TABLE SMSPHONEBOOK (
			MEMBER_NO VARCHAR2(20) NOT NULL, 
			MEMBER_FULLNAME VARCHAR2(50) NOT NULL, 
			TELEPHONE_NUMBER VARCHAR2(30) NOT NULL, 
			USEPHONEBOOK_FLAG NUMBER(1,0) DEFAULT 0 NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL, 
			UPDATE_DATE DATE default sysdate NOT NULL  
			) ";
get_single_value_oci($sql,$value);

$sql="CREATE TABLE SMSPHONEBOOKGROUP (
			GROUP_CODE VARCHAR2(20) NOT NULL, 
			GROUP_NAME VARCHAR2(50) NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL
			) ";
get_single_value_oci($sql,$value);


$sql="CREATE TABLE SMSPHONEBOOKGROUPMAP (
			GROUP_CODE VARCHAR2(20) NOT NULL, 
			MEMBER_NO VARCHAR2(20) NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL
			) ";
get_single_value_oci($sql,$value);

*/

?>
<h2>จัดคู่เบอร์สมาชิก </h2> 
<?php

	
?>
<script>


     function onSubmitForm(){
         if(confirm('กรุณายืนยันการทำรายการใช้หรือไม่?')){
             //document.getElementById("report_result").src ="?"+document.getElementById("REPORT_").value;
             //document.getElementById("form_request").action ="?"+document.getElementById("REPORT_").value;
             return true;
         }else{
             return false;
         }
     }

     function doPrint(){
         if(confirm('กรุณายืนยันการทำรายการใช้หรือไม่?')){
            var frm= document.getElementById("report_result").contentWindow;
                frm.focus();// focus on contentWindow is needed on some ie versions
                frm.print();
             return true;
         }else{
             return false;
         }
     }
</script>
 <link rel="stylesheet" href="css/style.css">
<table class=responstable width="100%"><tr>
<form id="form_request" action ="" method="get" onsubmit="return onSubmitForm()" target="report_result" >
<input type="hidden" name="p" value="report" />
<input type="hidden" name="page_no" value="1" />
<tr><th style="width:50px;"><font size='2'>เลขสมาชิก : </font></th><td><input type="text" name="MEMBER_NO" value="<?=$_SESSION["MEMBER_NO"]?>"  style="width:70px;" /></td>
	   <th  style="width:50px;" ><font size='2'>เบอร์โทร :  </font></th><td><input type="text" name="TELEPHONE_NUMBER" value="<?=$_SESSION["TELEPHONE_NUMBER"]?>" style="width:100px;" maxlength="10" /></td>	
	    <th style="width:50px;" ><font size='2'>สถานะส่ง :  </font></th><td>
   <select name="POST_FLAG" style="width:50px;" >
	  <option value=""><font size='2'>ALL</font></option>
            <option value="-9" <?=$_SESSION["POST_FLAG"]==1?"selected":""?> >Failure </option>
            <option value="1" <?=$_SESSION["POST_FLAG"]==1?"selected":""?> >Success </option>
            <option value="0" <?=$_SESSION["POST_FLAG"]==0?"selected":""?>>Waiting </option>
		</select>	</td> 
</tr>
<tr>
	  <th><font size='2'>รูปแบบ  :  </font></th>
	  <td colspan="3">
	  <select name="PATTERN_CODE" style="width:400px;">
	  <option value=""><font size='2'>ALL</font></option>
	  <?php

			$strSQL="select * from smspatternconfig  order by sms_trans_code asc ";
			$value=array('SMS_TRANS_CODE','SMS_TRANS_DESC','SMS_PATTERN','ENABLE_FLAG','SCHEDULER_TIME','REALTIME_FLAG','FROM_SYSTEM','SMS_TRANS_SQL','SMS_VIEWS_SQL');
			list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
					$m=0;
					for($n=0;$n<$Num_Rows1;$n++){
						$SMS_PATTERN_CODE[$n] = $list_info1[$n][$m++];
						$SMS_PATTERN_DESC[$n] = conv($list_info1[$n][$m++]);
						?>
						<option value="<?=$SMS_PATTERN_CODE[$n]?>"><?=$SMS_PATTERN_CODE[$n]?>:<?=$SMS_PATTERN_DESC[$n]?></option>
						<?php
						$m=0;
					}
	  ?>
	  </select></td> 
	  <th style="width:50px;" ><font size='2'>จำนวนข้อมูล :  </font></th><td>
      <select name="ROWNUM" style="width:50px;" >
	  <option value=""><font size='2'>ALL</font></option>
            <option value="20" <?=$_SESSION["ROWNUM"]==20?"selected":""?>>20 </option>
            <option value="50" <?=$_SESSION["ROWNUM"]==50?"selected":""?>>50 </option>
            <option value="100" <?=$_SESSION["ROWNUM"]==100?"selected":""?> >100 </option>
            <option value="200" <?=$_SESSION["ROWNUM"]==200?"selected":""?> >200 </option>
		</select>	</td> 
</tr>
<tr>
	  	 
	 
	  <th><font size='2'>ช่วงวันที่  :  <font style="color:red">(*)</font></font></th><td><input type="text" name="OPERATE_DATE" value="<?=$_SESSION["OPERATE_DATE"]==""?date("Ymd"):$_SESSION["OPERATE_DATE"]?>" style="width:70px;" />(yyyymmdd)</td>
     <th><font size='2'>ถึง วันที่  :  <font style="color:red">(*)</font></font></th><td><input type="text" name="OPERATE_DATE1" value="<?=$_SESSION["OPERATE_DATE1"]==""?date("Ymd"):$_SESSION["OPERATE_DATE1"]?>" style="width:70px;" />(yyyymmdd)</td>
	 <th style="width:50px;" ><font size='2'>แสดง SQL :  </font></th><td>
   <select name="SQL_FLAG" style="width:50px;" >
            <option value="0" <?=$_SESSION["SQL_FLAG"]==0?"selected":""?>>No </option>
            <option value="1" <?=$_SESSION["SQL_FLAG"]==1?"selected":""?> >Yes </option>
		</select>	</td> 
</tr>
<tr>
	  <th><font size='2'>รายงาน :  </font></th><td colspan="2">  
		<select name="r" id="r"  >
            <option value="SMSSD01" ><font size='2'>[SMSSD01]รายงานสรุป : SMS รายวัน</font> </option>
            <option value="SMSSM01" ><font size='2'>[SMSSM01]รายงานสรุป : SMS รายเดือน</font></option>
            <option value="SMSMM01" ><font size='2'>[SMSMM01]รายงาน : ทะเบียนสมาชิก SMS  </font></option>
            <option value="SMSAD01" ><font size='2'>[SMSAD01]รายงาน : SMS เรียงตามวันเวลา </font></option>
            <option value="SMSAD02" ><font size='2'>[SMSAD02]รายงาน : SMS เรียงตามสมาชิก </font></option>
		</select>	
	  </td>	 
	  <td colspan="3">
                  <input type="submit" value="submit" style="width:70px;"/>
                  <input type="RESET" value="reset" style="width:70px;"/>
                  <input type="button" value="Print" onclick="doPrint()" style="width:70px;"/>
	 </td>
</tr>

</form>
</table>
<table cellspacing="0" cellpadding="0" border="0" align="center" bgcolor="White" style="border-color:rgb(212,208,200); border-style:dotted;">
<tr><td align="center">
    <iframe src="" id="report_result" name="report_result" frameborder="0" scrolling="auto" width="700" height="550"/>
   </td></tr></table>
   <br/><br/>
<?php 


 ?>