using System.Collections;
using UnityEngine;
using System.IO;

public class ShareButton : MonoBehaviour
{
    private string shareMessage;
    [SerializeField] private GameManager gameManager;

    public void ClickShareButon()
    {
        shareMessage = "Damn I scored " + gameManager.GetScore() + " points in BOUNCE BACK BLUE! Try to beat that! https://linktr.ee/MCKerim";
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("Bounce Back Blue").SetText(shareMessage)
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}
