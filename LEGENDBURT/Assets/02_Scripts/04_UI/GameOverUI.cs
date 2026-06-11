using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EventChannelSO playerChannel;
    [Header("Title")]
    [SerializeField] private TextMeshProUGUI OnSuccessTitleTxt;
    [SerializeField] private TextMeshProUGUI OnFailTitleTxt;
    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI bestSpeedTxt;
    [SerializeField] private TextMeshProUGUI driftCountTxt;
    [SerializeField] private TextMeshProUGUI earnedItemCountTxt;
    [SerializeField] private TextMeshProUGUI fartCountTxt;
    [Header("Effect")]
    [SerializeField] private CanvasGroup[] hideCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;
    [SerializeField] private TopBottomBarLabel tbbl;
    private void Awake()
    {
        playerChannel.AddListener<OnGameOverEvent>(HandleOnGameOverEvent);
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;
        
    }
    private void OnDestroy()
    {
        playerChannel.RemoveListener<OnGameOverEvent>(HandleOnGameOverEvent);
    }

    private void HandleOnGameOverEvent(OnGameOverEvent @event)
    {
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.blocksRaycasts = true;

        OnSuccessTitleTxt.gameObject.SetActive(@event.IsGameSuccess);
        OnFailTitleTxt.gameObject.SetActive(!@event.IsGameSuccess);

        foreach (CanvasGroup cg in hideCanvasGroup)
        {
            cg.interactable = false;
            cg.DOFade(0f, 1f);
        }
        gameOverCanvasGroup.interactable = true;
        gameOverCanvasGroup.DOFade(1f, 1f);

        bestSpeedTxt.text = "ÃÖ´ë¼Óµµ: " + GameOverManager.Instance.BestSpeed.ToString();
        fartCountTxt.text = "¹æ±Í ²ï È½¼ö: " + GameOverManager.Instance.FartCount.ToString();
        driftCountTxt.text = "µå¸®ÇÁÆ® È½¼ö: " + GameOverManager.Instance.DriftCount.ToString();
        earnedItemCountTxt.text = "È¹µæÇÑ À¯¹° °³¼ö: " + GameOverManager.Instance.EarnedItemCount.ToString();
        tbbl.Show();
    }
}
