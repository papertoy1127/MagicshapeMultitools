using UnityEngine;
using UnityModManagerNet;

namespace MagicshapeMultitools {
    public class MainSettings : UnityModManager.ModSettings, IDrawable {
        public Vector2 Position = new(-20, -20);
        public int VertexCount = 4;

        public override void Save(UnityModManager.ModEntry modEntry) {
            Save(this, modEntry);
        }

        public void OnChange() { }
    }
}