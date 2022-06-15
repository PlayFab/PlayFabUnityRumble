//--------------------------------------------------------------------------------------
// SteamInventoryManager.cs
//
// The Manager class for the Steam Inventory Manager and functionality.
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// 
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamInventoryManager : InventoryManager
{
    private SteamInventoryResult_t SteamInventoryResult;
    private SteamItemDetails_t[] SteamItemDetails;
    private Callback<SteamInventoryResultReady_t> SteamInventoryResultReady;
    private Callback<SteamInventoryFullUpdate_t> SteamInventoryFullUpdate;
    private Callback<SteamInventoryDefinitionUpdate_t> SteamInventoryDefinitionUpdate;
    private event Action OnResultReady;

    public class InventoryItemId
    {
        public static readonly SteamItemDef_t DropLuckCoin = (SteamItemDef_t)11;
        public static readonly SteamItemDef_t LuckCoin = (SteamItemDef_t)4;
    }

    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamInventoryManager();
            Instance.Initialize();
        }
    }

    ~SteamInventoryManager()
    {
        OnResultReady -= GetInventoryItemFunction;
    }

    public override void Initialize()
    {
        SteamInventoryResult = SteamInventoryResult_t.Invalid;
        SteamItemDetails = null;
        SteamInventoryResultReady = Callback<SteamInventoryResultReady_t>.Create(OnSteamInventoryResultReady);
        SteamInventoryFullUpdate = Callback<SteamInventoryFullUpdate_t>.Create(OnSteamInventoryFullUpdate);
        SteamInventoryDefinitionUpdate = Callback<SteamInventoryDefinitionUpdate_t>.Create(OnSteamInventoryDefinitionUpdate);
        OnResultReady += GetInventoryItemFunction;
        TriggerItemDrop(InventoryItemId.DropLuckCoin);
        base.Initialize();
    }

    public override void GetInventoryItems()
    {
        bool ret = SteamInventory.GetAllItems(out SteamInventoryResult);
        base.GetInventoryItems();
    }

    public override void GetInventoryItemFunction()
    {
        uint OutItemsArraySize = 0;
        bool ret = SteamInventory.GetResultItems(SteamInventoryResult, null, ref OutItemsArraySize);
        SteamItemDetails = new SteamItemDetails_t[OutItemsArraySize];
        logs = new List<string>();
        ret = SteamInventory.GetResultItems(SteamInventoryResult, SteamItemDetails, ref OutItemsArraySize);
        logs.Add("SteamInventory.GetResultItems(" + SteamInventoryResult + ", m_SteamItemDetails, ref OutItemsArraySize) - " + ret + " -- InventoryCount: " + OutItemsArraySize+"\n");
        Debug.Log(logs[0]);
        if (ret && OutItemsArraySize > 0)
        {
            for (int i = 0; i < OutItemsArraySize; i++)
            {
                string name = "";
                if (SteamItemDetails[i].m_iDefinition == InventoryItemId.LuckCoin)
                {
                    name = "Luck Coin";
                }
                else if (SteamItemDetails[i].m_iDefinition == InventoryItemId.DropLuckCoin)
                {
                    name = "DropLuckCoin";
                }
                logs.Add(string.Format("ItemId(Alone):{0} - IDefinition:{1} - Name:{2}\n", SteamItemDetails[i].m_itemId, SteamItemDetails[i].m_iDefinition, name));
                Debug.LogFormat("ItemId(Alone):{0} - IDefinition:{1} - Name:{2}\n", SteamItemDetails[i].m_itemId, SteamItemDetails[i].m_iDefinition, name);
            }
            base.GetInventoryItemFunction();
        }
        else
        {
            Debug.Log("SteamInventory.GetResultItems(" + SteamInventoryResult + ", null, ref OutItemsArraySize) - " + ret + " -- InventoryCount: " + OutItemsArraySize);
        }
    }

    public void TriggerItemDrop(SteamItemDef_t inventoryItemId)
    {
        bool ret = SteamInventory.TriggerItemDrop(out SteamInventoryResult, inventoryItemId);
        Debug.Log("SteamInventory.TriggerItemDrop: " + inventoryItemId + "-----ret: " + ret);
    }

    public void AddPromoItem(SteamItemDef_t inventoryItemId)
    {
        bool ret = SteamInventory.AddPromoItem(out SteamInventoryResult, inventoryItemId);
        Debug.Log("SteamInventory.AddPromoItem: " + inventoryItemId + "-----ret: " + ret);
    }

    public void GenerateItems(SteamItemDef_t inventoryItemId)
    {
        bool ret = SteamInventory.GenerateItems(out SteamInventoryResult, new SteamItemDef_t[] { inventoryItemId }, null, 1);
        Debug.Log("SteamInventory.GenerateItems: " + inventoryItemId + "-----ret: " + ret);
    }

    void OnSteamInventoryResultReady(SteamInventoryResultReady_t pCallback)
    {
        Debug.Log("[" + SteamInventoryResultReady_t.k_iCallback + " - SteamInventoryResultReady] - " + pCallback.m_handle + " -- " + pCallback.m_result);
        SteamInventoryResult = pCallback.m_handle;
        OnResultReady?.Invoke();
    }

    void OnSteamInventoryFullUpdate(SteamInventoryFullUpdate_t pCallback)
    {
        Debug.Log("[" + SteamInventoryFullUpdate_t.k_iCallback + " - SteamInventoryFullUpdate] - " + pCallback.m_handle);

        SteamInventoryResult = pCallback.m_handle;
    }

    void OnSteamInventoryDefinitionUpdate(SteamInventoryDefinitionUpdate_t pCallback)
    {
        Debug.Log("[" + SteamInventoryDefinitionUpdate_t.k_iCallback + " - SteamInventoryDefinitionUpdate]");
    }

    void DestroyResult()
    {
        if (SteamInventoryResult != SteamInventoryResult_t.Invalid)
        {
            SteamInventory.DestroyResult(SteamInventoryResult);
            Debug.Log("SteamInventory.DestroyResult(" + SteamInventoryResult + ")");
            SteamInventoryResult = SteamInventoryResult_t.Invalid;
        }
    }

    private void OnDisable()
    {
        DestroyResult();
    }
}
