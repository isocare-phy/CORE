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

if(isset($_REQUEST["o"])){
$_SESSION["o"]=$_REQUEST["o"];
}

if(isset($_REQUEST["p"])){
$_SESSION["p"]=$_REQUEST["p"];
}

$connectby = "desktop";
?>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title><?=$title?></title>
	<link href="css/jquery-ui-1.10.4.css" rel="stylesheet">
	<link rel="shortcut icon" href="../img/logo.png">
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Open+Sans:300,400,700">
    <script src="js/jquery.js"></script>
	<script src="js/index.js"></script>
	<link rel="stylesheet" href="css/normalize.min.css">
	<link rel="stylesheet" href="css/style.css">
</head>
<body>
<table width="100%">
  <tr><td valign="top" >
	<?php
  if($_REQUEST["r"]==""){
  	if($_SESSION["p"]!="process"){ ?>
		<table width="100%">
			<tr>
			<td><a href="index.php?p=setup" style="<?=$_SESSION["p"]=="setup"?"font-weight: bold;":""?>">ค่าระบบ</a>|</td>
			<td><a href="index.php?p=test"  style="<?=$_SESSION["p"]=="test"?"font-weight: bold;":""?>">จำลองส่ง Mail</a>|</td>
			<td><a href="index.php?p=pattern"  style="<?=$_SESSION["p"]=="pattern"?"font-weight: bold;":""?>">รูปแบบข้อความ</a>|</td>
			<td><a href="index.php?p=news"  style="<?=$_SESSION["p"]=="news"?"font-weight: bold;":""?>">ข่าวสาร</a>|</td>
			<td><a href="index.php?p=transaction"  style="<?=$_SESSION["p"]=="transaction"?"font-weight: bold;":""?>">จัดการรายการ</a>|</td>
			<td><a href="index.php?p=report"  style="<?=$_SESSION["p"]=="report"?"font-weight: bold;":""?>">รายงาน</a>|</td>
			<td><a href="#" onclick="window.open('../Mail/process/?o=mail','p','location=no,width=800,height=600,scrollbars=yes,resizable =yes')">ประมวลส่ง Mail</a></td>
			</tr>
		</table>
     <?php }else { ?>
	  <font color=red>*** เปิดหน้าจอนี้ค้างไว้ และ ควรเปิด เพียงหน้าต่างเดียว *** </font>
	 <?php } 
	 } ?>
  </td></tr>
  <tr><td  valign="top">
		<?php

			if($_REQUEST["r"]!=""){

				if(file_exists("inc.".$_SESSION["p"].".".$_REQUEST["r"].".php"))
					include("inc.".$_SESSION["p"].".".$_REQUEST["r"].".php");
					
			//	echo "inc.".$_SESSION["p"].".".$_REQUEST["r"].".php";
			
			}else{	
			
			//echo "inc.".$_SESSION["p"].".php";
				if(file_exists("inc.".$_SESSION["p"].".php"))
					include("inc.".$_SESSION["p"].".php");
					
			}
								
		//echo phpinfo();
		?>
  </td></tr>
</table>
</body>
</html>