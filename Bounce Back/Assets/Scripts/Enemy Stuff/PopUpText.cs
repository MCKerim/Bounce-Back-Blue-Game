using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUptext;
    [SerializeField] private LeanTweenType alphaFade;
    [SerializeField] private LeanTweenType moveY;
    [SerializeField] private LeanTweenType moveX;


    public void Serialize(string text)
    {
        popUptext.SetText(text);

        //LeanTween.moveLocalY(gameObject, 4, 0.5f);
        LeanTween.moveY(gameObject, transform.position.y + 1, 1f).setEase(moveY);
        LeanTween.moveX(gameObject, transform.position.x + 0.1f, 1f).setEase(moveX);
        LeanTween.alphaCanvas(gameObject.GetComponent<CanvasGroup>(), 0, 1f).setEase(alphaFade).setDestroyOnComplete(true);
    }
}
