<?php
header ('Content-Type: image/png'); 
echo file_get_contents("http://104.194.10.102:8090/?" . $_SERVER['QUERY_STRING']); 
?>