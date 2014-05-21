$WW::Exec::serverCmd = true;
if(!$WW::DediPrefs)
{
	$WW::Dedicated = $Server::Dedicated;
	$WW::DediTime = 10;
	$WW::DediRounds = 25;
	$WW::DediPrefs = true;
}

function serverCmdWario(%this, %cmd)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(getSubStr(%cmd, 0, 1) $= "/")
	{
		messageClient(%this, '', "\c0Do not include the slash in the command name.");
		return;
	}
	switch$(%cmd)
	{
		case "startRound":
			messageClient(%this, '', "\c3/startRound \c2[length]");
			messageClient(%this, '', "\c3Admin Command");
			messageClient(%this, '', "\c6Starts a round of Wario Ware with \c2[length] \c6microgames.");
			messageClient(%this, '', "\c6Minimum length is" SPC %WW::DefaultRounds @ ".");
		case "endRound":
			messageClient(%this, '', "\c3/endRound");
			messageClient(%this, '', "\c3Admin Command");
			messageClient(%this, '', "\c6Force-ends the current round.");
			messageClient(%this, '', "\c0WARNING: DISABLES DEDICATION");
		case "toggleMicroGame":
			messageClient(%this, '', "\c3/toggleMicroGame \c1[name]");
			messageClient(%this, '', "\c3Admin Command");
			messageClient(%this, '', "\c6Toggles the specified microgame on/off.");
			messageClient(%this, '', "\c0NOTICE: USES SCRIPT NAME!");
			messageClient(%this, '', "\c1Do \c3/wario \c2microgames \c1for a list of script names.");
		case "WWIgnoreMe":
			messageClient(%this, '', "\c3/WWIgnoreMe \c2[player]");
			messageClient(%this, '', "\c3Admin Command");
			messageClient(%this, '', "\c6Removes/re-adds a player from Wario Ware.");
			messageClient(%this, '', "\c6If no player is specified, it defaults to the player using the command.");
		case "WWDedi":
			messageClient(%this, '', "\c3/WWDedi \c2[command] \c5<arguments>");
			messageClient(%this, '', "\c3Admin Command");
			messageClient(%this, '', "\c6Use with no parameters for details.");
		case "Microgames":
			if(!isObject($WW::GameGroup))
			{
				messageClient(%this, '', "\c0No GameGroup! Go yell at ottosparks for being bad at coding!");
				return;
			}
			%count = $WW::GameGroup.getCount();
			for(%i = 0; %i < %count; %i++)
			{
				%obj = $WW::GameGroup.getObject(%i);
				messageClient(%this, '', (%obj.disabled ? "\c0" : "\c2") @ %obj.name);
			}
			messageClient(%this, '', "\c2You may need to page up!");
		default:
			messageClient(%this, '', "\c3WARIO WARE ADMINISTRATION COMMANDS");
			messageClient(%this, '', "\c2For more information on a specific command:");
			messageClient(%this, '', "\c3/wario \c2[command; no slash]");
			messageClient(%this, '', "\c3/startRound \c2[length]");
			messageClient(%this, '', "\c3/endRound");
			messageClient(%this, '', "\c3/toggleMicroGame \c1[name]");
			messageClient(%this, '', "\c3/WWIgnoreMe \c2 [player]");
			messageClient(%this, '', "\c3/WWDedi \c2[command] \c5<arguments>");
			messageClient(%this, '', "\c2You may need to page up!");
	}
}

function serverCmdStartRound(%this, %length)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(!isObject($WW::Mini))
		return;
	if(%length <= $WW::DefaultRounds)
		%length = $WW::DefaultRounds;
	WW_StartRound(%length);
}

function serverCmdEndRound(%this)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(!isObject($WW::Mini))
		return;
	if(!$WW::Mini.gameRunning)
		return;
	if($WW::Dedicated)
	{
		$WW::Dedicated = false;
		%tog = true;
	}
	WW_RoundEnd(true);
	messageAll('', "\c3" @ %this.getPlayerName() SPC "\c6has force-ended the round" @ (%tog ? " and turned dedication off." : "."));
}

function serverCmdWWDedi(%this, %cmd, %arg)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	switch$(%cmd)
	{
		case "time":
			%time = firstWord(%arg);
			if(%time < 5)
				%time = 5;
			$WW::DediTime = %time;
			messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has set dedicated round delay to \c3" @ $WW::DediTime @ "\c6 seconds.");
		case "t":
			%time = firstWord(%arg);
			if(%time < 5)
				%time = 5;
			$WW::DediTime = %time;
			messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has set dedicated round delay to \c3" @ $WW::DediTime @ "\c6 seconds.");
		case "check":
			%dedicated = ($WW::Dedicated ? "\c2ON\c6" : "\c0OFF\c6");
			%time = "\c3" @ $WW::DediTime @ "\c6 seconds";
			%rounds = "\c3" @ $WW::DediRounds @ "\c6 microgames";
			messageClient(%this, '', "\c6--WARIOWARE DEDICATED STATUS--");
			messageClient(%this, '', "\c6Currently:" SPC %dedicated);
			messageClient(%this, '', "\c6Round Delay:" SPC %time);
			messageClient(%this, '', "\c6Round Length:" SPC %rounds);
			messageClient(%this, '', "\c6------------------------------");
		case "c":
			%dedicated = ($WW::Dedicated ? "\c2ON\c6" : "\c0OFF\c6");
			%time = "\c3" @ $WW::DediTime @ "\c6 seconds";
			%rounds = "\c3" @ $WW::DediRounds @ "\c6 microgames";
			messageClient(%this, '', "\c6--WARIOWARE DEDICATED STATUS--");
			messageClient(%this, '', "\c6Currently:" SPC %dedicated);
			messageClient(%this, '', "\c6Round Delay:" SPC %time);
			messageClient(%this, '', "\c6Round Length:" SPC %rounds);
			messageClient(%this, '', "\c6------------------------------");
		case "rounds":
			%rounds = firstWord(%arg);
			if(%rounds <= 0)
				%rounds = 1;
			$WW::DediRounds = %rounds;
			messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has set dedicated round length to \c3" @ $WW::DediRounds @ "\c6 microgames.");
		case "r":
			%rounds = firstWord(%arg);
			if(%rounds <= 0)
				%rounds = 1;
			$WW::DediRounds = %rounds;
			messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has set dedicated round length to \c3" @ $WW::DediRounds @ "\c6 microgames.");
		case "toggle":
			$WW::Dedicated = !$WW::Dedicated;
			switch($WW::Dedicated)
			{
				case 1: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has dedicated Wario Ware.");
				case 0: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has un-dedicated Wario Ware.");
			}
		case "tog":
			$WW::Dedicated = !$WW::Dedicated;
			switch($WW::Dedicated)
			{
				case 1: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has dedicated Wario Ware.");
				case 0: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has un-dedicated Wario Ware.");
			}
		case "on":
			switch($WW::Dedicated)
			{
				case 1: messageClient(%this, '', "\c0Wario Ware is already dedicated.");
				case 0:
					$WW::Dedicated = 1;
					messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has dedicated Wario Ware.");
			}
		case "off":
			switch($WW::Dedicated)
			{
				case 0: messageClient(%this, '', "\c0Wario Ware is not dedicated.");
				case 1:
					$WW::Dedicated = 0;
					messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has un-dedicated Wario Ware.");
			}
		default:
			messageClient(%this, '', "\c6The Wario Ware gamemode has a built-in system that will automatically start rounds when active.");
			messageClient(%this, '', "\c6This command can modify that dedication system by using it like so:");
			messageClient(%this, '', "\c3/WWDedi \c2[command] \c5<arguments>");
			messageClient(%this, '', "\c6The commands are:");
			messageClient(%this, '', "\c2TIME / T\c6: Changes how long after each round that a new round will start in seconds. (Default is 10)");
			messageClient(%this, '', "\c2ROUNDS / R\c6: Changes how many microgames that will be in each round started by the system. (Default is 25)");
			messageClient(%this, '', "\c2CHECK / C\c6: Checks the current status of the system. (No arguments)");
			messageClient(%this, '', "\c2TOGGLE / TOG\c6: Toggles the dedication system on/off. (Off by default)");
			messageClient(%this, '', "\c2ON / OFF\c6: Self-explanatory. Turns the dedication system on/off respectively. (Off by default)");
			messageClient(%this, '', "\c2You may need to page up!");
	}
}

function serverCmdToggleMicroGame(%this, %game)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(!isObject($WW::Mini))
		return;
	%games = $WW::GameGroup.getCount();
	for(%i = 0; %i < %games; %i++)
	{
		%micro = $WW::GameGroup.getObject(%i);
		if(%micro.name $= %game)
		{
			%bool = WW_ToggleGameOn(%micro);
			switch(%bool)
			{
				case 0: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has turned on the \c3" @ %micro.name @ "\c6 microgame.");
				case 1: messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 has turned off the \c3" @ %micro.name @ "\c6 microgame.");
				default: messageClient(%this, '', "\c0Error in toggling microgame."); //would not ever happen
			}
			return;
		}
	}
	messageClient(%this, '', "\c0Microgame not found.");
	messageClient(%this, '', "\c1Do \c3/wario \c2microgames \c1for a list of script names.");
}

function serverCmdWWIgnoreMe(%this, %player)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(!isObject($WW::Mini))
		return;
	if(isObject(%player = findClientByName(%player)))
	{
		%oldThis = %this;
		%this = %player;
	}
	%curIgnore = %this.WW_notPlaying;
	switch(%curIgnore)
	{
		case 0:
			%this.WW_notPlaying = true;
			if(isObject(%oldThis))
				messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 is now disincluded from Wario Ware. (" @ %oldThis.getPlayerName() @ ")");
			else
				messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 is now disincluded from Wario Ware.");
		case 1:
			%this.WW_notPlaying = false;
			%this.WW_CURRnotPlaying = false;
			if(isObject(%oldThis))
				messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 is now included in Wario Ware. (" @ %oldThis.getPlayerName() @ ")");
			else
				messageAll('', "\c3" @ %this.getPlayerName() @ "\c6 is now included in Wario Ware.");
	}
}

function serverCmdWW_RecInfo(%this, %type)
{
	switch$(%type)
	{
		case "DEDI":
			if(!(%this.isAdmin || %this.isSuperAdmin))
			{
				commandToClient(%this, 'WW_AdminRec', "NOTADMIN");
				return;
			}
			%sendString = "ACTIVE" SPC $WW::Dedicated TAB "ROUNDS" SPC $WW::DediRounds TAB "TIME" SPC $WW::DediTime;
			commandToClient(%this, 'WW_AdminRec', "DEDI", %sendString);
		case "GAME":
			if(!(%this.isAdmin || %this.isSuperAdmin))
			{
				commandToClient(%this, 'WW_AdminRec', "NOTADMIN");
				return;
			}
			%gameCt = $WW::GameGroup.getCount();
			%curGame = 0;
			%groups = mFloor(%gameCt / 5);
			for(%i = 0; %i <= %groups; %i++)
			{
				if(%curGame >= %gameCt)
					break;
				for(%x = 0; %x < 5; %x++)
				{
					%game = $WW::GameGroup.getObject(%curGame);
					%string = %game.name SPC %game.disabled;
					%group = trim(%group[%i] TAB %string);
					%curGame++;
				}
				commandToClient(%this, 'WW_AdminRec', "GAME", %group);
			}
		case "POINTS":
			commandToClient(%this, 'WW_ShopRec', "POINTS" SPC %this.score);
		case "NAME":
			commandToClient(%this, 'WW_ShopRec', "NAME" SPC %this.WW_ShapeNameColor);
		case "SHOP":
			%entries = $WW::Shop.getCount();
			for(%i = 0; %i < %entries; %i++)
			{
				%entry = $WW::Shop.getObject(%i);
				%string = "SHOP" SPC %entry.sname SPC %entry.cost SPC %entry.name SPC %entry.description;
				commandToClient(%this, 'WW_ShopRec', %string);
			}
	}
}