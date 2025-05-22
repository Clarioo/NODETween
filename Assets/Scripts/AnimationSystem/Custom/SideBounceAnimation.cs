namespace AnimationSystem.Custom
{
    using System;
    using UnityEngine;

    public class SideBounceAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float bounceSpeedVertical = 1f;
        [SerializeField] private float bounceSpeedHorizontal = 1f;
        [SerializeField] private Vector2 bounceRangeVertical = new Vector2(0, 0.1f);
        [SerializeField] private Vector2 bounceRangeHorizontal = new Vector2(0.1f, 0.1f);

        private void Update()
        {
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Lerp(bounceRangeHorizontal.x, bounceRangeHorizontal.y, Mathf.Cos(Time.time * bounceSpeedHorizontal) * 0.5f + 0.5f),
                Mathf.Lerp(bounceRangeVertical.x, bounceRangeVertical.y, Mathf.Sin(Time.time * bounceSpeedVertical) * 0.5f + 0.5f));
        }
    }
}