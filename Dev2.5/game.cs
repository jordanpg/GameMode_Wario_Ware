//Useful if you want examples of how WW_RegisterGame is used.
//Your games should be registered here, most likely.
//WW_RegisterGame(%name, %items, %flags, %execfile); --See Documentation.
WW_RegisterGame("DONTMOVE", "", 		"DEATH", 				$WW::Dir::Micro @ "/0_move.cs");
WW_RegisterGame("PLATFORM", "", 		"DEATH" TAB "USEACT", 	$WW::Dir::Micro @ "/1_platform.cs");
WW_RegisterGame("SAY", 		"", 		"", 					$WW::Dir::Micro @ "/2_say.cs");
WW_RegisterGame("MATH", 	"", 		"", 					$WW::Dir::Micro @ "/3_math.cs");
WW_RegisterGame("GUN", 		"Gun", 		"DEATH" TAB "OFF", 		$WW::Dir::Micro @ "/4_gun.cs");
WW_RegisterGame("EMOTE", 	"", 		"", 					$WW::Dir::Micro @ "/5_emote.cs");
WW_RegisterGame("FALLING", 	"", 		"", "DEATH" TAB "OFF", 	$WW::Dir::Micro @ "/6_falling.cs");