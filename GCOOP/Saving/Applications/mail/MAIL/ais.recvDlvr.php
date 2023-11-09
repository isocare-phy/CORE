<?php
	// Reponse to SMSGW by XML Response with SMID posted
	header("Content-Type: text/xml");
	echo "<XML><STATUS>OK</STATUS><DETAIL></DETAIL></XML>";

	// write log DR
	//CMD=DLVRREP&NTYPE=REP&FROM=66935783405&SMID=1502000103625&STATUS=OK&DETAIL=DELIVRD
	$filename = getcwd() . "/log/" . date("Ymd") . ".txt";
	if(!file_exists($filename)){
		touch($filename);
	}
	$fh = fopen($filename, 'a') or die("can't open file");
	$stringData = date("Y-m-d H:i:s") . " - FROM:" . $_POST["FROM"] . " STATUS:" . $_POST["STATUS"] . " DETAIL:" . $_POST["DETAIL"] . " SMID:" . $_POST["SMID"] . "\r\n";
	fwrite($fh, $stringData);
	fclose($fh);
	// write log DR
?>