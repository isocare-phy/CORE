<?php
session_start();
//header('Content-Type: text/html; charset=tis-620');
header('Content-type: text/html; charset=UTF-8');
include("include/conf.c.php");
//include("include/jquery.popup.php");
include("include/conf.conn.php");
include("include/lib.Oracle.php");
include("include/lib.MySql.php");
include("include/lib.Etc.php");
include("convert.php");
?>
<style>
 body,div,td,tr,h2 {
	 font-size : 10px;
 }
</style>	
<?php
$send_flag=0;

if(isset($_REQUEST["send_flag"])){
$send_flag=$_REQUEST["send_flag"];
}

if(isset($_REQUEST["o"])){
$_SESSION["o"]=$_REQUEST["o"];
}

if(isset($_REQUEST["p"])){
$_SESSION["p"]=$_REQUEST["p"];
}
$connectby = "desktop";

$operate_date=isset($_REQUEST["operate_date"])?$_REQUEST["operate_date"]:date("Ymd");
$rownum=isset($_REQUEST["rownum"])&&$_REQUEST["rownum"]!="0"?(" and rownum < ".$_REQUEST["rownum"].""):"";
$sms_trans_code=$_REQUEST["code"];

$strSQL="select * from smsconfig where enable_flag=1 ";
$value=array('SENDER_NUMBER','SENDER_CODE','PHONENETWORK','USEPHONEBOOK_FLAG','ENABLE_FLAG','USER_NAME','USER_PWD','FROM_PHONENO','TO_PHONENO','URL','URL_PARAMS');
list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$_REQUEST["SENDER_NUMBER"] = $list_info1[$n][$m++];
			$_REQUEST["SENDER_CODE"] = conv($list_info1[$n][$m++]);
			$_REQUEST["PHONENETWORK"]= conv($list_info1[$n][$m++]);
			$_REQUEST["USEPHONEBOOK_FLAG"]= conv($list_info1[$n][$m++]);
			$_REQUEST["ENABLE_FLAG"] = $list_info1[$n][$m++];
			$_REQUEST["USER_NAME"] = $list_info1[$n][$m++];
			$_REQUEST["USER_PWD"] = $list_info1[$n][$m++];
			$_REQUEST["FROM_PHONENO"]= $list_info1[$n][$m++];
			$_REQUEST["TO_PHONENO"] = $list_info1[$n][$m++];
			$_REQUEST["URL"] = $list_info1[$n][$m++];
			$_REQUEST["URL_PARAMS"] = $list_info1[$n][$m++];
		    $m=0;
		}
//	echo 	$_REQUEST["FROM_PHONENO"];
$strSQL="select * from smspatternconfig where sms_trans_code ='".$sms_trans_code."' and use_flag=1 ";
$value=array('SMS_TRANS_CODE','SMS_TRANS_DESC','SMS_PATTERN','ENABLE_FLAG','FROM_SYSTEM','SMS_TRANS_SQL','SMS_VIEWS_SQL','SCHEDULER_TIME','REALTIME_FLAG');
	list($Num_Rows1,$list_info1) = get_value_many_oci($strSQL,$value);
		$m=0;
		for($n=0;$n<$Num_Rows1;$n++){
			$SMS_TRANS_CODE= $list_info1[$n][$m++];
			$SMS_TRANS_DESC = conv($list_info1[$n][$m++]);
			$SMS_PATTERN = conv($list_info1[$n][$m++]);
			$ENABLE_FLAG = $list_info1[$n][$m++];
			$FROM_SYSTEM = $list_info1[$n][$m++];
			$SMS_TRANS_SQL = $list_info1[$n][$m++];
			$SMS_VIEWS_SQL= $list_info1[$n][$m++];
			$SCHEDULER_TIME = $list_info1[$n][$m++];
			$REALTIME_FLAG = $list_info1[$n][$m++];
		    $m=0;
		}
	$CURRENT_DATE=date("Y-m-d H:i:s");
	$CURRENT_TIME=date("H:i");
	//echo $SCHEDULER_TIME.",".$CURRENT_TIME."='".(preg_match(('/'.$CURRENT_TIME.'/'),$SCHEDULER_TIME)?"1":"0")."'<br/>";

	$PROCESS_FLAG=($REALTIME_FLAG ==1)||(preg_match(('/'.$CURRENT_TIME.'/'),$SCHEDULER_TIME));
	?>
	<script>
	function setParentValueByID(id,v)
	{
	   window.parent.document.getElementById(id).innerHTML=v;
	}	
	 //alert(window.parent.document.getElementById("SCHEDUER_<?=$SMS_TRANS_CODE?>"));
	 setParentValueByID("DESC_<?=$SMS_TRANS_CODE?>","<?=$SMS_TRANS_DESC?>");
	 setParentValueByID("ENABLE_<?=$SMS_TRANS_CODE?>","<?=$ENABLE_FLAG==1?"Y":"N"?>");
	 setParentValueByID("SCHEDUER_<?=$SMS_TRANS_CODE?>","<?=$SCHEDULER_TIME?>");
	 setParentValueByID("REALTIME_<?=$SMS_TRANS_CODE?>","<?=$REALTIME_FLAG==1?"Y":"N"?>");
	</script>
	<?php
if($ENABLE_FLAG ==1){
	
		
		echo "Update Time :".$CURRENT_DATE;
				
	   if($send_flag==1){
			   $sql="select  ".$SMS_PATTERN." as sms_text,s.* from (
								select * from ".str_replace("?",$operate_date,$SMS_TRANS_SQL)." 
								".$rownum."
								) s where s.pk_id not in (select t.pk_id from SMSTRANSACTION t where t.SMS_TRANS_CODE='".$SMS_TRANS_CODE."' ) ";
			   //echo "<hr/>".$sql."<hr/>";
			   $value=array('SMS_TEXT','MEMBER_NO','PHONE_NUMBER','REF_NO','PK_ID');
				list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				//echo " Waiting ".$Num_Rows1." sms for sending "."<br/>";
				$PK_ID_="";
				for($n=0;$n<$Num_Rows1;$n++){
					$SMS_TEXT= $list_info1[$n][$m++];
					$MEMBER_NO = $list_info1[$n][$m++];
					$PHONE_NUMBER = $list_info1[$n][$m++];
					$REF_NO = $list_info1[$n][$m++];
					$PK_ID = $list_info1[$n][$m++];
					
					
					if($PK_ID_!=$PK_ID&&$SMS_TRANS_CODE=="A01" ){						
						$sql="update smsnewsmessage  set send_flag=1 where MSG_ID='".$PK_ID."' ";
						get_value_many_oci($sql,$value);			
					  $PK_ID_=$PK_ID;
					}
					
					$POST_FLAG=0;			
					$MESSAGE_STATUS=1;
						
						if($_REQUEST["USEPHONEBOOK_FLAG"]==1){	
							$sql="select * from SMSPHONEBOOK where member_no='".$MEMBER_NO."' "	; 
						    $value2=array('TELEPHONE_NUMBER','ENABLE_FLAG','USEPHONEBOOK_FLAG');
							list($Num_Rows2,$list_info2) = get_value_many_oci($sql,$value2);
							$USEPHONEBOOK_FLAG=$list_info2[0][2];
							if($USEPHONEBOOK_FLAG==1){	
								$PHONE_NUMBER=$list_info2[0][0];
								$POST_FLAG=$list_info2[0][1]==1?0:-9;
								$MESSAGE_STATUS=$list_info2[0][1]==1?1:-9;
							}
						}
						
						$sql="insert into SMSTRANSACTION (REF_NO,PK_ID,MEMBER_NO,TELEPHONE_NUMBER,MESSAGE_TEXT,CREATE_DATE ,SEND_DATE,MESSAGE_STATUS ,FROM_SYSTEM,SMS_TRANS_CODE,POST_FLAG) values(
						 '".$REF_NO."',
						 '".$PK_ID."',
						 '".convSQL($MEMBER_NO)."',
						 '".$PHONE_NUMBER."',
						 '".((convSQL($SMS_TEXT)==""||convSQL($SMS_TEXT)==null)?$SMS_TEXT:convSQL($SMS_TEXT))."',
						 sysdate ,
						 to_date('".$operate_date."','yyyymmdd'),
						 ".$MESSAGE_STATUS." ,
						 '".$FROM_SYSTEM."', 
						 '".$SMS_TRANS_CODE."',
						 ".$POST_FLAG." 
						)";
						//if($n==0)echo $sql.";<br/>";
						get_single_value_oci($sql,$value);
						
						
					$m=0;
				}
				
			}
			
			   $sql="select count(*) as CNT , sum(decode(nvl(INSTR(send_status_msg,'SUCCESS', 1),0),0,0,1)) as CNT_S  , sum(decode(nvl(INSTR(send_status_msg,'SUCCESS', 1),0),0,1,0)) as CNT_F from SMSTRANSACTION where SMS_TRANS_CODE='".$SMS_TRANS_CODE."' and post_flag<>0 and MESSAGE_STATUS=1 and to_char(send_date,'yyyymmdd')='".$operate_date."' ";
			  // echo "<hr/>".$sql."<hr/>";
				$value=array('CNT','CNT_S','CNT_F');
				list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				echo "<br/> ALL:". $list_info1[0][0].",S=". $list_info1[0][1].",F=". $list_info1[0][2]."";
				
			   $sql="select count(*) as CNT_W from SMSTRANSACTION where SMS_TRANS_CODE='".$SMS_TRANS_CODE."' and post_flag=0 and MESSAGE_STATUS=1  and to_char(send_date,'yyyymmdd')='".$operate_date."' ";
			  // echo "<hr/>".$sql."<hr/>";
				$value=array('CNT_W');
				list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				//echo " Waiting ".$Num_Rows1." sms for sending "."<br/>";
				echo "<br/> Waiting ".$list_info1[0][0]." sms for sending ";
				
				if($list_info1[0][0]==0&&$SMS_TRANS_CODE=='A01' ){
				$sql="update smsnewsmessage  set send_flag=1 where to_char(operate_date,'yyyymmdd')='".$operate_date."' ";
				//echo $sql;
				get_single_value_oci($sql,$value);	
				get_single_value_oci("commit",$value);			
				}
			
			   $sql="select * from SMSTRANSACTION where SMS_TRANS_CODE='".$SMS_TRANS_CODE."' and post_flag=0 and MESSAGE_STATUS=1  and to_char(send_date,'yyyymmdd')='".$operate_date."' and rownum<=$sms_send_limit";
			  // echo "<hr/>".$sql."<hr/>";
				$value=array('REF_NO','PK_ID','MEMBER_NO','TELEPHONE_NUMBER','MESSAGE_TEXT');
				list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
			
			if($send_flag==1){	
				
				$m=0;
				for($n=0;$n<$Num_Rows1;$n++){
					$REF_NO = $list_info1[$n][$m++];
					$PK_ID = $list_info1[$n][$m++];
					$MEMBER_NO = $list_info1[$n][$m++];
					$PHONE_NUMBER = $list_info1[$n][$m++];
					$SMS_TEXT=  conv($list_info1[$n][$m++]);
					//echo $n.".".$SMS_TEXT."<hr/>";
				 	
						if($PROCESS_FLAG){
						
						 if(strlen(trim($PHONE_NUMBER))==10&&$sms_send_enable_flag){
						   if($_REQUEST["PHONENETWORK"]=="AIS"){
								
								$response=send_sms_ais(
								$_REQUEST["URL"],
								$SMS_TEXT,
								$_REQUEST["FROM_PHONENO"],
								$PHONE_NUMBER,
								$_REQUEST["SENDER_CODE"],
								$_REQUEST["FROM_PHONENO"]
								);
								
									$sql="update SMSTRANSACTION set post_flag=".((strpos($response, "SUCCESS")=== false)?"-9":((strpos($response, ",")=== false)?"1":"2"))." ,SEND_STATUS_MSG='".$response."' ,send_prodate=sysdate 
										 where SMS_TRANS_CODE='".$SMS_TRANS_CODE."' and pk_id='".$PK_ID."' and ref_no='".$REF_NO."' ";
									get_single_value_oci($sql,$value);
							
							}	
						  }else{
								$sql="update SMSTRANSACTION set post_flag=-9 ,SEND_STATUS_MSG='Error' ,send_prodate=sysdate 
								     where SMS_TRANS_CODE='".$SMS_TRANS_CODE."' and pk_id='".$PK_ID."' and ref_no='".$REF_NO."' ";
								get_single_value_oci($sql,$value);
						  }			
						}
						
						$m=0;
					}
					
				}
				
				/*
						$m=0;
						for($n=0;$n<$Num_Rows1;$n++){
							$sms_text[$n] = $list_info1[$n][$m++];
							$member_no[$n] = $list_info1[$n][$m++];
							$phone_number[$n] = $list_info1[$n][$m++];
							$m=0;
						}
				*/
}else{
	echo "N";
}
?>
<form action="index.postsms.php" id="processSMS" method="get" >
  <input type="hidden" name="send_flag" value="<?=$send_flag?>"/>
  <input type="hidden" name="operate_date" value="<?=$operate_date?>"/>
  <input type="hidden" name="code" value="<?=$SMS_TRANS_CODE?>"/>
  <input type="hidden" name="rownum" value="<?=$_REQUEST["rownum"]?>"/>
</form>
<script>

  function submitForm(){
     document.getElementById("processSMS").submit();
  }
  
  setTimeout("submitForm()",1000*50);//Refresh 60 secs

</script>