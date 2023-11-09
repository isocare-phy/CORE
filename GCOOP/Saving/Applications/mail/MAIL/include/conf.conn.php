<?php
//@session_start();
//header('Content-Type: text/html; charset=tis-620');

//copy this to http.conf
//SetEnv NLS_DATE_FORMAT DD-MM-YYYY
//SetEnv NLS_LANG AMERICAN_AMERICA.TH8TISASCII
//SetEnv NLS_LANG AMERICAN_AMERICA.WE8MSWIN1252	

if($connectionmode==1){
    putenv("ORACLE_SID=gcoop");
    putenv("NLS_LANG=AMERICAN_AMERICA.TH8TISASCII");  
    putenv("NLS_DATE_FORMAT=DD-MM-YYYY");    

	error_reporting(E_ALL & ~E_NOTICE & ~E_STRICT & ~E_DEPRECATED);
    
    /**** function connection to database Oracle****/	
	$IPSERVER = '127.0.0.1';
	$SERVICEDB = 'gcoop';
	$USER = 'iscobtg';
	$PASSWORD = 'iscobtg';

    $objConnect = oci_connect($USER,$PASSWORD,$IPSERVER.'/'.$SERVICEDB);                      
        
    if(!$objConnect){
        echo '<script type="text/javascript"> window.alert("ไม่สามารถเชื่อมต่อกับ ฐานข้อมูล Oracle ได้ กรุณาลองใหม่ภายหลัง") </script> ';
    }
	
}else{

    /**** function connection to database MySql****/
	$dbhost="localhost:3307";
	$dbuser = "root";
	$dbpass = "";
	$conn=mysql_connect($dbhost,$dbuser,$dbpass);
	mysql_query("SET character_set_results=tis620");
    	mysql_query("SET character_set_client=tis620");
   	mysql_query("SET character_set_connection=tis620");
   	$objDB = mysql_select_db("Webportal");
	if (!$conn) {
		echo '<script type="text/javascript"> window.alert("ไม่สามารถเชื่อมต่อกับ ฐานข้อมูล Mysql ได้ กรุณาลองใหม่ภายหลัง") </script> ';	
	}

}
    
?>
