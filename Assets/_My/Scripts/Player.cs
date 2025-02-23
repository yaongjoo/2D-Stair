using UnityEngine;

public class Player : MonoBehaviour
{

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 StartPosition;
    private Vector3 oldPosition;
    private bool isTurn = false;


    private int moveCount = 0;
    private int turnCount = 0;
    private int spawnCount = 0;

    private bool isDie = false; //false�� �ʱ�ȭ

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartPosition = transform.position;
        Init();



    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CharMove();
        }
    }

    private void Init() // ����� �ִϸ��̼�
    {
        anim.SetBool("Die", false);
        transform.position = StartPosition;
        oldPosition = StartPosition;
        moveCount = 0;
        spawnCount = 0;
        turnCount = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = isTurn == true ? false : true;

        spriteRenderer.flipX = isTurn;
    }

    public void CharMove()
    {
        if (isDie)

            return;

        moveCount++;
        MoveDirection();

        if (isFailTurn())
        { // �߸��� �������� ���� ���
            CharDie();
            return; // ����ߴٸ� �Լ��� ���������� ��
        }

        if (moveCount > 5)
        {
            //��� ����
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    }

    private void MoveDirection()
{
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
        if (isTurn == true) // ���࿡ �ݴ������� �ִٸ� ������ �ٲ��
        {
            CharTurn();
            isTurn = false;
        }
        oldPosition += new Vector3(0.75f, 0.5f, 0); // �� ĭ ������
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
        if (isTurn == false) // ���࿡ �ݴ������� �ִٸ� ������ �ٲ��
        {
            CharTurn();
            isTurn = true;
        }
        oldPosition += new Vector3(-0.75f, 0.5f, 0); // �� ĭ ������
    }

    transform.position = oldPosition;
    anim.SetTrigger("Move");
}

    //private void MoveDirection()
    //{
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        if (isTurn == true) //���࿡ �ݴ������� �ִٸ� ������ �ٲ��
    //        {
    //            CharTurn();
    //            //������ ��ȯ�� ���¸� true�� �ٲ�
    //            return;
    //        }
    //        oldPosition += new Vector3(0.75f, 0.5f, 0);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        if (isTurn == true) { 
    //            CharTurn();
    //            return;
    //    }
            
    //        oldPosition += new Vector3(-0.75f, 0.5f, 0);
    //    }

    //    transform.position = oldPosition;
    //    anim.SetTrigger("Move");
    //}

    //private void MoveDirection()
    //{
    //    if (isTurn) //Left
    //    {

    //        oldPosition += new Vector3(-0.75f, 0.5f, 0);
    //    }
    //    else //Right
    //    {

    //        oldPosition += new Vector3(0.75f, 0.5f, 0);

    //    }
    //    transform.position = oldPosition;
    //    anim.SetTrigger("Move");
    //}




    private bool isFailTurn()
    {
        bool result = false;

        if (GameManager.Instance.isTurn[turnCount] != isTurn)
        {
            result = true;
        }

        turnCount++;

        if (turnCount > GameManager.Instance.Stairs.Length - 1) //0~19 length 20
        {
            turnCount = 0;
        }
        return result;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCount);
        spawnCount++;

        if (spawnCount > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCount = 0;
        }
    }

    private void CharDie()
    {
        GameManager.Instance.GameOver();
        anim.SetBool("Die", true);
        isDie = true;
    }

    public void Restart()
    {
        if (isDie)
        {

            Init();
            GameManager.Instance.Init();
            GameManager.Instance.InitStairs();

        }
    }
}
