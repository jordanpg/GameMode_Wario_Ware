$WW::Exec::Flag = true;

function WW_InitFlags()
{
	if(isObject(WW_Flags))
		WW_Flags.delete();
	$WW::Flags = 	new ScriptObject(WW_Flags)
					{
						FlagCt = 0;
					};
}

function WW_AddDefaultFlags()
{
	$WW::Flags::Defaults = false;
	$WW::Flags.registerFlag("BOSS", false, "boss");
	$WW::Flags.registerFlag("DEATH", false, "death");
	$WW::Flags.registerFlag("TIME INT", 3, "death");
	$WW::Flags.registerFlag("PLAYER STR", "No Jet Player", "player");
	$WW::Flags.registerFlag("MUSIC STR", -1, "music");
	$WW::Flags.registerFlag("MUSICBRICK WORD", -1, "musicbrick");
	$WW::Flags.registerFlag("USEACT", false, "useAct");
	$WW::Flags.registerFlag("SPAWNTIME TIME", 1000, "spawnTime");
	$WW::Flags.registerFlag("POINTS POINTS", 1, "points");
	$WW::Flags.registerFlag("SPAWN BL TR", _WW_ARENASPAWNA.getID() TAB _WW_ARENASPAWNB.getID(), "spawnCornerA spawnCornerB");
	$WW::Flags::Defaults = true;
}

function WW_Flags::registerFlag(%this, %flag, %default, %var)
{
	%fname = strupr(firstWord(%flag));
	if(striPos(%this.Flags, %fname) != -1 || %flag $= "" || %var $= "")
		return;
	%args = restWords(%flag);
	%this.FlagName[$WW::FlagCt] = %fname;
	%this.FlagArgs[%fname] = %args;
	%this.Flag[%fname] = $WW::FlagCt;
	%this.FlagVar[%fname] = %var;
	%this.FlagDef[%fname] = %default;
	%this.Flags = trim($WW::Flags SPC %fname);
	%this.FlagCt++;
}