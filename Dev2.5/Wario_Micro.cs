//These are class functions for WWGame.
//MAKE SURE TO CALL THE PARENT IN YOUR MICROGAME'S METHODS
//THESE MAY INCLUDE VITAL FUNCTIONALITY FOR MICROGAMES
$WW::Exec::Micro = true;

function WWGame::Start(%this)
{
	%this.tick = %this.schedule(1000, Time);
	%this.ticks = 0;
	if(%this.death)
		$WW::Mini.respawnTime = -1;
	%this.curSpawnMsg = %this.SpawnMsg();
}

function WWGame::SpawnMsg(%this)
{
	%msg = %this.msg[getRandom(1, %this.msgs)];
	return %msg;
}

function WWGame::DoSpawnMsg(%this, %client, %msg)
{
	if(%client.hasWWClient)
		commandToClient(%client, 'WW_GameMsg', %msg, false);
	else
		%client.centerPrint("<color:FFFF33>" @ %msg, 1);
}

function WWGame::Spawn(%this, %client)
{
	%client.spawnInArea(%this.spawnCornerA.getPosition(), %this.spawnCornerB.getPosition());
	if(%this.itemCt > 0)
	{
		for(%i = 0; %i < %this.itemCt; %i++)
			%client.player.addItem(%this.item[%i]);
	}
	%this.DoSpawnMsg(%client, %this.curSpawnMsg);
	%client.play2D(WarioStartSound);
}

function WWGame::End(%this, %forced)
{
	if(!isObject($WW::Mini.curGame))
		return;
	if($WW::Mini.curGame.getID() $= %this.getID())
	{
		for(%i = 0; %i < $WW::Mini.numMembers; %i++)
		{
			%client = $WW::Mini.member[%i];
			%client.dead = false;
			if(%client.WW_CURRnotPlaying || !isObject(%client.player))
				continue;
			if(%this.canRewardPlayer(%client))
			{
				%points = %this.RewardPlayer(%client);
				%client.wonLastMicroGame = %points;
			}
			%client.player.WWG_Act = false;
		}
	}
	%this.ticks = 0;
	%this.curSpawnMsg = "";
	if(%this.death)
		$WW::Mini.respawnTime = 1000;
	if(!%forced)
		WW_HandleGameEnd(%this);
}

function WWGame::Death(%this, %client, %killer)
{
	if(!isObject($WW::Mini.curGame))
		return;
	if($WW::Mini.curGame.getID() !$= %this.getID())
		return;
	if(%this.death)
		%client.dead = true;
	else
		%client.dead = false;
	for(%i = 0; %i < $WW::Mini.numMembers; %i++)
	{
		%cl = $WW::Mini.member[%i];
		if(%client.dead)
			continue;
		%alive[%notDead++] = %cl;
	}
	if(%notDead == 0)
		%this.AllPlayersDead(%client);
	else if(%notDead == 1)
		%this.OnePlayerLeft((isObject(%alive[1].player) ? %alive[1].player : -1));
	return %client.dead;
}

//This function may be overwritten in your microgames.
function WWGame::AllPlayersDead(%this, %lastPlayer)
{
	if(isObject($WW::Mini))
		$WW::Mini.respawnTime = 1000;
	%this.End();
}

function WWGame::OnePlayerLeft(%this, %lastPlayer)
{
		if(isObject($WW::Mini))
			$WW::Mini.respawnTime = 1000;
}

function WWGame::Act(%this, %client)
{
	if(isObject(%client.player))
		%client.player.WWG_Act = true;
}

function WWGame::UnAct(%this, %client)
{
	if(isObject(%client.player))
		%client.player.WWG_Act = false;
}

function WWGame::GetLength(%this)
{
	return %this.time;
}

function WWGame::TimeAction(%this, %tick)
{
}

function WWGame::Time(%this)
{
	cancel(%this.tick);
	%this.TimeAction(%this.ticks);
	%this.ticks++;
	if(%this.ticks >= %this.GetLength())
	{
		%this.End();
		return;
	}
	%this.tick = %this.schedule(1000, Time);
}

function WWGame::canRewardPlayer(%this, %client)
{
	if(!isObject(%client.player))
		return false;
	if(%client.player.WWG_Act)
		return true;
	return false;
}

function WWGame::RewardPlayer(%this, %client)
{
	%client.score += %this.points;
	return %this.points;
}