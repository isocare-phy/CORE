<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');

?>
<?php 
$operate_date=isset($_REQUEST["operate_date"])?$_REQUEST["operate_date"]:date("Ymd");

$operate_date="20171017";

$strSQL="select * from smspatternconfig where use_flag=1 order by SMS_ORDER asc ";
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
<div style="font-size : 8px;" >	
<h2>สรุป สถานะการส่ง SMS : ณ <?=$operate_date?></h2> 	
<table border=1 width="100%" >
	  <tr bgcolor="#CCDD00"><td>CODE</td><td>DESC</td><td>ENABLE</td><td>SCHEDUER</td><td>REALTIME</td><td> Status</td></tr>
		<?php
	//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
	  <tr>
	  <td><div id="CODE_<?=$code[$i]?>"><?=$code[$i]?></div></td>
	  <td><div id="DESC_<?=$code[$i]?>"><?=$desc[$i]?></div></td>
	  <td><div id="ENABLE_<?=$code[$i]?>"><?=$enable_flag[$i]==1?"Y":"N"?></div></td>
	  <td><div id="SCHEDUER_<?=$code[$i]?>"><?=str_replace(",",",<br/>",$scheduler_time[$i])?></div></td>
	  <td><div id="REALTIME_<?=$code[$i]?>"><?=$realtime_flag[$i]==1?"Y":"N"?></div></td>
	  <td><iframe src="index.postsms.php?code=<?=$code[$i]?>&operate_date=<?=$operate_date?>&rownum=0&send_flag=0" name="sms<?=$code[$i]?>" frameborder="0" width="350" height="80" scrollbar="auto" ></iframe></td>
	  </tr>
	  <?php
	}
?>
</table>
</div>
<?php

 
 ?>