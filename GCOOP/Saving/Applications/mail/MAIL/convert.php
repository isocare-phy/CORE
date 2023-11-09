<?php
	//header('content-type:text/html;charset=utf-8');
	// hex2bin requires PHP >= 5.4.0.
	// If, for whatever reason, you are using a legacy version of PHP, you can implement hex2bin with this function:
	 
	if (!function_exists('hex2bin')) {
			function hex2bin($hexstr) {
				$n = strlen($hexstr);
				$sbin = "";
				$i = 0;
				while ($i < $n) {
					$a = substr($hexstr, $i, 2);
					$c = pack("H*", $a);
					if ($i == 0) {
						$sbin = $c;
					} else {
						$sbin .= $c;
					}
					$i += 2;
				}
			return $sbin;
		}
	}

	function concatpercent($string) {
		$strLen = strlen($string);
		if($strLen > 0){
			$output .= "%";
			for($i=0; $i < $strLen; $i++) {
				$output .= $string{$i};
				if($i % 2 != 0 && $i < $strLen - 1){
					$output .= "%";
				}
			}
		}
		return $output;
	 }

	function unicodeMessageEncode($sString){
		$sString = strtoupper(mb_strtoupper(bin2hex(mb_convert_encoding($sString, 'UTF-16BE', 'UTF-8'))));
		return concatpercent($sString);
	}

	function unicodeMessageDecode($sString) {
		$sString = str_replace("%", "", $sString);
		$_message = hex2bin($sString);
		$sString = mb_convert_encoding($_message, 'UTF-8', 'UCS-2');
		return $sString;
	}
	 
	if ($_POST["type"] == "unicode") {
        $out = unicodeMessageDecode($_POST["data"]);
    }else if ($_POST["type"] == "text") {
		$out = unicodeMessageEncode($_POST["data"]);
	}
	//return $out;
?>