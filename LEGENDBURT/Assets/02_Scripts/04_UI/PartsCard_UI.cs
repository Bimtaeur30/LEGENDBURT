using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartsCard_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image partsImage;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descriptionTxt;
    [Header("Setting")]
    [SerializeField] float OnPointerEnterScale = 1.1f;
    [SerializeField] float OnPointerExitScale = 1f;
    [SerializeField] float AnimationDuration = 0.5f;

    public Action<PartsDataSO> OnCardSelected;
    private PartsDataSO myData;
    public void Initialize(PartsDataSO data)
    {
        myData = data;
        partsImage.sprite = data.PartsIcon;
        titleTxt.text = data.PartsName;
        descriptionTxt.text = data.PartsDescription;

        button.onClick.AddListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        OnCardSelected.Invoke(myData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(OnPointerEnterScale, AnimationDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(OnPointerExitScale, AnimationDuration);
    }
}
