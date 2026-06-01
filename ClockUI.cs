using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClockMod
{
    public static class ClockUI
    {
        public static GameObject Panel { get; private set; }
        public static Text ClockText { get; private set; }

        public static void Build(Transform parentRoot)
        {
            if (Panel != null) return; 

            Panel = new GameObject("ClockUI");
            Panel.transform.SetParent(parentRoot, false);

            var panelRect = Panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0f);
            panelRect.anchorMax = new Vector2(0.5f, 0f);
            panelRect.pivot = new Vector2(0.5f, 1f);

            var bgImage = Panel.AddComponent<Image>();
            bgImage.color = new Color(0.05f, 0.05f, 0.05f, 0.95f);

            CreateBorder("TopTrim", new Vector2(0, 1), new Vector2(1, 1));
            CreateBorder("BottomTrim", new Vector2(0, 0), new Vector2(1, 0));

            CreateRivet("TopLeftRivet", new Vector2(0, 1));
            CreateRivet("TopRightRivet", new Vector2(1, 1));
            CreateRivet("BottomLeftRivet", new Vector2(0, 0));
            CreateRivet("BottomRightRivet", new Vector2(1, 0));

            BuildTextElement();
            ApplyConfigs();
        }

        private static void BuildTextElement()
        {
            var textObj = new GameObject("ClockText");
            textObj.transform.SetParent(Panel.transform, false);

            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(5, 0);
            textRect.offsetMax = new Vector2(-5, 0);

            ClockText = textObj.AddComponent<Text>();
            ClockText.alignment = TextAnchor.MiddleCenter;
            ClockText.color = Color.white;
            ClockText.lineSpacing = 0.9f;
            ClockText.text = "...";

            var averia = Resources.FindObjectsOfTypeAll<Font>().FirstOrDefault(f => f.name == "AveriaSerifLibre-Bold");
            if (averia != null) ClockText.font = averia;

            var outline = textObj.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(1, -1);
        }

        private static void CreateBorder(string name, Vector2 anchorMin, Vector2 anchorMax)
        {
            var borderObj = new GameObject(name);
            borderObj.transform.SetParent(Panel.transform, false);

            var rect = borderObj.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.sizeDelta = new Vector2(0, 2);
            rect.anchoredPosition = Vector2.zero;

            var img = borderObj.AddComponent<Image>();
            img.color = new Color(0.85f, 0.65f, 0.2f, 1f);
        }

        private static void CreateRivet(string name, Vector2 anchorPosition)
        {
            var rivetObj = new GameObject(name);
            rivetObj.transform.SetParent(Panel.transform, false);

            var rect = rivetObj.AddComponent<RectTransform>();
            rect.anchorMin = anchorPosition;
            rect.anchorMax = anchorPosition;
            rect.sizeDelta = new Vector2(4, 4);
            rect.anchoredPosition = Vector2.zero;

            var img = rivetObj.AddComponent<Image>();
            img.color = new Color(0.6f, 0.6f, 0.6f, 1f);
        }

        public static void ApplyConfigs()
        {
            if (Panel == null || ClockText == null) return;

            var panelRect = Panel.GetComponent<RectTransform>();
            panelRect.anchoredPosition = new Vector2(ClockPlugin.ConfigPosX.Value, ClockPlugin.ConfigPosY.Value);
            panelRect.sizeDelta = new Vector2(ClockPlugin.ConfigWidth.Value, ClockPlugin.ConfigHeight.Value);

            ClockText.fontSize = ClockPlugin.ConfigFontSize.Value;
        }
    }
}