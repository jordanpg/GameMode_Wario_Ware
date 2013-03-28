//DON'T MOVE WARIOWARE MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//A good reference for microgames implementing delayed events as well as microgames following the theme of "DON'T X".

//Using Spawn instead of start because spawn includes a client and we also don't want to do this until we know there's a player.
function WWG_DONTMOVE::Spawn(%this, %client)
{
	parent::Spawn(%this, %client);
	%client.player.WWG_DONTMOVE_POS = ""; //Not necessary, but makes me feel better.
	%client.player.WWG_Act = true; //The player's done nothing wrong yet.
}

function WWG_DONTMOVE::TimeAction(%this, %tick)
{
	if(%tick < 1)
	{
		for(%i = 0; %i < $WW::Mini.numMembers; %i++)
		{
			%client = $WW::Mini.member[%i];
			if(%client.WW_CURRnotPlaying)
				continue;
			%player = %client.player;
			if(!isObject(%player))
				continue;
			%player.WWG_DONTMOVE_POS = %player.getPosition();
		}
	}
	else
	{
		for(%i = 0; %i < $WW::Mini.numMembers; %i++)
		{
			%client = $WW::Mini.member[%i];
			if(%client.WW_CURRnotPlaying)
				continue;
			%player = %client.player;
			if(!isObject(%player))
				continue;
			%pos = %player.getPosition();
			%posDiff = VectorSub(%player.WWG_DONTMOVE_POS, %pos);
			if(%posDiff !$= "0 0 0")
				%player.WWG_Act = false;
		}
	}
//	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
//	{
//		%client = $WW::Mini.member[%i];
//		if(%client.WW_CURRnotPlaying) //We don't want players that aren't included in this round.
//			continue;
//		%player = %client.player;
//		if(!isObject(%player))
//			continue;
//		%pos = %player.getPosition();
//		if(%this.ticks == 0) //We wait until the first second has passed to make it easier on players.
//			%player.WWG_DONTMOVE_POS = %pos; //Set the position for reference at the end.
//		else
//		{
//			if(%pos !$= %player.WWG_DONTMOVE_POS) //The "don't move" part. This is rather safe because it'd be difficult to move and go back to the exact same spot.
//				%client.player.WWG_Act = false;
//		}
//	}
	parent::TimeAction(%this, %tick);
}

package WW_DONTMOVE
{
	function Armor::onTrigger(%this, %obj, %slot, %val)
	{
		parent::onTrigger(%this, %obj, %slot, %val);
		%client = %obj.client;
		if(!isObject(%client))
			return;
		%mini = getMinigameFromObject(%client);
		if(!isObject(%mini))
			return;
		if(%mini.getID() !$= $WW::Mini.getID())
			return;
		if(!isObject(%mini.curGame))
			return;
		if(%mini.curGame.getID() !$= WWG_DONTMOVE.getID() || WWG_DONTMOVE.ticks < 1)
			return;
		if(%slot $= 2 && %val)
				%obj.WWG_Act = false;
	}
};
activatePackage(WW_DONTMOVE);