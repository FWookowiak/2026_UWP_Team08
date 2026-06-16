using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUIBuilder : EditorWindow
{
    [MenuItem("Tools/Generate Tower Upgrade UI")]
    public static void GenerateUI()
    {
        // Find or create Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create Presenter if missing
        TowerUpgradePresenter presenter = FindObjectOfType<TowerUpgradePresenter>();
        if (presenter == null)
        {
            GameObject presenterObj = new GameObject("TowerUpgradeManager");
            presenter = presenterObj.AddComponent<TowerUpgradePresenter>();
        }

        // Check if panel already exists
        Transform existingPanel = canvas.transform.Find("TowerUpgradePanel");
        if (existingPanel != null)
        {
            Debug.LogWarning("TowerUpgradePanel already exists!");
            return;
        }

        // Create Panel
        GameObject panelObj = new GameObject("TowerUpgradePanel");
        panelObj.transform.SetParent(canvas.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0);
        panelRect.anchorMax = new Vector2(0.5f, 0);
        panelRect.pivot = new Vector2(0.5f, 0);
        panelRect.anchoredPosition = new Vector2(0, 50);
        panelRect.sizeDelta = new Vector2(600, 200);
        
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

        // Attach View
        TowerUpgradeView view = panelObj.AddComponent<TowerUpgradeView>();
        
        // --- Texts ---
        TextMeshProUGUI towerNameText = CreateText(panelObj.transform, "TowerNameText", "Tower Name", new Vector2(0, 160), new Vector2(580, 40));
        towerNameText.alignment = TextAlignmentOptions.Center;
        towerNameText.fontSize = 24;
        
        TextMeshProUGUI statsText = CreateText(panelObj.transform, "StatsText", "Stats...", new Vector2(0, 120), new Vector2(580, 40));
        statsText.alignment = TextAlignmentOptions.Center;
        statsText.fontSize = 18;

        // --- Upgrade Buttons ---
        GameObject upgradeBtnObj = CreateButton(panelObj.transform, "UpgradeBtn1", "Upgrade 1\n$100", new Vector2(-200, 40), new Vector2(150, 60));
        Button upgradeBtn1 = upgradeBtnObj.GetComponent<Button>();
        TextMeshProUGUI upgradeLbl1 = upgradeBtnObj.GetComponentInChildren<TextMeshProUGUI>();

        // --- Strategy Buttons ---
        // Nearest, Strongest, Weakest, First
        string[] stratNames = { "Nearest", "Strongest", "Weakest", "First" };
        string[] stratBtnNames = { "StrategyBtn_Nearest", "StrategyBtn_Strongest", "StrategyBtn_Weakest", "StrategyBtn_First" };
        Button[] stratBtns = new Button[4];
        
        for(int i = 0; i < 4; i++)
        {
            GameObject stratObj = CreateButton(panelObj.transform, stratBtnNames[i], stratNames[i], new Vector2(-200 + i * 110, -30), new Vector2(100, 40));
            stratBtns[i] = stratObj.GetComponent<Button>();
            stratBtns[i].GetComponentInChildren<TextMeshProUGUI>().fontSize = 14;
        }

        // --- Sell Button ---
        GameObject sellBtnObj = CreateButton(panelObj.transform, "SellBtn", "Sell\n+$50", new Vector2(200, 40), new Vector2(150, 60));
        Button sellBtn = sellBtnObj.GetComponent<Button>();
        TextMeshProUGUI sellLbl = sellBtnObj.GetComponentInChildren<TextMeshProUGUI>();

        // Link View to properties via SerializedObject to bypass private fields
        SerializedObject viewSo = new SerializedObject(view);
        viewSo.FindProperty("upgradePanel").objectReferenceValue = panelObj;
        viewSo.FindProperty("towerNameText").objectReferenceValue = towerNameText;
        viewSo.FindProperty("statsText").objectReferenceValue = statsText;
        
        // Arrays
        SerializedProperty upgradeBtnsProp = viewSo.FindProperty("upgradeButtons");
        upgradeBtnsProp.arraySize = 1;
        upgradeBtnsProp.GetArrayElementAtIndex(0).objectReferenceValue = upgradeBtn1;

        SerializedProperty upgradeLblsProp = viewSo.FindProperty("upgradeLabels");
        upgradeLblsProp.arraySize = 1;
        upgradeLblsProp.GetArrayElementAtIndex(0).objectReferenceValue = upgradeLbl1;

        SerializedProperty strategyBtnsProp = viewSo.FindProperty("strategyButtons");
        strategyBtnsProp.arraySize = 4;
        for(int i=0; i<4; i++) strategyBtnsProp.GetArrayElementAtIndex(i).objectReferenceValue = stratBtns[i];

        viewSo.FindProperty("sellButton").objectReferenceValue = sellBtn;
        viewSo.FindProperty("sellLabel").objectReferenceValue = sellLbl;
        
        viewSo.ApplyModifiedProperties();

        // Link Presenter
        SerializedObject presSo = new SerializedObject(presenter);
        presSo.FindProperty("upgradeView").objectReferenceValue = view;

        // Try to find TowerUpgradeData
        string[] guids = AssetDatabase.FindAssets("t:TowerUpgradeData");
        if (guids.Length > 0)
        {
            SerializedProperty availableUpgradesProp = presSo.FindProperty("availableUpgrades");
            availableUpgradesProp.arraySize = guids.Length;
            for(int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                TowerUpgradeData data = AssetDatabase.LoadAssetAtPath<TowerUpgradeData>(path);
                availableUpgradesProp.GetArrayElementAtIndex(i).objectReferenceValue = data;
            }
        }
        else
        {
            // Create a dummy one
            TowerUpgradeData dummyData = ScriptableObject.CreateInstance<TowerUpgradeData>();
            dummyData.upgradeName = "Basic Upgrade";
            dummyData.cost = 100;
            dummyData.rangeBonus = 1f;
            dummyData.fireRateBonus = 0.5f;
            dummyData.damageBonus = 5f;
            dummyData.maxLevel = 3;
            
            if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects"))
                AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
            if (!AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Towers"))
                AssetDatabase.CreateFolder("Assets/ScriptableObjects", "Towers");
                
            AssetDatabase.CreateAsset(dummyData, "Assets/ScriptableObjects/Towers/BasicUpgrade.asset");
            AssetDatabase.SaveAssets();

            SerializedProperty availableUpgradesProp = presSo.FindProperty("availableUpgrades");
            availableUpgradesProp.arraySize = 1;
            availableUpgradesProp.GetArrayElementAtIndex(0).objectReferenceValue = dummyData;
        }

        presSo.ApplyModifiedProperties();

        // Hide it by default
        panelObj.SetActive(false);

        Debug.Log("Successfully generated Tower Upgrade UI!");
    }

    private static TextMeshProUGUI CreateText(Transform parent, string name, string text, Vector2 pos, Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = Color.white;
        return tmp;
    }

    private static GameObject CreateButton(Transform parent, string name, string text, Vector2 pos, Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;

        Image img = obj.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        Button btn = obj.AddComponent<Button>();

        TextMeshProUGUI tmp = CreateText(obj.transform, "Text", text, Vector2.zero, size);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;
        tmp.fontSize = 18;

        return obj;
    }
}
