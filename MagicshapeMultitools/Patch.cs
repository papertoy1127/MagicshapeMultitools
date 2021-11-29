using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using ADOFAI;
using HarmonyLib;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityModManagerNet;
using Object = UnityEngine.Object;

namespace MagicshapeMultitools {
    [HarmonyPatch(typeof(scrLevelMaker), "InstantiateFloatFloors")]
    public static class GhostFloorPatch {
        internal static List<scrFloor> GhostFloors = new List<scrFloor>();

        public static void Postfix(scrLevelMaker __instance) {
            var gameObject1 = GameObject.Find("GhostFloors");
            if (gameObject1 == null)
                gameObject1 = new GameObject("GhostFloors");
            foreach (var listFloor in GhostFloors.Where(listFloor => listFloor != null)) {
                Object.DestroyImmediate(listFloor.gameObject);
            }

            GhostFloors = new List<scrFloor>();
            if (!Toolbar.GhostTiles) return;
            
            if (scrLevelMaker.instance.listFloors.Count == 0) return;
            var lastFloor = scrLevelMaker.instance.listFloors.Last();
            var scrFloor = lastFloor;
            bool flag = true;
            var zero = lastFloor.transform.position;
            var count = Main.Settings.VertexCount;
            var addangle = 360.0 / count;
            for (int i = 1; i < count; i++) {
                for (int index = 0; index < __instance.floorAngles.Length; ++index) {
                    double startRadius = scrController.instance.startRadius;
                    int num = Application.isEditor ? 1 : 0;
                    float floorAngle = __instance.floorAngles[index];
                    double angle = floorAngle == 999 ? scrFloor.entryangle : (-(double) floorAngle + 90.0 + addangle * i) * (Math.PI / 180.0);
                    scrFloor.exitangle = angle;
                    var vectorFromAngle = scrMisc.getVectorFromAngle(angle, startRadius);
                    zero += vectorFromAngle;
                    var gameObject2 =
                        Object.Instantiate(__instance.meshFloor, zero, Quaternion.identity);
                    gameObject2.name = string.Format("{0}/Floor {1}", index + 1, floorAngle);
                    gameObject2.gameObject.transform.parent = gameObject1.transform;
                    var component2 = gameObject2.GetComponent<scrFloor>();
                    if (scrFloor != lastFloor) {
                        scrFloor.nextfloor = component2;
                    }
                    GhostFloors.Add(component2);
                    component2.floatDirection = floorAngle;
                    component2.seqID = index + 1;
                    component2.entryangle = (angle + 3.14159274101257) % 6.28318548202515;
                    component2.isCCW = !flag;
                    component2.speed = 1f;
                    if (floorAngle == 999) scrFloor.midSpin = true;

                    //var orig = scrLevelMaker.instance.listFloors[i];
                    
                    scrFloor.UpdateAngleComp();
                    scrFloor = component2;
                    
                    var material = scrFloor.floorRenderer.material;
                    material.SetTexture("_MainTex", RDC.data.tex_floorEdgeDefault);
                    material.SetTexture("_TileTex", RDC.data.tex_floorTileNone);
                    material.SetTexture("_PerlinTex", RDC.data.tex_perlinNone);
                    scrFloor.floorRenderer.color = new Color(1, 1, 1, 0.25f);
                }

            }
            scrFloor.exitangle = scrFloor.entryangle + 3.14159274101257;
            scrFloor.UpdateAngleComp();
        }
    }

    [HarmonyPatch(typeof(scnEditor), "SelectFloor")]
    public static class UnselectablePatch {
        public static bool Prefix(scrFloor floorToSelect) {
            if (GhostFloorPatch.GhostFloors.Contains(floorToSelect)) return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(scnEditor), "Awake")]
    public static class ShowToolbarPatch {
        public static void Postfix() {
            Toolbar.Init();
        }
    }

    public static class Utils {
        public static float NormalizeAngle(this float angle) {
            do {
                angle += 360;
                angle %= 360;
            } while (angle < 0);

            return angle;
        }

        public static LevelEvent LevelEvent(int newFloor, LevelEventType type) {
            var constructor = typeof(LevelEvent).GetConstructors();
            UnityModManager.Logger.Log(constructor.Length.ToString());
            var r77 = constructor.FirstOrDefault(i => i.GetParameters().Length == 4);
            if (r77 != null) {
                return (LevelEvent) r77.Invoke(new object[] {newFloor, type, null, null});
            }
            return (LevelEvent) constructor.First(i => i.GetParameters().Length == 2).Invoke(new object[] {newFloor, type});
        }

        public static void UpdateAngleComp(this scrFloor floor) {
            var updateAngle = typeof(scrFloor).GetMethod("UpdateAngle");
            updateAngle!.Invoke(floor, updateAngle.GetParameters().Select(parameter => parameter.DefaultValue).ToArray());
        }
    }
}