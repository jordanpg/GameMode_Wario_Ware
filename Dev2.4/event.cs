function fxDTSBrick::WWG_onStart(%obj, %microgame)
{
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["MiniGame"] = getMinigameFromObject(%obj);
	
	if(%microgame.class $= "WWGame")
		$InputTarget_["MicroGame"] = %microgame;

	%obj.processInputEvent("WWG_onStart", %microgame);
}

function fxDTSBrick::WWG_onEnd(%obj, %microgame)
{
	$InputTarget_["Self"]   = %obj;
	$InputTarget_["MiniGame"] = getMinigameFromObject(%obj);
	
	if(%microgame.class $= "WWGame")
		$InputTarget_["MicroGame"] = %microgame;

	%obj.processInputEvent("WWG_onEnd", %microgame);
}

function MiniGameSO::WWG_Act(%this, %client)
{
	if(%this.getID() !$= $WW::Mini.getID() || !isObject(%client) || !isObject(%this.curGame) || !%this.curGame.useAct)
		return;
	
	%this.curGame.Act(%client);
}

function MiniGameSO::WWG_UnAct(%this, %client)
{
	if(%this.getID() !$= $WW::Mini.getID() || !isObject(%client) || !isObject(%this.curGame))
		return;
	
	%this.curGame.UnAct(%client);
}
registerInputEvent(fxDTSBrick, WWG_onStart, "Self fxDTSBrick	MiniGame MiniGameSO	MicroGame WWGame");
registerInputEvent(fxDTSBrick, WWG_onEnd, "Self fxDTSBrick	MiniGame MiniGameSO	MicroGame WWGame");
registerOutputEvent("Minigame", "WWG_Act", "", 1);
registerOutputEvent("Minigame", "WWG_UnAct", "", 1);