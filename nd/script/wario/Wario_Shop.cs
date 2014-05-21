$WW::Exec::Shop = true;

function WW_InitShop()
{
	if(isObject(WW_Shop))
		WWShop.delete();
	$WW::Shop = new ScriptGroup(WW_Shop);
	warn("Created Wario Ware shop group.");
}

function findShopEntryByName(%name)
{
	if(!isObject($WW::Shop))
		return -1;
	%count = $WW::Shop.getCount();
	for(%i = 0; %i < %count; %i++)
	{
		%entry = $WW::Shop.getObject(%i);
		if(striPos(%entry.sname, %name) != -1)
			return %entry;
	}
	return -1;
}

function WW_RegisterShopEntry(%name, %description, %cost, %sname, %type, %data)
{
	if(!isObject($WW::Shop))
	{
		echo("WW_RegisterShopEntry : Shop group not created!");
		return;
	}
	if(%sname $= "")
	{
		echo("WW_RegisterShopEntry : Must provide a script name!");
		return;
	}
	if(%name $= "")
	{
		echo("WW_RegisterShopEntry : Must provide a GUI name!");
		return;
	}
	if(isObject(findShopEntryByName(%sname)))
	{
		echo("WW_RegisterShopEntry : Shop using this name already exists!");
		return;
	}
	
	%sname = strupr(%sname);
	
	switch$(%type)
	{
		case "Cosmetic": 		%class = "WWCosEntry";
		case "Cos":				%class = "WWCosEntry";
		case "C":				%class = "WWCosEntry";
		case "0":				%class = "WWCosEntry";
		case "Script":			%class = "WWScrEntry";
		case "Scr":				%class = "WWScrEntry";
		case "S":				%class = "WWScrEntry";
		case "1":				%class = "WWScrEntry";
		case "Miscellaneous":	%class = "WWMscEntry";
		case "Misc":			%class = "WWMscEntry";
		case "Msc":				%class = "WWMscEntry";
		case "M":				%class = "WWMscEntry";
		case "2":				%class = "WWMscEntry";
		default:
			echo("WW_RegisterShopEntry : Invalid entry type!");
			return;
	}
	
	%this = new ScriptObject("WWS_" @ %sname)
			{
				//SCRIPT VARIABLES
				superClass = "WWShopEntry";
				class = %class;
				
				//WARIO WARE VARIABLES
				cost = %cost;
				
				sname = %sname;
				name = %name;
				description = %description;
			};
			
	if(%class $= "WWCosEntry")
	{
		%cosDataBlock = getField(%data, 0);
		if(!isObject(nameToID(%cosDataBlock)))
		{
			echo("WW_RegisterShopEntry : Cosmetic datablock does not exist!");
			%this.delete();
			return;
		}
		%this.cosDataBlock = nameToID(%cosDataBlock);
		%this.cosMountPoint = getField(%data, 1);
	}
	%success = $WW::Shop.addEntry(%this);
	if(%success)
		echo("Registered shop entry" SPC %sname @ ".");
	else
	{
		echo("WW_RegisterShopEntry : ottosparks is bad at coding");
		%this.delete();
	}
}

function WW_Shop::addEntry(%this, %obj)
{
	if(%obj.getClassName() !$= "ScriptObject")
		return false;
	if(%obj.superClass !$= "WWShopEntry")
		return false;
	if(%obj.class !$= "WWCosEntry" && %obj.class !$= "WWScrEntry" && %obj.class !$= "WWMscEntry")
		return false;
	
	%this.add(%obj);
	return true;
}

function WWShopEntry::Purchase(%this, %client)
{
	if(%client.WW_HasEntry[%this.sname])
		return -1;
	if(%client.score < %this.cost)
		return 0;
//	%str = "PURCHASE" SPC %this.sname SPC true; Move this to serverCmd
//	commandToClient(%client, 'WW_ShopRec', %str);
	%client.WW_HasEntry[%this.sname] = true;
	%client.score -= %this.cost;
	return 1;
}

function WWCosEntry::onEquip(%this, %client, %val)
{
	//empty for now
}

function WWScrEntry::onUpdate(%this, %client, %data)
{
	//default function to avoid script errors
}

function Player::WW_EquipCos(%this, %entry, %val)
{
	if(!isObject(%client = %this.client))
		return;
	if(!isObject(%entry))
		return;
	if(!%client.WW_HasEntry[%entry.sname])
		return;
	
	if(%val)
	{
		if(isObject(%this.WW_CosMounted[%entry.cosMountPoint]))
			%this.WW_EquipCos(%this.WW_CosMounted[%entry.cosMountPoint], false);
		%this.mountImage(%entry.cosDataBlock, %entry.cosMountPoint);
		%this.WW_CosMounted[%entry.cosMountPoint] = %entry;
	}
	else
	{
		if(!isObject(%this.WW_CosMounted[%entry.cosMountPoint]))
			return;
		if(%this.WW_CosMounted[%entry.cosMountPoint].getID() !$= %entry.getID())
			return;
		%this.unMountImage(%entry.cosMountPoint);
		%this.WW_CosMounted[%entry.cosMountPoint] = -1;
	}
	%entry.onEquip(%client, %val);
}