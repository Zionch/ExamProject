using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenBagpackEventArgs : GameEventArgs
{
    public static int EventId => typeof(OpenBagpackEventArgs).GetHashCode();
    public override int Id => EventId;

    public static OpenBagpackEventArgs Create() {
        return new OpenBagpackEventArgs();
    }
}

public class UseItemEventArgs : GameEventArgs
{
    public static int EventId => typeof(UseItemEventArgs).GetHashCode();
    public override int Id => EventId;
    public int ItemId { get; private set; }

    public UseItemEventArgs(int itemId) {
        ItemId = itemId;
    }

    public static UseItemEventArgs Create(int itemId) {
        return new UseItemEventArgs(itemId);
    }
}

public class UIBagpack : UIView
{
    [SerializeField]
    private GameObject itemPrefab;

    private GameObject inventory, info;
    private Button closeButton;

    private Transform bagpackContent;

    public class UIItem
    {
        private Button mainButton;
        private RawImage mainImg;
        private Text amountText;

        public readonly Texture MainTexture;
        public readonly string Name, Description;

        public int Id { get; private set; }
        public int Count { get; private set; }

        public readonly Transform Root;

        public UIItem(Transform root, int id, int count, System.Action<UIItem> OnTap) {
            this.Root = root;
            this.Id = id;
            this.Count = count;
            mainImg = root.GetChild("MainImg").GetComponent<RawImage>();
            mainButton = mainImg.GetComponent<Button>();
            amountText = root.GetChild("AmountText").GetComponent<Text>();

            var dt = DataTableManager.Instance.GetDataTable<DRItem>();
            var d = dt.GetData(id);

            MainTexture = ResourceManager.Instance.LoadAsset(AssetUtility.GetTexture("Item", d.IconFileName)) as Texture;
            Name = d.Name;
            Description = d.Description;

            mainImg.texture = MainTexture;
            amountText.text = count.ToString();
            mainButton.onClick.AddListener(() => OnTap(this));
        }

        public void Increase() {
            Count++;
            amountText.text = Count.ToString();
        }

        public void Decrease() {
            Count--;
            amountText.text = Count.ToString();
        }
    }

    private List<UIItem> displayedItems;

    private class UIInfo
    {
        private EventTrigger useEventTrigger;
        private Text titleText, descriptionText;
        private Transform infoRoot, uiItemRoot;

        private UIItem item;

        public int CurrentId { get; private set; }

        private bool isPressedUse;
        private float pressTime;
        private const float TriggerUseThreshold = 2;
        private System.Action<UIItem> onTapUse;

        public UIInfo(Transform root) {
            this.infoRoot = root;

            titleText = root.GetChild("Title").GetComponent<Text>();
            descriptionText = root.GetChild("Description").GetComponent<Text>();
            uiItemRoot = root.GetChild("UIItem");
            useEventTrigger = root.GetChild("UseButton").GetComponent<EventTrigger>();
        }

        public void Update() {
            if (!isPressedUse) {
                pressTime = 0;
            } else {
                pressTime += Time.deltaTime;
                if(pressTime >= TriggerUseThreshold) {
                    pressTime = 0;
                    onTapUse?.Invoke(item);
                }
            }
        }

        public void Open(UIItem uiItem, System.Action<UIItem> onTapUse) {
            this.onTapUse = onTapUse;
            CurrentId = uiItem.Id;
            this.item = new UIItem(uiItemRoot, uiItem.Id, uiItem.Count, delegate { });

            descriptionText.text = item.Description;
            titleText.text = item.Name;

            useEventTrigger.triggers.Clear();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((e) => onTapUse(uiItem));
            useEventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((e) => isPressedUse = true);
            useEventTrigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((e) => isPressedUse = false);
            useEventTrigger.triggers.Add(entry);

            infoRoot.gameObject.SetActive(true);
        }

        public void Close() {
            CurrentId = -1;
            isPressedUse = false;
            infoRoot.gameObject.SetActive(false);
        }
    }
    private UIInfo uiInfo;

    public void Init() {
        displayedItems = new List<UIItem>();

        inventory = gameObject.GetChild("Inventory");
        info = inventory.GetChild("Info");
        uiInfo = new UIInfo(info.transform);
        closeButton = inventory.GetChild("Close").GetComponent<Button>();
        closeButton.onClick.AddListener(() => Close());

        bagpackContent = inventory.GetChild("ListView").transform;

        Close();
    }

    private void Update() {
        uiInfo.Update();
    }

    public override void Open() {
        inventory.SetActive(true);
        uiInfo.Close();
        EventManager.Instance.Fire(this, OpenBagpackEventArgs.Create());
    }

    public override void Close() {
        inventory.SetActive(false);
    }

    public void DisplayItems(Item[] bagpackItems) {
        bagpackContent.DestroyAllChildren();
        displayedItems.Clear();

        for (int i = 0; i < bagpackItems.Length; i++) {
            var g = Instantiate(itemPrefab, bagpackContent);

            displayedItems.Add(new UIItem(g.transform, bagpackItems[i].Id, bagpackItems[i].Count, OnTapItem));
        }
    }

    private void OnTapItem(UIItem item) {
        DisplayItemInfo(item);
    }

    private void DisplayItemInfo(UIItem item) {
        uiInfo.Open(item, OnTapUse);
    }

    private void OnTapUse(UIItem item) {
        EventManager.Instance.Fire(this, UseItemEventArgs.Create(item.Id));
    }

    public void DecreaseItem(int itemId) {
        var item = displayedItems.Find(x => x.Id == itemId);
        if (item == null) return;//this may happen if not opened
        item.Decrease();

        if(itemId == uiInfo.CurrentId) {
            uiInfo.Open(item, OnTapUse);
        }
    }

    public void IncreaseItem(int itemId) {
        var item = displayedItems.Find(x => x.Id == itemId);
        if (item == null) return; 
        item.Increase();

        if (itemId == uiInfo.CurrentId) {
            uiInfo.Open(item, OnTapUse);
        }
    }

    public void RemoveItem(int itemId) {
        var item = displayedItems.Find(x => x.Id == itemId);
        if (item == null) return; 
        Destroy(item.Root.gameObject);

        if(uiInfo.CurrentId == itemId) {
            uiInfo.Close();
        }
    }
}