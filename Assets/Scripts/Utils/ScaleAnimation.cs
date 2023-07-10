using UnityEngine;
using DG.Tweening;

public class ScaleAnimation : MonoBehaviour
{
    public float minScale = 0.8f; // 最小缩放比例
    public float maxScale = 1.2f; // 最大放大比例
    public float scaleDuration = 0.4f; // 每次缩放的持续时间

    private Sequence scaleSequence; // 缩放动画序列

    private void Start()
    {
        // 创建缩放动画序列
        scaleSequence = DOTween.Sequence();

        // 设置初始缩放比例
        transform.localScale = Vector3.one * minScale;

        // 添加缩放动画到序列
        scaleSequence.Append(transform.DOScale(maxScale, scaleDuration));
        scaleSequence.Append(transform.DOScale(minScale, scaleDuration));

        // 循环播放缩放动画
        scaleSequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        // 销毁脚本时停止动画序列并释放资源
        scaleSequence.Kill();
        scaleSequence = null;
    }
}