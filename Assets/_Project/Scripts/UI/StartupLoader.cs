// StartupLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartupLoader : MonoBehaviour
{
    [Header("要旋转的 Logo 物体（空物体 or Canvas）")]
    public Transform logoToRotate;

    [Header("Logo 淡出组件（需 CanvasGroup）")]
    public CanvasGroup logoCanvasGroup;

    [Header("目标场景名称")]
    public string sceneToLoad = "01_Intro.unity";

    [Header("旋转速度（度/秒）")]
    public float rotateSpeed = 60f;

    [Header("等待时间（秒）")]
    public float waitBeforeFade = 1.5f;

    [Header("淡出时间（秒）")]
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(StartupSequence());
    }

    void Update()
    {
        if (logoToRotate != null)
        {
            logoToRotate.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        }
    }

    IEnumerator StartupSequence()
    {
        // 等待一段时间再开始淡出
        yield return new WaitForSeconds(waitBeforeFade);

        // 开始 Logo 渐隐
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            if (logoCanvasGroup != null)
                logoCanvasGroup.alpha = alpha;
            yield return null;
        }

        // 场景异步加载
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
