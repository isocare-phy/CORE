<?php
//session_start();
//header('Content-Type: text/html; charset=tis-620');


    function get_value_many_sql($strSQL,$colunm = array()){       
        $objQuery = mysql_query($strSQL) ;//or die ("Error Query [".$strSQL."]");
       	$Num_Rows  = mysql_num_rows($objQuery);
		$i = 0;
		while($objResult = mysql_fetch_assoc($objQuery)){
			for($j=0;$j<count($colunm);$j++){
				$value[$i][$j]  = $objResult[$colunm[$j]];
			}  
			$i++;
		}
		return array($Num_Rows,$value);
    }
	
    function get_single_value_sql($strSQL,$value){        // select field เดียว Mysql      
        $objQuery = mysql_query($strSQL);// or die ("Error Query [".$strSQL."]");
        while($objResult = mysql_fetch_array($objQuery)){
             return $objResult[$value];
        }            
    }
	
 	function insert_value_sql($table,$condition,$value){   // insert ลงตาราง  Mysql     
        $strSQL = "INSERT INTO $table $condition VALUES $value";
        $objQuery = mysql_query($strSQL);// or die ("Error Query [".$strSQL."]");
        return $objQuery;     
    }
	
	function update_value_sql($table,$condition,$value){   // update ลงตาราง  Mysql     
       $strSQL = "UPDATE $table SET $value $condition ";
       $objQuery = mysql_query($strSQL);//or die ("Error Query [".$strSQL."]");
       return $objQuery;
    }
	
	function delete_value_sql($table,$id){  // delete จากตาราง  Mysql     
		$strSQL = "DELETE FROM $table WHERE id = $id ";
		$objQuery = mysql_query($strSQL);// or die ("Error Query [".$strSQL."]");
		return $objQuery;

	}

	
?>