//FALLING MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//Probably another reference for microgames using named bricks, custom rewards, and custom spawn messages.
//Also another reference for microgames using delayed events.

$WWG::FALLING::NTNAME = "_WW_TOP"; //Name of brick(s) spawning projectiles.
$WWG::FALLING::SPAWNERS = 9; //Number of bricks spawning projectiles.
$WWG::FALLING::PROJ = "Gun Bullet"; //UIName of projectiles used. Separate by field.
$WWG::FALLING::PROJDISPLAY = "bullets"; //Used in the spawn message. Cooresponds to the respective field in the PROJ global.
$WWG::FALLING::SCALE = "R 1 2"; 	//Determines size of projectiles. Cooresponds to the respective field in the PROJ global.
									//Usage:
									//A field using an integer as the first word will always use that scale.
									//A field using "R" as the first word will use a random float between the two following words for each INDIVIDUAL projectile.
									//e.g. "R 1 2" would spawn projectiles with random scales between 1.0 and 2.0.
									//A field using "RAll" as the first word will use a random float between the two following words for ALL projectiles spawned.
									//e.g. "RAll 1 2" would spawn projectiles all sharing a random scale between 1.0 and 2.0.
									//Using any of the R flags, but instead with "RInt" will force integers to be used instead of floats.
									//Minimum scale is 0.2. Maximum scale is two.
$WWG::FALLING::POINTS = "1"; //Points given for succeeding. Cooresponds to the respective field in the PROJ global.
$WWG::FALLING::SPEEDRANGE = "10 30";

function WWG_FALLING::Start(%this)
{
	if(!$dataNameTableCreated)
		createDataNameTable();
	%p = $WWG::FALLING::PROJ;
	%rand = getRandom(0, getFieldCount(%p) - 1);
	%projectile = getField(%p, %rand);
	%this.projectileDB = $dataNameTable[%projectile];
	%this.projectile = getField($WWG::FALLING::PROJDISPLAY, %rand);
	%range = $WWG::FALLING::SPEEDRANGE;
	%this.speed = getRandom(getWord(%range, 0), getWord(%range, 1));
	%scale = getField($WWG::FALLING::SCALE, %rand);
	%type = firstWord(%scale);
	if(%type !$= "R" && %type !$= "RInt")
	{
		%this.individual = false;
		switch$(%type)
		{
			case "RAll":
				%min = getWord(%scale, 1);
				if(%min < 0.2)
					%min = 0.2;
				%max = getWord(%scale, 2);
				if(%max > 2)
					%max = 2;
				%this.scale = getRandomFloat(%min, %max);
			case "RIntAll":
				%min = getWord(%scale, 1);
				if(%min < 0.2)
					%min = 0.2;
				%max = getWord(%scale, 2);
				if(%max > 2)
					%max = 2;
				%this.scale = getRandom(%min, %max);
			default:
				if(%type < 0.2)
					%type = 0.2;
				if(%type > 2)
					%type = 2;
				%this.scale = %type;
		}
	}
	else
	{
		%this.individual = true;
		%this.range = getWords(%scale, 1, 2) SPC (striPos(%type, "Int") !$= -1);
	}
	parent::Start(%this);
}

function WWG_FALLING::SpawnMsg(%this)
{
	return "Dodge falling" SPC %this.projectile @ "!";
}

function WWG_FALLING::Spawn(%this, %client)
{
	parent::Spawn(%this, %client);
	if(isObject(%client.player))
		%client.player.WWG_Act = true;
}

function WWG_FALLING::End(%this)
{
	parent::End(%this);
	%this.projectileDB = "";
	%this.projectile = "";
	%this.speed = "";
	%this.individual = "";
	%this.scale = "";
	%this.range = "";
}

function WWG_FALLING::TimeAction(%this, %tick)
{
	if(%time > 0 && %time < %this.GetLength() - 1)
	{
		if(!%this.individual)
			NTSpawnProjectileAll($WWG::BrickGroup, $WWG::FALLING::NTNAME, "0 0" SPC %this.speed, %this.projectileDB, "0 0 0", %this.scale);
		else
		{
			%min = getWord(%this.range, 0);
			%max = getWord(%this.range, 1);
			%int = getWord(%this.range, 2);
			for(%i = 0; %i < $WWG::BrickGroup.NTObjectCount[$WWG::FALLING::NTNAME]; %i++)
			{
				if(%int)
					$WWG::BrickGroup.NTObject[$WWG::FALLING::NTNAME, %i].spawnProjectile("0 0" SPC %this.speed, %this.projectileDB, "0 0 0", getRandom(%min, %max));
				else
					$WWG::BrickGroup.NTObject[$WWG::FALLING::NTNAME, %i].spawnProjectile("0 0" SPC %this.speed, %this.projectileDB, "0 0 0", getRandomFloat(%min, %max));
			}
		}
	}
}

package WWG_FALLING
{
	function ShapeBase::damage(%this, %sourceObject, %pos, %directDamage, %damageType)
	{
		%r = Parent::damage(%this, %sourceObject, %pos, %directDamage, %damageType);
		if(!isObject(%this.client))
			return %r;
		%micro = %this.client.findCurrentMicroGame();
		if(!isObject(%micro))
			return %r;
		if(%micro.getID() !$= WWG_FALLING.getID())
			return %r;
		if(!isObject(WWG_FALLING.projectileDB)) //should never happen.
			return %r;
		if(%sourceObject.getDatablock().getID() !$= WWG_FALLING.projectileDB.getID())
			%this.WWG_Act = false;
		return %r;
	}
	
	function NTSpawnProjectileAll(%group, %nt, %vectorA, %datablock, %vectorB, %scale)
	{
		if(!isObject(%group) || !isObject(%datablock))
			return false;
		if(!%group.hasNTObject(%nt) $= -1)
			return false;
		for(%i = 0; %i < %bg.NTObjectCount[%nt]; %i++)
			%bg.NTObject[%nt, %i].spawnProjectile(%vectorA, %datablock.getID(), %vectorB, %scale);
		return true;
	}
};
activatePackage(WWG_FALLING);