<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');

?>

<h2>จำลองการส่ง Mail  </h2> 
<?php

if(isset($_REQUEST["mail"])){
		
	$result=sendMail(
		$_REQUEST["SENDER_SERVER"],
		$_REQUEST["SENDER_PORT"],
		$_REQUEST["USER_NAME"],
		$_REQUEST["USER_PWD"],
		$_REQUEST["FROM_MAIL"],
		$_REQUEST["FROM_MAIL_NM"],
		array($_REQUEST["TO_MAIL"]),
		array($_REQUEST["TO_MAIL_NM"]),
		$_REQUEST["SUBJECT"],
		$_REQUEST["DETAIL"],
		1,
		true,
		'tls'
	);
		
	if($result=="1"){
      $result="Message sent!";
	}	
}else{

$strSQL="select * from mailconfig where ENABLE_FLAG=1 ";
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
			$_REQUEST["SUBJECT_SUFFIX"] = conv($list_info1[$n][$m++]);
			if(isset($_REQUEST["SUBJECT"])==false)$_REQUEST["SUBJECT"]=$_REQUEST["SUBJECT_PREFIX"]  ;
			$_REQUEST["BODY_PREFIX"] = conv($list_info1[$n][$m++]);
			if(isset($_REQUEST["DETAIL"])==false)$_REQUEST["DETAIL"]=$_REQUEST["BODY_PREFIX"].'<br/>'.$_REQUEST["SUBJECT_SUFFIX"]  ;
		    $m=0;
		}
}

?>
<form action="" method="POST" >MAIL Test
<table class=responstable width="50%"> 
<tr><th style="width:150px;"><font size='2'>SENDER_SERVER</th> <td colspan="2" bgcolor="#ffffff">  <input name="SENDER_SERVER" value="<?=$_REQUEST["SENDER_SERVER"]?>" type="text"  /></td></font></tr>
<tr><th><font size='2'>SENDER_PORT</th> <td colspan="2" bgcolor="#ffffff">  <input name="SENDER_PORT" value="<?=$_REQUEST["SENDER_PORT"]?>" type="text"  /></td></font></tr>
<tr><th><font size='2'>SENDER_NM</th> <td colspan="2" bgcolor="#ffffff">  <input name="SENDER_NM" value="<?=$_REQUEST["SENDER_NM"]?>" type="text"  /></td></font></tr>
<tr><th><font size='2'>FROM_MAIL </th><td colspan="2" bgcolor="#ffffff">  <input name="FROM_MAIL" value="<?=$_REQUEST["FROM_MAIL"]?>" type="text"/></td></font></tr>
<tr><th><font size='2'>FROM_MAIL_NM</th><td colspan="2" bgcolor="#ffffff">  <input name="FROM_MAIL_NM" value="<?=$_REQUEST["FROM_MAIL_NM"]?>" type="text"/></td></font></tr>
<tr><th><font size='2'>USER_NAME </th><td colspan="2" bgcolor="#ffffff">  <input name="USER_NAME" value="<?=$_REQUEST["USER_NAME"]?>" type="text"/></td></font></tr>
<tr><th><font size='2'>USER_PWD</th><td colspan="2" bgcolor="#ffffff">  <input name="USER_PWD" value="<?=$_REQUEST["USER_PWD"]?>" type="text"/></td></font></tr>
<tr>
<th><font size='2'>DEBUG :  </th><td colspan="2" bgcolor="#ffffff">  
		<select name="DEBUG" id="DEBUG"  >
            <option value="1">Yes </option>
            <option value="0" >No </option>
		</select>	
	  </font></td></tr>	 
<tr><th><font size='2'>TO_MAIL</th><td colspan="2" bgcolor="#ffffff">  <input name="TO_MAIL" value="<?=$_REQUEST["TO_MAIL"]?>" type="text"/></td></font></tr>
<tr><th><font size='2'>TO_MAIL_NM</th><td colspan="2" bgcolor="#ffffff">  <input name="FROM_MAIL_NM" value="<?=$_REQUEST["TO_MAIL_NM"]?>" type="text"/></td></font></tr>
<tr><th><font size='2'>SUBJECT  </th><td colspan="2" bgcolor="#ffffff"><textarea name="SUBJECT"  rows="4" cols="50" style="width:300px;height:100px;" ><?=$_REQUEST["SUBJECT"]?></textarea></td></font></tr>
<tr><th><font size='2'>DETAIL  </th><td colspan="2" bgcolor="#ffffff"><textarea name="DETAIL"  rows="4" cols="50" style="width:300px;height:100px;" ><?=$_REQUEST["DETAIL"]?></textarea></td></font></tr>
<tr><td colspan="3" bgcolor="#ffffff"><input type="submit" name="mail"style="width:120px;"  value="Send Mail"/></td></tr>
<tr><th><font size='2'>Output  </th><td colspan="2" bgcolor="#ffffff"><textarea name="Output" rows="6" cols="50"  style="width:300px;height:100px;" ><?=$response?><?php echo $result; ?></textarea></td></font></tr>
</table>
</form>
<?PHP
/*
$mail = new PHPMailer();

$body = "ISCOBTG : ทดสอบการส่งอีเมล์ภาษาไทย UTF-8 ผ่าน <b>SMTP Server ด้วย PHPMailer.</b>";

$mail->CharSet = "utf-8";
$mail->IsSMTP();
$mail->SMTPDebug = 1;
$mail->SMTPAuth = true;
$mail->SMTPSecure = 'tls';
$mail->Host = "smtp.gmail.com"; // SMTP server
$mail->Port = 587; // พอร์ท
$mail->Username = "isocare.iscobtg@gmail.com"; // account SMTP
$mail->Password = "@Icoop2018"; // รหัสผ่าน SMTP

$mail->IsHTML(true);
$mail->SetFrom("isocare.iscobtg@gmail.com", "สอ.เบทาโกร");
$mail->AddReplyTo("polwat23@gmail.com", "-");
$mail->Subject = "ISCOBTG : ทดสอบ PHPMailer.";

$mail->MsgHTML($body);

$mail->AddAddress("gensoft.polwat@gmail.com", "Polwat"); // ผู้รับคนที่หนึ่ง
$mail->AddAddress("polwat23@gmail.com", "Mr.Polwat"); // ผู้รับคนที่สอง

if(!$mail->Send()) {
    echo "Mailer Error: " . $mail->ErrorInfo;
} else {
    echo "Message sent!";
}
*/
?>