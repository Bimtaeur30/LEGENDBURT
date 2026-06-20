using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoSingleton<SceneChangeManager>
{
    [SerializeField] private CanvasGroup loadingCanvasGroup;
    [SerializeField] private RectTransform loadingIcon;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float loadInterval = 1f;
    private bool isSceneMoved = false;
    Sequence seq;

    protected override void Awake()
    {
        base.Awake();
        seq = DOTween.Sequence();
    }

    private void Update()
    {
        loadingIcon.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 50f);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (isSceneMoved)
        {
            loadingCanvasGroup.alpha = 1f;

            seq?.Kill();
            seq = DOTween.Sequence();

            seq.AppendInterval(loadInterval / 2).SetUpdate(true);
            seq.Append(loadingCanvasGroup.DOFade(0f, fadeTime)).SetUpdate(true);
        }
        else
        {
            loadingCanvasGroup.alpha = 0f;
        }
    }

    public void ChangeScene(string sceneName)
    {
        seq.Kill();
        seq = DOTween.Sequence();
        seq.Append(loadingCanvasGroup.DOFade(1f, fadeTime)).SetUpdate(true);
        seq.AppendInterval(loadInterval / 2).SetUpdate(true).OnComplete(() =>
        {
            isSceneMoved = true;
            SceneManager.LoadScene(sceneName);
        });
    }
}
