$WW::Exec::Main = true;
if(!$WW::BuildingMode)
	$WW::ScriptLoadOrder = "dep/MOTD dep/Other dep/Players wario/Wario_Flags wario/Wario_Game wario/Wario_Data wario/Wario_Micro event serverCmd init game";
else
	$WW::ScriptLoadOrder = "event";

function WW_LoadScripts()
{
	exec($WW::Dir::Root @ "data/datablocks.cs");
	%scripts = getWordCount($WW::ScriptLoadOrder);
	for(%i = 0; %i < %scripts; %i++)
		exec($WW::Dir::Script @ getWord($WW::ScriptLoadOrder, %i) @ ".cs");
}
schedule(0, 0, WW_LoadScripts);