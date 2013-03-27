datablock AudioProfile(WarioStartSound)
{
	filename = $WW::Dir::Data @ "microstart.wav";
	description = AudioClosest3d;
	preload = false;
};

datablock AudioProfile(WarioWonSound)
{
	filename = $WW::Dir::Data @ "microwon.wav";
	description = AudioClosest3d;
	preload = false;
};

datablock AudioProfile(WarioFailedSound)
{
	filename = $WW::Dir::Data @ "microfailed.wav";
	description = AudioClosest3d;
	preload = false;
};

datablock PlayerData(WarioInvulnPlayer : NoJetPlayer)
{
	uiName = "Invincible No-Jet Player";
	isInvincible = true;
};