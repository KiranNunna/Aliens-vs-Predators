using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeAction : MonoBehaviour
{
	void Start()
	{
		ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
		GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
		SwordAction.OnAnySword += SwordAction_OnAnySword;
	}

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnShootEventArgs e)
	{
		ScreenShake.Instance.Shake();
	}
	
	private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
	{
		ScreenShake.Instance.Shake(5f);
	}
	
    private void SwordAction_OnAnySword(object sender, SwordAction.OnSwordEventArgs e)
    {
        ScreenShake.Instance.Shake(2f);
    }
}
