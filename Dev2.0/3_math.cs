//MATH MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//Another example of a microgame using chat or custom rewards.
//Feel free to use the generateMathProblem function anywhere.

function generateMathProblem(%terms, %min, %max, %ops)
{
	if(%ops $= "")
		%ops = "+ -";
	for(%i = 0; %i < %terms; %i++)
	{
		%const = getRandom(%min, %max);
		if(%i == %terms - 1)
		{
			%prob = %prob SPC %const;
			break;
		}
		%op = getWord(%ops, getRandom(0, getWordCount(%ops) - 1));
		if(%i == 0)
			%prob = %const SPC %op;
		else
			%prob = %prob SPC %const SPC %op;
	}
	return trim(%prob) TAB eval("return (" @ %prob @ ");");
}

function WWG_MATH::Start(%this)
{
	%this.terms = getRandom(2, 5);
	%this.min = getRandom(1, 25);
	%this.max = getRandom(10, 35);
	%this.opUseMultiply = (getRandom(1, 5) == 1);
	%this.math = generateMathProblem(%this.terms, %this.min, %this.max, "+ -" @ (%this.opUseMultiply ? " *" : ""));
	%this.problem = getField(%this.math, 0);
	%this.answer = getField(%this.math, 1);
	parent::Start(%this);
}

function WWG_MATH::SpawnMsg(%this)
{
	return "What is <shadow:-2:-2><shadowcolor:CCCCCC>" @ %this.problem @ "<shadow:0:0>?";
}

function WWG_MATH::End(%this)
{
	parent::End(%this);
	%this.terms = "";
	%this.min = "";
	%this.max = "";
	%this.opUseMultiply = "";
	%this.math = "";
	%this.problem = "";
	%this.answer = "";
}

function WWG_MATH::RewardPlayer(%this, %client)
{
	%range = mAbs(%this.max - %this.min);
	%rangePts = mFloor(%range / 15) * 0.5;
	if(%rangePts < 1)
		%rangePts = 0.5;
	%opPts = %opUseMultiply * 0.5;
	%termPts = mFloor(%this.terms / 2.5) * 0.5;
	if(%termPts < 1)
		%termPts = 0.5;
	%total = mFloor(%termPts + %opPts + %rangePts);
	%client.score += %total;
	return %total;
}

package WWG_MATH
{
	function serverCmdMessageSent(%this, %text)
	{
		if(!%this.WW_CURRnotPlaying && isObject(%this.player))
		{
			if(isObject($WW::Mini) && isObject($WW::Mini.curGame))
			{
				if($WW::Mini.curGame.getID() $= WWG_MATH.getID())
				{
					if(firstWord(%text) $= WWG_MATH.answer)
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
activatePackage(WWG_MATH);