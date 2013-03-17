function player::addItem( %this, %tool ) //Port's doing.
{
	if ( !isObject( %tool ) )
	{
		return;
	}

	%tool = %tool.getID();
	%slots = %this.getDataBlock().maxTools;

	for ( %i = 0 ; %i < %slots ; %i++ )
	{
		if ( !isObject( %this.tool[ %i ] ) )
		{
			%this.tool[ %i ] = %tool;

			if ( isObject( %cl = %this.client )  )
			{
				messageClient( %cl, 'MsgItemPickup', '', %i, %tool );
			}

			break;
		}
	}
}

function GameConnection::spawnInArea(%this, %cornerA, %cornerB)
{
	%this.instantRespawn();
	while(ObjectAtPosition(%vect = getRandomVect(%cornerA, %cornerB), $TypeMasks::PlayerObjectType | $TypeMasks::fxBrickObjectType))
	{
		%i++;
		%vect = getRandomVect(%cornerA, %cornerB);
		if(%i >= 20)
			return;
	}
	%this.player.position = %vect;
}

function GameConnection::ObsStart(%this)
{
	%target = %this.ObsNext();
	if(!isObject(%target))
		return 0;
	return %target;
}

function GameConnection::ObsInfo(%this, %client)
{
	if(isObject(%client))
		%this.centerPrint("<color:FFFFFF><shadow:-2:-2><shadowcolor:55555555>" @ %client.getPlayerName() NL "<color:FFFF33>Next--Fire" NL "Prev--Alt. Fire", -1, 1);
	else
		%this.centerPrint("<color:CCCCCC><shadow:-2:-2><shadowcolor:55555555> Nobody" NL "<color:FFFF33>Next--Fire" NL "Prev--Alt. Fire", -1, 1);
}

function GameConnection::ObsOrbitPlayer(%this, %player)
{
	if(!isObject(%player) || !isObject(%cam = %this.camera))
		return false;
	%cam.setMode("Corpse", %player);
	%this.setControlObject(%cam);
}

function GameConnection::ObsNext(%this)
{
	%cam = %this.camera;
	%curTarget = %this.obsTarget;
	%mini = getMinigameFromObject(%this);
	if(!isObject(%cam) || !isObject(%mini))
		return;
	if(%curTarget >= %mini.numMembers - 1)
		%curTarget = -1;
	for(%i = %curTarget + 1; %i < %mini.numMembers; %i++)
	{
		%client = %mini.member[%i];
		if(%client.WW_CURRnotPlaying)
			continue;
		if(!isObject(%client.player))
		{
			if(!%repeat && %i >= %mini.numMembers - 1)
			{
				%repeat = true;
				%i = -1;
			}
			continue;
		}
		
		%cam.setMode("Corpse", %client.player);
		%this.setControlObject(%cam);
		%this.obsTarget = %i;
		%this.ObsInfo(%client);
		
		return %client;
	}
	%this.ObsInfo();
	return 0;
}

function GameConnection::ObsPrev(%this)
{
	%cam = %this.camera;
	%curTarget = %this.obsTarget;
	%mini = getMinigameFromObject(%this);
	if(!isObject(%cam) || !isObject(%mini))
		return;
	if(%curTarget <= 0)
		%curTarget = %mini.numMembers;
	for(%i = %curTarget - 1; %i >= 0; %i--)
	{
		%client = %mini.member[%i];
		if(%client.WW_CURRnotPlaying)
			continue;
		if(!isObject(%client.player))
		{
			if(!%repeat && %i <= 0)
			{
				%repeat = true;
				%i = %mini.numMembers;
			}
			continue;
		}
		
		%cam.setMode("Corpse", %client.player);
		%this.setControlObject(%cam);
		%this.obsTarget = %i;
		%this.ObsInfo(%client);
		
		return %client;
	}
	%this.ObsInfo();
	return 0;
}

function GameConnection::findCurrentMicroGame(%this)
{
	if(!isObject($WW::Mini))
		return -1;
	if(!isObject($WW::Mini.curGame))
		return -1;
	if(%this.WW_CURRnotPlaying)
		return -1;
	return $WW::Mini.curGame;
}

package WW_Players //Thanks to Port and Greek2Me for reference in this part of the code!
{
//	function Observer::onTrigger(%this, %player, %slot, %val)
//	{
//		if(!isObject(%player))
//			return parent::onTrigger(%this, %player, %slot, %val);
//
//		%client = %player.getControllingClient();
//		if(!%val || !isObject(%client))
//			return parent::onTrigger(%this, %player, %slot, %val);
//		if(!isObject($WW::Mini))
//			return parent::onTrigger(%this, %player, %slot, %val);
//		if(!isObject(%mini = getMinigameFromObject(%client)) || %mini.getID() !$= $WW::Mini.getID())
//			return parent::onTrigger(%this, %player, %slot, %val);
//		if($WW::Mini.gameRunning && !%this.WW_CURRnotPlaying)
//		{
//			if(%client.dead)
//			{
//				switch(%slot)
//				{
//					case 0: %client.ObsNext();
//					case 4: %client.ObsPrev();
//				}
//			}
//		}
//	}
	
//	function GameConnection::onDeath(%this, %killer, %killerClient, %type, %loc)
//	{
//		parent::onDeath(%this, %killer, %killerClient, %type, %loc);
//		
//		if(!isObject($WW::Mini))
//			return;
//		if(!isObject(%mini = getMinigameFromObject(%this)))
//			return;
//		if(%mini.getID() !$= $WW::Mini.getID())
//			return;
//		if($WW::Mini.gameRunning && !%this.WW_CURRnotPlaying)
//		{
//			%dead = $WW::Mini.curGame.Death(%this);
//			if(%dead)
//			{
//				messageClient(%this, 'MsgYourSpawn');
//				%this.ObsStart();
//				messageClient(%this, "\c5You cannot respawn in this microgame.", 2);
//			}
//		}
//	}
	
	function GameConnection::createPlayer(%this, %spawnPoint)
	{
		%this.dead = false;
		return parent::createPlayer(%this, %spawnPoint);
	}
	
	function GameConnection::spawnPlayer(%this)
	{
		%r = parent::spawnPlayer(%this);
		%micro = %this.findCurrentMicroGame();
		if(%micro $= -1)
			%this.player.position = getRandomVect(_WW_ARENASPAWNA.getPosition(), _WW_ARENASPAWNB.getPosition());
		else
			%this.player.position = getRandomVect(%micro.spawnCornerA.getPosition(), %micro.spawnCornerB.getPosition());
	}
	
	function MiniGameSO::removeMember(%this, %client)
	{
		%client.dead = false;
		parent::removeMember(%this, %client);
	}
	
	function MiniGameSO::reset(%this)
	{
		for(%i = 0; %i < %this.numMembers; %i++)
			%this.member[%i].dead = false;
		parent::reset(%this);
	}
	
	function serverCmdDropCameraAtPlayer(%this)
	{
		if(!isObject($WW::Mini))
			return parent::serverCmdDropCameraAtPlayer(%this);
		%mini = getMinigameFromObject(%this);
		if($WW::Mini.getID() !$= %mini.getID())
			return parent::serverCmdDropCameraAtPlayer(%this);
		if(!%this.dead)
			return parent::serverCmdDropCameraAtPlayer(%this);
		return;
	}
	
	function serverCmdDropPlayerAtCamera(%this)
	{
		if(!isObject($WW::Mini))
			return parent::serverCmdDropPlayerAtCamera(%this);
		%mini = getMinigameFromObject(%this);
		if($WW::Mini.getID() !$= %mini.getID())
			return parent::serverCmdDropPlayerAtCamera(%this);
		if(!%this.dead)
			return parent::serverCmdDropPlayerAtCamera(%this);
		return;
	}
};
activatePackage(WW_Players);