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
<head><link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">

  
      <link rel="stylesheet" href="css/style.css">
	  </head>

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
<h2>จัดการ SMS </h2> 
<?php

 if($_REQUEST["PK_ID"]!=""){
 
			if($_REQUEST["delete"]!=""){
				$sql="delete from smstransaction  
					where PK_ID='".$_REQUEST["PK_ID"]."'  and MEMBER_NO='".$_REQUEST["MEMBER_NO"]."'  and SMS_TRANS_CODE='".$_REQUEST["SMS_TRANS_CODE"]."' ";
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>ลบสำเร็จ":"<font color=red>ลบไม่สำเร็จ ".$sql)."</font><br/>";
				
			}else 
			if($_REQUEST["Resend"]!=""){
			
				$sql="update smstransaction set 
					SEND_DATE=trunc(sysdate) ,POST_FLAG=0
					where PK_ID='".$_REQUEST["PK_ID"]."'  and MEMBER_NO='".$_REQUEST["MEMBER_NO"]."'  and SMS_TRANS_CODE='".$_REQUEST["SMS_TRANS_CODE"]."' ";
			    //echo $sql;
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue> ปรับปรุง สำเร็จ":"<font color=red> ปรับปรุง ไม่สำเร็จ ".$sql)."</font><br/>";

			   	//get_single_value_oci("commit",$value); 
			   //echo $sql;
			}

}



	
     if(isset($_REQUEST["page_no"])){
		$_SESSION["page_no"]=$_REQUEST["page_no"];
	}
	
	if(isset($_SESSION["page_no"])==false)
	  $_SESSION["page_no"]=1;
	
	$rowperpage=500;
	
	$rows_s=$rowperpage*($_SESSION["page_no"]-1);
	$rows_e=$rowperpage*($_SESSION["page_no"]);
	
	if(isset($_REQUEST["MEMBER_NO_"]))
	  $_SESSION["MEMBER_NO_"]=$_REQUEST["MEMBER_NO_"];
	  
	if(isset($_REQUEST["MEMBER_FULLNAME"]))
	  $_SESSION["MEMBER_FULLNAME"]=$_REQUEST["MEMBER_FULLNAME"];
	  
	if(isset($_REQUEST["SEND_PRODATE_"]))
	  $_SESSION["SEND_PRODATE_"]=$_REQUEST["SEND_PRODATE_"];
	  
	if(isset($_REQUEST["TELEPHONE_NUMBER_"]))
	  $_SESSION["TELEPHONE_NUMBER_"]=$_REQUEST["TELEPHONE_NUMBER_"];
	  
	if(isset($_REQUEST["POST_FLAG_"]))
	  $_SESSION["POST_FLAG_"]=$_REQUEST["POST_FLAG_"];
	  
	if(isset($_REQUEST["ROWNUM"]))
	  $_SESSION["ROWNUM"]=100;
	  
	if(isset($_REQUEST["SMS_PATTERN_CODE_"]))
	  $_SESSION["SMS_PATTERN_CODE_"]=$_REQUEST["SMS_PATTERN_CODE_"];
	  	  
	
	 $strSQL="select count(s.MEMBER_NO) as CNT 
	     from smstransaction s ,mbmembmaster m, smspatternconfig p 
	    where m.member_no=s.member_no  and p.SMS_TRANS_CODE=s.SMS_TRANS_CODE and rownum <=".$_SESSION["ROWNUM"]."
		".($_SESSION["SMS_PATTERN_CODE_"]!=""?(" and s.SMS_TRANS_CODE like '%".$_SESSION["SMS_PATTERN_CODE_"]."%' "):"")."
		".($_SESSION["MEMBER_NO_"]!=""?(" and s.MEMBER_NO like '%".$_SESSION["MEMBER_NO_"]."%' "):"")."
		".($_SESSION["SEND_PRODATE_"]!=""?(" and ( to_char(s.SEND_PRODATE,'yyyymmdd') like '%".$_SESSION["SEND_PRODATE_"]."%' or to_char(s.SEND_DATE,'yyyymmdd') like '%".$_SESSION["SEND_PRODATE_"]."%' ) "):"")."
		".($_SESSION["MEMBER_FULLNAME"]!=""?(" and (m.memb_name||' '||m.memb_surname) like '%".convSQL($_SESSION["MEMBER_FULLNAME"])."%' "):"")."
		".($_SESSION["POST_FLAG_"]!=""?(" and s.POST_FLAG like '%".convSQL($_SESSION["POST_FLAG"])."%' "):"")."
		".($_SESSION["TELEPHONE_NUMBER_"]!=""?(" and s.TELEPHONE_NUMBER like '%".convSQL($_SESSION["TELEPHONE_NUMBER_"])."%' "):"")."";
	 $value=array('CNT');
	 list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
	 $CNT = $list_info1[0][0];	 	  
		  
	$strSQL="select * from (
					select s.*, ROWNUM rnum ,m.memb_name||' '||m.memb_surname as MEMBER_FULLNAME 
						 ,to_char(s.SEND_PRODATE,'yyyy-mm-dd hh24:mi:ss') as SEND_PRODATE_
						 , p.SMS_TRANS_CODE||'-'||p.SMS_TRANS_DESC as SMS_TRANS_DESC
						 from smstransaction s ,mbmembmaster m, smspatternconfig p 
						where m.member_no=s.member_no  and p.SMS_TRANS_CODE=s.SMS_TRANS_CODE and rownum <= ".(($_SESSION["page_no"])*$rowperpage)." 
						".($_SESSION["SMS_PATTERN_CODE_"]!=""?(" and s.SMS_TRANS_CODE like '%".$_SESSION["SMS_PATTERN_CODE_"]."%' "):"")."
						".($_SESSION["MEMBER_NO_"]!=""?(" and s.MEMBER_NO like '%".$_SESSION["MEMBER_NO_"]."%' "):"")."
						".($_SESSION["SEND_PRODATE_"]!=""?(" and ( to_char(s.SEND_PRODATE,'yyyymmdd') like '%".$_SESSION["SEND_PRODATE_"]."%' or to_char(s.SEND_DATE,'yyyymmdd') like '%".$_SESSION["SEND_PRODATE_"]."%' ) "):"")."
						".($_SESSION["MEMBER_FULLNAME"]!=""?(" and (m.memb_name||' '||m.memb_surname) like '%".convSQL($_SESSION["MEMBER_FULLNAME"])."%' "):"")."
						".($_SESSION["POST_FLAG_"]!=""?(" and s.POST_FLAG like '%".convSQL($_SESSION["POST_FLAG"])."%' "):"")."
						".($_SESSION["TELEPHONE_NUMBER_"]!=""?(" and s.TELEPHONE_NUMBER like '%".convSQL($_SESSION["TELEPHONE_NUMBER_"])."%' "):"")."
						order by s.SEND_PRODATE desc,s.MEMBER_NO asc  )
				where rnum >= ".(($_SESSION["page_no"]-1)*$rowperpage)."  ";
	//echo $strSQL;
	$value=array('PK_ID','MEMBER_NO','MEMBER_FULLNAME','TELEPHONE_NUMBER','POST_FLAG','MESSAGE_TEXT','SEND_STATUS_MSG','SEND_PRODATE_','SMS_TRANS_DESC','SMS_TRANS_CODE');
	list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
			$m=0;
			for($n=0;$n<$Num_Rows1;$n++){
				$PK_ID[$n] = $list_info1[$n][$m++];
				$MEMBER_NO[$n] = $list_info1[$n][$m++];
				$MEMBER_FULLNAME[$n] = conv($list_info1[$n][$m++]);
				$TELEPHONE_NUMBER[$n] = conv($list_info1[$n][$m++]);
				$POST_FLAG[$n]=$list_info1[$n][$m++];
				$MESSAGE_TEXT[$n] = conv($list_info1[$n][$m++]);
				$SEND_STATUS_MSG[$n] = conv($list_info1[$n][$m++]);
				$SEND_PRODATE[$n]=$list_info1[$n][$m++];
				$SMS_TRANS_DESC[$n]=conv($list_info1[$n][$m++]);
				$SMS_TRANS_CODE[$n]=$list_info1[$n][$m++];
				$m=0;
			}
	
?>
<table class=responstable   width="100%"><tr>
<form action="" method="get" onsubmit="return confirm('ยืนยันทำรายการ?')">
<input type="hidden" name="p" value="sms" />
<input type="hidden" name="page_no" value="1" />
<tr><th style="width:50px;background-color:#dbf2fe;" ><font size='2'>เลขทะเบียน:</font></th>
<td><input type="text" name="MEMBER_NO_" value="<?=$_SESSION["MEMBER_NO_"]?>" style="width:100px;"  /></td>
	  <th style="width:70px;background-color:#dbf2fe;" ><font size='2'>ชื่อ/สกุล:</font></th>
	  <td style="background-color:#ffffff;" ><input type="text" name="MEMBER_FULLNAME" value="<?=$_SESSION["MEMBER_FULLNAME"]?>" style="width:150px;" /></td>
	 <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>ใช้สมุดโทรศัพท์:</font></th>
	 <td style="background-color:#ffffff;" >
   <select name="POST_FLAG_" style="width:50px;" >
            <option value="" <?=$_SESSION["POST_FLAG_"]==""?"selected":""?> >ALL </option>
            <option value="1" <?=$_SESSION["POST_FLAG_"]=="1"?"selected":""?> >Yes </option>
            <option value="-9" <?=$_SESSION["POST_FLAG_"]=="-9"?"selected":""?>>No </option>
            <option value="0" <?=$_SESSION["POST_FLAG_"]=="0"?"selected":""?>>Wait </option>
		</select>	
	 </td>
</tr>
<tr>
	  <th><font size='2'>รูปแบบ  :  </font></th>
	  <td colspan="3">
	  <select name="SMS_PATTERN_CODE_" style="width:400px;">
	  <option value=""><font size='2'>ALL</font></option>
	  <?php

			$strSQL="select * from smspatternconfig  order by sms_trans_code asc ";
			$value=array('SMS_TRANS_CODE','SMS_TRANS_DESC','SMS_PATTERN','ENABLE_FLAG','SCHEDULER_TIME','REALTIME_FLAG','FROM_SYSTEM','SMS_TRANS_SQL','SMS_VIEWS_SQL');
			list($Num_Rows1_,$list_info1) = get_value_many_oci($strSQL,$value);
					$m=0;
					for($n=0;$n<$Num_Rows1_;$n++){
						$SMS_PATTERN_CODE[$n] = $list_info1[$n][$m++];
						$SMS_PATTERN_DESC[$n] = conv($list_info1[$n][$m++]);
						?>
						<option value="<?=$SMS_PATTERN_CODE[$n]?>" <?=$_SESSION["SMS_PATTERN_CODE_"]==$SMS_PATTERN_CODE[$n]?"selected":""?>><?=$SMS_PATTERN_CODE[$n]?>:<?=$SMS_PATTERN_DESC[$n]?></option>
						<?php
						$m=0;
					}
	  ?>
	  </select></td> 
	  <th style="width:50px;" ><font size='2'>จำนวนข้อมูล :  </font></th><td>
      <select name="ROWNUM" style="width:50px;" >
	  <option value=""><font size='2'>ALL</font></option>
            <option value="20" <?=$_SESSION["ROWNUM"]=="20"?"selected":""?>>20 </option>
            <option value="50" <?=$_SESSION["ROWNUM"]=="50"?"selected":""?>>50 </option>
            <option value="100" <?=$_SESSION["ROWNUM"]=="100"?"selected":""?> >100 </option>
            <option value="200" <?=$_SESSION["ROWNUM"]=="200"?"selected":""?> >200 </option>
		</select>	</td> 
</tr>
<tr>
	  <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>เบอร์โทร:</font></th>
	  <td style="background-color:#ffffff;" ><input type="text" name="TELEPHONE_NUMBER_" value="<?=$_SESSION["TELEPHONE_NUMBER_"]?>" style="width:100px;" maxlength="10" /></td>	 
	  <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>วันที่  :  <font style="color:red">(*)</font></font></th>
	  <td style="background-color:#ffffff;" ><input type="text" name="SEND_PRODATE_" value="<?=$_SESSION["SEND_PRODATE_"]==""?date("Ymd"):$_SESSION["SEND_PRODATE_"]?>" style="width:100px;" />(yyyymmdd)</td>
	  <td colspan="2" style="background-color:#ffffff;" ><input type="submit" name="Search" value="ค้น" style="width:70px;" /></td>
</tr>
</form>
</table>

<table class=responstable width="100%">
     <tr><td colspan="7" ><font size='2'>
	   <?php 
	   $pg=1;
	   for($c=0;$c<=$CNT;$c+=$rowperpage,$pg++){?>
			<a href="?page_no=<?=$pg?>" style="<?=$_SESSION["page_no"]==$pg?"font-weight: bold;":""?>"><?=$pg?></a> &nbsp;
		<?php }  ?>
			<a href="?page_no=<?=$pg?>" style="<?=$_SESSION["page_no"]==$pg?"font-weight: bold;":""?>"><?=$pg?></a> &nbsp;
	 </font></td></tr>
	 <tr>
    <th><font size='2'>ลำดับ</font></th>
    <th ><span><font size='2'>เลขสมาชิก</font></span></th>
    <th><font size='2'>ชื่อ/สกุล</font></th>
    <th><font size='2'>เบอร์ 
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></th>
    <th><font size='2'>สถานะ</font></th>
    <th><font size='2'>วันที่
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></th>
	<th><font size='2'>ข้อความ  
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</th>
	<th><font size='2'>
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;</th>
  </tr>
     <!--<tr bgcolor="#CCDD00"><td>NO</td><td>MEMBER_NO</td><td>NAME</td><td>PHONE</td><td>USEPHONEBOOK</td><td>ENABLE_FLAG</td><td><a href="?MEMBER_NO=NEW">Add</a> </td></tr>-->
	  
		<?php
//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
	  <tr><td><?=$i+1+((($_SESSION["page_no"]-1)*$rowperpage))?></td>
	  <td><font size='2'><?=$MEMBER_NO[$i]?></font></td>
	  <td><font size='2'><?=$MEMBER_FULLNAME[$i]?></font></td>
	  <td><font size='2'><?=$TELEPHONE_NUMBER[$i]?></font></td>
	  <td><font size='2'><?=$POST_FLAG[$i]>=1?("สำเร็จ"."<br/>(".$POST_FLAG[$i].")"):($POST_FLAG[$i]==0?"รอส่ง":"ล้มเหลว")?></font></td>
	  <td><font size='2'><?=$SEND_PRODATE[$i]?></font></td>
	  <td><font size='2'><?=$SMS_TRANS_DESC[$i]?><hr/><?=$MESSAGE_TEXT[$i]?></font></td>
	  <td><font size='2'><a href="?PK_ID=<?=$PK_ID[$i]?>&MEMBER_NO=<?=$MEMBER_NO[$i]?>&SMS_TRANS_CODE=<?=$SMS_TRANS_CODE[$i]?>&Resend=1">กดเพื่อส่งอีกครั้ง</a></font></td>
	  </tr>
	  <?php
	}
?>
</table>
<?php 

 ?>