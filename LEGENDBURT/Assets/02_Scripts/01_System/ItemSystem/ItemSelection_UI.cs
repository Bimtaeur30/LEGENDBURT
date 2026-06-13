using DG.Tweening;
using UnityEngine;

public class ItemSelection_UI : MonoBehaviour
{
    [SerializeField] private CanvasGroup itemSelectionCanvasGroup;
    [SerializeField] private EventChannelSO playerChannel;
    [SerializeField] private ItemCard_UI itemCardPrefab;
    [SerializeField] private RectTransform itemCardParent;
    [SerializeField] private ArtifactSO[] randomArtifacts;

    private void Awake()
    {
        playerChannel.AddListener<OnItemSelectEvent>(HandleOnItemSelectEvent);
        DeactivateSelection();
    }

    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnItemSelectEvent>(HandleOnItemSelectEvent);
    }

    private void HandleOnItemSelectEvent(OnItemSelectEvent @event)
    {
        ActivateSelection();
    }

    private void ActivateSelection()
    {
        Time.timeScale = 0f;

        DestroyCards();
        itemSelectionCanvasGroup.DOFade(1f, 1f).SetUpdate(true);
        itemSelectionCanvasGroup.interactable = true;
        itemSelectionCanvasGroup.blocksRaycasts = true;

        for (int i = 0; i < 3; i++)
        {
            ArtifactSO randArtifact = randomArtifacts[Random.Range(0, randomArtifacts.Length - 1)];
            ItemCard_UI card = Instantiate(itemCardPrefab, itemCardParent);
            card.Initialize(randArtifact);
            card.OnItemCardSelectEvent += HandleOnItemCardSelectEvent;
        }
    }

    private void DeactivateSelection()
    {
        itemSelectionCanvasGroup.DOFade(0f, 1f).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
        });
        itemSelectionCanvasGroup.interactable = false;
        itemSelectionCanvasGroup.blocksRaycasts = false;
    }

    private void DestroyCards()
    {
        for (int i = 0; i < itemCardParent.childCount; i++)
        {
            Destroy(itemCardParent.GetChild(i).gameObject);
        }
    }

    private void HandleOnItemCardSelectEvent(ArtifactSO sO)
    {
        DeactivateSelection();
        playerChannel.RasiseEvent(PlayerEvents.EquipItemEvent.Init(sO));
    }
}
