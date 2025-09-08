using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName; //切り替えたいシーン名を指名

    //シーンを切り替える機能を持ったメソッド作成
    public void Load()
    {
        //引数に指定した名前のシーンに切り替えてくれるメソッド呼び出し
        SceneManager.LoadScene(sceneName);
    }
}
