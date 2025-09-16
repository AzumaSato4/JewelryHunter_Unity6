using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject mainImage;    //アナウンスをする画像
    public GameObject buttonPanel;  //ボタンをグループ化しているパネル

    public GameObject retryButton;  //リトライボタン
    public GameObject nextButton;   //ネクストボタン

    public Sprite gameClearSprite;  //ゲームクリアの絵
    public Sprite gameOverSprite;   //ゲームオーバーの絵

    TimeController timeCnt;         //TimeController.csの参照
    public GameObject timeText;     //TimeBarを取得

    public GameObject scoreText;    //スコアテキスト

    AudioSource audio;
    SoundController soundController; //自作したスクリプト

    public GameObject onPadButton;   //バーチャルパッドを表示非表示を切り替えるボタン
    public GameObject padButton;
    public bool isPad = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeCnt = GetComponent<TimeController>();

        buttonPanel.SetActive(false); //存在を非表示

        //時間差でメソッドを発動
        Invoke("InactiveImage", 1.0f);

        UpdateScore();

        //AudioSourceとSoundControllerの取得
        audio = GetComponent<AudioSource>();
        soundController = GetComponent<SoundController>();

        //padButtonの初期設定
        onPadButton.SetActive(false);
        padButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == "gameclear")
        {
            buttonPanel.SetActive(true); //ボタンパネルの復活
            mainImage.SetActive(true); //メイン画像の復活
            //メイン画像オブジェクトのImageコンポーネントが所持している変数spriteに"ステージクリア"の絵を代入
            mainImage.GetComponent<Image>().sprite = gameClearSprite;
            //リトライボタンオブジェクトのButtonコンポーネントが所持している変数interactalbeを無効（ボタン機能を無効）
            retryButton.GetComponent<Button>().interactable = false;
            onPadButton.SetActive(false);
            padButton.SetActive(false);
            isPad = false;

            //ステージクリアによってステージスコアが確定したので
            //トータルスコアに加算
            GameManager.totalScore += GameManager.stageScore;
            GameManager.stageScore = 0; //次に備えてステージスコアはリセット

            timeCnt.isTimeOver = true; //タイムカウント停止
            float times = timeCnt.displayTime;
            if (timeCnt.isCountDown) //カウントダウン
            {
                //残時間をそのままタイムボーナスとしてトータルスコアに加算
                GameManager.totalScore += (int)times * 10;
            }
            else //カウントアップ
            {
                float gameTime = timeCnt.gameTime; //基準時間の取得
                GameManager.totalScore -= (int)(gameTime - times) * 10;
            }

            UpdateScore(); //UIに最終的な数字を反映

            //サウンドストップ
            audio.Stop();
            audio.PlayOneShot(soundController.bgm_GameClear);

            GameManager.gameState = "gameend";
        }

        else if (GameManager.gameState == "gameover")
        {
            buttonPanel.SetActive(true); //ボタンパネルの復活
            mainImage.SetActive(true); //メイン画像の復活
            //メイン画像オブジェクトのImageコンポーネントが所持している変数spriteに"ゲームオーバー"の絵を代入
            mainImage.GetComponent<Image>().sprite = gameOverSprite;
            //ネクストボタンオブジェクトのButtonコンポーネントが所持している変数interactalbeを無効（ボタン機能を無効）
            nextButton.GetComponent<Button>().interactable = false;
            onPadButton.SetActive(false);
            padButton.SetActive(false);
            isPad = false;

            timeCnt.isTimeOver = true;

            //サウンドストップ
            audio.Stop();
            audio.PlayOneShot(soundController.bgm_GameOver);

            GameManager.gameState = "gameend";
        }
        else if (GameManager.gameState == "playing")
        {
            //いったんdisplayTimeの数字を変数timesに渡す
            float times = timeCnt.displayTime;
            timeText.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(times).ToString();

            if (timeCnt.isCountDown)
            {
                if (timeCnt.displayTime <= 0)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GameOver();
                    GameManager.gameState = "gameover";
                }
            }
            else
            {
                if (timeCnt.displayTime >= timeCnt.gameTime)
                {
                    GameManager.gameState = "gameover";
                }
            }

            //スコアもリアルタイムに更新
            UpdateScore();

            onPadButton.SetActive(true);
        }
    }

    //メイン画像を非表示するためだけのメソッド
    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    void UpdateScore()
    {
        int score = GameManager.stageScore + GameManager.totalScore;
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void SetPadButton()
    {
        isPad = !isPad;
        padButton.SetActive(isPad);
    }
}
