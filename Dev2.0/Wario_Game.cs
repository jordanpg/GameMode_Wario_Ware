if(!$WW::Exec::Flags)
{
	if(isFile($WW::Dir::Script @ "wario/Wario_Flags.cs"))
		exec($WW::Dir::Script @ "wario/Wario_Flags.cs");
	else
		return;
}
$WW::DefaultRounds = 25;
$WW::Exec::Game = true;

function WW_Init() //Function to initialize WarioWare on the default minigame.
{
	if(!isObject($DefaultMiniGame))
		return false; //May consider changing this to create a minigame.
	%this = $DefaultMiniGame;
	
	if(!isObject($WW::GameGroup) && !isObject(%this.gameGroup)) //If nothing exists...
		$WW::GameGroup = %this.gameGroup = new scriptGroup(WWGameGroup); //Create the group for game objects.
	else if(isObject($WW::GameGroup) && !isObject(%this.gameGroup)) //If the group exists, but the minigame doesn't know it...
		%this.gameGroup = $WW::GameGroup; //Make the minigame know it.
	else if(!isObject($WW::GameGroup) && isObject(%this.gameGroup)) //If the group doesn't exist, but the minigame has something it thinks it is...
		%this.gameGroup = $WW::GameGroup = new scriptGroup(WWGameGroup); //Get that joker in line.

	$WW::Mini = %this;
	return true;
}

function WW_RegisterGame(%name, %items, %flags, %execfile) //See documentation.
{
	if(!isObject($WW::Mini) || !isObject($WW::GameGroup)) //We have to have WarioWare loaded and the GameGroup created.
	{
		%success = WW_Init(); //Try to create both with this...
		if(!%success) //If it fails...
			return false; //We all fail.
	}
	if(!isObject($WW::Flags))
	{
		if($WW::Exec::Flags)
			WW_InitFlags();
		else
			return;
	}
	if(!$WW::Flags::Defaults)
		WW_AddDefaultFlags();
	if(isObject("WWG_" @ %name)) //Checks if a microgame of this name already exists.
		return false;
	if(%name $= "")
		return false;
	if(!$dataNameTableCreated) //Check if the data name table exists. See server.cs.
		createDataNameTable();

	%this =	new ScriptObject("WWG_" @ %name) //Create the microgame object
			{
				//SCRIPT
				class = "WWGame";
				
				//MICROGAME
				name = %name;
			};
			
	for(%i = 0; %i < $WW::Flags.FlagCt; %i++) //This block sets flags.
	{
		%fname = $WW::Flags.FlagName[%i];
		if(striPos(%flags, %fname) != -1)
		{
			if($WW::Flags.FlagArgs[%fname] $= "")
			{
				eval("%this." @ firstWord($WW::Flags.FlagVar[%fname]) @ " = true;");
				continue;
			}
			%field = getField(%flags, findFieldFirst(%flags, %fname));
			%args = restWords(%field);
			%aCt = getWordCount($WW::Flags.FlagArgs[%fname]);
			%argCt = getWordCount(%args) - 1;
			if(%aCt == 1)
			{
				eval("%this." @ firstWord($WW::Flags.FlagVar[%fname]) @ " = " @ %args @ ";");
				continue;
			}
			for(%x = 0; %x < %aCt; %x++)
			{
				if(%x > %argCt)
				{
					eval("%this." @ getWord($WW::Flags.FlagVar[%fname], %x) @ " = " @ getWord($WW::Flags.FlagDef[%fname], %x) @ ";");
					continue;
				}
				eval("%this." @ getWord($WW::Flags.FlagVar[%fname], %x) @ " = " @ getWord(%args, %x) @ ";");
			}
		}
		else
		{
			%aCt = getWordCount($WW::Flags.FlagArgs[%fname]);
			if(%aCt <= 1)
			{
				eval("%this." @ $WW::Flags.FlagVar[%fname] @ " = " @ getField($WW::Flags.FlagDef[%fname], 0) @ ";");
				continue;
			}
			for(%x = 0; %x < %aCt; %x++)
				eval("%this." @ getWord($WW::Flags.FlagVar[%fname], %x) @ " = " @ getField($WW::Flags.FlagDef[%fname], %x) @ ";");
		}
	}
	

	if(%items !$= "")
	{
		%this.itemCt = %itemCt = getFieldCount(%items);
		for(%i = 0; %i <= %itemCt; %i++)
		{
			%item = getField(%items, %i);
			%this.item[%i] = (isObject($dataNameTable[%item]) ? $dataNameTable[%item] : $dataNameTable["Hammer "]);
		}
	}
	else
		%this.itemCt = 0;

	%msgfile = $WW::Dir::Msg @ %name @ "Msg.txt";
	if(isFile(%msgfile))
	{
		%file = new fileObject();
		%file.openForRead(%msgfile);
		for(%i = %file.readLine(); !%file.isEOF(); %i = %file.readLine())
			%this.msg[%this.msgs++] = %i;
		%file.close();
		%file.delete();
	}
	else
		%this.msg[%this.msgs++] = %name;
	$WW::GameGroup.add(%this); //Add to GameGroup
	if(isFile(%execfile)) //Execute execfile
		exec(%execfile);
	echo("WarioWare  :  Added microgame" SPC %name SPC "to GameGroup.");
}

function WW_ToggleGameOn(%game)
{
	if(!isObject(%game) || %game.class !$= "WWGame")
		return;
	return %game.disabled = !%game.disabled;
}

function WW_StartRound(%numRounds)
{
	if(!isObject($WW::Mini) || !isObject($WW::GameGroup)) //We have to have WarioWare loaded and the GameGroup created.
	{
		%success = WW_Init(); //Try to create both with this...
		if(!%success) //If it fails...
			return false; //We all fail.
	}
	
	%numMicroGames = $WW::GameGroup.getCount() - 1;
	for(%i = 0; %i < %numRounds; %i++)
	{
		%index = getRandom(0, %numMicroGames);
		%game = $WW::GameGroup.getObject(%index);
		if(%game.boss || %game.disabled)
			continue;
		$WW::Mini.game[%i] = %game.getID();
	}
	$WW::Mini.games = %numRounds;
	$WW::Mini.listed = true;
	$WW::Mini.curGame = $WW::Mini.game0;
	$WW::Mini.curRound = 0;
	
//	$loadOffset = $WW::Mini.curOffset = $WW::DefaultOffset;
//	$WW::Mini.largestY = 0;
//	$WW::Mini.largestX = 0;
//	$WW::Mini.curPos = "0 0";
	
	//WW_LoadGrid();
	
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_notPlaying)
		{
			%client.WW_CURRnotPlaying = true;
			continue;
		}
	}
	$WW::Mini.gameRunning = true;
	WW_BeginGame($WW::Mini.game[$WW::Mini.curRound]);
}

function WW_RoundEnd()
{
	if(!isObject($WW::Mini) || !isObject($WW::GameGroup)) //We have to have WarioWare loaded and the GameGroup created.
	{
		%success = WW_Init(); //Try to create both with this...
		if(!%success) //If it fails...
			return false; //We all fail.
	}
	if(!$WW::Mini.gameRunning)
		return;

	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_CURRnotPlaying)
		{
			%client.WW_CURRnotPlaying = false;
			continue;
		}
		%client.spawnInArea(_WW_ARENASPAWNA.getPosition(), _WW_ARENASPAWNB.getPosition());
	}
	for(%i = 0; %i < $WW::Mini.games; %i++)
		$WW::Mini.game[%i] = "";
	$WW::Mini.curGame = "";
	$WW::Mini.games = "";
	$WW::Mini.gameRunning = false;
	$WW::Mini.curRound = "";
	$WW::Mini.listed = false;
}

function WW_BeginGame(%game)
{
	if(!isObject($WW::Mini) || !isObject($WW::GameGroup)) //We have to have WarioWare loaded and the GameGroup created.
	{
		%success = WW_Init(); //Try to create both with this...
		if(!%success) //If it fails...
			return false; //We all fail.
	}
	if(!isObject(%game) || %game.class !$= "WWGame")
		return;
	$WW::Mini.curGame = %game;
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_CURRnotPlaying)
			continue;
		%game.Spawn(%client);
		%client.wonLastMicroGame = 0;
	}
	%game.Start();
}

function WW_HandleGameEnd(%game)
{
	if(!isObject($WW::Mini) || !isObject($WW::GameGroup)) //We have to have WarioWare loaded and the GameGroup created.
	{
		%success = WW_Init(); //Try to create both with this...
		if(!%success) //If it fails...
			return false; //We all fail.
	}
	if(!$WW::Mini.gameRunning)
		return;
	if(!$WW::Mini.curGame.getID() == %game.getID())
		return;
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_CURRnotPlaying)
			continue;
		if(%client.wonLastMicroGame > 0)
		{
			if(isObject(%client.player))
				%client.player.emote(winStarProjectile);
			%client.play2D(rewardSound);
			%client.centerPrint("\c6You got \c3+" @ %client.wonLastMicroGame @ "pt\c6 for winning!", 1);
		}
	}
	
	if($WW::Mini.curRound >= $WW::Mini.games - 1)
	{
		WW_RoundEnd();
		return;
	}
	$WW::Mini.curRound++;
	schedule(1000, 0, WW_BeginGame, $WW::Mini.game[$WW::Mini.curRound]);
}