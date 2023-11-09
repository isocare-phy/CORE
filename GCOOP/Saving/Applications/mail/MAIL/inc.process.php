<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');

?><?php 
//$operate_date=isset($_REQUEST["operate_date"])?$_REQUEST["operate_date"]:date("Ymd");
$operate_date=date("Ymd");

$operate_date_fix=false;
//$operate_date="20180308";

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
<table class=responstable width="100%">
  <tr>
    <th style="width:50px;"><font size='2'>รหัส</font></th>
    <th style="width:150px;"><span><font size='2'>ข้อความ&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</font></span></th>
    <th style="width:50px;"><font size='2'>ใช้งาน</font></th>
    <th style="width:150px;"><font size='2'>ส่งตามเวลา</font></th>
    <th style="width:50px;"><font size='2'>ส่งทันที</font></th>
	<th style="width:350px;"><font size='2'>ประมวลส่ง SMS : ณ <?=$operate_date?></font></th>
  </tr><?php
	//echo $Num_Rows1;
	for($i=0;$i<$Num_Rows1;$i++){
	  ?>
	  <tr>
	  <td><font size='2'><div id="CODE_<?=$code[$i]?>"><?=$code[$i]?></div></font></td>
	  <td><font size='2'><div id="DESC_<?=$code[$i]?>"><?=$desc[$i]?></div></font></td>
	  <td><font size='2'><div id="ENABLE_<?=$code[$i]?>"><?=$enable_flag[$i]==1?"Y":"N"?></div></font></td>
	  <td><font size='2'><div id="SCHEDUER_<?=$code[$i]?>"><?=str_replace(",",",<br/>",$scheduler_time[$i])?></div></font></td>
	  <td><font size='2'><div id="REALTIME_<?=$code[$i]?>"><?=$realtime_flag[$i]==1?"Y":"N"?></div></font></td>
	  <td><iframe src="index.postsms.php?code=<?=$code[$i]?>&operate_date=<?=$operate_date?>&rownum=0&send_flag=1" name="sms<?=$code[$i]?>" frameborder="0" width="350" height="35" scrollbar="auto" ></iframe></td>
	  </tr>
	  <?php
	}
?>
</table><style> div,td,tr,h2 {  font-size : 10px; }</style>	
<head><link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css"><link rel="stylesheet" href="css/style.css"></head>
<?php

 if($operate_date_fix==false){
 ?>
 <input type="hidden" id="opetate_date_fix" name="opetate_date_fix" value="<?=$operate_date?>"/>
 <script>
	Date.prototype.yyyymmdd = function() {
	  var mm = this.getMonth() + 1; // getMonth() is zero-based
	  var dd = this.getDate();

	  return [this.getFullYear(),
			  (mm>9 ? '' : '0') + mm,
			  (dd>9 ? '' : '0') + dd
			 ].join('');
	};

	var date = new Date();

  function submitForm(){
   //alert(date.yyyymmdd());
    if(document.getElementById("opetate_date_fix").value!=date.yyyymmdd()){
      window.location.reload();
	 }
  }
  
  setTimeout("submitForm()",1000*50);//Refresh 60 secs

</script>

<?php } ?>