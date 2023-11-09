
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

$sql="CREATE TABLE SMSNEWSMESSAGE (
			MSG_ID NUMBER(20,0) NOT NULL, 
			MSG_TITLE VARCHAR2(30) NOT NULL, 
			MSG_DETAIL VARCHAR2(150) NOT NULL, 
			SEND_DATE DATE NOT NULL, 
			OPERATE_DATE DATE NOT NULL, 
			SENDGROUP_CODE VARCHAR2(20) NULL, 
			MEMBER_NO VARCHAR2(20) NULL, 
			SEND_TO VARCHAR2(20) DEFAULT 'ALL' NOT NULL, 
			SEND_FLAG NUMBER(1,0) DEFAULT 0 NOT NULL, 
			ENTRY_ID VARCHAR2(50) NOT NULL, 
			ENTRY_DATE DATE DEFAULT sysdate NOT NULL) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE SMSNEWSMESSAGE ADD ( CONSTRAINT SMSNEWSMESSAGE_PK PRIMARY KEY ( MSG_ID )) ";
get_single_value_oci($sql,$value);

?>
<h2>ข่าวสาร </h2> 
<?php

 if($_REQUEST["MSG_ID"]!=""||$_REQUEST["code"]!=""){
			if($_REQUEST["delete"]!=""){
				$sql="delete from SMSNEWSMESSAGE  
					where MSG_ID='".$_REQUEST["MSG_ID"]."' ";
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>ลบสำเร็จ":"<font color=red>ลบไม่สำเร็จ ".$sql)."</font><br/>";
				
			}else 
			if($_REQUEST["update"]!=""){
			
				$sql="update SMSNEWSMESSAGE set 
					MSG_TITLE='".convSQL($_REQUEST["MSG_TITLE"])."' ,
					MSG_DETAIL='".convSQL($_REQUEST["MSG_DETAIL"])."' ,
					MEMBER_NO='".convSQL($_REQUEST["MEMBER_NO"])."' ,
					SENDGROUP_CODE='".convSQL($_REQUEST["SENDGROUP_CODE"])."' ,
					SEND_DATE=to_date('".str_replace("-","",convSQL($_REQUEST["OPERATE_DATE"]))."','yyyymmdd'),
					OPERATE_DATE=to_date('".str_replace("-","",convSQL($_REQUEST["OPERATE_DATE"]))."','yyyymmdd'),
					ENTRY_ID='admin',ENTRY_DATE=sysdate,
					SEND_FLAG='".convSQL($_REQUEST["SEND_FLAG"])."'
					where MSG_ID='".$_REQUEST["MSG_ID"]."' ";
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue> ปรับปรุง สำเร็จ":"<font color=red> ปรับปรุง ไม่สำเร็จ ".$sql)."</font><br/>";

			   //	get_single_value_oci("commit",$value); 
			  // echo $sql;
			}else 
			if($_REQUEST["add"]!=""){
			
				$sql="insert into SMSNEWSMESSAGE  (MSG_TITLE,MSG_ID,MSG_DETAIL,MEMBER_NO,SEND_FLAG,ENTRY_ID,ENTRY_DATE,OPERATE_DATE,SEND_DATE,SENDGROUP_CODE)values( 
					'".convSQL($_REQUEST["MSG_TITLE"])."' ,
					'".convSQL($_REQUEST["MSG_ID"])."' ,
					'".convSQL($_REQUEST["MSG_DETAIL"])."' ,
					'".convSQL($_REQUEST["MEMBER_NO"])."' ,
					'".convSQL($_REQUEST["SEND_FLAG"])."',
					'admin',sysdate,					
					to_date('".str_replace("-","",convSQL($_REQUEST["OPERATE_DATE"]))."','yyyymmdd'),
					to_date('".str_replace("-","",convSQL($_REQUEST["OPERATE_DATE"]))."','yyyymmdd'),
					'".convSQL($_REQUEST["SENDGROUP_CODE"])."'
					) "; 
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>สร้างใหม่ สำเร็จ":"<font color=red>สร้างใหม่ ไม่สำเร็จ ".$sql)."</font><br/>";

			  // echo $sql;
			  //	get_single_value_oci("commit",$value);
			}	

$strSQL="select m.*,to_char(OPERATE_DATE,'yyyymmdd') as OPERATE_DATE_ from SMSNEWSMESSAGE m where m.MSG_ID='".$_REQUEST["MSG_ID"]."' order by m.MSG_ID asc ";
$value=array('MSG_ID','MSG_TITLE','MSG_DETAIL','OPERATE_DATE_','SENDGROUP_CODE','MEMBER_NO','SEND_FLAG');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$MSG_ID = $list_info1[$n][$m++];
			$MSG_TITLE = conv($list_info1[$n][$m++]);
			$MSG_DETAIL = conv($list_info1[$n][$m++]);
			$OPERATE_DATE=$list_info1[$n][$m++];
			$SENDGROUP_CODE = $list_info1[$n][$m++];
			$MEMBER_NO = $list_info1[$n][$m++];
			$SEND_FLAG = $list_info1[$n][$m++];
		    $m=0;
		}
	
if($_REQUEST["code"]!=""){
$strSQL="select max(MSG_ID)+1 as MSG_ID,to_char(sysdate,'yyyymmdd')  as OPERATE_DATE_ from SMSNEWSMESSAGE ";
$value=array('MSG_ID','OPERATE_DATE_');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
     $m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$MSG_ID = $list_info1[$n][$m++];
			$OPERATE_DATE=$list_info1[$n][$m++];
	    }
		
		if($MSG_ID=="") $MSG_ID=1;
}	
		
?>
<table class=responstable width="100%">
<form action="" method="post" onsubmit="return confirm('กรุณายืนยันทำรายการ')">
<tr><td colspan="2" ><?=$msg?></td></tr>
<tr><th style="width:100px;"><font size='2'>ลำดับ</font></th><td><input type="text" name="MSG_ID" value="<?=$MSG_ID?>"/></td></tr>
<tr><th><font size='2'>หัวข้อ</font></th><td><input type="text" name="MSG_TITLE" value="<?=$MSG_TITLE?>" style="width:200px;" /></td></tr>
<tr><th><font size='2'>รายละเอียด</font></th><td><input type="text" name="MSG_DETAIL" value="<?=$MSG_DETAIL?>" style="width:450px;" /></td></tr>
<tr><th><font size='2'>วันที่</font></th><td><input type="text" name="OPERATE_DATE" value="<?=$OPERATE_DATE?>" style="width:100px;" />(yyyymmdd)</td></tr>
<tr style="display:none;"><th><font size='2'>SENDGROUP_CODE</font></th><td><input type="text" name="SENDGROUP_CODE" value="<?=$SENDGROUP_CODE?>" style="width:150px;" /></td></tr>
<tr style="display:none;"><th><font size='2'>MEMBER_NO</font></th><td><input type="text" name="MEMBER_NO" value="<?=$MEMBER_NO?>" style="width:100px;" /></td></tr>
<tr><th><font size='2'>สถานะส่งแล้ว</font></th><td>
   <select name="SEND_FLAG" style="width:50px;" >
            <option value="1" <?=$SEND_FLAG==1?"selected":""?> >Yes </option>
            <option value="0" <?=$SEND_FLAG==0?"selected":""?>>No </option>
		</select>	
</td></tr>
<tr><td></td><td>
<input type="button" name="cancel" value="cancel"  style="width:50px;height:25;" onclick="window.location='?MSG_ID=';"/>
<input type="submit" name="update" value="update" style="width:50px;height:25;"/> 
<input type="submit" name="add" value="add"  style="width:50px;height:25;"/>
<input type="submit" name="delete" value="delete"  style="width:50px;height:25;"/>

</td></tr>
</form>
</table>
<?php 


}else{ 

	
	$strSQL="select * from SMSNEWSMESSAGE order by OPERATE_DATE desc ";
	$value=array('MSG_ID','MSG_TITLE','MSG_DETAIL','OPERATE_DATE','SENDGROUP_CODE','MEMBER_NO','SEND_FLAG');
	list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
			$m=0;
			for($n=0;$n<$Num_Rows1;$n++){
				$MSG_ID[$n] = $list_info1[$n][$m++];
				$MSG_TITLE[$n] = conv($list_info1[$n][$m++]);
				$MSG_DETAIL[$n] = conv($list_info1[$n][$m++]);
				$OPERATE_DATE[$n]=$list_info1[$n][$m++];
				$SENDGROUP_CODE[$n] = $list_info1[$n][$m++];
				$MEMBER_NO[$n] = $list_info1[$n][$m++];
				$SEND_FLAG[$n] = $list_info1[$n][$m++];
				$m=0;
			}
	
?>	
<table class=responstable width="100%">
  
  <tr>
    <th style="width:100px;"><font size='2'>วันที่</font></th>
    <th ><span><font size='2'>หัวข้อ</font></span></th>
    <th style="display:none;"><font size='2'>ข้อความ</font></th>
    <th style="display:none;"><font size='2' >Sendgroup_code</font></th>
    <th style="width:50px;"><font size='2'>สถานะส่งแล้ว</font></th>
	<th style="width:50px;"><font size='2'><a href="?code=NEW">Add</a></font></th>
		
		

	  
		<?php
//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
	  <tr><td><font size='2'><?=$OPERATE_DATE[$i]?></font></td>
	  <td><font size='2'><?=$MSG_TITLE[$i]?></font></td>
	  <td style="display:none;"><font size='2'><?=$MSG_DETAIL[$i]?></font></td>
	  <td style="display:none;"><font size='2'><?=$SENDGROUP_CODE[$i]?></font></td>
	  <td><font size='2'><?=$SEND_FLAG[$i]==1?"Y":"N"?></font></td>
	  <td><font size='2'><a href="?MSG_ID=<?=$MSG_ID[$i]?>">Edit</a></font></td></tr>
	  <?php
	}
?>
</table>
<?php 

}
 ?>