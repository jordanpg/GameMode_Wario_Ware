//EMOTE MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//Good reference if your microgame intercepts arbitrary serverCmds.

$WWG::EMOTE::EMOTES = "Love Hate Confusion Alarm";

function WWG_EMOTE::Start(%this)
{
	%this.emote = getWord($WWG::EMOTE::EMOTES, getRandom(0, getWordCount($WWG::EMOTE::EMOTES) - 1));
	parent::Start(%this);
}

function WWG_EMOTE::SpawnMsg(%this)
{
	return "Do the" SPC %this.emote SPC "emote!";
}

function WWG_EMOTE::End(%this)
{
	parent::End(%this);
	%this.emote = "";
}

package WWG_EMOTE
{
	function serverCmdLove(%this)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_EMOTE.getID() && WWG_EMOTE.emote $= "Love")
					%this.player.WWG_Act = true;
			}
		}
		parent::serverCmdLove(%this);
	}
	
	function serverCmdHate(%this)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_EMOTE.getID() && WWG_EMOTE.emote $= "Hate")
					%this.player.WWG_Act = true;
			}
		}
		parent::serverCmdHate(%this);
	}
	
	function serverCmdConfusion(%this)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_EMOTE.getID() && WWG_EMOTE.emote $= "Confusion")
					%this.player.WWG_Act = true;
			}
		}
		parent::serverCmdConfusion(%this);
	}
	
	function serverCmdWtf(%this)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_EMOTE.getID() && WWG_EMOTE.emote $= "Confusion")
					%this.player.WWG_Act = true;
			}
		}
		parent::serverCmdWtf(%this);
	}
	
	function serverCmdAlarm(%this)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_EMOTE.getID() && WWG_EMOTE.emote $= "Alarm")
					%this.player.WWG_Act = true;
			}
		}
		parent::serverCmdAlarm(%this);
	}
};
activatePackage(WWG_EMOTE);