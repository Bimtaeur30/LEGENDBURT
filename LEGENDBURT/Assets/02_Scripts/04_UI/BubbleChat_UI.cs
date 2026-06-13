using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BubbleChat_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chatTxt;
    [SerializeField] private float lifeTime = 3f;
    private CanvasGroup cg;
    private RectTransform rect;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect.transform.localPosition = new Vector3(0, -2, 0);
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    public void Initialize(string message)
    {
        chatTxt.text = message;
        cg.DOFade(1f, 0.3f);
        rect.DOAnchorPosY(0, 0.3f);
        StartCoroutine(LifeTime());
    }

    public void DestroyChat()
    {
        Destroy(gameObject);
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyChat();
    }
}
