function MiniGameSO::WWG_Act(%this, %client)
{
	if(%this.getName() !$= "WarioWare" || !isObject(%client) || !isObject(%this.curGame))
		return;
	%this.curGame.Act(%client);
}

function MiniGameSO::WWG_UnAct(%this, %client)
{
	if(%this.getName() !$= "WarioWare" || !isObject(%client) || !isObject(%this.curGame))
		return;
	%this.curGame.UnAct(%client);
}
registerOutputEvent("Minigame", "WWG_Act", "", 1);
registerOutputEvent("Minigame", "WWG_UnAct", "", 1);