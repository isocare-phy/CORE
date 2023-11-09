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
<tr><td style="font-size:14px;"><b>[SMSSM01]รายงานสรุป : SMS รายเดือน </b> ณ วันที่ <?=$_REQUEST["OPERATE_DATE"]?> ถึง  <?=$_REQUEST["OPERATE_DATE1"]?> วันที่พิมพ์ <?=$_REQUEST["PRINT_DATE"]?> 
	 <?php 
	   $sql="select count(member_no) as CNT ,sum(decode(nvl(INSTR(send_status_msg,'SUCCESS', 1),0),0,0,1)) as POST_FLAG_S,sum(decode(nvl(INSTR(send_status_msg,'SUCCESS', 1),0),0,1,0))  as POST_FLAG_F, SEND_PRODATE_ ,SMS_TRANS_DESC from ( ";
	   $sql.="select s.*,to_char(s.SEND_PRODATE,'yyyy-mm') as SEND_PRODATE_, p.SMS_TRANS_CODE||'-'||p.SMS_TRANS_DESC as SMS_TRANS_DESC from  SMSTRANSACTION s , smspatternconfig p ";
	   $sql.=" where p.SMS_TRANS_CODE=s.SMS_TRANS_CODE ";
	   $sql.=" and to_char(s.SEND_PRODATE,'yyyymm')  = '".substr($_REQUEST["OPERATE_DATE"],0,6)."' ";
	   $sql.=" and s.MEMBER_NO  like  '%".$_REQUEST["MEMBER_NO"]."%' ";
	   $sql.=" and s.TELEPHONE_NUMBER  like  '%".$_REQUEST["TELEPHONE_NUMBER"]."%' ";
	   $sql.=" and s.POST_FLAG  like  '%".$_REQUEST["POST_FLAG"]."%' ";
	   $sql.=" and s.sms_trans_code  like  '%".$_REQUEST["PATTERN_CODE"]."%' ";
	   if($_REQUEST["ROWNUM"]!=""){
	      $sql.=" and rownum < = ".$_REQUEST["ROWNUM"]." ";
	   }
	   $sql.=" ) group by SEND_PRODATE_ , SMS_TRANS_DESC order by SEND_PRODATE_ asc,SMS_TRANS_DESC asc   ";	
	   if($_REQUEST["SQL_FLAG"]=="1")
	     echo "<hr/>".$sql."<hr/>";
	   $value=array('CNT','POST_FLAG_S','POST_FLAG_F','SEND_PRODATE_','SMS_TRANS_DESC');
		list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				$m=0;
				$c_success=0;
				$c_fail=0;
				$c_=0;
	 ?>
        <center>-</center>
         <table class=table11_1 width="100%">
             <tr><td align="center" colspan="12" ><hr/></td></tr>
              <tr>
                <th align="center" >
                 ลำดับ
                </th>
                <th align="center" >
                  ประเภท
                </th>
                <th align="center" >
                  ปีเดือนที่ส่ง
                </th>
                <th align="center" >
                  สำเร็จ
                </th>
                <th align="center" >
                  ไม่สำเร็จ
                </th>
                <th align="center" >
                  รวม
                </th>
              </tr>
              <tr><th align="center" colspan="12" ><hr/></th></tr><tr>
			<?php 
			for($n=0;$n<$Num_Rows1;$n++){
					$CNT[$n] = $list_info1[$n][$m++];
					$POST_FLAG_S[$n] =$list_info1[$n][$m++];
					$POST_FLAG_F[$n] =$list_info1[$n][$m++];
					$SEND_PRODATE[$n] = $list_info1[$n][$m++];
					$SMS_TRANS_DESC[$n] = conv($list_info1[$n][$m++]);
					$m=0;
					$c_+=$CNT[$n];
					$c_success+=$POST_FLAG_S[$n];
					$c_fail+=$POST_FLAG_F[$n];
			?>
              <tr>
                <td align="center" >
                  <?=$n+1?>
                </td>
                <td align="left" >
                  <?=$SMS_TRANS_DESC[$n]?>
                </td>
                <td align="center" >
                  <?=$SEND_PRODATE[$n]?>
                </td>
                <td align="center" >
                  <?=$POST_FLAG_S[$n]?>
                </td>
                <td align="left" >
                  <?=$POST_FLAG_F[$n]?>
                </td>
                <td align="center" >
                  <?=$CNT[$n]?>
                </td>
              </tr>
			  <?php
			  
				}
			  ?>
			  <tr><td align="center" colspan="12" ><hr/></td></tr>
		     <tr>
                <td align="center" colspan="12" >
                    รวมรายการ: <?=$n?>&nbsp;&nbsp;
                    ส่งทั้งหมดรายการ: <?=$c_?>&nbsp;&nbsp;
                    ส่งสำเร็จรวมรายการ: <?=$c_success?>&nbsp;&nbsp;
                    ส่งไม่สำเร็จรวมรายการ: <?=$c_fail?>&nbsp;&nbsp;
                </td>
              </tr>
		   <tr><td align="center" colspan="12" ><hr/></td></tr>
		   
            </table>
   </td></tr></table>
   <br/><br/>
 <?php //out.print(sql); ?>
 
 
 
 

