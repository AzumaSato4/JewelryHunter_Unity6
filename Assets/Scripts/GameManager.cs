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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
