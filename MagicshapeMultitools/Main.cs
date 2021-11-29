using HarmonyLib;
using UnityModManagerNet;

namespace MagicshapeMultitools {
    public static class Main {
        public static Harmony Harmony { get; private set; }
        internal static UnityModManager.ModEntry _mod;
        internal static MainSettings Settings { get; private set; }
        
        private static bool Load(UnityModManager.ModEntry modEntry) { 
            _mod = modEntry;
            _mod.OnToggle = OnToggle;
            _mod.OnGUI = OnGUI;
            _mod.OnSaveGUI = OnSaveGUI;

            Harmony = new Harmony(modEntry.Info.Id);
            Settings = UnityModManager.ModSettings.Load<MainSettings>(modEntry);
            Assets.Load();
            
            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value) {
            if (value) {
                Harmony.PatchAll();
            } else {
                Harmony.UnpatchAll(_mod.Info.Id);
            }

            return true;
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry) {
            Settings.Draw(modEntry);
        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry) {
            Settings.Save(modEntry);
        }
    }
}