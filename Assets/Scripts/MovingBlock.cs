using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.Timeline;

public class MovingBlock : MonoBehaviour
{
    [Header("移動距離/時間/間隔")]
    public float moveX = 0.0f; //x移動距離
    public float moveY = 0.0f; //y移動距離
    public float time = 0.0f; //時間
    public float wait = 0.0f; //停止時間

    [Header("乗ってから動くフラグ")]
    public bool isMoveWhenOn = false; //乗った時に動くフラグ

    bool isCanMove = true; //動くフラグ
    Vector3 startPos; //初期位置
    Vector3 endPos; //移動位置
    bool isReverse = false; //反転フラグ
    float movep = 0; //移動補完値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position; //初期位置
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY); //移動位置
        if (isMoveWhenOn)
        {
            //乗った時に動くので最初は動かさない
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos); //移動距離
            float ds = distance / time; //1秒の移動距離
            float df = ds * Time.deltaTime; //1フレームの移動距離
            movep += df / distance; //移動補完値
            if (isReverse)
            {
                transform.position = Vector2.Lerp(endPos, startPos, movep); //逆移動
            }
            else
            {
                transform.position = Vector2.Lerp(startPos, endPos, movep); //正移動
            }
            if (movep >= 1.0f)
            {
                movep = 0.0f; //移動補完値リセット
                isReverse = !isReverse; //移動を逆転
                isCanMove = false; //移動停止
                if (isMoveWhenOn == false)
                {
                    //乗った時に動くフラグOFF
                    Invoke("Move", wait); //移動フラグを立てる遅延実行
                }
            }

        }
    }

    //移動フラグを立てる
    public void Move()
    {
        isCanMove = true;
    }

    //移動フラグをおろす
    public void Stop()
    {
        isCanMove = false;
    }

    //接触開始
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子にする
            collision.transform.SetParent(transform);
            if (isMoveWhenOn)
                {
                //乗った時に動くフラグON
                isCanMove = true; //移動フラグを立てる
            }
        }
    }

    //接触終了
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //接触したのがプレイヤーなら移動床の子から外す
            collision.transform.SetParent(null);
        }
    }

    //移動範囲表示
    private void OnDrawGizmosSelected()
    {
        Vector2 fromPos;
        if (startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else
        {
            fromPos = startPos;
        }
        //移動線
        Gizmos.DrawLine(fromPos, new Vector2(fromPos.x + moveX, fromPos.y + moveY));
        //スプリントのサイズ
        Vector2 size = GetComponent<SpriteRenderer>().size;
        //初期位置
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));
        //移動位置
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
