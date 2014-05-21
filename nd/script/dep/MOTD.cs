//Advanced Message of the Day Mod
//By: ottosparks
//NOTE TO SELF:
//Probably remove before release.
$MOTDFile = $WW::Dir::Root @ "MOTD.txt";
function loadAdvMOTD()
{
	if(!isFile($MOTDFile))
		return false;
	deleteVariables("$Server::MOTD*");
	%file = new fileObject();
	%file.openForRead($MOTDFile);
	for(%i = 0; !%file.isEOF(); %i++)
		$Server::MOTD[%i] = %file.readLine();
	$Server::MOTDCt = %i + 1;
	%file.close();
	%file.delete();
	echo("Updated MOTDs...");
	$Server::MOTDLoad = true;
	return true;
}

function serverCmdReloadMOTDs(%this)
{
	if(!(%this.isAdmin || %this.isSuperAdmin))
		return;
	%success = loadAdvMOTD();
	if(%success)
		messageAll('', "\c3" @ %this.getPlayerName() SPC "\c6reloaded the MOTDs.");
	else
		messageClient('', "\c0MOTD file not found!");
}

function serverCmdDoMOTD(%this)
{
	if($Server::MOTDLoad)
	{
		for(%i = 0; %i < $Server::MOTDCt; %i++)
			messageClient(%this, '', $Server::MOTD[%i]);
	}
}

package AdvMOTD
{
	function GameConnection::autoAdminCheck(%this)
	{
		%r = parent::autoAdminCheck(%this);
		if(!$Server::MOTDLoad)
		{
			%success = loadAdvMOTD();
			if(!%success)
				return %r;
		}
		schedule(0, 0, serverCmdDoMOTD, %this);
		return %r;
	}
};
activatePackage(AdvMOTD);