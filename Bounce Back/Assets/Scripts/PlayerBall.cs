using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerBallScriptableObject", order = 1)]
public class PlayerBall : ScriptableObject
{
    public string ballName;
    public Sprite sprite;
    public string description;
    public int price;
}