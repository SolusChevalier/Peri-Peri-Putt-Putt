using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 90)]
public class PlayerDataSO : ScriptableObject
{
    #region FIELDS

    public float shotStrength;
    public bool SliderPower = false;
    public bool isAiming = false;

    #endregion FIELDS

    #region METHODS

    public void SetShotStrength(float strength)
    {
        shotStrength = strength;
    }

    public float GetShotStrength()
    {
        return shotStrength;
    }

    public void SetSliderPower(bool power)
    { SliderPower = power; }

    public bool GetSliderPower()
    { return SliderPower; }

    public void SetIsAiming(bool aiming)
    { isAiming = aiming; }

    public bool GetIsAiming()
    { return isAiming; }

    #endregion METHODS
}