<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');


$sql="CREATE TABLE MAILCONFIG (
			SENDER_CODE VARCHAR2(30) NOT NULL, 
			SENDER_SERVER VARCHAR2(20) NOT NULL, 
			SENDER_PORT VARCHAR2(5) NOT NULL, 
			SENDER_NM VARCHAR2(150) NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL, 
			FROM_MAIL VARCHAR2(150), 
			FROM_MAIL_NM VARCHAR2(150), 
			USER_NAME VARCHAR2(50) NOT NULL, 
			USER_PWD VARCHAR2(50) NOT NULL, 
			TO_MAIL VARCHAR2(150), 
			TO_MAIL_NM VARCHAR2(150), 
			SUBJECT_PREFIX VARCHAR2(150), 
			BODY_PREFIX VARCHAR2(255), 
			BODY_SUFFIX VARCHAR2(255)
			) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILCONFIG ADD ( CONSTRAINT SMSCONFIG_PK PRIMARY KEY ( SENDER_CODE)) ";
get_single_value_oci($sql,$value);
$sql="insert into MAILCONFIG (SENDER_CODE,SENDER_SERVER,SENDER_PORT,SENDER_NM,ENABLE_FLAG,FROM_MAIL,FROM_MAIL_NM,USER_NAME,USER_PWD,TO_MAIL,TO_MAIL_NM,SUBJECT_PREFIX,BODY_PREFIX,BODY_SUFFIX)values(
		'MAIL','smtp.gmail.com','587','ISCOBTG',1,'isocare.iscobtg@gmail.com','สอ.เบทาโกร','isocare.iscobtg','@Icoop2018','isocare.developer@gmail.com','isocare developer','สหกรณ์:','สหกรณ์: <br><hr><br/>','<br/><hr/><br> จาก สหกรณ์ ')";
get_single_value_oci($sql,$value);

$sql="CREATE TABLE MAILBOOK (
			MEMBER_NO VARCHAR2(20) NOT NULL, 
			MEMBER_FULLNAME VARCHAR2(150) NOT NULL, 
			EMAIL VARCHAR2(100) NOT NULL, 
			USEMAILBOOK_FLAG NUMBER(1,0) DEFAULT 0 NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL, 
			UPDATE_DATE DATE default sysdate NOT NULL  
			) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILBOOK ADD ( CONSTRAINT MAILBOOK_PK PRIMARY KEY ( MEMBER_NO )) ";
get_single_value_oci($sql,$value);

$sql="CREATE TABLE MAILBOOKGROUP (
			GROUP_CODE VARCHAR2(20) NOT NULL, 
			GROUP_NAME VARCHAR2(150) NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL
			) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILBOOKGROUP ADD ( CONSTRAINT MAILBOOKGROUP_PK PRIMARY KEY ( GROUP_CODE )) ";
get_single_value_oci($sql,$value);


$sql="CREATE TABLE MAILBOOKGROUPMAP (
			GROUP_CODE VARCHAR2(20) NOT NULL, 
			MEMBER_NO VARCHAR2(20) NOT NULL, 
			ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL
			) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILBOOKGROUPMAP ADD ( CONSTRAINT MAILBOOKGROUPMAP_PK PRIMARY KEY ( GROUP_CODE,MEMBER_NO )) ";
get_single_value_oci($sql,$value);


$sql="CREATE TABLE MAILNEWSMESSAGE (
			MSG_ID NUMBER(20,0) NOT NULL, 
			MSG_TITLE VARCHAR2(255) NOT NULL, 
			SEND_DATE DATE NOT NULL, 
			OPERATE_DATE DATE NOT NULL, 
			SENDGROUP_CODE VARCHAR2(20) NULL, 
			MEMBER_NO VARCHAR2(20) NULL, 
			SEND_TO VARCHAR2(20) DEFAULT 'ALL' NOT NULL, 
			SEND_FLAG NUMBER(1,0) DEFAULT 0 NOT NULL, 
			ENTRY_ID VARCHAR2(50) NOT NULL, 
			ENTRY_DATE DATE DEFAULT sysdate NOT NULL,			
			MSG_DETAIL VARCHAR2(4000) NOT NULL) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILNEWSMESSAGE ADD ( CONSTRAINT MAILNEWSMESSAGE_PK PRIMARY KEY ( MSG_ID )) ";
get_single_value_oci($sql,$value);

$sql="CREATE TABLE MAILPATTERNCONFIG (
		MAIL_TRANS_CODE VARCHAR2(10) NOT NULL, 
		MAIL_TRANS_DESC VARCHAR2(150) NOT NULL, 
		MAIL_PATTERN VARCHAR2(500) NOT NULL, 
		FROM_SYSTEM VARCHAR2(500) NOT NULL, 
		ENABLE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL, 
		MAIL_TRANS_SQL VARCHAR2(500) NOT NULL, 
		SCHEDULER_TIME VARCHAR2(150), 
		REALTIME_FLAG NUMBER(1,0) DEFAULT 0 NOT NULL, 
		MAIL_VIEWS_SQL VARCHAR2(4000),
		MAIL_ORDER NUMBER(3,0) DEFAULT 0 NOT NULL ,
		USE_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL ) ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILPATTERNCONFIG ADD ( CONSTRAINT MAILPATTERNCONFIG_PK PRIMARY KEY ( MAIL_TRANS_CODE, FROM_SYSTEM )) ";
get_single_value_oci($sql,$value);

		
$sql="CREATE TABLE MAILTRANSACTION (
		REF_NO VARCHAR2(100) NOT NULL, 
		PK_ID VARCHAR2(100) NOT NULL, 
		MEMBER_NO VARCHAR2(10) NOT NULL, 
		EMAIL VARCHAR2(100) NOT NULL, 
		MESSAGE_TITLE VARCHAR2(255) NOT NULL, 
		CREATE_DATE DATE NOT NULL, 
		SEND_DATE DATE NOT NULL, 
		MESSAGE_STATUS NUMBER(1,0) DEFAULT 1 NOT NULL, 
		FROM_SYSTEM VARCHAR2(10), 
		MAIL_TRANS_CODE VARCHAR2(10), 
		POST_FLAG NUMBER(1,0) DEFAULT 1 NOT NULL, 
		SEND_STATUS_MSG VARCHAR2(255), 
		SEND_PRODATE DATE DEFAULT sysdate NOT NULL, 
		SEND_PROTIME VARCHAR2(5) DEFAULT '00:00' NOT NULL,
		MESSAGE_TEXT VARCHAR2(4000) NOT NULL )  ";
get_single_value_oci($sql,$value);
$sql="ALTER TABLE MAILTRANSACTION ADD ( CONSTRAINT SMSTRANSACTION_PK PRIMARY KEY ( REF_NO, PK_ID, MEMBER_NO,EMAIL ))  ";
get_single_value_oci($sql,$value);
			
			
?>
<h2>กำหนดค่าระบบ  </h2> 
<?php if($_REQUEST["SENDER_CODE"]!=""){

if($_REQUEST["delete"]!=""){
	$sql="delete from mailconfig  
		where SENDER_CODE='".$_REQUEST["SENDER_CODE"]."' ";
	get_single_value_oci($sql,$value); 
	$msg=($status==1?"<font color=blue>ลบสำเร็จ":"<font color=red>ลบไม่สำเร็จ ".$sql)."</font><br/>";
	
}else 
if($_REQUEST["update"]!=""){
	
	$sql="update mailconfig set 
		SENDER_SERVER='".convSQL($_REQUEST["SENDER_SERVER"])."' ,
		SENDER_PORT='".convSQL($_REQUEST["SENDER_PORT"])."' ,
		SENDER_NM='".convSQL($_REQUEST["SENDER_NM"])."' ,
		ENABLE_FLAG='".convSQL($_REQUEST["ENABLE_FLAG"])."' ,
		FROM_MAIL='".convSQL($_REQUEST["FROM_MAIL"])."', 
		FROM_MAIL_NM='".convSQL($_REQUEST["FROM_MAIL_NM"])."', 
		USER_NAME='".convSQL($_REQUEST["USER_NAME"])."' ,
		USER_PWD='".convSQL($_REQUEST["USER_PWD"])."' ,
		TO_MAIL='".convSQL($_REQUEST["TO_MAIL"])."', 
		TO_MAIL_NM='".convSQL($_REQUEST["TO_MAIL_NM"])."', 
		SUBJECT_PREFIX='".convSQL($_REQUEST["SUBJECT_PREFIX"])."' , 
		BODY_PREFIX='".convSQL($_REQUEST["BODY_PREFIX"])."' , 
		BODY_SUFFIX='".convSQL($_REQUEST["BODY_SUFFIX"])."' 
		where SENDER_CODE='".$_REQUEST["SENDER_CODE"]."' ";
	get_single_value_oci($sql,$value); 
	$msg=($status==1?"<font color=blue> ปรับปรุง สำเร็จ":"<font color=red> ปรับปรุง ไม่สำเร็จ ".$sql)."</font><br/>";

   //	get_single_value_oci("commit",$value); 
   //echo $sql;
}else 
if($_REQUEST["add"]!=""){
	$sql="insert into smspatternconfig  (SENDER_CODE,SENDER_SERVER,SENDER_PORT,SENDER_NM,ENABLE_FLAG,FROM_MAIL,FROM_MAIL_NM,USER_NAME,USER_PWD,TO_MAIL,TO_MAIL_NM,SUBJECT_PREFIX,BODY_PREFIX,BODY_SUFFIX)values( 
		'".convSQL($_REQUEST["SENDER_CODE"])."' ,
		'".convSQL($_REQUEST["SENDER_SERVER"])."' ,
		'".convSQL($_REQUEST["SENDER_PORT"])."' ,
		'".convSQL($_REQUEST["SENDER_NM"])."' ,
		'".convSQL($_REQUEST["ENABLE_FLAG"])."' ,
		'".convSQL($_REQUEST["FROM_MAIL"])."', 
		'".convSQL($_REQUEST["FROM_MAIL_NM"])."', 
		'".convSQL($_REQUEST["USER_NAME"])."' ,
		'".convSQL($_REQUEST["USER_PWD"])."', 
		'".convSQL($_REQUEST["TO_MAIL"])."' ,
		'".convSQL($_REQUEST["TO_MAIL_NM"])."' ,
		'".convSQL($_REQUEST["SUBJECT_PREFIX"])."' ,
		'".convSQL($_REQUEST["BODY_PREFIX"])."' ,
		'".convSQL($_REQUEST["BODY_SUFFIX"])."' 
		) "; 
	get_single_value_oci($sql,$value); 
	$msg=($status==1?"<font color=blue>สร้างใหม่ สำเร็จ":"<font color=red>สร้างใหม่ ไม่สำเร็จ ".$sql)."</font><br/>";

  // echo $sql;
  //	get_single_value_oci("commit",$value);
}

$strSQL="select * from mailconfig where SENDER_CODE='".$_REQUEST["SENDER_CODE"]."'  ";
$value=array('SENDER_CODE','SENDER_SERVER','SENDER_PORT','SENDER_NM','ENABLE_FLAG','FROM_MAIL','FROM_MAIL_NM','USER_NAME','USER_PWD','TO_MAIL','TO_MAIL_NM','SUBJECT_PREFIX','BODY_PREFIX','BODY_SUFFIX');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$_REQUEST["SENDER_CODE"] = conv($list_info1[$n][$m++]);
			$_REQUEST["SENDER_SERVER"] = $list_info1[$n][$m++];
			$_REQUEST["SENDER_PORT"] = $list_info1[$n][$m++];
			$_REQUEST["SENDER_NM"]= conv($list_info1[$n][$m++]);
			$_REQUEST["ENABLE_FLAG"] = $list_info1[$n][$m++];
			$_REQUEST["FROM_MAIL"] = $list_info1[$n][$m++];
			$_REQUEST["FROM_MAIL_NM"] = conv($list_info1[$n][$m++]);
			$_REQUEST["USER_NAME"] = $list_info1[$n][$m++];
			$_REQUEST["USER_PWD"] = $list_info1[$n][$m++];
			$_REQUEST["TO_MAIL"] = $list_info1[$n][$m++];
			$_REQUEST["TO_MAIL_NM"] = conv($list_info1[$n][$m++]);
			$_REQUEST["SUBJECT_PREFIX"] = conv($list_info1[$n][$m++]);
			$_REQUEST["BODY_PREFIX"] = conv($list_info1[$n][$m++]);
			$_REQUEST["BODY_SUFFIX"] = conv($list_info1[$n][$m++]);
		    $m=0;
		}
?>
<table class=responstable width="50%"> 

<form action="" method="post" onsubmit="return confirm('กรุณายืนยันทำรายการ')">
<tr><td></td><td><?=$msg?></td></tr>
<tr><th>รหัส</th><td><input type="text" name="SENDER_CODE" value="<?=$_REQUEST["SENDER_CODE"]?>"/></td></tr>
<tr><th style="width:150px;">เครือข่าย(Server)</th><td><input type="text" name="SENDER_SERVER" value="<?=$_REQUEST["SENDER_SERVER"]?>"/></td></tr>
<tr><th >เครือข่าย(Port)</th><td><input type="text" name="SENDER_PORT" value="<?=$_REQUEST["SENDER_PORT"]?>"/></td></tr>
<tr><th>ชื่อเครือข่าย</th><td><input type="text" name="SENDER_NM" value="<?=$_REQUEST["SENDER_NM"]?>"/></td></tr>
<tr><th>สถานะ</th><td>
   <select name="ENABLE_FLAG" style="width:50px;" >
            <option value="1" <?=$_REQUEST["ENABLE_FLAG"]==1?"selected":""?> >Yes </option>
            <option value="0" <?=$_REQUEST["ENABLE_FLAG"]==0?"selected":""?>>No </option>
		</select>	</td></tr>
<tr><th>จาก Email</th><td><input type="text" name="FROM_MAIL" value="<?=$_REQUEST["FROM_MAIL"]?>"/></td></tr>
<tr><th>ชื่อ Email ผู้ส่ง</th><td><input type="text" name="FROM_MAIL_NM" value="<?=$_REQUEST["FROM_MAIL_NM"]?>"/></td></tr>
<tr><th>รหัสผู้ใช้</th><td><input type="text" name="USER_NAME" value="<?=$_REQUEST["USER_NAME"]?>"/></td></tr>
<tr><th>รหัสผ่าน</th><td><input type="text" name="USER_PWD" value="<?=$_REQUEST["USER_PWD"]?>"  /></td></tr>
<tr><th>ถึง Email (Default)</th><td><input type="text" name="TO_MAIL" value="<?=$_REQUEST["TO_MAIL"]?>"/></td></tr>
<tr><th>ชื่อ Email ผู้รับ (Default)</th><td><input type="text" name="TO_MAIL_NM" value="<?=$_REQUEST["TO_MAIL_NM"]?>"/></td></tr>
<tr><th>ข้อความขึ้นต้นหัว EMail</th><td><textarea name="SUBJECT_PREFIX" cols="4" rows="5" style="width:400px;height:50px;" ><?=$_REQUEST["SUBJECT_PREFIX"]?></textarea><br/><?=$_REQUEST["SUBJECT_PREFIX"]?></td></tr>
<tr><th>ข้อความขึ้นต้นเนื้อหา EMail</th><td><textarea name="BODY_PREFIX" cols="4" rows="5" style="width:400px;height:50px;" ><?=$_REQUEST["BODY_PREFIX"]?></textarea><br/><?=$_REQUEST["BODY_PREFIX"]?></td></tr>
<tr><th>ข้อความลงท้ายเนื้อหา EMail</th><td><textarea name="BODY_SUFFIX" cols="4" rows="5" style="width:400px;height:50px;" ><?=$_REQUEST["BODY_SUFFIX"]?></textarea><br/><?=$_REQUEST["BODY_SUFFIX"]?></td></tr>
<tr><td></td><td>
<input type="button" name="cancel" value="cancel"  style="width:50px;height:25;" onclick="window.location='?SENDER_CODE=';"/>
<input type="submit" name="update" value="update" style="width:50px;height:25;"/> 
<input type="submit" name="add" value="add"  style="width:50px;height:25;"/>
<input type="submit" name="delete" value="delete"  style="width:50px;height:25;"/>

</td></tr>
</form>
</table>
<?php }else{ 


$strSQL="select * from mailconfig order by SENDER_code asc ";
$value=array('SENDER_CODE','SENDER_SERVER','SENDER_PORT','SENDER_NM','ENABLE_FLAG','FROM_MAIL','FROM_MAIL_NM','USER_NAME','USER_PWD','TO_MAIL','TO_MAIL_NM','SUBJECT_PREFIX','BODY_PREFIX','BODY_SUFFIX');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$SENDER_CODE[$n] = conv($list_info1[$n][$m++]);
			$SENDER_SERVER[$n] = $list_info1[$n][$m++];
			$SENDER_PORT[$n] = $list_info1[$n][$m++];
			$SENDER_NM[$n] = conv($list_info1[$n][$m++]);
			$ENABLE_FLAG[$n] = $list_info1[$n][$m++];
			$FROM_MAIL[$n]=$list_info1[$n][$m++];
			$FROM_MAIL_NM[$n]=conv($list_info1[$n][$m++]);
			$USER_NAME[$n] = $list_info1[$n][$m++];
			$USER_PWD[$n] = $list_info1[$n][$m++];
			$TO_MAIL[$n] = $list_info1[$n][$m++];
			$TO_MAIL_NM[$n] = conv($list_info1[$n][$m++]);
			$SUBJECT_PREFIX[$n] = conv($list_info1[$n][$m++]);
			$BODY_PREFIX[$n] = conv($list_info1[$n][$m++]);
			$BODY_SUFFIX[$n] = conv($list_info1[$n][$m++]);
		    $m=0;
		}
		?>
		
  
</head>

<body>

 
<table class=responstable width="100%">
  
  <tr>
    <th><font size='2'>เครือข่าย &nbsp;&nbsp; </font></th>
    <th data-th="Driver details"><span><font size='2'>ผู้ส่ง&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></span></th>
    <th><font size='2'>ชื่อเครือข่าย</font></th>
    <th><font size='2'>สถานะ</font></th>
	<th>-</th>
  </tr>
  <?php
//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
  <tr>
    <td><font size='2'><?=$SENDER_SERVER[$i]?>:<?=$SENDER_PORT[$i]?></font></td>
    <td><font size='2'><?=$FROM_MAIL[$i]?></font></td>
    <td><font size='2'><?=$SENDER_NM[$i]?></font></td>
	<td><font size='2'><?=$ENABLE_FLAG[$i]==1?"Y":"N"?></font></td>
	<td><font size='2'><a href="?SENDER_CODE=<?=$SENDER_CODE[$i]?>">Edit</a></font></td>
  </tr>
  <?php
	}
?>
</table>
<?php } ?>
