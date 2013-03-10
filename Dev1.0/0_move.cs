//DON'T MOVE WARIOWARE MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//This is probably a bad example for most microgames. Don't use this for extensive reference unless your idea is similar fundamentally.

//Using Spawn instead of start because spawn includes a client and we also don't want to do this until we know there's a player.
function WWG_DONTMOVE::Spawn(%this, %client)
{
	parent::Spawn(%this, %client);
	%client.player.WWG_DONTMOVE_POS = ""; //Not necessary, but makes me feel better.
	%client.player.WWG_Act = true; //The player's done nothing wrong yet.
}

function WWG_DONTMOVE::Time(%this)
{
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_CURRnotPlaying) //We don't want players that aren't included in this round.
			continue;
		%player = %client.player;
		%pos = %player.getPosition();
		if(%this.ticks == 0) //We wait until the first second has passed to make it easier on players.
			%player.WWG_DONTMOVE_POS = %pos; //Set the position for reference at the end.
		else
		{
			if(%pos !$= %player.WWG_DONTMOVE_POS) //The "don't move" part. This is rather safe because it'd be difficult to move and go back to the exact same spot.
				%client.player.WWG_Act = false;
		}
	}
	parent::Time(%this);
}

function WWG_DONTMOVE::End(%this)
{
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%client = $WW::Mini.member[%i];
		if(%client.WW_CURRnotPlaying)
			continue;
		%player = %client.player;
		if(%player.WWG_Act) //This will be false only if their position was incorrect at any time.
			%client.WW_Points += %this.points; //add points!!!!
	}
	parent::End(%this);
}