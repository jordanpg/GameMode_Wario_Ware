function getRandomFloat( %min, %max )
{
	return %min + getRandom() * ( %max - %min );
}

//getRandomVect(%vectA, %vectB[, %len]);
//Returns a random vector between Vector3F %vectA and Vector3F %vectB.
//%len is an unsigned integer 0-5 for the allowed float length, defaults to two.
function getRandomVect(%vectA, %vectB)
{
	%xA = getWord(%vectA, 0);
	%yA = getWord(%vectA, 1);
	%zA = getWord(%vectA, 2);
	%xB = getWord(%vectB, 0);
	%yB = getWord(%vectB, 1);
	%zB = getWord(%vectB, 2);
	if(%xA != %xB)
		%x = getRandomFloat(%xA, %xB);
	else
		%x = %xA;
		
	if(%yA != %yB)
		%y = getRandomFloat(%yA, %yB);
	else
		%y = %xA;
		
	if(%zA != %zB)
		%z = getRandomFloat(%zA, %zB);
	else
		%z = %zA;
	return %x SPC %y SPC %z;
}

function findField(%str, %searchString)
{
	%fcount = getFieldCount(%str);
	for(%i = 0; %i < %fcount; %i++)
	{
		%field = getField(%str, %i);
		if(striPos(%field, %searchString) != -1)
			return %i;
	}
	return -1;
}

function findFieldFirst(%str, %searchString)
{
	%fcount = getFieldCount(%str);
	for(%i = 0; %i < %fcount; %i++)
	{
		%field = firstWord(getField(%str, %i));
		if(striPos(%field, %searchString) != -1)
			return %i;
	}
	return -1;
}

function ShapeBase::setPosition(%this, %vect)
{
	%this.position = %vect;
}

package WW_Other
{
	function getMinigameFromObject(%obj)
	{
		if(%obj.getClassName() $= "ScriptObject" && %obj.class $= "WWGame")
			return (isObject($WW::Mini) ? $WW::Mini : -1);
		return parent::getMinigameFromObject(%obj);
	}
};
activatePackage(WW_Other);