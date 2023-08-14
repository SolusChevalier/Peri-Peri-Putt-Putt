using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class Hud : MonoBehaviour
    {
        #region FIELDS
        
        public PlayerDataSO playerDataSO;
        public Slider shotStrengthSlider;
        
        #endregion

        #region UNITY METHODS
        
        #endregion

        #region METHODS
        
        public void SetShotStrength() {
            playerDataSO.SetShotStrength(shotStrengthSlider.value);
        }
        
        
        
        #endregion
    }