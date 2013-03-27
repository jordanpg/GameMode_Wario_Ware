//PLATFORM WARIOWARE MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//This is probably an okay example for microgames that use bricks. It's a bit more complex than what yours may be, however.

$WWG::PLATFORM::PLATFORMS = "1 2 3 4 5 6 7 8 9";
$WWG::PLATFORM::CLICKPUSH = 10;
//Platform Patterns (Some repeated for weighting. r5 is random platform, r is random platform excluding five.):
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 2 3 4 6 7 8 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 5 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "5";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 5 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 2 3 4 6 7 8 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 5 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "2 4 6 8";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "2 4 6 8";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "2 4 6 8";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "2 4 5 6 8";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 2 3 4 5 6 7 8 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "1 3 7 9";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "r r r r";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "r5 r r";
$WWG::PLATFORM::PATTERN[$WWG::PLATFORM::PATTERNCT++ - 1] = "r5 r r r r";

function WWG_PLATFORM::Start(%this)
{
	parent::Start(%this);
	%pattern = $WWG::PLATFORM::PATTERN[getRandom(0, $WWG::PLATFORM::PATTERNCT - 1)]; //Get a random platform pattern from the list.
	%platformCt = getWordCount(%pattern);
	%availablePlatforms = $WWG::PLATFORM::PLATFORMS;
	for(%i = 0; %i < %platformCt; %i++)
	{
		%platform = getWord(%pattern, %i);
		if(%platform !$= "r" && %platform !$= "r5")
		{
			$WW::BrickGroup.disappearNTObject("_WWG_PLATFORM" @ %platform, true);
			%availablePlatforms = removeWord(%availablePlatforms, findWord(%availablePlatforms, %platform));
		}
		else if(%platform $= "r")
		{
			if(striPos(%availablePlatforms, "5") != -1)
			{
				%availablePlatforms = removeWord(%availablePlatforms, 5); //Remove five temporarily.
				%removedFive = true;
			}
			%newPlatform = getWord(%availablePlatforms, getRandom(0, getWordCount(%availablePlatforms) - 1));
			$WW::BrickGroup.disappearNTObject("_WWG_PLATFORM" @ %newPlatform, true);
			%availablePlatforms = removeWord(%availablePlatforms, findWord(%availablePlatforms, %newPlatform)) @ (%removedFive ? " 5" : "");
			%removedFive = false;
		}
		else
		{
			%newPlatform = getWord(%availablePlatforms, getRandom(0, getWordCount(%availablePlatforms) - 1));
			$WW::BrickGroup.disappearNTObject("_WWG_PLATFORM" @ %newPlatform, true);
			%availablePlatforms = removeWord(%availablePlatforms, findWord(%availablePlatforms, %newPlatform));
		}
	}
	if($WW::BrickGroup.hasNTObject("_WWG_PLATFORMZONE"))
	{
		for(%i = 0; %i < $WW::BrickGroup.NTObjectCount_WWG_PLATFORMZONE; %i++)
			$WW::BrickGroup.NTObject_WWG_PLATFORMZONE_[%i].WWG_onStart(%this);
	}
}

function WWG_PLATFORM::End(%this)
{
	%platformCt = getWordCount($WWG::PLATFORM::PLATFORMS);
	for(%i = 0; %i < %platformCt; %i++)
		$WW::BrickGroup.disappearNTObject("_WWG_PLATFORM" @ getWord($WWG::PLATFORM::PLATFORMS, %i), false);
	if($WW::BrickGroup.hasNTObject("_WWG_PLATFORMZONE"))
	{
		for(%i = 0; %i < $WW::BrickGroup.NTObjectCount_WWG_PLATFORMZONE; %i++)
			$WW::BrickGroup.NTObject_WWG_PLATFORMZONE_[%i].WWG_onEnd(%this);
	}
	parent::End(%this);
}

package WWG_PLATFORM
{
	function Player::activateStuff(%this)
	{
		%return = parent::activateStuff(%this);
		
		if(!isObject($WW::Mini))
			return %return;
		%client = %this.client;
		if(!isObject(getMinigameFromObject(%client)))
			return %return;
		if(getMinigameFromObject(%client).getID() !$= $WW::Mini.getID() || !isObject($WW::Mini.curGame))
			return %return;
		if($WW::Mini.curGame.getID() !$= WWG_PLATFORM.getID())
			return %return;
		
		%target = containerRayCast(%this.getEyePoint(), vectorAdd(vectorScale(vectorNormalize(%this.getEyeVector()), 2), %this.getEyePoint()), $TypeMasks::PlayerObjectType, %this);
		if(!isObject(%target) || %target == %this || %this.getObjectMount() == %target)
			return %return;
			
		%target.setVelocity(vectorAdd(%target.getVelocity(), vectorScale(%this.getEyeVector(), $WWG::PLATFORM::CLICKPUSH)));
		return %return;
	}
};
activatePackage(WWG_PLATFORM);