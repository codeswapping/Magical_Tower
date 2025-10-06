using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TMPro.TextMeshPro popupText;
    public float animationTime;
    public AnimationCurve animateCurve;
    private float progress;
    private float rate;
    private Vector3 startPos, endPos; 
    private void Awake()
    {
        rate = 1f / animationTime;
    }
    public void ShowPopup(string text)
    {
        progress = 0;
        startPos = transform.position;
        transform.LookAt(Camera.main.transform);
        endPos = startPos + Vector3.up * 3;
        popupText.text = text;
        gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        GameManager.OnUpdate += OnUpdate;
    }
    private void OnDisable()
    {
        GameManager.OnUpdate -= OnUpdate;
    }

    private void OnUpdate()
    {
        progress += Time.deltaTime * rate;
        transform.position = Vector3.Lerp(startPos, endPos, animateCurve.Evaluate(progress));
        if(progress >= 1f)
        {
            gameObject.SetActive(false);
        }
    }
}
