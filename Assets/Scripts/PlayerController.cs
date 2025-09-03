using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; //PlayerについているRigidbody2Dを扱うための変数

    float axisH; //入力の方向を記憶するための変数
    public float speed = 3.0f; //プレイヤーのスピードを調整

    public float jumpPower = 9.0f; //プレイヤーのジャンプ力
    bool goJump = false; //ジャンプフラグ（true：真on、false：偽off）

    bool onGround = false; //地面にいるかどうかの判定（地面にいる：ture、地面にいない：false）

    public LayerMask groundLayer; //地面レイヤーを指名するための変数

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Playerについているコンポーネント情報を取得
    }

    // Update is called once per frame
    void Update()
    {
        //Velocityの元となる値の取得（右なら1.0f、左なら-1.0f、なにもなければ0）
        axisH = Input.GetAxisRaw("Horizontal");

        if (axisH > 0)
        {
            //右を向く
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (axisH < 0)
        {
            //左を向く
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //GetButtonDownメソッド→引数に指定したボタンが押されたらtureを返す、押されていなければfalseを返す
        if (Input.GetButtonDown("Jump"))
        {
            Jump(); //Jumpメソッドの発動
        }

    }

    //1秒間に50回繰り返すように制御しながら行う繰り返しメソッド
    void FixedUpdate()
    {
        //地面判定をサークルキャストで行って、その結果を変数onGroundに代入
        onGround = Physics2D.CircleCast(
            transform.position, //発射位置＝プレイヤーの位置
            0.2f,               //調査する円の半径
            new Vector2(0, 1.0f), //発射方向 ※下方向
            0,                  //発射距離
            groundLayer         //対象となるレイヤー情報
            );

        //Velosityに値を代入
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);

        //ジャンプフラグが立ったら
        if (goJump == true)
        {
            //ジャンプさせる→プレイヤーを上に押し出す
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            goJump = false;
        }
    }

    //ジャンプボタンが押されたときに呼び出されるメソッド
    void Jump()
    {
        goJump = true; //ジャンプフラグをON
    }
}
