using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    //扱う曲達
    public AudioClip bgm_Title;
    public AudioClip bgm_Stage;
    public AudioClip bgm_Result;
    public AudioClip bgm_GameClear;
    public AudioClip bgm_GameOver;

    AudioSource audio; //CanvasについているAudioSourceの情報

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>(); //AudioSourceの情報取得
        //現在シーン情報の取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Title")
        {
            PlayBGM(bgm_Title);
        }
        else if (currentSceneName == "Result")
        {
            PlayBGM(bgm_Result);
        }
        else
        {
            PlayBGM(bgm_Stage);
        }
    }

    //引数に指定した曲を鳴らすメソッド
    void PlayBGM(AudioClip clip)
    {
        audio.clip = clip;
        audio.loop = true; //BGMはループする
        audio.Play(); //再生する
    }

}
