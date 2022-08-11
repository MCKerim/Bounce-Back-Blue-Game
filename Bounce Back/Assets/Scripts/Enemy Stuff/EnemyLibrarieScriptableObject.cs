using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyLibrarieScriptableObject", order = 2)]
public class EnemyLibrarieScriptableObject : ScriptableObject
{
    public string enemyName;
    public string enemyNameInEnemyLibrarie;
    public string enemyDiscription;
    public Sprite enemySprite;
    public Color enemyColor;
}
