using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;
using UnityEngine.Splines.ExtrusionShapes;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class RPM : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private int MaxSlot = 10;
    [SerializeField] private int AnchorSlot = 7;
    [SerializeField] private EventChannelSO PlayerChannel;

    [Header("RPM")]
    [SerializeField] private int MaxChargeTime = 10;
    [SerializeField] private int MinChargeTime = 5;
    [SerializeField] private HorizontalLayoutGroup NumberSlotGroup;
    [SerializeField] private RPM_NumberSlot_Pref NumberSlot_Pref;
    [SerializeField] private Slider ChargeSlider;
    [SerializeField] private TextMeshProUGUI ComboTxt;

    private readonly List<RPM_NumberSlot_Pref> numberSlots = new();
    private Coroutine chargeCoroutine;
    private int currentCombo = 0;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;
        if (NumberSlotGroup == null || NumberSlot_Pref == null) return;

        MaxSlot = Mathf.Max(1, MaxSlot);
        MinChargeTime = Mathf.Max(0, MinChargeTime);
        MaxChargeTime = Mathf.Max(MinChargeTime, MaxChargeTime);

        EditorApplication.delayCall -= RefreshEditorSlots;
        EditorApplication.delayCall += RefreshEditorSlots;
    }

    private void RefreshEditorSlots()
    {
        if (this == null) return;
        if (Application.isPlaying) return;
        if (NumberSlotGroup == null || NumberSlot_Pref == null) return;

        Transform groupTransform = NumberSlotGroup.transform;

        for (int i = groupTransform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(groupTransform.GetChild(i).gameObject);
        }

        for (int i = 0; i < MaxSlot; i++)
        {
            RPM_NumberSlot_Pref slot =
                Instantiate(NumberSlot_Pref.gameObject, groupTransform)
                .GetComponent<RPM_NumberSlot_Pref>();

            slot.Activate((i + 1).ToString(), false);
        }
    }
#endif

    private void Start()
    {
        InitializeSlots();

        ChargeSlider.minValue = 0f;
        ChargeSlider.maxValue = MaxSlot;
        ChargeSlider.value = 0f;

        chargeCoroutine = StartCoroutine(ChargeCoroutine());
    }

    private void OnDisable()
    {
        if (chargeCoroutine != null)
        {
            StopCoroutine(chargeCoroutine);
            chargeCoroutine = null;
        }
    }

    private void InitializeSlots()
    {
        numberSlots.Clear();

        if (NumberSlotGroup == null) return;

        Transform groupTransform = NumberSlotGroup.transform;

        for (int i = 0; i < groupTransform.childCount; i++)
        {
            RPM_NumberSlot_Pref slot = groupTransform.GetChild(i).GetComponent<RPM_NumberSlot_Pref>();

            if (slot == null) continue;

            slot.Activate((i + 1).ToString(), false);
            numberSlots.Add(slot);
        }
    }

    private IEnumerator ChargeCoroutine()
    {
        while (true)
        {
            RemoveDestroyedSlots();

            if (numberSlots.Count <= 0)
                yield break;

            float chargeTime = Random.Range((float)MinChargeTime, MaxChargeTime);
            int targetIndex = Random.Range(0, numberSlots.Count);

            RPM_NumberSlot_Pref targetSlot = numberSlots[targetIndex];

            if (targetSlot == null)
                continue;

            targetSlot.Activate((targetIndex + 1).ToString(), true);

            float curTime = 0f;
            bool isPressed = false;

            while (curTime < chargeTime)
            {
                if (ChargeSlider == null)
                    yield break;

                curTime += Time.deltaTime;

                float normalizedTime = Mathf.Clamp01(curTime / chargeTime);
                ChargeSlider.value = Mathf.Lerp(0f, MaxSlot, normalizedTime);

                // 슬라이더가 가리키는 슬롯 하이라이트
                int highlightIndex = Mathf.Clamp(
                    Mathf.FloorToInt(ChargeSlider.value),
                    0,
                    numberSlots.Count - 1
                );
                for (int i = 0; i < numberSlots.Count; i++)
                    numberSlots[i]?.OnHighlight(i == highlightIndex);

                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                {
                    isPressed = true;
                    CheckResult(targetIndex);
                    break;
                }

                yield return null;
            }

            // 차징 완료 또는 입력 후 하이라이트 전체 해제
            for (int i = 0; i < numberSlots.Count; i++)
                numberSlots[i]?.OnHighlight(false);

            if (!isPressed && ChargeSlider != null)
            {
                ChargeSlider.value = MaxSlot;
            }

            yield return new WaitForSeconds(1f);

            if (targetSlot != null)
            {
                targetSlot.Activate((targetIndex + 1).ToString(), false);
            }

            if (ChargeSlider != null)
            {
                ChargeSlider.value = 0f;
            }
        }
    }

    private void RemoveDestroyedSlots()
    {
        for (int i = numberSlots.Count - 1; i >= 0; i--)
        {
            if (numberSlots[i] == null)
                numberSlots.RemoveAt(i);
        }
    }

    private void CheckResult(int targetIndex)
    {
        if (ChargeSlider == null) return;

        int currentSlotIndex = Mathf.Clamp(
            Mathf.FloorToInt(ChargeSlider.value),
            0,
            numberSlots.Count - 1
        );

        if (currentSlotIndex == targetIndex)
        {
            Debug.Log($"성공! 현재 슬롯: {currentSlotIndex + 1} / 목표 슬롯: {targetIndex + 1}");
            currentCombo++;
            PlayerChannel.RasiseEvent(PlayerEvents.ActiveBurtEvent);
        }
        else
        {
            Debug.Log($"실패! 현재 슬롯: {currentSlotIndex + 1} / 목표 슬롯: {targetIndex + 1}");
            currentCombo = 0;
        }

        ComboTxt.text = currentCombo.ToString();
        Sequence seq = DOTween.Sequence();
        seq.Append(ComboTxt.transform.DOScale(0.8f, 0.2f));
        seq.Append(ComboTxt.transform.DOScale(1.2f, 0.1f));
        seq.Append(ComboTxt.transform.DOScale(1f, 0.2f));
    }
}