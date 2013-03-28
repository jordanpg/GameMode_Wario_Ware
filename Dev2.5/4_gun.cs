//GUN MICROGAME
//By: ottosparks
//NOTES TO CODERS:
//A good reference for microgames using items, custom rewarding conditions, or the death method.
//Uses a datablock defined in datablocks.cs.

function WWG_GUN::Start(%this)
{
	parent::Start(%this);
}

function WWG_GUN::Death(%this, %client, %killer)
{
	if(!isObject(%killer))
		return Parent::Death(%this, %client, %killer);
	%killer.setDataBlock(WarioInvulnPlayer);
	%killer.removeItem($dataNameTable[%this.item0]);
	return Parent::Death(%this, %client, %killer);
}

function WWG_GUN::canRewardPlayer(%this, %client)
{
	if(!isObject(%client.player))
		return false;
	if(%client.player.getDatablock().isInvincible)
		return true;
	return false;
}