using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static string gameState;

    public static int totalScore; //ゲーム全般を通してのスコア
    public static int stageScore; //そのステージで獲得したスコア

    // Startより前に処理される
    private void Awake()
    {
        gameState = "playing";
        
    }
}
