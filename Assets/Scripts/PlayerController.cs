using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの能力値")]
    public float speed = 3.0f; //プレイヤーのスピードを調整
    public float jumpPower = 9.0f; //プレイヤーのジャンプ力

    [Header("地面判定の対象レイヤー")]
    public LayerMask groundLayer; //地面レイヤーを指名するための変数


    Rigidbody2D rbody; //PlayerについているRigidbody2Dを扱うための変数
    Animator animator; //Animatorコンポーネントを扱うための変数

    float axisH; //入力の方向を記憶するための変数
    bool goJump = false; //ジャンプフラグ（true：真on、false：偽off）

    bool onGround = false; //地面にいるかどうかの判定（地面にいる：ture、地面にいない：false）

    AudioSource audio;
    public AudioClip se_Jump;
    public AudioClip se_ItemGet;
    public AudioClip se_Damage;

    UIController UICon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Playerについているコンポーネント情報を取得
        animator = GetComponent<Animator>();

        audio = GetComponent<AudioSource>(); //AudioSourceコンポーネントの情報を代入

        UICon = GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {

        //ゲームのステータスがplayingでないなら
        if (GameManager.gameState != "playing")
        {
            return; //その1フレームを強制終了
        }

        //Velocityの元となる値の取得（右なら1.0f、左なら-1.0f、なにもなければ0）
        axisH = Input.GetAxisRaw("Horizontal");
        if (Gamepad.current != null)
        {
            //バーチャルパッドが表示されていたらaxisHの入力元を変える
            var stickValue = Gamepad.current.leftStick.ReadValue();
            axisH = stickValue.x;
        }


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
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump(); //Jumpメソッドの発動
        }

    }

    //1秒間に50回繰り返すように制御しながら行う繰り返しメソッド
    void FixedUpdate()
    {
        //ゲームのステータスがplayingでないなら
        if (GameManager.gameState != "playing")
        {
            return; //その1フレームを強制終了
        }

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
        if (goJump)
        {
            //ジャンプさせる→プレイヤーを上に押し出す
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            goJump = false;
        }

        //if (onGround) //地面の上にいるとき
        //{
        if (axisH == 0) //左右が押されていない
        {
            animator.SetBool("Run", false); //Idleアニメに切り替え
        }
        else //左右が押されている
        {
            animator.SetBool("Run", true); //Runアニメに切り替え
        }
        //}

    }

    //ジャンプボタンが押されたときに呼び出されるメソッド
    void Jump()
    {
        if (onGround)
        {
            //SEを鳴らす
            audio.PlayOneShot(se_Jump);

            goJump = true; //ジャンプフラグをON
            animator.SetTrigger("Jump");
        }
    }

    //isTrigger特性をもっているColliderとぶつかったら処理される
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Goal")
        if (collision.gameObject.CompareTag("Goal"))
        {
            GameManager.gameState = "gameclear";
            Debug.Log("ゴールに接触した！");
            Goal();
        }

        if (collision.gameObject.CompareTag("Dead"))
        {
            //SEを鳴らす
            audio.PlayOneShot(se_Damage);

            GameManager.gameState = "gameover";
            Debug.Log("ゲームオーバー！");
            GameOver();
        }

        if (collision.gameObject.CompareTag("ScoreItem"))
        {
            //SEを鳴らす
            audio.PlayOneShot(se_ItemGet);

            GameManager.stageScore += collision.gameObject.GetComponent<ItemData>().value;
            Destroy(collision.gameObject);
        }
    }

    public void Goal()
    {
        animator.SetBool("Clear", true);
        GameStop(); //プレイヤーのVelocityを止めるメソッド
    }

    //ゲームオーバー時のメソッド
    public void GameOver()
    {
        animator.SetBool("Dead", true);
        GameStop();

        //当たり判定を無効
        GetComponent<CapsuleCollider2D>().enabled = false;

        //少し上に飛び跳ねさせる
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        //プレイヤーを3秒後に抹消
        Destroy(gameObject, 3.0f);
    }

    void GameStop()
    {
        //速度を0にリセット
        //rbody.linearVelocity = new Vector2(0, 0);
        rbody.linearVelocity = Vector2.zero;
    }

}
