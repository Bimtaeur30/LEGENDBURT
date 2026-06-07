using DG.Tweening;
using System;
using UnityEngine;

public class PartsCardSelector_UI : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] private PartsDataSO[] randomPartsDataSO;
    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("UI")]
    [SerializeField] private CanvasGroup cardSelectGroup;
    [SerializeField] private PartsCard_UI parsCardPrefab;
    [SerializeField] private Transform cardParents;
    [Header("Setting")]
    [SerializeField] private int cardCount;

    private void Awake()
    {
        playerChannel.AddListener<OnCardSelectEvent>(HandleOnCardSelectEvent);
    }

    private void HandleOnCardSelectEvent(OnCardSelectEvent @event)
    {
        cardSelectGroup.DOFade(1f, 1f);
        cardSelectGroup.blocksRaycasts = true;
        GenerateCards();
    }

    private void GenerateCards()
    {
        for (int i = 0; i < cardCount; i++)
        {
            PartsDataSO randomItem = randomPartsDataSO[UnityEngine.Random.Range(0, randomPartsDataSO.Length)];
            PartsCard_UI card = Instantiate(parsCardPrefab, cardParents);
            card.Initialize(randomItem);

            card.OnCardSelected += OnCardSelectEventEnd;
        }
    }

    private void OnCardSelectEventEnd(PartsDataSO selectedPartsData)
    {
        cardSelectGroup.DOFade(0f, 1f);
        cardSelectGroup.blocksRaycasts = false;

        playerChannel.RasiseEvent(PlayerEvents.AttachPartsEvent.Init(selectedPartsData.PartPrefab, PartsJointPos.FirstSlot));
    }
}
