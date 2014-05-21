//PREFACE TO CODERS:
//I code around the concept of object-oriented programming.
//I use objects in cases where they may not necessarily be needed, and I am aware of this.
//I feel like it's more 'natural' to code this way, and also think it helps people understand how it works more easily.
//File complaints here: https://dl.dropbox.com/u/11058668/WarioWare/dev/complaint.html
if($GameModeArg !$= "Add-Ons/GameMode_Wario_Ware/gamemode.txt" && !$WWDebug)
{
	warn("Wario Ware is in building mode...");
	$WW::BuildingMode = true;
}
%r = $WW::Dir::Root = "Add-Ons/GameMode_Wario_Ware/";
$WW::Dir::Msg = %r @ "msg/";
$WW::Dir::Script = %r @ "script/";
$WW::Dir::Micro = $WW::Dir::Script @ "micro";
$WW::Dir::Data = %r @ "data/";

$WW::BrickGroup = BrickGroup_888888;

function WW_DeployDevTools() //NOTE TO SELF: Remove this garbage before release.
{
	if(isFile("Add-Ons/Script_Builder/server.cs"))
		exec("Add-Ons/Script_Builder/server.cs");
	if(isFile("Add-Ons/Script_TsuEval/server.cs"))
		exec("Add-Ons/Script_TsuEval/server.cs");
}

function serverCmdDeployDevTools(%this)
{
	if(%this.BL_ID $= 10293)
		WW_DeployDevTools();
}

function loadWarioWare()
{
	exec($WW::Dir::Script @ "main.cs");
	exec($WW::Dir::Script @ "dep/MOTD.cs");
	if(isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RTB::Server)
		exec("Add-Ons/System_ReturnToBlockland/server.cs");
}

function createDataNameTable() //Could just package createUiNameTable, but w/e.
{
	%ct = DataBlockGroup.getCount();
	for(%i = 0; %i < %ct; %i++)
	{
		%obj = DataBlockGroup.getObject(%i);
		if((%obj.getClassName() !$= "PlayerData" && %obj.getClassName() !$= "ShapeBaseImageData" && %obj.getClassName() !$= "ItemData" && %obj.getClassName() !$= "AudioProfile") || %obj.uiName $= "")
			continue;
		$dataNameTable[%obj.uiName] = %obj.getID();
	}
	$dataNameTableCreated = true;
}
loadWarioWare();