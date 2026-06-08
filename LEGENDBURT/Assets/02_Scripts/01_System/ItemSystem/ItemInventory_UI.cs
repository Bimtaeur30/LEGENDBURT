using System;
using UnityEngine;

public class ItemInventory_UI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("UI")]
    [SerializeField] private ItemSlot_UI itemSlotPrefab;
    [SerializeField] private Transform itemSlotParent;

    private void Awake()
    {
        playerChannel.AddListener<EquipItemEvent>(HandleEquipItemEvent);
    }

    private void HandleEquipItemEvent(EquipItemEvent @event)
    {
        ItemSlot_UI itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
        itemSlot.Initialize(@event.artifactSO);
    }
}
