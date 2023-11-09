<?php

session_start();
@header('Content-Type: text/html; charset=utf-8');

$ip=$_REQUEST["ip"];
$sid=$_REQUEST["sid"];
$u=$_REQUEST["u"];
$p=$_REQUEST["pwd"];
$coop_id=$_REQUEST["coop_id"];

$f=$_REQUEST["f"];
$app=$_REQUEST["a"];

//http://localhost/CORE/GCOOP/WcfService125/n_deposit.svc?singleWsdl
$svc = $app;
$port = '80';
$func = $f;
$url = 'http://127.0.0.1:'.$port.'/CORE/GCOOP/WcfService125/'.$svc.".svc";

$client = @new SoapClient($url."?singleWsdl");

if($app=="n_deposit"){

//http://localhost/CORE/GCOOP/Saving/Applications/mobile/svc.php?ip=127.0.0.1&sid=gcoop&u=iscodoa&pwd=iscodoa&coop_id=032001&f=of_withdraw_deposit_trans&a=n_deposit&p[]=0104016155&p[]=0101005551&p[]=2018-10-24T13:23:00&p[]=WTX&p[]=DTX&p[]=100.00&p[]=0.00
	if($f=="of_withdraw_deposit_trans"){
		try {	
				//กรณี เป็น Date ต้องเป็น Format : date("Y-m-d")."T".date("H:i:s"),		
				$parameters = array(
					'as_wspass' => "Data Source=$ip/$sid;Persist Security Info=True;User ID=$u;Password=$p;Unicode=True;coop_id=$coop_id;coop_control=$coop_id;",
					'as_src_deptaccount_no' => $_REQUEST["p"][0],
					'as_dest_deptaccount_no' => $_REQUEST["p"][1],
					'adtm_operate' => $_REQUEST["p"][2],
					'as_wslipitem_code'=>$_REQUEST["p"][3],
					'as_dslipitem_code'=>$_REQUEST["p"][4],
					'adc_amt'=>$_REQUEST["p"][5],
					'adc_fee' =>$_REQUEST["p"][6]
				);
				
			   $ret = $client->of_withdraw_deposit_trans($parameters);
			   print_r($ret);
			   //echo $ret;
		}catch(Exception $e){
			   echo $e->getMessage();
		}
	}
//http://localhost/CORE/GCOOP/Saving/Applications/mobile/svc.php?ip=127.0.0.1&sid=gcoop&u=iscodoa&pwd=iscodoa&coop_id=032001&f=of_chk_withdrawcount_amt&a=n_deposit&p[]=0104016155&p[]=2018-10-24T13:23:00&p[]=WTX&p[]=100.00	
	if($f=="of_chk_withdrawcount_amt"){
		try {	
				//กรณี เป็น Date ต้องเป็น Format : date("Y-m-d")."T".date("H:i:s"),		
				$parameters = array(
					'as_wspass' => "Data Source=$ip/$sid;Persist Security Info=True;User ID=$u;Password=$p;Unicode=True;coop_id=$coop_id;coop_control=$coop_id;",
					'as_account_no' => $_REQUEST["p"][0],
					'adtm_date' => $_REQUEST["p"][1],
					'as_itemtype_code'=>$_REQUEST["p"][2],
					'adc_amt'=>$_REQUEST["p"][3]
				);
				
			   $ret = $client->of_chk_withdrawcount_amt($parameters);
			   print_r($ret);
			   //echo $ret;
		}catch(Exception $e){
			   echo $e->getMessage();
		}
	}
}
?>