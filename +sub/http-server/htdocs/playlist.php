<?php
/**
 * Example Application
 *
 */


$id = $_GET['id'] ?? 'no-id';

require_once __DIR__ . '/libs/Smarty.class.php';

$smarty = new \Smarty\Smarty;

$smarty->debugging = true;
$smarty->caching = true;
$smarty->cache_lifetime = 120;
$smarty->assign("Name", "Fred Irving Johnathan Bradley Peppergill", true);
$smarty->assign("FirstName", array("John", "Mary", "James", "Henry"));
$smarty->assign("LastName", array("Doe", "Smith", "Johnson", "Case"));
$smarty->assign("playlistId", $id);

$json = shell_exec('yt-info-playlist.exe ' . $id);
$arr = json_decode($json,true);
$smarty->assign("json", $arr);

$smarty->display('playlist.tpl');
