using UnityEngine;

public class CameraController : MonoBehaviour
{

    GameObject player;
    float x, y, z; //カメラの座標を決めるための変数

    [Header("カメラの限界値")]
    public float leftLimit;
    public float rightLimit;
    public float bottomLimit;
    public float topLimit;

    [Header("カメラのスクロール設定")]
    public bool isScrollX; //横方向に強制スクロールするかのフラグ
    public float scrollSpeedX = 0.5f;
    public bool isScrollY; //縦方向に強制スクロールするかのフラグ
    public float scrollSpeedY = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Playerタグをもったゲームオブジェクトを探して、変数playerに代入
        player = GameObject.FindGameObjectWithTag("Player");
        //カメラのZ座標は初期値のままを維持したい
        z = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //いったんプレイヤーのX座標、Y座標に位置を変数に取得
        y = player.transform.position.y;
        x = player.transform.position.x;

        //もしも横の強制スクロールフラグが立っていたら
        if (isScrollX)
        {
            //前の座標に変数分だけ加算した座標
            x = transform.position.x + (scrollSpeedX * Time.deltaTime);
        }

        //もしも左右の限界までプレイヤーが移動したら
        if (x < leftLimit)
        {
            x = leftLimit;
        }
        else if (x > rightLimit)
        {
            x = rightLimit;
        }


        if (isScrollY)
        {
            y = transform.position.y + (scrollSpeedY * Time.deltaTime);
        }

        if (y < bottomLimit)
        {
            y = bottomLimit;
        }
        else if (y > topLimit)
        {
            y = topLimit;
        }


        //取り決めた各変数x, y ,zの値をカメラのポジションとする
        transform.position = new Vector3(x, y, z);

    }
}
