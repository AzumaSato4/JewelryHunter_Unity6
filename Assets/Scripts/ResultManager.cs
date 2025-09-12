using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //リザルト画面のScoreTextオブジェクトが持つ
        //TextMeshPro(UGUI)のtext欄に
        //GameManagerのstatic変数であるtotalScoreを代入
        //※ただしstring型に型変更が必要
        scoreText.GetComponent<TextMeshProUGUI>().text = GameManager.totalScore.ToString();
    }
}
