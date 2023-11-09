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
//header('Content-type: text/html; charset=UTF-8');

include("convert.php");

if(isset($_REQUEST["sms"])){

include("include/conf.c.php");
//include("include/jquery.popup.php");
include("include/conf.conn.php");
//include("include/lib.Oracle.php");
//include("include/lib.MySql.php");
//include("include/lib.Etc.php");

$url = 'http://110.49.202.31:4652';
$FROM="66613843408";
$CHARGE="66613843408";
$TO="66875514386";
$CODE="45140268003";
$CONTENT=$_REQUEST["CONTENT"];
//$CONTENT = mb_convert_encoding($CONTENT, 'TIS-620' , 'UTF-16BE');
//$CONTENT=$convert->str_to_unicode($CONTENT, 'UTF-8');
//echo $CONTENT;
//$CONTENT=mb_convert_encoding($CONTENT , 'UTF-8' , 'UTF-16BE');
//$CONTENT=urlencode($CONTENT);
//echo $CONTENT;
$CONTENT= unicodeMessageEncode($CONTENT);
$myvars ="CMD=SENDMSG"."&".
				"FROM=".$_REQUEST["FROM"]."&".
				"TO=".$_REQUEST["TO"]."&".
				"CODE=".$_REQUEST["CODE"]."&".
				"REPORT=Y"."&".
				"CHARGE=".$_REQUEST["CHARGE"]."&".
				"CTYPE=LUNICODE"."&".
				"CONTENT=".$CONTENT."&";
$ch = curl_init( $url );
curl_setopt( $ch, CURLOPT_POST, 1);
curl_setopt( $ch, CURLOPT_POSTFIELDS, $myvars);
curl_setopt( $ch, CURLOPT_FOLLOWLOCATION, 1);
curl_setopt( $ch, CURLOPT_HEADER, 0);
curl_setopt( $ch, CURLOPT_RETURNTRANSFER, 1);
curl_setopt( $ch, CURLOPT_HTTPHEADER,array('Content-Type: text/plain; charset=UTF-8'));
$response = curl_exec( $ch );
var_dump($response);
}

			//$sender_number[$n] = $list_info1[$n][$m++];
			//$sender_code[$n] = $list_info1[$n][$m++];
			//$phonenetwork[$n] = $list_info1[$n][$m++];
			//$enable_flag[$n] = $list_info1[$n][$m++];
			//$user_name[$n] = $list_info1[$n][$m++];
			//$user_pwd[$n] = $list_info1[$n][$m++];
			//$url[$n] = $list_info1[$n][$m++];
			//$url_params[$n] = $list_info1[$n][$m++];
			
?>
<form action="" method="POST" >AIS SMS
<table class=responstable width="50%"> 
<colgroup span="6" width="100"> </colgroup>
<tr><td><font size='2'>CMD</td> <td colspan="2" bgcolor="#ffffff">  <input name="CMD" value="SENDMSG" type="text" readonly /></td></font></tr>
<tr><td><font size='2'>FROM</td><td colspan="2" bgcolor="#ffffff">  <input name="FROM" value="<?=$FROM_PHONENO[0]?>" type="text"/></td></font></tr>
<tr><td><font size='2'>TO  </td><td colspan="2" bgcolor="#ffffff"><input name="TO" value="<?=$TO_PHONENO[0]?>" type="text"/></td></font></tr>
<tr><td><font size='2'>CODE </td><td colspan="2" bgcolor="#ffffff"><input name="CODE" value="<?=$SENDER_CODE[0]?>" type="text"/></td></font></tr>


<td><font size='2'>Report :  </td><td colspan="2" bgcolor="#ffffff">  
		<select name="REPORT" id="REPORT"  >
            <option value="Y">Yes </option>
            <option value="N" >No </option>
		</select>	
	  </font></td>	 

<!--<tr><td>REPORT  </td><td><input name="REPORT" value="Y" type="text"/></td></tr>-->




<tr><td><font size='2'>CHARGE  </td><td colspan="2" bgcolor="#ffffff"><input name="CHARGE" value="<?=$FROM_PHONENO[0]?>" type="text"/></td></font></tr>
<tr><td><font size='2'>CTYPE  </td><td colspan="2" bgcolor="#ffffff"><input name="CTYPE" value="LUNICODE" type="text"/></td></font></tr>
<tr><td><font size='2'>CONTENT  </td><td colspan="2" bgcolor="#ffffff"><textarea name="CONTENT"  rows="4" cols="50" style="width:300px;height:100px;" >ทดสอบส่ง SMS </textarea></td></font></tr>
<tr><td colspan="3" bgcolor="#ffffff"><input type="submit" name="sms"style="width:120px;"  value="Send SMS"/></td></tr>
<tr><td><font size='2'>Output  </td ><td colspan="2" bgcolor="#ffffff"><textarea name="Output" rows="6" cols="50"  style="width:300px;height:100px;" ><?=$response?><?php var_dump($response); ?></textarea></td></font></tr>
</table>
</form>
<?php
/*
$data = array(
				'CMD' => 'SENDMSG', 
				'FROM' => $FROM, 
				'TO' => $TO, 
				'CODE' => $CODE, 
				'REPORT' => 'Y', 
				'CHARGE' => $CHARGE, 
				'CTYPE' => 'LUNICODE', 
				'CONTENT' => $CONTENT
				);
// use key 'http' even if you send the request to https://...
$options = array(
    'http' => array(
        'header'  => "Content-type: application/x-www-form-urlencoded\r\n",
        'method'  => 'POST',
        'content' => http_build_query($data)
    )
);

$context  = stream_context_create($options);
$result = file_get_contents($url, false, $context);
if ($result === FALSE) { 
 
}

var_dump($result);
*/
?>