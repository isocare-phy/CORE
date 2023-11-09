<style>
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

</style>
<head><link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">

  
      <link rel="stylesheet" href="css/style.css">
	  </head>
<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');

$strSQL="alter table smspatternconfig modify SMS_TRANS_DESC varchar2(150)";
get_single_value_oci($strSQL,$value);
$strSQL="alter table smspatternconfig add SMS_ORDER NUMBER(3,0) DEFAULT 0 NOT NULL ";
get_single_value_oci($strSQL,$value);
$strSQL="alter table smspatternconfig add USE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL ";
get_single_value_oci($strSQL,$value);
$strSQL="alter table smspatternconfig add scheduler_time varchar2(150)";
get_single_value_oci($strSQL,$value);
$strSQL="alter table smspatternconfig add realtime_flag number(1,0) default 0 not null ";
get_single_value_oci($strSQL,$value);
$strSQL="alter table smspatternconfig add SMS_VIEWS_SQL varchar2(4000)";
get_single_value_oci($strSQL,$value);


?>
<h2>กำหนดรูปแบบข้อความ  </h2> 
<?php 

if($_REQUEST["code"]!=""){

				if($_REQUEST["delete"]!=""){
					$sql="delete from smspatternconfig  
						where sms_trans_code='".$_REQUEST["code"]."' ";
					get_single_value_oci($sql,$value); 
					$msg=($status==1?"<font color=blue>ลบสำเร็จ":"<font color=red>ลบไม่สำเร็จ ".$sql)."</font><br/>";
					
				}else 
				if($_REQUEST["update"]!=""){
					$sql="update smspatternconfig set 
						SMS_TRANS_DESC='".convSQL($_REQUEST["desc"])."' ,
						SMS_PATTERN='".convSQL($_REQUEST["pattern"])."' ,
						enable_flag='".convSQL($_REQUEST["enable_flag"])."' ,
						use_flag='".convSQL($_REQUEST["use_flag"])."' ,
						sms_order='".convSQL($_REQUEST["sms_order"])."' ,
						scheduler_time='".convSQL($_REQUEST["scheduler"])."' ,
						realtime_flag='".convSQL($_REQUEST["realtime"])."' ,
						from_system='".convSQL($_REQUEST["from_system"])."', 
						SMS_TRANS_SQL='".convSQL($_REQUEST["trans_sql"])."', 
						SMS_VIEWS_SQL='".convSQL($_REQUEST["views_sql"])."' 
						where sms_trans_code='".$_REQUEST["code"]."' ";
					get_single_value_oci($sql,$value); 
					$msg=($status==1?"<font color=blue> ปรับปรุง สำเร็จ":"<font color=red> ปรับปรุง ไม่สำเร็จ ".$sql)."</font><br/>";

				   //	get_single_value_oci("commit",$value); 
				  // echo $sql;
				}else 
				if($_REQUEST["add"]!=""){
					$sql="insert into smspatternconfig (SMS_TRANS_DESC,SMS_PATTERN,enable_flag,scheduler_time,realtime_flag,from_system,SMS_TRANS_SQL,SMS_VIEWS_SQL,sms_trans_code) values( 
						'".convSQL($_REQUEST["desc"])."' ,
						'".convSQL($_REQUEST["pattern"])."' ,
						'".convSQL($_REQUEST["enable_flag"])."' ,
						'".convSQL($_REQUEST["use_flag"])."' ,
						'".convSQL($_REQUEST["sms_order"])."' ,
						'".convSQL($_REQUEST["scheduler"])."' ,
						'".convSQL($_REQUEST["realtime"])."' ,
						'".convSQL($_REQUEST["from_system"])."', 
						'".convSQL($_REQUEST["trans_sql"])."', 
						'".convSQL($_REQUEST["views_sql"])."' ,
						'".$_REQUEST["code"]."' ) "; 
					get_single_value_oci($sql,$value); 
					$msg=($status==1?"<font color=blue>สร้างใหม่ สำเร็จ":"<font color=red>สร้างใหม่ ไม่สำเร็จ ".$sql)."</font><br/>";

				  // echo $sql;
				  //	get_single_value_oci("commit",$value);
				}

			$strSQL="select * from smspatternconfig where sms_trans_code='".$_REQUEST["code"]."' order by SMS_ORDER asc ";
			$value=array('SMS_TRANS_CODE','SMS_TRANS_DESC','SMS_PATTERN','ENABLE_FLAG','USE_FLAG','SMS_ORDER','SCHEDULER_TIME','REALTIME_FLAG','FROM_SYSTEM','SMS_TRANS_SQL','SMS_VIEWS_SQL');
			list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
					$m=0;
					for($n=0;$n<$Num_Rows1;$n++){
						$_REQUEST["code"] = $list_info1[$n][$m++];
						$_REQUEST["desc"] = conv($list_info1[$n][$m++]);
						$_REQUEST["pattern"] = conv($list_info1[$n][$m++]);
						$_REQUEST["enable_flag"] = $list_info1[$n][$m++];
						$_REQUEST["use_flag"] = $list_info1[$n][$m++];
						$_REQUEST["sms_order"] = $list_info1[$n][$m++];
						$_REQUEST["scheduler"] = $list_info1[$n][$m++];
						$_REQUEST["realtime"] = $list_info1[$n][$m++];
						$_REQUEST["from_system"] = $list_info1[$n][$m++];
						$_REQUEST["trans_sql"] = $list_info1[$n][$m++];
						$_REQUEST["views_sql"] = $list_info1[$n][$m++];
						$m=0;
					}

										
			if(isset($_REQUEST["g"])){
				$sql="".$_REQUEST["views_sql"];
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>Generate View Script สำเร็จ":"<font color=red>Generate View Script ไม่สำเร็จ ".$sql)."</font><br/>";
			}

?>
<table class=responstable > 
<form action="" method="post" onsubmit="return confirm('กรุณายืนยันทำรายการ')">
<tr><th style="width:100px;"><font size='2'>วันที่แก้ไข</font></th><td>
<input type="text" name="operate_date" value="<?=(isset($_REQUEST["operate_date"]))?$_REQUEST["operate_date"]:date("Ymd")?>" style="width:100px;" />
</td></tr><tr><th><font size='2'>ROW NUM</font></th><td>
<input type="text" name="rownum" value="<?=(isset($_REQUEST["rownum"]))?$_REQUEST["rownum"]:10?>" style="width:100px;" />
</td></tr><tr><td colspan=2>
<input type="button" name="back" value="ย้อนกลับ"  style="width:50px;height:25;" onclick="window.location='?code=';"/>
<input type="submit" name="viewdata" value="viewdata"  style="width:100px;height:25;"/>
</td></tr>
<tr><td colspan=2><hr/></td><tr>
<tr><td colspan=2>
<?php 

			  if(isset($_REQUEST["viewdata"])){
			 
			?>
			<table class=table11_1 width="100%">
				  <tr><font size='2'><td>member_no</td><td>phone_no</td><td>pk_id</td><td>SMS Text</td><td>-</td></tr>
			<?php

			   $sql="select  ".$_REQUEST["pattern"]." as sms_text,s.member_no,
									nvl((select telephone_number from smsphonebook where member_no=s.member_no and enable_flag=1 and usephonebook_flag=1 ) ,s.phone_number) as phone_number,
									s.ref_no,s.pk_id from (
								select * from ".str_replace("?",$_REQUEST["operate_date"],$_REQUEST["trans_sql"])." 
								and rownum < ".$_REQUEST["rownum"]."
								) s ";
			   echo "<hr/>".$sql."<hr/>";
			   $value=array('SMS_TEXT','MEMBER_NO','PHONE_NUMBER','REF_NO','PK_ID');
				list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
						$m=0;
						for($n=0;$n<$Num_Rows1;$n++){
							$sms_text[$n] = $list_info1[$n][$m++];
							$member_no[$n] = $list_info1[$n][$m++];
							$phone_number[$n] = $list_info1[$n][$m++];
							$ref_no[$n] = $list_info1[$n][$m++];
							$pk_id[$n] = $list_info1[$n][$m++];
							?>
							 <tr><td><?=$member_no[$n]?></td><td><?=$phone_number[$n]?></td><td><?=$pk_id[$n]?></td><td><?=$sms_text[$n]?></td></tr>
							<?php
							$m=0;
						}

					?>
			   </table>
			<?php		
						
			  }
	?>		  
</td><tr>
<tr><td colspan="2"><hr/></td><tr>
<tr><td colspan="2"><?=$msg?></td></tr>
<tr><th><font size='2'>ลำดับ</font></th><td><input type="text" name="sms_order" value="<?=$_REQUEST["sms_order"]?>" style="width:50px;" /></td></tr>
<tr><th><font size='2'>รหัสรูปแบบ</font></th><td><input type="text" name="code" value="<?=$_REQUEST["code"]?>"/></td></tr>
<tr><th><font size='2'>ประเภทรูปแบบ</font></th><td><input type="text" name="desc" value="<?=$_REQUEST["desc"]?>"/></td></tr>
<tr><th><font size='2'>SQL</font></th><td><textarea name="pattern" cols="4" rows="1" style="width:400px;height:50px;" ><?=$_REQUEST["pattern"]?></textarea></td></tr>
<tr><th><font size='2'>สถานะรูปแบบ</font></th><td>
   <select name="enable_flag" style="width:50px;" >
            <option value="1" <?=$_REQUEST["enable_flag"]==1?"selected":""?> >Yes </option>
            <option value="0" <?=$_REQUEST["enable_flag"]==0?"selected":""?>>No </option>
		</select>	
</td></tr>
<tr><th><font size='2'>สถานะเปิดใช้งาน</font></th><td>
   <select name="use_flag" style="width:50px;" >
            <option value="1" <?=$_REQUEST["use_flag"]==1?"selected":""?> >Yes </option>
            <option value="0" <?=$_REQUEST["use_flag"]==0?"selected":""?>>No </option>
		</select>	
</td></tr>
<tr><th><font size='2'>เวลาที่กำหนด</font></th><td><input type="text" name="scheduler" value="<?=$_REQUEST["scheduler"]?>"/>(HH24:MM,HH24:MM, ..)</td></tr>
<tr><th><font size='2'>ส่งทันที</font></th><td><input type="text" name="realtime" value="<?=$_REQUEST["realtime"]?>" style="width:50px;" /></td></tr>
<tr><th><font size='2'>Code ระบบงาน</font></th><td><input type="text" name="from_system" value="<?=$_REQUEST["from_system"]?>" style="width:50px;" maxlength="3"/></td></tr>
<tr><th><font size='2'>TRANS SQL</font></th><td><textarea name="trans_sql" cols="4" rows="5" style="width:600px;height:50px;" ><?=$_REQUEST["trans_sql"]?></textarea></td></tr>
<tr><th><font size='2'>VIEWS SQL</font></th><td><textarea name="views_sql" cols="4" rows="5" style="width:600px;height:150px;" ><?=$_REQUEST["views_sql"]?></textarea></td></tr>
<tr><td colspan=2>
<input type="button" name="cancel" value="cancel"  style="width:50px;height:25;" onclick="window.location='?code=';"/>
<input type="submit" name="update" value="update" style="width:50px;height:25;"/> 
<input type="submit" name="add" value="add"  style="width:50px;height:25;"/>
<input type="submit" name="delete" value="delete"  style="width:50px;height:25;"/>
<input type="submit" name="g" value="genView"  style="width:100px;height:25;"/>
</td>
</tr>
</form>
</table>

<?php 

  
 }else{ 


$strSQL="select * from smspatternconfig where use_flag=1 order by sms_order asc ";
$value=array('SMS_TRANS_CODE','SMS_TRANS_DESC','SMS_PATTERN','ENABLE_FLAG','FROM_SYSTEM','SMS_TRANS_SQL','SMS_VIEWS_SQL','SCHEDULER_TIME','REALTIME_FLAG');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$code[$n] = $list_info1[$n][$m++];
			$desc[$n] = conv($list_info1[$n][$m++]);
			$pattern[$n] = conv($list_info1[$n][$m++]);
			$enable_flag[$n] = $list_info1[$n][$m++];
			$from_system[$n] = $list_info1[$n][$m++];
			$trans_sql[$n] = $list_info1[$n][$m++];
			$view_sql[$n] = $list_info1[$n][$m++];
			$scheduler_time[$n] = $list_info1[$n][$m++];
			$realtime_flag[$n] = $list_info1[$n][$m++];
		    $m=0;
		}
		?>
	<table class=responstable width="100%">

  <tr>
    <th><font size='2'><P Align=left>ลำดับ</p>
    <th data-th="Driver details">
	
	<span><font size='2'>ข้อความ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></span></th>
    <th><font size='2'>สถานะ</font></th>
    <th><font size='2'>ตรางเวลา</font></th>
    <th><font size='2'>ตรงตามเวลา</font></th>
	 <th><font size='2'>สร้างตัวดึงข้อมูล&nbsp;&nbsp;</font></th>
	<!--<th><font size='2'>ดูข้อมูล &nbsp;&nbsp;&nbsp;</font></th>-->
	<th><a href="?code=NEW"  style="text-decoration: none;" ><font size='2'>Add</font></a></th>
  </tr>	
<!--<table class=table11_1 width="100%">
	  <tr bgcolor="#CCDD00"><td>CODE</td><td>DESC</td><td>ENABLE</td><td>SCHEDUER</td><td>REALTIME</td><td>GEN SCRIPT</td><td><a href="?code=NEW">Add</a></td></tr>-->
		<?php
//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
	  <tr>
	  <td><font size='2'><?=($i+1)?></font></td>
	  <td><a href="?code=<?=$code[$i]?>"><font size='2'><?=$desc[$i]?></font></a></td>
	  <td><font size='2'><?=$enable_flag[$i]==1?"Y":"N"?></font></td>
	  <td><font size='2'><?=$scheduler_time[$i]?></font></td>
	  <td><font size='2'><?=$realtime_flag[$i]==1?"Y":"N"?></font></td>
	  <td><a href="?code=<?=$code[$i]?>&g=1"><font size='2'>สร้างตัวดึงข้อมูล</font></a></td>
	  <!--<td><a href="?code=<?=$code[$i]?>&viewdata=1"><font size='2'>ดูข้อมูล</font></a></td>-->
	  <td><a href="?code=<?=$code[$i]?>"><font size='2'>Edit</font></a></td>
	  </tr>
	  <?php
	}
?>
</table>
<?php

 } 
 
 ?>