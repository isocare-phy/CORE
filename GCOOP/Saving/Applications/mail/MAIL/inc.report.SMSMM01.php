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
<tr><td style="font-size:14px;"><b>[SMSMM01]รายงาน : ทะเบียนสมาชิก SMS </b> ณ วันที่ <?=$_REQUEST["OPERATE_DATE"]?> ถึง  <?=$_REQUEST["OPERATE_DATE1"]?> วันที่พิมพ์ <?=$_REQUEST["PRINT_DATE"]?> 
	 <?php 
	   $sql="select s.*,( select nvl(replace(NVL(m.addr_mobilephone,m.addr_phone),'-'),'-')  as TELEPHONE_NUMBER from mbmembmaster m where m.member_no =s.MEMBER_NO ) as TELEPHONE_NUMBER_ ";
	   $sql.=" from  SMSPHONEBOOK s  ";
	   $sql.=" where s.MEMBER_NO  like  '%".$_REQUEST["MEMBER_NO"]."%' ";
	   $sql.=" and s.TELEPHONE_NUMBER  like  '%".$_REQUEST["TELEPHONE_NUMBER"]."%' ";
	   if($_REQUEST["ROWNUM"]!=""){
	      $sql.=" and rownum < = ".$_REQUEST["ROWNUM"]." ";
	   }
	   $sql.=" order by s.MEMBER_NO asc ";	
	   if($_REQUEST["SQL_FLAG"]=="1")
	     echo "<hr/>".$sql."<hr/>";
	   
	$value=array('MEMBER_NO','MEMBER_FULLNAME','TELEPHONE_NUMBER','TELEPHONE_NUMBER_','USEPHONEBOOK_FLAG','ENABLE_FLAG','UPDATE_DATE');
	list($Num_Rows1,$list_info1) = get_value_many_oci($sql,$value);
				$m=0;
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
                  ชื่อ-สกุล
                </th>
                <th align="center" >
                  เบอร์โทรศัพท์<br/>(ตามทะเบียน SMS)
                </th>
                <th align="center" >
                  เบอร์โทรศัพท์ <br/>(ตามทะเบียนสหกรณ์)
                </th>
                <th align="center" >
                  สถานะใช้เบอร์<br/>(ตามทะเบียน SMS)
                </th>
                <th align="center" >
                  สถานะใช้งาน <br/>(ทะเบียน SMS)
                </th>
                <th align="center" >
                  วันที่ปรับปรุง
                </th>
              </tr>
              <tr><th align="center" colspan="12" ><hr/></th></tr><tr>
			<?php 
			for($n=0;$n<$Num_Rows1;$n++){
				$MEMBER_NO[$n] = $list_info1[$n][$m++];
				$MEMBER_FULLNAME[$n] = conv($list_info1[$n][$m++]);
				$TELEPHONE_NUMBER[$n] = conv($list_info1[$n][$m++]);
				$TELEPHONE_NUMBER_[$n] = conv($list_info1[$n][$m++]);
				$USEPHONEBOOK_FLAG[$n]=$list_info1[$n][$m++];
				$ENABLE_FLAG[$n] = $list_info1[$n][$m++];
				$UPDATE_DATE[$n] = $list_info1[$n][$m++];
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
                  <?=$MEMBER_FULLNAME[$n]?>
                </td>
                <td align="center" >
                  <?=$TELEPHONE_NUMBER[$n]?>
                </td>
                <td align="center" >
                  <?=$TELEPHONE_NUMBER_[$n]?>
                </td>
                <td align="center" >
                  <?=($USEPHONEBOOK_FLAG[$n]==1?"Y":"N")?>
                </td>
                <td align="center" >
                  <?=($ENABLE_FLAG[$n]==1?"Y":"N")?>
                </td>
                <td align="center" >
                  <?=$UPDATE_DATE[$n]?>
                </td>
              </tr>
			  <?php
			  
				}
			  ?>
			  <tr><td align="center" colspan="12" ><hr/></td></tr>
		     <tr>
                <td align="center" colspan="12" >
                    รวมรายการ: <?=$n?>&nbsp;&nbsp;
                </td>
              </tr>
		   <tr><td align="center" colspan="12" ><hr/></td></tr>
		   
            </table>
   </td></tr></table>
   <br/><br/>
 <?php //out.print(sql); ?>
 
 
 
 

