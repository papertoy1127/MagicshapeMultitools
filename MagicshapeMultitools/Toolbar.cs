using System;
using System.Linq;
using ADOFAI;
using DG.Tweening;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;
using Object = UnityEngine.Object;

namespace MagicshapeMultitools {
    public class Toolbar {
        public static bool GhostTiles => CurrOpenButton == OpenButton.GhostTiles;

        private enum OpenButton {
            None,
            GhostTiles,
            AddMultiplier
        }

        private static GameObject _popup;
        private static RectTransform _panelOuter;
        private static RectTransform _panel;
        private static Image _openBtn;
        private static RectTransform _openBtnIcon;
        private static RectTransform _buttons;

        private static Image _buttonGhostTile;
        private static Image _buttonAddMultiplier;

        private static RectTransform _ghostTiles;
        private static RectTransform _addMultipliers;
        private static RectTransform _ghostTileButtons;
        private static RectTransform _addMultiplierButtons;

        private static Image _vertexPlus;
        private static Image _vertexMinus;
        private static Image _vertexMinusIcon;
        private static Image _createGhostTiles;

        private static Image _swirlBlue;
        private static Image _swirlRed;
        private static Image _noSwirl;

        public static bool IsOpen { get; private set; }

        private static OpenButton CurrOpenButton;

        public static void Init() {
            CurrOpenButton = OpenButton.None;
            UnityModManager.Logger.Log("asodhjskfdagksfdjkdahfsdakjda");

            if (_popup != null) Object.Destroy(_popup);
            
            if (GCS.standaloneLevelMode) return;
            
            _popup = Object.Instantiate(Assets.Popup);
            _panelOuter = _popup.transform.GetChild(0).GetComponent<RectTransform>();
            _panel = _panelOuter.GetChild(0).GetComponent<RectTransform>();
            _openBtn = _panel.GetChild(0).GetComponent<Image>();
            _openBtnIcon = _openBtn.transform.GetChild(0).GetComponent<RectTransform>();
            _buttons = _panel.GetChild(1).GetComponent<RectTransform>();
            _buttonGhostTile = _buttons.GetChild(0).GetComponent<Image>();
            _buttonAddMultiplier = _buttons.GetChild(1).GetComponent<Image>();

            _ghostTiles = _panelOuter.GetChild(1).GetComponent<RectTransform>();
            _addMultipliers = _panelOuter.GetChild(2).GetComponent<RectTransform>();
            _ghostTileButtons = _ghostTiles.GetChild(0).GetComponent<RectTransform>();
            _addMultiplierButtons = _addMultipliers.GetChild(0).GetComponent<RectTransform>();

            _vertexPlus = _ghostTileButtons.GetChild(0).GetComponent<Image>();
            _vertexMinus = _ghostTileButtons.GetChild(1).GetComponent<Image>();
            _createGhostTiles = _ghostTileButtons.GetChild(2).GetComponent<Image>();
            _vertexMinusIcon = _vertexMinus.transform.GetChild(0).GetComponent<Image>();

            _swirlBlue = _addMultiplierButtons.GetChild(0).GetComponent<Image>();
            _swirlRed = _addMultiplierButtons.GetChild(1).GetComponent<Image>();
            _noSwirl = _addMultiplierButtons.GetChild(2).GetComponent<Image>();

            _panelOuter.gameObject.AddComponent<MouseDragBehaviour>();
            _openBtn.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                UnityModManager.Logger.Log(IsOpen.ToString());
                IsOpen = !IsOpen;
                if (IsOpen) {
                    DOTween.Kill("Close", false);
                    _openBtnIcon.DORotate(new Vector3(0, 0, -30), 0.3f).SetUpdate(true).SetId("Open");
                    _openBtn.DOColor(new Color32(255, 255, 255, 96), 0.3f).SetUpdate(true).SetId("Open");
                    _panel.DOSizeDelta(new Vector2(115, 40), 0.3f).SetUpdate(true).SetId("Open");
                    _buttons.gameObject.SetActive(true);
                } else {
                    DOTween.Kill("Open", false);
                    _openBtnIcon.DORotate(new Vector3(0, 0, 0), 0.3f).SetUpdate(true).SetId("Close");
                    _openBtn.DOColor(new Color32(255, 255, 255, 32), 0.3f).SetUpdate(true).SetId("Close");
                    _panel.DOSizeDelta(new Vector2(40, 40), 0.3f).OnComplete(() => _buttons.gameObject.SetActive(false))
                        .SetUpdate(true).SetId("Close");
                }
            });

            _buttons.gameObject.SetActive(false);
            _panel.sizeDelta = new Vector2(40, 40);
            _panelOuter.anchoredPosition = Main.Settings.Position;

            _ghostTiles.sizeDelta = new Vector2(40, 0);
            _addMultipliers.sizeDelta = new Vector2(40, 0);

            _buttonGhostTile.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                UnityModManager.Logger.Log(IsOpen.ToString());
                switch (CurrOpenButton) {
                    case OpenButton.GhostTiles:
                        CurrOpenButton = OpenButton.None;
                        DOTween.Kill("OpenGT", false);
                        _buttonGhostTile.DOColor(new Color32(255, 255, 255, 32), 0.3f).SetUpdate(true).SetId("CloseGT");
                        _ghostTiles.DOSizeDelta(new Vector2(40, 0), 0.3f)
                            .OnComplete(() => _ghostTileButtons.gameObject.SetActive(false)).SetUpdate(true)
                            .SetId("CloseGT");
                        break;
                    case OpenButton.AddMultiplier:
                        DOTween.Kill("OpenAM", false);
                        _buttonAddMultiplier.DOColor(new Color32(255, 255, 255, 32), 0.3f).SetUpdate(true)
                            .SetId("CloseAM");
                        _addMultipliers.DOSizeDelta(new Vector2(40, 0), 0.3f)
                            .OnComplete(() => _addMultiplierButtons.gameObject.SetActive(false)).SetUpdate(true)
                            .SetId("CloseAM");
                        goto default;
                    default:
                        CurrOpenButton = OpenButton.GhostTiles;
                        DOTween.Kill("CloseGT", false);
                        _buttonGhostTile.DOColor(new Color32(255, 255, 255, 64), 0.3f).SetUpdate(true).SetId("OpenGT");
                        _ghostTiles.DOSizeDelta(new Vector2(40, 110), 0.3f).SetUpdate(true).SetId("OpenGT");
                        _ghostTileButtons.gameObject.SetActive(true);
                        break;
                }

                scnEditor.instance.RemakePath();
            });

            _buttonAddMultiplier.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                UnityModManager.Logger.Log(IsOpen.ToString());
                switch (CurrOpenButton) {
                    case OpenButton.AddMultiplier:
                        CurrOpenButton = OpenButton.None;
                        DOTween.Kill("OpenAM", false);
                        _buttonAddMultiplier.DOColor(new Color32(255, 255, 255, 32), 0.3f).SetUpdate(true)
                            .SetId("CloseAM");
                        _addMultipliers.DOSizeDelta(new Vector2(40, 0), 0.3f)
                            .OnComplete(() => _addMultiplierButtons.gameObject.SetActive(false)).SetUpdate(true)
                            .SetId("CloseAM");
                        break;
                    case OpenButton.GhostTiles:
                        DOTween.Kill("OpenGT", false);
                        _buttonGhostTile.DOColor(new Color32(255, 255, 255, 32), 0.3f).SetUpdate(true).SetId("CloseGT");
                        _ghostTiles.DOSizeDelta(new Vector2(40, 0), 0.3f)
                            .OnComplete(() => _ghostTileButtons.gameObject.SetActive(false)).SetUpdate(true)
                            .SetId("CloseGT");
                        CurrOpenButton = OpenButton.AddMultiplier;
                        scnEditor.instance.RemakePath();
                        goto default;
                    default:
                        CurrOpenButton = OpenButton.AddMultiplier;
                        DOTween.Kill("CloseAM", false);
                        _buttonAddMultiplier.DOColor(new Color32(255, 255, 255, 64), 0.3f).SetUpdate(true)
                            .SetId("OpenAM");
                        _addMultipliers.DOSizeDelta(new Vector2(40, 110), 0.3f).SetUpdate(true).SetId("OpenAM");
                        _addMultiplierButtons.gameObject.SetActive(true);
                        break;
                }
            });

            _vertexPlus.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                Main.Settings.VertexCount += 1;
                _vertexMinusIcon.color = Color.white;

                scnEditor.instance.RemakePath();
            });

            _vertexMinus.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                Main.Settings.VertexCount = Mathf.Max(Main.Settings.VertexCount - 1, 2);
                if (Main.Settings.VertexCount <= 2) _vertexMinusIcon.color = new Color(1, 1, 1, 0.5f);
                else _vertexMinusIcon.color = Color.white;

                scnEditor.instance.RemakePath();
            });

            _createGhostTiles.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                using (new SaveStateScope(scnEditor.instance)) {
                    var count = Main.Settings.VertexCount;
                    var addangle = 360.0f / count;
                    for (int i = 1; i < count; i++) {
                        foreach (var angle in scrLevelMaker.instance.floorAngles) {
                            CustomLevel.instance.levelData.angleData.Add(angle == 999 ? angle : angle - addangle * i);
                        }
                    }
                }

                scnEditor.instance.RemakePath();
            });

            _swirlBlue.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                AddSwirls(false);
                scnEditor.instance.RemakePath();
            });

            _swirlRed.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                AddSwirls(true);
                scnEditor.instance.RemakePath();
            });

            _noSwirl.gameObject.AddComponent<SimpleButton>().@event.AddListener(() => {
                AddSwirls(null);
                scnEditor.instance.RemakePath();
            });
        }

        [HarmonyPatch(typeof(scnEditor), "SwitchToEditMode")]
        public static class ShowPatch {
            public static void Postfix() {
                _popup?.SetActive(true);
            }
        }
        
        [HarmonyPatch(typeof(scnEditor), "Play")]
        public static class HidePatch {
            public static void Postfix() {
                _popup?.SetActive(false);
            }
        }

        private static void AddSwirls(bool? inner) {
            using (new SaveStateScope(scnEditor.instance)) {

                if (inner != null) {
                    CustomLevel.instance.events.RemoveAll(e =>
                        e.eventType is LevelEventType.Twirl or LevelEventType.SetSpeed);
                }
                var angles = scrLevelMaker.instance.floorAngles.ToList();
                float lastAngle = angles.First();
                angles.RemoveAt(0);
                bool swirl = false;
                int index = 1;
                float multiplier = 1;
                foreach (float angle in angles) {
                    if (inner == null) {
                        swirl ^= CustomLevel.instance.events.Count(e =>
                            e.floor == index && e.eventType == LevelEventType.Twirl) % 2 == 1;
                    }
                    if (angle == 999) {
                        lastAngle = (lastAngle + 180).NormalizeAngle();
                        index++;
                        continue;
                    }

                    var currangle = ((180 + lastAngle - angle) * (swirl ? -1 : 1)).NormalizeAngle();
                    if (inner == true) {
                        if (currangle > 180) {
                            CustomLevel.instance.events.Add(Utils.LevelEvent(index, LevelEventType.Twirl));
                            swirl = !swirl;
                            currangle = (360 - currangle).NormalizeAngle();
                        }
                    } else if (inner == false) {
                        if (currangle < 180) {
                            CustomLevel.instance.events.Add(Utils.LevelEvent(index, LevelEventType.Twirl));
                            swirl = !swirl;
                            currangle = (360 - currangle).NormalizeAngle();
                        }
                    }

                    var currMultiplier = currangle / 180 / multiplier;
                    var evnt = Utils.LevelEvent(index, LevelEventType.SetSpeed);
                    evnt.data["speedType"] = SpeedType.Multiplier;
                    evnt.data["bpmMultiplier"] = currMultiplier;
                    if (Math.Round(currMultiplier, 3) != 1)
                        CustomLevel.instance.events.Add(evnt);
                    multiplier = currMultiplier * multiplier;
                    UnityModManager.Logger.Log(currangle + " / " + currMultiplier);

                    lastAngle = angle;
                    index++;
                }
            }
        }
    }
}