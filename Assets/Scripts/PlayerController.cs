using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody; //Player�ɂ��Ă���Rigidbody2D���������߂̕ϐ�

    float axisH; //���͂̕������L�����邽�߂̕ϐ�
    public float speed = 3.0f; //�v���C���[�̃X�s�[�h�𒲐�

    public float jumpPower = 9.0f; //�v���C���[�̃W�����v��
    bool goJump = false; //�W�����v�t���O�itrue�F�^on�Afalse�F�Uoff�j

    bool onGround = false; //�n�ʂɂ��邩�ǂ����̔���i�n�ʂɂ���Fture�A�n�ʂɂ��Ȃ��Ffalse�j

    public LayerMask groundLayer; //�n�ʃ��C���[���w�����邽�߂̕ϐ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>(); //Player�ɂ��Ă���R���|�[�l���g�����擾
    }

    // Update is called once per frame
    void Update()
    {
        //Velocity�̌��ƂȂ�l�̎擾�i�E�Ȃ�1.0f�A���Ȃ�-1.0f�A�Ȃɂ��Ȃ����0�j
        axisH = Input.GetAxisRaw("Horizontal");

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
        if (Input.GetButtonDown("Jump"))
        {
            Jump(); //Jump���\�b�h�̔���
        }

    }

    //1�b�Ԃ�50��J��Ԃ��悤�ɐ��䂵�Ȃ���s���J��Ԃ����\�b�h
    void FixedUpdate()
    {
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
        if (goJump == true)
        {
            //�W�����v�����遨�v���C���[����ɉ����o��
            rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            goJump = false;
        }
    }

    //�W�����v�{�^���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    void Jump()
    {
        goJump = true; //�W�����v�t���O��ON
    }
}
