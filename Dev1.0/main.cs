$WW::ScriptLoadOrder = "dep/Other dep/Players wario/Wario_Flags wario/Wario_Game wario/Wario_Micro event game init";

function WW_LoadScripts()
{
	for(%i = 0; %i <= getWordCount($WW::ScriptLoadOrder); %i++)
		exec($WW::Dir::Script @ getWord($WW::ScriptLoadOrder, %i) @ ".cs");
}