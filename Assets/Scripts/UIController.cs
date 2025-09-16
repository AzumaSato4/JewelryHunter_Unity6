using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject mainImage;    //�A�i�E���X������摜
    public GameObject buttonPanel;  //�{�^�����O���[�v�����Ă���p�l��

    public GameObject retryButton;  //���g���C�{�^��
    public GameObject nextButton;   //�l�N�X�g�{�^��

    public Sprite gameClearSprite;  //�Q�[���N���A�̊G
    public Sprite gameOverSprite;   //�Q�[���I�[�o�[�̊G

    TimeController timeCnt;         //TimeController.cs�̎Q��
    public GameObject timeText;     //TimeBar���擾

    public GameObject scoreText;    //�X�R�A�e�L�X�g

    AudioSource audio;
    SoundController soundController; //���삵���X�N���v�g

    public GameObject onPadButton;   //�o�[�`�����p�b�h��\����\����؂�ւ���{�^��
    public GameObject padButton;
    public bool isPad = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeCnt = GetComponent<TimeController>();

        buttonPanel.SetActive(false); //���݂��\��

        //���ԍ��Ń��\�b�h�𔭓�
        Invoke("InactiveImage", 1.0f);

        UpdateScore();

        //AudioSource��SoundController�̎擾
        audio = GetComponent<AudioSource>();
        soundController = GetComponent<SoundController>();

        //padButton�̏����ݒ�
        onPadButton.SetActive(false);
        padButton.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState == "gameclear")
        {
            buttonPanel.SetActive(true); //�{�^���p�l���̕���
            mainImage.SetActive(true); //���C���摜�̕���
            //���C���摜�I�u�W�F�N�g��Image�R���|�[�l���g���������Ă���ϐ�sprite��"�X�e�[�W�N���A"�̊G����
            mainImage.GetComponent<Image>().sprite = gameClearSprite;
            //���g���C�{�^���I�u�W�F�N�g��Button�R���|�[�l���g���������Ă���ϐ�interactalbe�𖳌��i�{�^���@�\�𖳌��j
            retryButton.GetComponent<Button>().interactable = false;
            onPadButton.SetActive(false);
            padButton.SetActive(false);
            isPad = false;

            //�X�e�[�W�N���A�ɂ���ăX�e�[�W�X�R�A���m�肵���̂�
            //�g�[�^���X�R�A�ɉ��Z
            GameManager.totalScore += GameManager.stageScore;
            GameManager.stageScore = 0; //���ɔ����ăX�e�[�W�X�R�A�̓��Z�b�g

            timeCnt.isTimeOver = true; //�^�C���J�E���g��~
            float times = timeCnt.displayTime;
            if (timeCnt.isCountDown) //�J�E���g�_�E��
            {
                //�c���Ԃ����̂܂܃^�C���{�[�i�X�Ƃ��ăg�[�^���X�R�A�ɉ��Z
                GameManager.totalScore += (int)times * 10;
            }
            else //�J�E���g�A�b�v
            {
                float gameTime = timeCnt.gameTime; //����Ԃ̎擾
                GameManager.totalScore -= (int)(gameTime - times) * 10;
            }

            UpdateScore(); //UI�ɍŏI�I�Ȑ����𔽉f

            //�T�E���h�X�g�b�v
            audio.Stop();
            audio.PlayOneShot(soundController.bgm_GameClear);

            GameManager.gameState = "gameend";
        }

        else if (GameManager.gameState == "gameover")
        {
            buttonPanel.SetActive(true); //�{�^���p�l���̕���
            mainImage.SetActive(true); //���C���摜�̕���
            //���C���摜�I�u�W�F�N�g��Image�R���|�[�l���g���������Ă���ϐ�sprite��"�Q�[���I�[�o�["�̊G����
            mainImage.GetComponent<Image>().sprite = gameOverSprite;
            //�l�N�X�g�{�^���I�u�W�F�N�g��Button�R���|�[�l���g���������Ă���ϐ�interactalbe�𖳌��i�{�^���@�\�𖳌��j
            nextButton.GetComponent<Button>().interactable = false;
            onPadButton.SetActive(false);
            padButton.SetActive(false);
            isPad = false;

            timeCnt.isTimeOver = true;

            //�T�E���h�X�g�b�v
            audio.Stop();
            audio.PlayOneShot(soundController.bgm_GameOver);

            GameManager.gameState = "gameend";
        }
        else if (GameManager.gameState == "playing")
        {
            //��������displayTime�̐�����ϐ�times�ɓn��
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

            //�X�R�A�����A���^�C���ɍX�V
            UpdateScore();

            onPadButton.SetActive(true);
        }
    }

    //���C���摜���\�����邽�߂����̃��\�b�h
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
