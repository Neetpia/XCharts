/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace XCharts
{
    public class XChartsEditor : Editor
    {
        private static Transform GetParent()
        {
            GameObject selectObj = Selection.activeGameObject;
            if (selectObj == null)
            {
                var canvas = GameObject.FindObjectOfType<Canvas>();
                if (canvas != null) return canvas.transform;
                else
                {
                    var canvasObject = new GameObject();
                    canvasObject.name = "Canvas";
                    canvas = canvasObject.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasObject.AddComponent<CanvasScaler>();
                    canvasObject.AddComponent<GraphicRaycaster>();
                    var eventSystem = new GameObject();
                    eventSystem.name = "EventSystem";
                    eventSystem.AddComponent<EventSystem>();
                    eventSystem.AddComponent<StandaloneInputModule>();
                    return canvas.transform;
                }
            }
            else
            {
                return selectObj.transform;
            }
        }

        private static string GetName(Transform parent, string name)
        {
            if (parent.Find(name) == null) return name;
            for (int i = 1; i <= 10; i++)
            {
                var newName = string.Format("{0} ({1})", name, i);
                if (parent.Find(newName) == null)
                {
                    return newName;
                }
            }
            return name;
        }

        public static T AddChart<T>(string chartName) where T : BaseChart
        {
            var parent = GetParent();
            if (parent == null) return null;
            var chart = new GameObject();
            chart.name = GetName(parent, chartName);
            var t = chart.AddComponent<T>();
            chart.transform.SetParent(parent);
            chart.transform.localScale = Vector3.one;
            chart.transform.localPosition = Vector3.zero;
            var rect = chart.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            Selection.activeGameObject = chart;
            EditorUtility.SetDirty(chart);
            return t;
        }

        [MenuItem("XCharts/EmptyChart", priority = 43)]
        [MenuItem("GameObject/XCharts/EmptyChart", priority = 43)]
        public static void AddBaseChart()
        {
            var chart = AddChart<BaseChart>("EmptyChart");
            chart.GetChartComponent<Title>().text = "EmptyChart";
        }

        [MenuItem("XCharts/LineChart", priority = 44)]
        [MenuItem("GameObject/XCharts/LineChart", priority = 44)]
        public static void AddLineChart()
        {
            AddChart<LineChart>("LineChart");
        }

        [MenuItem("XCharts/BarChart", priority = 45)]
        [MenuItem("GameObject/XCharts/BarChart", priority = 45)]
        public static void AddBarChart()
        {
            AddChart<BarChart>("BarChart");
        }

        [MenuItem("XCharts/PieChart", priority = 46)]
        [MenuItem("GameObject/XCharts/PieChart", priority = 46)]
        public static void AddPieChart()
        {
            AddChart<PieChart>("PieChart");
        }

        [MenuItem("XCharts/RadarChart", priority = 47)]
        [MenuItem("GameObject/XCharts/RadarChart", priority = 47)]
        public static void AddRadarChart()
        {
            AddChart<RadarChart>("RadarChart");
        }

        [MenuItem("XCharts/ScatterChart", priority = 48)]
        [MenuItem("GameObject/XCharts/ScatterChart", priority = 48)]
        public static void AddScatterChart()
        {
            AddChart<ScatterChart>("ScatterChart");
        }

        [MenuItem("XCharts/HeatmapChart", priority = 49)]
        [MenuItem("GameObject/XCharts/HeatmapChart", priority = 49)]
        public static void AddHeatmapChart()
        {
            AddChart<HeatmapChart>("HeatmapChart");
        }

        [MenuItem("XCharts/RingChart", priority = 51)]
        [MenuItem("GameObject/XCharts/RingChart", priority = 51)]
        public static void AddRingChart()
        {
            AddChart<RingChart>("RingChart");
        }

        [MenuItem("XCharts/LiquidChart", priority = 53)]
        [MenuItem("GameObject/XCharts/LiquidChart", priority = 53)]
        public static void AddLiquidChart()
        {
            AddChart<LiquidChart>("LiquidChart");
        }

        [MenuItem("XCharts/CandlestickChart", priority = 54)]
        [MenuItem("GameObject/XCharts/CandlestickChart", priority = 54)]
        public static void CandlestickChart()
        {
            AddChart<CandlestickChart>("CandlestickChart");
        }

        [MenuItem("XCharts/ParallelChart", priority = 55)]
        [MenuItem("GameObject/XCharts/ParallelChart", priority = 55)]
        public static void ParallelChart()
        {
            AddChart<ParallelChart>("ParallelChart");
        }

        [MenuItem("XCharts/Themes Reload")]
        public static void ReloadTheme()
        {
            XCThemeMgr.ReloadThemeList();
        }

        [MenuItem("XCharts/TextMeshPro Enable")]
        public static void EnableTextMeshPro()
        {
            if (!XChartsMgr.IsExistTMPAssembly())
            {
                Debug.LogError("TextMeshPro is not in the project, please import TextMeshPro package first.");
                return;
            }
            XChartsMgr.EnableTextMeshPro();
            XChartsMgr.ModifyTMPRefence();
        }

        [MenuItem("XCharts/TextMeshPro Disable")]
        public static void DisableTextMeshPro()
        {
            XChartsMgr.ModifyTMPRefence(true);
            XChartsMgr.DisableTextMeshPro();
        }

        [MenuItem("XCharts/Theme/Export Default Theme")]
        public static void ExportDefaultTheme()
        {
            var profile = ScriptableObject.CreateInstance<Theme>();
            profile.name = "Default";
            profile.themeName = "Default";
            profile.themeType = ThemeType.Default;

            var themeName = XCSettings.THEME_ASSET_NAME_PREFIX + profile.themeName;
            var path = XCSettings.THEME_ASSET_FOLDER + "/" + themeName + ".asset";
            AssetDatabase.CreateAsset(profile, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}