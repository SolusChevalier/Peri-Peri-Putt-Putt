using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 90)]
    public class PlayerDataSO : ScriptableObject
    {
        #region FIELDS
        
        public float shotStrength;
        
        #endregion

        #region METHODS
        
        public void SetShotStrength(float strength) {
            shotStrength = strength;
        }
        public float GetShotStrength() {
            return shotStrength;
        }
        
        #endregion
    }