using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("�v���C���[�̔\�͒l")]
    public float speed = 3.0f; //�v���C���[�̃X�s�[�h�𒲐�
    public float jumpPower = 9.0f; //�v���C���[�̃W�����v��

    [Header("�n�ʔ���̑Ώۃ��C���[")]
    public LayerMask groundLayer; //�n�ʃ��C���[���w�����邽�߂̕ϐ�


    Rigidbody2D rbody; //Player�ɂ��Ă���Rigidbody2D���������߂̕ϐ�
    Animator animator; //Animator�R���|�[�l���g���������߂̕ϐ�

    float axisH; //���͂̕������L�����邽�߂̕ϐ�
    bool goJump = false; //�W�����v�t���O�itrue�F�^on�Afalse�F�Uoff�j

    bool onGround = false; //�n�ʂɂ��邩�ǂ����̔���i�n�ʂɂ���Fture�A�n�ʂɂ��Ȃ��Ffalse�j

    AudioSource audio;
    public AudioClip se_Jump;
    public AudioClip se_ItemGet;
    public AudioClip se_Damage;

    UIController UICon;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Player�ɂ��Ă���R���|�[�l���g�����擾
        animator = GetComponent<Animator>();

        audio = GetComponent<AudioSource>(); //AudioSource�R���|�[�l���g�̏�����

        UICon = GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {

        //�Q�[���̃X�e�[�^�X��playing�łȂ��Ȃ�
        if (GameManager.gameState != "playing")
        {
            return; //����1�t���[���������I��
        }

        //Velocity�̌��ƂȂ�l�̎擾�i�E�Ȃ�1.0f�A���Ȃ�-1.0f�A�Ȃɂ��Ȃ����0�j
        axisH = Input.GetAxisRaw("Horizontal");
        if (Gamepad.current != null)
        {
            //�o�[�`�����p�b�h���\������Ă�����axisH�̓��͌���ς���
            var stickValue = Gamepad.current.leftStick.ReadValue();
            axisH = stickValue.x;
        }


        if (axisH > 0)
        {
            //�E������
            transform.localScale = new Vector3(1, 1, 1);

        }
        else if (axisH < 0)
        {
            //��������
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //GetButtonDown���\�b�h�������Ɏw�肵���{�^���������ꂽ��ture��Ԃ��A������Ă��Ȃ����false��Ԃ�
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump(); //Jump���\�b�h�̔���
        }

    }

    //1�b�Ԃ�50��J��Ԃ��悤�ɐ��䂵�Ȃ���s���J��Ԃ����\�b�h
    void FixedUpdate()
    {
        //�Q�[���̃X�e�[�^�X��playing�łȂ��Ȃ�
        if (GameManager.gameState != "playing")
        {
            return; //����1�t���[���������I��
        }

        //�n�ʔ�����T�[�N���L���X�g�ōs���āA���̌��ʂ�ϐ�onGround�ɑ��
        onGround = Physics2D.CircleCast(
            transform.position, //���ˈʒu���v���C���[�̈ʒu
            0.2f,               //��������~�̔��a
            new Vector2(0, 1.0f), //���˕��� ��������
            0,                  //���ˋ���
            groundLayer         //�ΏۂƂȂ郌�C���[���
            );

        //Velosity�ɒl����
        rbody.linearVelocity = new Vector2(axisH * speed, rbody.linearVelocity.y);

        //�W�����v�t���O����������
        if (goJump)
        {
            //�W�����v�����遨�v���C���[����ɉ����o��
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            goJump = false;
        }

        //if (onGround) //�n�ʂ̏�ɂ���Ƃ�
        //{
        if (axisH == 0) //���E��������Ă��Ȃ�
        {
            animator.SetBool("Run", false); //Idle�A�j���ɐ؂�ւ�
        }
        else //���E��������Ă���
        {
            animator.SetBool("Run", true); //Run�A�j���ɐ؂�ւ�
        }
        //}

    }

    //�W�����v�{�^���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    void Jump()
    {
        if (onGround)
        {
            //SE��炷
            audio.PlayOneShot(se_Jump);

            goJump = true; //�W�����v�t���O��ON
            animator.SetTrigger("Jump");
        }
    }

    //isTrigger�����������Ă���Collider�ƂԂ������珈�������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Goal")
        if (collision.gameObject.CompareTag("Goal"))
        {
            GameManager.gameState = "gameclear";
            Debug.Log("�S�[���ɐڐG�����I");
            Goal();
        }

        if (collision.gameObject.CompareTag("Dead"))
        {
            //SE��炷
            audio.PlayOneShot(se_Damage);

            GameManager.gameState = "gameover";
            Debug.Log("�Q�[���I�[�o�[�I");
            GameOver();
        }

        if (collision.gameObject.CompareTag("ScoreItem"))
        {
            //SE��炷
            audio.PlayOneShot(se_ItemGet);

            GameManager.stageScore += collision.gameObject.GetComponent<ItemData>().value;
            Destroy(collision.gameObject);
        }
    }

    public void Goal()
    {
        animator.SetBool("Clear", true);
        GameStop(); //�v���C���[��Velocity���~�߂郁�\�b�h
    }

    //�Q�[���I�[�o�[���̃��\�b�h
    public void GameOver()
    {
        animator.SetBool("Dead", true);
        GameStop();

        //�����蔻��𖳌�
        GetComponent<CapsuleCollider2D>().enabled = false;

        //������ɔ�ђ��˂�����
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        //�v���C���[��3�b��ɖ���
        Destroy(gameObject, 3.0f);
    }

    void GameStop()
    {
        //���x��0�Ƀ��Z�b�g
        //rbody.linearVelocity = new Vector2(0, 0);
        rbody.linearVelocity = Vector2.zero;
    }

}
