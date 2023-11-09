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
<h2>จัดคู่เบอร์สมาชิก Phonebook</h2> 
<?php

 if($_REQUEST["MEMBER_NO"]!=""){
			if($_REQUEST["delete"]!=""){
				$sql="delete from SMSPHONEBOOK  
					where MEMBER_NO='".$_REQUEST["MEMBER_NO"]."' ";
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>ลบสำเร็จ":"<font color=red>ลบไม่สำเร็จ ".$sql)."</font><br/>";
				
			}else 
			if($_REQUEST["update"]!=""){
			
				$sql="update SMSPHONEBOOK set 
					MEMBER_FULLNAME='".convSQL($_REQUEST["MEMBER_FULLNAME"])."' ,
					TELEPHONE_NUMBER='".convSQL($_REQUEST["TELEPHONE_NUMBER"])."' ,
					USEPHONEBOOK_FLAG='".convSQL($_REQUEST["USEPHONEBOOK_FLAG"])."' ,
					ENABLE_FLAG='".convSQL($_REQUEST["ENABLE_FLAG"])."' ,
					UPDATE_DATE=sysdate
					where MEMBER_NO='".$_REQUEST["MEMBER_NO"]."' ";
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue> ปรับปรุง สำเร็จ":"<font color=red> ปรับปรุง ไม่สำเร็จ ".$sql)."</font><br/>";

			   //	get_single_value_oci("commit",$value); 
			  // echo $sql;
			}else 
			if($_REQUEST["add"]!=""){
				$sql="insert into SMSPHONEBOOK  (MEMBER_FULLNAME,MEMBER_NO,TELEPHONE_NUMBER,USEPHONEBOOK_FLAG,ENABLE_FLAG)values( 
					'".convSQL($_REQUEST["MEMBER_FULLNAME"])."' ,
					'".convSQL($_REQUEST["MEMBER_NO"])."' ,
					'".convSQL($_REQUEST["TELEPHONE_NUMBER"])."' ,
					'".convSQL($_REQUEST["USEPHONEBOOK_FLAG"])."' ,
					'".convSQL($_REQUEST["ENABLE_FLAG"])."'
					) "; 
				get_single_value_oci($sql,$value); 
				$msg=($status==1?"<font color=blue>สร้างใหม่ สำเร็จ":"<font color=red>สร้างใหม่ ไม่สำเร็จ ".$sql)."</font><br/>";

			  // echo $sql;
			  //	get_single_value_oci("commit",$value);
			}	

$strSQL="select * from SMSPHONEBOOK where MEMBER_NO='".$_REQUEST["MEMBER_NO"]."' order by MEMBER_NO asc ";
$value=array('MEMBER_NO','MEMBER_FULLNAME','TELEPHONE_NUMBER','USEPHONEBOOK_FLAG','ENABLE_FLAG','UPDATE_DATE');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$MEMBER_NO = $list_info1[$n][$m++];
			$MEMBER_FULLNAME = conv($list_info1[$n][$m++]);
			$TELEPHONE_NUMBER = conv($list_info1[$n][$m++]);
			$USEPHONEBOOK_FLAG=$list_info1[$n][$m++];
			$ENABLE_FLAG = $list_info1[$n][$m++];
			$UPDATE_DATE = $list_info1[$n][$m++];
		    $m=0;
		}
?>
<table class=responstable width="100%">
<form action="" method="post" onsubmit="return confirm('กรุณายืนยันทำรายการ')">
<tr><td colspan="2"><?=$msg?></td></tr>
<tr><th  style="width:100px;"><font size='2'>หมายเลขสมาชิก</font></th><td><input type="text" name="MEMBER_NO" value="<?=$MEMBER_NO?>"/></td></tr>
<tr><th><font size='2'>ชื่อ-สกุลสมาชิก</font></th><td><input type="text" name="MEMBER_FULLNAME" value="<?=$MEMBER_FULLNAME?>" style="width:250px;" /></td></tr>
<tr><th><font size='2'>เบอร์โทรศัพ</font></th><td><input type="text" name="TELEPHONE_NUMBER" value="<?=$TELEPHONE_NUMBER?>" size="10" maxlength="10" /></td></tr>
<tr><th><font size='2'>สถานะใช้สมุดโทรศัพท์</font></th><td>
   <select name="USEPHONEBOOK_FLAG" style="width:50px;" >
            <option value="1" <?=$USEPHONEBOOK_FLAG==1?"selected":""?> >Yes </option>
            <option value="0" <?=$USEPHONEBOOK_FLAG==0?"selected":""?>>No </option>
		</select>	
</td></tr>
<tr><th><font size='2'>สถานะ</font></th><td>
   <select name="ENABLE_FLAG" style="width:50px;" >
            <option value="1" <?=$ENABLE_FLAG==1?"selected":""?> >Yes </option>
            <option value="0" <?=$ENABLE_FLAG==0?"selected":""?>>No </option>
		</select>	
</td></tr>
<tr><td></td><td>
<input type="button" name="cancel" value="cancel"  style="width:50px;height:25;" onclick="window.location='?MEMBER_NO=';"/>
<input type="submit" name="update" value="update" style="width:50px;height:25;"/> 
<input type="submit" name="add" value="add"  style="width:50px;height:25;"/>
<input type="submit" name="delete" value="delete"  style="width:50px;height:25;"/>

</td></tr>
</form>
</table>
<?php 


}else{ 

	
	if(isset($_REQUEST["ACTION"])){    
		//	truncate table SMSPHONEBOOK;
			$strSQL="alter table SMSPHONEBOOK modify TELEPHONE_NUMBER varchar2(50)";
			get_value_many_oci($strSQL,$value);
			$strSQL="insert into SMSPHONEBOOK  (MEMBER_NO,MEMBER_FULLNAME,TELEPHONE_NUMBER) 
			select 
										 m.member_no as member_no 
										 ,m.memb_name||' '||m.memb_surname as MEMBER_FULLNAME
										 ,nvl(replace(NVL(m.addr_mobilephone,m.addr_phone),'-'),'-') as TELEPHONE_NUMBER
										from mbmembmaster m
										where m.resign_status <> 1 and m.member_status <> -9 and m.member_no <>'00000000'
										order by m.member_no asc ";		
			get_value_many_oci($strSQL,$value);
			get_value_many_oci("COMMIT",$value);
		//	commit;
		    $strSQL="update SMSPHONEBOOK s set 
						MEMBER_FULLNAME=( select m.memb_name||' '||m.memb_surname as MEMBER_FULLNAME from mbmembmaster m where m.member_no =s.MEMBER_NO )
						,TELEPHONE_NUMBER=( select nvl(replace(NVL(m.addr_mobilephone,m.addr_phone),'-'),'-') as TELEPHONE_NUMBER from mbmembmaster m where m.member_no =s.MEMBER_NO ) 
						,UPDATE_DATE=sysdate
						,enable_flag=(case  length(trim(s.TELEPHONE_NUMBER)) when 10 then 1 else 0 end )
						,USEPHONEBOOK_FLAG=(case length(trim(s.TELEPHONE_NUMBER)) when 10  then 1 else 0 end )  ";
			get_value_many_oci($strSQL,$value);
			get_value_many_oci("COMMIT",$value);
	
	      /*
			$strSQL="select 
							 m.member_no as member_no 
							 ,m.memb_name||' '||m.memb_surname as MEMBER_FULLNAME
							 ,replace(NVL(m.addr_mobilephone,m.addr_phone),'-') as TELEPHONE_NUMBER
							from mbmembmaster m
							where m.resign_status <> 1 and m.member_status <> -9 and m.member_no <>'00000000'
							order by m.member_no asc ";
			$value=array('MEMBER_NO','MEMBER_FULLNAME','TELEPHONE_NUMBER');
			list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
			//$msg="<font color=blue>สร้างใหม่ สำเร็จ จำนวน ".$Num_Rows1." รายการ</font><br/>";
			$m=0;
			for($n=0;$n<$Num_Rows1;$n++){
				$_REQUEST["MEMBER_NO"] = $list_info1[$n][$m++];
				$_REQUEST["MEMBER_FULLNAME"]= conv($list_info1[$n][$m++]);
				$_REQUEST["TELEPHONE_NUMBER"]= conv($list_info1[$n][$m++]);
				
					$sql="insert into SMSPHONEBOOK  (MEMBER_FULLNAME,MEMBER_NO,TELEPHONE_NUMBER)values( 
						'".convSQL($_REQUEST["MEMBER_FULLNAME"])."' ,
						'".convSQL($_REQUEST["MEMBER_NO"])."' ,
						'".convSQL($_REQUEST["TELEPHONE_NUMBER"])."' 
						) "; 
					get_single_value_oci($sql,$value); 
					
					$sql="update SMSPHONEBOOK set 
						MEMBER_FULLNAME='".convSQL($_REQUEST["MEMBER_FULLNAME"])."' ,
						TELEPHONE_NUMBER='".convSQL($_REQUEST["TELEPHONE_NUMBER"])."' ,
						UPDATE_DATE=sysdate() 
						where MEMBER_NO='".$_REQUEST["MEMBER_NO"]."' ";
					get_single_value_oci($sql,$value); 
					
				$m=0;
			}
	      */
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
	  
	if(isset($_REQUEST["USEPHONEBOOK_FLAG_"]))
	  $_SESSION["USEPHONEBOOK_FLAG_"]=$_REQUEST["USEPHONEBOOK_FLAG_"];
	  
	if(isset($_REQUEST["TELEPHONE_NUMBER_"]))
	  $_SESSION["TELEPHONE_NUMBER_"]=$_REQUEST["TELEPHONE_NUMBER_"];
	  
	if(isset($_REQUEST["ENABLE_FLAG_"]))
	  $_SESSION["ENABLE_FLAG_"]=$_REQUEST["ENABLE_FLAG_"];
	  
	if(isset($_REQUEST["PHONE_CHECK_FLAG_"]))
	  $_SESSION["PHONE_CHECK_FLAG_"]=$_REQUEST["PHONE_CHECK_FLAG_"];
	else
	  $_SESSION["PHONE_CHECK_FLAG_"]="A";
			  
	if(isset($_REQUEST["enable_all_btn"])){
	    $strSQL="update SMSPHONEBOOK set enable_flag=1,USEPHONEBOOK_FLAG=1 ";
		get_value_many_oci($strSQL,$value);
	    $strSQL="commit ";
		get_value_many_oci($strSQL,$value);
    }	
		
	if(isset($_REQUEST["disable_all_btn"])){
	    $strSQL="update SMSPHONEBOOK set enable_flag=0 ";
		get_value_many_oci($strSQL,$value);
	    $strSQL="commit ";
		get_value_many_oci($strSQL,$value);
    }	
	  
	 $strSQL="select count(MEMBER_NO) as CNT from SMSPHONEBOOK  ";
	 $value=array('CNT');
	 list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
	 $CNT = $list_info1[0][0];
	 
	 if($_SESSION["PHONE_CHECK_FLAG_"]!="0"){ 
			$strSQL="select * from (
				select s.*, ROWNUM rnum from SMSPHONEBOOK s
				where rownum <= ".(($_SESSION["page_no"])*$rowperpage)." 
				".($_SESSION["MEMBER_NO_"]!=""?(" and MEMBER_NO like '%".$_SESSION["MEMBER_NO_"]."%' "):"")."
				".($_SESSION["MEMBER_FULLNAME"]!=""?(" and MEMBER_FULLNAME like '%".convSQL($_SESSION["MEMBER_FULLNAME"])."%' "):"")."
				".($_SESSION["USEPHONEBOOK_FLAG_"]!=""?(" and USEPHONEBOOK_FLAG like '%".convSQL($_SESSION["USEPHONEBOOK_FLAG_"])."%' "):"")."
				".($_SESSION["TELEPHONE_NUMBER_"]!=""?(" and TELEPHONE_NUMBER like '%".convSQL($_SESSION["TELEPHONE_NUMBER_"])."%' "):"")."
				".($_SESSION["ENABLE_FLAG_"]!=""?(" and ENABLE_FLAG like '%".convSQL($_SESSION["ENABLE_FLAG_"])."%' "):"")."
				order by MEMBER_NO asc  )
				where rnum >= ".(($_SESSION["page_no"]-1)*$rowperpage)."  ";
	 }else{
        $strSQL="select * from (
		        select s.*, ROWNUM rnum from SMSPHONEBOOK s
				where rownum <= ".(($_SESSION["page_no"])*$rowperpage)." 
				order by MEMBER_NO asc  )
				where rnum >= ".(($_SESSION["page_no"]-1)*$rowperpage)."  ";
     }	 
				
	//echo $strSQL;
	
	$value=array('MEMBER_NO','MEMBER_FULLNAME','TELEPHONE_NUMBER','USEPHONEBOOK_FLAG','ENABLE_FLAG','UPDATE_DATE');
	list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
			$m=0;
			for($n=0;$n<$Num_Rows1;$n++){
				$MEMBER_NO[$n] = $list_info1[$n][$m++];
				$MEMBER_FULLNAME[$n] = conv($list_info1[$n][$m++]);
				$TELEPHONE_NUMBER[$n] = conv($list_info1[$n][$m++]);
				$USEPHONEBOOK_FLAG[$n]=$list_info1[$n][$m++];
				$ENABLE_FLAG[$n] = $list_info1[$n][$m++];
				$UPDATE_DATE[$n] = $list_info1[$n][$m++];
				$m=0;
			}
	
//echo $Num_Rows1;
?>
<table class=responstable   width="100%"><tr>
<form action="" method="get" onsubmit="return confirm('ยืนยันทำรายการ?')">
<input type="hidden" name="p" value="member" />
<input type="hidden" name="page_no" value="1" />
<tr><th style="width:50px;background-color:#dbf2fe;" ><font size='2'>เลขทะเบียน:</font></th>
<td><input type="text" name="MEMBER_NO_" value="<?=$_SESSION["MEMBER_NO_"]?>" style="width:100px;"  /></td>
	  <th style="width:70px;background-color:#dbf2fe;" ><font size='2'>ชื่อ/สกุล:</font></th>
	  <td style="background-color:#ffffff;" ><input type="text" name="MEMBER_FULLNAME" value="<?=$_SESSION["MEMBER_FULLNAME"]?>" style="width:150px;" /></td>
	 <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>ใช้สมุดโทรศัพท์:</font></th>
	 <td style="background-color:#ffffff;" >
   <select name="USEPHONEBOOK_FLAG_" style="width:50px;" >
            <option value="" <?=$_SESSION["USEPHONEBOOK_FLAG_"]==""?"selected":""?> >ALL </option>
            <option value="1" <?=$_SESSION["USEPHONEBOOK_FLAG_"]=="1"?"selected":""?> >Yes </option>
            <option value="0" <?=$_SESSION["USEPHONEBOOK_FLAG_"]=="0"?"selected":""?>>No </option>
		</select>	
	 </td>
</tr>
<tr>
	  <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>เบอร์โทร:</font></th>
	  <td style="background-color:#ffffff;" ><input type="text" name="TELEPHONE_NUMBER_" value="<?=$_SESSION["TELEPHONE_NUMBER_"]?>" style="width:100px;" maxlength="10" /></td>	 
	  <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>สถานะใช้งาน:</font></th>
	  <td style="background-color:#ffffff;" >
   <select name="ENABLE_FLAG_" style="width:50px;" >
            <option value="" <?=$_SESSION["ENABLE_FLAG_"]==""?"selected":""?> >ALL </option>
            <option value="1" <?=$_SESSION["ENABLE_FLAG_"]=="1"?"selected":""?> >Yes </option>
            <option value="0" <?=$_SESSION["ENABLE_FLAG_"]=="0"?"selected":""?>>No </option>
		</select>	</td>
      <th style="width:50px;background-color:#dbf2fe;" ><font size='2'>เบอร์โทรไม่ถูกต้อง:</font></th>
	  <td style="background-color:#ffffff;" >
      <select name="PHONE_CHECK_FLAG_" style="width:50px;" >
            <option value="A" <?=$_SESSION["PHONE_CHECK_FLAG_"]=="A"?"selected":""?> >ALL </option>
            <option value="1" <?=$_SESSION["PHONE_CHECK_FLAG_"]=="1"?"selected":""?> >NO </option>
            <option value="0" <?=$_SESSION["PHONE_CHECK_FLAG_"]=="0"?"selected":""?>>Yes </option>
		</select>	</td>
	</td>
</tr>
<tr><td colspan="6"><input type="submit" name="Search" value="ค้น" style="width:70px;" />
<input type="submit" name="enable_all_btn" value="เปิดใช้งานทั้งหมด" style="width:150px;" />
<input type="submit" name="disable_all_btn" value="ยกเลิกทั้งหมด" style="width:150px;" />
<input type="submit" name="ACTION" value="ดึงข้อมูลทั้งหมดจากฐานข้อมูลสหกรณ์" style="width:280px;"/>
</td>
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
    <th><font size='2'>ลำดับ&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></th>
    <th data-th="Driver details"><span><font size='2'>หมายเลขสมาชิก</font></span></th>
    <th><font size='2'>ชื่อ 
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
	&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></th>
    <th><font size='2'>เบอร์ &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></th>
    <th><font size='2'>สถานะใช้สมุดโทรศัพท์</font></th>
	<th><font size='2'>สถานะเปิดใช้งาน</th>
	<th><font size='2'><a href="?MEMBER_NO=NEW">Add</a></th>
  </tr>
     <!--<tr bgcolor="#CCDD00"><td>NO</td><td>MEMBER_NO</td><td>NAME</td><td>PHONE</td><td>USEPHONEBOOK</td><td>ENABLE_FLAG</td><td><a href="?MEMBER_NO=NEW">Add</a> </td></tr>-->
	  
		<?php
//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	/*
	   if(strlen(trim($TELEPHONE_NUMBER[$i]))>10){
	      $strSQLx="update SMSPHONEBOOK set enable_flag=0,USEPHONEBOOK_FLAG=1 where  MEMBER_NO='".$MEMBER_NO[$i]."'";
		  get_single_value_oci($strSQLx,$values);
		  $ENABLE_FLAG[$i]=0;
		  $USEPHONEBOOK_FLAG[$i]=1;
		}
	*/
	  ?>
	  <tr style="<?=strlen(trim($TELEPHONE_NUMBER[$i]))!=10?"color:red":""?>"><td><?=$i+1+((($_SESSION["page_no"]-1)*$rowperpage))?></td>
	  <td><font size='2'><?=$MEMBER_NO[$i]?></font></td>
	  <td><font size='2'><?=$MEMBER_FULLNAME[$i]?></font></td>
	  <td><font size='2'><?=$TELEPHONE_NUMBER[$i]?></font></td>
	  <td><font size='2'><?=$USEPHONEBOOK_FLAG[$i]==1?"Y":"N"?></font></td>
	  <td><font size='2'><?=$ENABLE_FLAG[$i]==1?"Y":"N"?></font></td>
	  <td><font size='2'><a href="?MEMBER_NO=<?=$MEMBER_NO[$i]?>">Edit</a></font></td></tr>
	  <?php
	}
?>
</table>
<?php 

}
 ?>