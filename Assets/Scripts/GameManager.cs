using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static string gameState;

    public static int totalScore; //�Q�[���S�ʂ�ʂ��ẴX�R�A
    public static int stageScore; //���̃X�e�[�W�Ŋl�������X�R�A

    // Start���O�ɏ��������
    private void Awake()
    {
        gameState = "playing";
        
    }
}
