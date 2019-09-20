using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [System.Serializable]
    public class Theme
    {
        public Color32 HP_FILL_COLOR;
        public Color32 HP_SECONDARY_COLOR;
        public Color32 ENEMY_COLOR;
        public Color32 TRAIL_COLOR;
    }


    public class StyleContainer : MonoBehaviour
    {
        //SINGLETON
        public static StyleContainer Instance;

        public Theme[] Themes;

        private void Awake()
        {
            Instance = this;
        }
    }