using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region UI Panels Struct
    [System.Serializable]
    private struct UIPanels
    {
        public GameObject MenuPanel;
        public GameObject GamePanel;
        public GameObject GameOverPanel;
        public GameObject PausePanel;
        public GameObject WinPanel;
        public GameObject ShopPanel;
        public GameObject LoadPanel;
    }
    #endregion

    #region UI Texts Struct
    [System.Serializable]
    private struct UITexts
    {
        public Text levelText;
        public Text levelTextOnScene;
        public Text coinTextOnMenu;
        public Text coinTextOnWin;
        public Text chickensSavedText;

    }
    #endregion

    #region Singleton
    public static UIManager Instance = null;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    [Header("-ITEM SLOTS-")]
    [SerializeField] private Transform itemSlotParent;
    [SerializeField] private ItemSlot itemSlotsPrefab;
    private ItemSlot[] itemSlots;

    [Header("-BULLET IMAGES-")]
    [SerializeField] private Image bulletImage;
    [SerializeField] private Image bulletImageInShop;
    [SerializeField] private Button btnRandomUnlock;
    [SerializeField] private Image[] bulletSlots;

    [Header("-DISTANCE BAR-")]
    [SerializeField] private Slider distanceBar;

    [Header("-PANELS-")]
    [SerializeField] private UIPanels uiPanels;

    [Header("-TEXTS-")]
    [SerializeField] private UITexts uiTexts;

    void Start()
    {
        CreateItemSlots();
        UpdateCoin();
        ActivateGamePanel(false);
        Time.timeScale = 1;
    }

    #region Distance Bar
    public void SetDistanceBarValue(float maxValue)
    {
        distanceBar.maxValue = maxValue;
    }
    public void UpdateDistanceBar(float value)
    {
        distanceBar.value = value;
    }
    #endregion

    #region Bullet
    public void SetBullets(int value)
    {
        foreach (Image i in bulletSlots)
            i.gameObject.SetActive(false);


        for (int i = 0; i < value; i++)
        {
            bulletSlots[i].gameObject.SetActive(true);
            bulletSlots[i].color = Data.Get.BulletData.GetCurrentBullet().Color;
        }
    }
    public void UpdateBullets(int index)
    {
        bulletSlots[index].color = Color.gray;
    }
    #endregion

    #region Panels
    public void ActivateMenuPanel(bool activate)
    {
        uiPanels.MenuPanel.SetActive(activate);
    }
    public void ActivateGamePanel(bool activate)
    {
        uiPanels.GamePanel.SetActive(activate);
    }
    public void ActivateGameOverPanel(bool activate)
    {
        uiPanels.GameOverPanel.SetActive(activate);
        ActivateGamePanel(false);
    }
    public void ActivatePausePanel(bool activate)
    {
        uiPanels.PausePanel.SetActive(activate);
        GameManager.Instance.GameStatus = activate ? GameStatus.Null : GameStatus.Normal;
        Time.timeScale = activate ? 0 : 1;
    }
    public void ActivateWinPanel(bool activate)
    {
        uiPanels.WinPanel.SetActive(activate);
        ActivateGamePanel(false);
    }
    public void ActivateShopPanel(bool activate)
    {
        if (activate)
        {
            uiPanels.ShopPanel.SetActive(true);
            UpdateShop();
        }
        else
            LevelManager.Instance.LoadCurrentLevel();
    }
    public void ActivateLoadPanel(bool activate)
    {
        uiPanels.LoadPanel.SetActive(activate);
    }
    #endregion

    #region Texts
    public void UpdateLevelTexts(int level)
    {
        uiTexts.levelText.text = "[ LEVEL " + level + " ]";
        uiTexts.levelTextOnScene.text = level.ToString();
    }
    public void UpdateCoin()
    {
        int value = Data.Get.Coin;
        uiTexts.coinTextOnMenu.text = value == 0 ? "0" : value.ToString("#,###");
    }
    public void UpdateWinTexts(int earnedCoins, int chickensSaved)
    {
        uiTexts.chickensSavedText.text = "x" + chickensSaved.ToString();
        uiTexts.coinTextOnWin.text = "0";
        StartCoroutine(UpdateCoinText(earnedCoins));
    }
    private IEnumerator UpdateCoinText(int amount)
    {
        yield return new WaitForSeconds(.6f);
        int i = 0;
        while (i < amount)
        {
            i += 5;
            if (i > amount) i = amount;
            uiTexts.coinTextOnWin.text = i.ToString();
            yield return new WaitForSeconds(.05f);
        }
    }

    #endregion

    #region Shop
    public void SelectBullet(int id)
    {
        Data.Get.BulletData.SelectedBullet = id;
        AudioManager.Instance.PlaySound(AudioType.Click);
        UpdateBullet(id);
    }
    public void UpdateBullet(int id)
    {
        print("selected bullet : " + id);
        bulletImage.sprite = Data.Get.BulletData.bullets[id].Icon;
        bulletImageInShop.sprite = bulletImage.sprite;
    }

    private void CreateItemSlots()
    {
        int max = Data.Get.BulletData.bullets.Length;
        itemSlots = new ItemSlot[max];
        for (int i = 0; i < max; i++)
        {
            ItemSlot itemSlot = ItemSlot.Instantiate(itemSlotsPrefab, itemSlotParent);
            itemSlots[i] = itemSlot;
        }
        UpdateShop();
    }

    public void UpdateShop()
    {
        IsUnlockedAll();

        for (int i = 0; i < itemSlots.Length; i++)
        {
            BulletData.Bullet bullet = Data.Get.BulletData.bullets[i];

            if (i == 0)
                bullet.IsPurchased = true;

            itemSlots[i].Set(bullet);
        }
    }
    public void RandomUnlock()
    {
        if (Data.Get.Coin >= 1000)
        {
            Data.Get.Coin -= 1000;
            StartCoroutine(Data.Get.BulletData.RandomUnlockBullet());
            AudioManager.Instance.PlaySound(AudioType.Purchase);
            IsUnlockedAll();
        }
    }
    private void IsUnlockedAll()
    {
        btnRandomUnlock.interactable = !Data.Get.BulletData.UnlockedAll();
    }

    #endregion

}

