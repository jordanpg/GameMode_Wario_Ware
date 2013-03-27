if(!$WW::DediPrefs)
{
	$WW::Dedicated = false;
	$WW::DediTime = 10;
	$WW::DediRounds = 25;
	$WW::DediPrefs = true;
}

function serverCmdStartRound(%this, %length)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	if(!isObject($WW::Mini))
		return;
	if(%length <= 0)
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
	WW_RoundEnd();
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
		case "t":
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
			messageClient(%this, '', "\c2TOGGLE / T\c6: Toggles the dedication system on/off. (Off by default)");
			messageClient(%this, '', "\c2ON / OFF\c6: Self-explanatory. Turns the dedication system on/off respectively. (Off by default)");
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
				default: messageClient(%this, '', "\c0Error in toggling microgame.");
			}
			return;
		}
	}
	messageClient(%this, '', "\c0Microgame not found.");
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