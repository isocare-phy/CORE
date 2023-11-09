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

<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');
$_REQUEST["PRINT_DATE"]=date("Y-m-d H:i:s");
?>
<table cellspacing="0" cellpadding="0" border="0" align="center" >
<tr><td style="font-size:14px;"><b>[SMSAD02]รายงาน : SMS เรียงตามสมาชิก </b> ณ วันที่ <?=$_REQUEST["OPERATE_DATE"]?> ถึง  <?=$_REQUEST["OPERATE_DATE1"]?> วันที่พิมพ์ <?=$_REQUEST["PRINT_DATE"]?> 
	 <?php 
	
	   $sql="select s.*,to_char(s.SEND_PRODATE,'yyyy-mm-dd hh24:mi:ss') as SEND_PRODATE_, p.SMS_TRANS_DESC as SMS_TRANS_DESC,INSTR(s.send_status_msg,'SUCCESS', 1) as POST_FLAG_  from  SMSTRANSACTION s , smspatternconfig p ";
	   $sql.=" where p.SMS_TRANS_CODE=s.SMS_TRANS_CODE ";
	   $sql.=" and ( trunc(s.SEND_PRODATE)  between to_date('".$_REQUEST["OPERATE_DATE"]."','yyyymmdd')  and to_date('".$_REQUEST["OPERATE_DATE1"]."','yyyymmdd') ";
	   $sql.=" or trunc(s.SEND_DATE)  between to_date('".$_REQUEST["OPERATE_DATE"]."','yyyymmdd')  and to_date('".$_REQUEST["OPERATE_DATE1"]."','yyyymmdd') ) ";
	   $sql.=" and s.MEMBER_NO  like  '%".$_REQUEST["MEMBER_NO"]."%' ";
	   $sql.=" and s.TELEPHONE_NUMBER  like  '%".$_REQUEST["TELEPHONE_NUMBER"]."%' ";
	   $sql.=" and s.POST_FLAG  like  '%".$_REQUEST["POST_FLAG"]."%' ";
	   $sql.=" and s.sms_trans_code  like  '%".$_REQUEST["PATTERN_CODE"]."%' ";
	   if($_REQUEST["ROWNUM"]!=""){
	      $sql.=" and rownum < = ".$_REQUEST["ROWNUM"]." ";
	   }
	   $sql.=" order by s.MEMBER_NO asc,s.SEND_PRODATE asc,s.SMS_TRANS_CODE asc ";	
	   if($_REQUEST["SQL_FLAG"]=="1")
	     echo "<hr/>".$sql."<hr/>";
	   $value=array('REF_NO','PK_ID','MEMBER_NO','TELEPHONE_NUMBER','MESSAGE_TEXT','CREATE_DATE','SEND_DATE','SMS_TRANS_CODE','POST_FLAG','SEND_STATUS_MSG','SEND_PRODATE_','SMS_TRANS_DESC');
		list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				$m=0;
				$c_success=0;
				$c_fail=0;
	 ?>
        <center>-</center>
         <table class=table11_1 width="100%">
             <tr><td align="center" colspan="12" ><hr/></td></tr>
              <tr>
                <th align="center" >
                 ลำดับ
                </th>
                <th align="center" >
                  เลขทะเบียน
                </th>
                <th align="center" >
                  เบอร์โทรศัพท์
                </th>
                <th align="center" >
                  ข้อความ
                </th>
                <th align="center" >
                  วันที่สร้าง
                </th>
                <th align="center" >
                  กำหนดส่ง
                </th>
                <th align="center" >
                  ประเภท
                </th>
                <th align="center" >
                  สถานะ
                </th>
                <th align="center" >
                  รายละเอียด
                </th>
                <th align="center" >
                  วันที่ส่ง
                </th>
              </tr>
              <tr><th align="center" colspan="12" ><hr/></th></tr><tr>
			<?php 
			for($n=0;$n<$Num_Rows1;$n++){
					$REF_NO[$n] = $list_info1[$n][$m++];
					$PK_ID[$n] = conv($list_info1[$n][$m++]);
					$MEMBER_NO[$n] = conv($list_info1[$n][$m++]);
					$TELEPHONE_NUMBER[$n] = $list_info1[$n][$m++];
					$MESSAGE_TEXT[$n] = conv($list_info1[$n][$m++]);
					$CREATE_DATE[$n] = $list_info1[$n][$m++];
					$SEND_DATE[$n] = $list_info1[$n][$m++];
					$SMS_TRANS_CODE[$n] = $list_info1[$n][$m++];
					$POST_FLAG[$n]=$list_info1[$n][$m++];
					$SEND_STATUS_MSG[$n] = $list_info1[$n][$m++];
					$SEND_PRODATE[$n] = $list_info1[$n][$m++];
					$SMS_TRANS_DESC[$n] = conv($list_info1[$n][$m++]);
					$m=0;

			?>
              <tr>
                <td align="center" >
                  <?=$n+1?>
                </td>
                <td align="center" >
                  <?=$MEMBER_NO[$n]?>
                </td>
                <td align="center" >
                  <?=$TELEPHONE_NUMBER[$n]?>
                </td>
                <td align="left" >
                  <?=$MESSAGE_TEXT[$n]?>
                </td>
                <td align="center" >
                  <?=$CREATE_DATE[$n]?>
                </td>
                <td align="center" >
                  <?=$SEND_DATE[$n]?>
                </td>
                <td align="left" >
                  <?=$SMS_TRANS_CODE[$n]?>-<?=$SMS_TRANS_DESC[$n]?>
                </td>
                <td align="center" >
				  <?php 
				  if($POST_FLAG[$n]==1){
				    $c_success++;
				  }else{
				    $c_fail++;
				  }?>
                  <?=$POST_FLAG[$n]==1?("สำเร็จ(".$POST_FLAG[$n].")"):($POST_FLAG[$n]==-9?"ไม่สำเร็จ":"รอ")?>
                </td>
                <td align="center" >
                  <?=$SEND_STATUS_MSG[$n]?>
                </td>
                <td align="center" >
                  <?=$SEND_PRODATE[$n]?>
                </td>
              </tr>
			  <?php
			  
				}
			  ?>
			  <tr><td align="center" colspan="12" ><hr/></td></tr>
		     <tr>
                <td align="center" colspan="12" >
                    รวมรายการ: <?=$n?>&nbsp;&nbsp;
                    ส่งสำเร็จรวมรายการ: <?=$c_success?>&nbsp;&nbsp;
                    ส่งไม่สำเร็จรวมรายการ: <?=$c_fail?>&nbsp;&nbsp;
                </td>
              </tr>
		   <tr><td align="center" colspan="12" ><hr/></td></tr>
		   
            </table>
   </td></tr></table>
   <br/><br/>
 <?php //out.print(sql); ?>
 
 
 
 

