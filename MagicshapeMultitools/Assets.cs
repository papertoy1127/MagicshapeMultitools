using System;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace MagicshapeMultitools {
    public static class Assets {
        public static AssetBundle Bundle { get; private set; }
        public static GameObject Popup { get; private set; }

        public static string AssetPath => Path.Combine(Directory.GetCurrentDirectory(), "Mods", "MagicshapeMultitools",
            "magicshapetools");

        public static void Load() {
            Bundle = AssetBundle.LoadFromFile(AssetPath);
            Popup = Bundle.LoadAsset<GameObject>("Popup");
            UnityModManager.Logger.Log("Loaded assets!");
        }
    }
}