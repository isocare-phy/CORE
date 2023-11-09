<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');

function get_single_value_oci($strSQL,$value){  	// ถึงข้อมูล Single จาก database		
        GLOBAL $objConnect; 
		GLOBAL $status;
        $stmt = oci_parse($objConnect, $strSQL );
        $status=@oci_execute($stmt);
        while ($row = @oci_fetch_assoc($stmt)) {   
            return $row[$value];
        }           
    } 

    function get_value_many_oci($strSQL,$colunm=array()){     // ถึงข้อมูล Mulit จาก database
        $value=array();
        GLOBAL $objConnect;  
		GLOBAL $status;
        $objParse = oci_parse ($objConnect, $strSQL);
        $status=@oci_execute ($objParse,OCI_DEFAULT);
        $Num_Rows = @oci_fetch_all($objParse, $Result);       
        for($i=0;$i<$Num_Rows;$i++){
            for($j=0;$j<count($colunm);$j++){
                $value[$i][$j] =  $Result[$colunm[$j]][$i];
            }     
        }    
        return array($Num_Rows,$value);
    } 
	
?>