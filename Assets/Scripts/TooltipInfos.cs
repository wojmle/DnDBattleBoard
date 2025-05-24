using System;
using UnityEngine;


    [Serializable]
    public struct TooltipInfos
    {
        public string Keyword;
        [TextArea]
        public string Description;
        public Sprite Image;
    }
