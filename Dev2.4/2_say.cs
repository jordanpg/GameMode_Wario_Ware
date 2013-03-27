//SAY MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//A good reference if your microgame uses chat or custom point rewards.
//Feel free to use the generateWord function wherever.

function generateWord(%len)
{
	%vowels = "a e i o u ea ee oo ie ei";
	%cons = "b c d f g h j k l m n p qu r s t v w x y z bb ll gg tt pp rr ng th ch ph kn pl cl sh rh wh kl mp rk st sp";
	%type = getRandom(0, 1);
	for(%i = 0; %i < %len; %i++)
	{
		%lastType = %type;
		%type = !%lastType;
		switch(%type)
		{
			case 0: %add = getWord(%vowels, getRandom(0, getWordCount(%vowels) - 1));
			case 1: %add = getWord(%cons, getRandom(0, getWordCount(%cons) - 1));
			default: %add = getWord(%cons, getRandom(0, getWordCount(%cons) - 1));
		}
		%word = %word @ %add;
	}
	return %word;
}

function WWG_SAY::Start(%this)
{
	%this.wordLength = getRandom(4, 12);
	%this.word = generateWord(%this.wordLength);
	parent::Start(%this); //Calling at the end for this because we want the SpawnMsg function to work.
}

function WWG_SAY::DoSpawnMsg(%this, %client, %msg)
{
	if(%client.hasWWClient)
		commandToClient(%client, 'WW_GameMsg', %msg, false);
	else
		%client.centerPrint("<color:FFFF33>" @ %msg, %this.GetLength());
}

function WWG_SAY::SpawnMsg(%this)
{
	return "Say " @ %this.word @ " in chat!";
}

function WWG_SAY::End(%this)
{
	parent::End(%this);
	%this.wordLength = "";
	%this.word = "";
}

function WWG_SAY::RewardPlayer(%this, %client)
{
	%points = mFloor(%this.wordLength / 4);
	%client.score += %points;
	return %points;
}

package WWG_SAY
{
	function serverCmdMessageSent(%this, %text)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_SAY.getID())
				{
					if(firstWord(%text) $= WWG_SAY.word)
					{
						%this.player.WWG_Act = true;
						return;
					}
				}
			}
		}
		parent::serverCmdMessageSent(%this, %text);
	}
};
activatePackage(WWG_SAY);