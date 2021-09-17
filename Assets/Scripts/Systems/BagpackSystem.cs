using System;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public readonly int Id;
    public int Count;

    public Item(int id, int count) {
        Id = id;
        Count = count;
    }
}

public class BagpackSystem
{
    private UIBagpack uiBagpack;

    private List<Item> bagpackItems;
    
    public void Init() {
        uiBagpack = GameObject.FindObjectOfType<UIBagpack>();
        bagpackItems = new List<Item>();

        DataTableManager.Instance.LoadDataTable("Item");

        EventManager.Instance.Subscribe(OpenBagpackEventArgs.EventId, OnOpenBagpack);
        EventManager.Instance.Subscribe(UseItemEventArgs.EventId, OnUseItem);
        EventManager.Instance.Subscribe(GetItemEventArgs.EventId, OnGetItem);
        LoadInitSettings();
    }

    private void OnGetItem(object sender, GameEventArgs e) {
        GetItemEventArgs args = (GetItemEventArgs)e;

        var item = bagpackItems.Find(x => x.Id == args.ItemId);
        if (item == null) {
            bagpackItems.Add(new Item(args.Id, 1));
        } else {
            item.Count++;
            uiBagpack.IncreaseItem(item.Id);
        }
    }

    private void OnUseItem(object sender, GameEventArgs e) {
        UseItemEventArgs args = (UseItemEventArgs)e;

        var item = bagpackItems.Find(x => x.Id == args.ItemId);
        item.Count--;
        if(item.Count <= 0) {
            bagpackItems.Remove(item);
            uiBagpack.RemoveItem(item.Id);
        } else {
            uiBagpack.DecreaseItem(item.Id);
        }
    }

    private void OnOpenBagpack(object sender, GameEventArgs e) {
        uiBagpack.DisplayItems(bagpackItems.ToArray());
    }

    private void LoadInitSettings() {
        var dt = DataTableManager.Instance.GetDataTable<DRItem>();
        var items = dt.GetAllDataRows();

        foreach (var item in items) {
            if(item.DefaultAmount > 0) {
                bagpackItems.Add(new Item(item.Id, item.DefaultAmount));
            }
        }
    }
}