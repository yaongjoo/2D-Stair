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

    private bool isDie = false; //false로 초기화

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

    private void Init() // 사망시 애니메이션
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
        { // 잘못된 방향으로 가면 사망
            CharDie();
            return; // 사망했다면 함수를 빠져나가게 함
        }

        if (moveCount > 5)
        {
            //계단 스폰
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    }

    private void MoveDirection()
{
    if (Input.GetKeyDown(KeyCode.RightArrow))
    {
        if (isTurn == true) // 만약에 반대쪽으로 있다면 방향을 바꿔라
        {
            CharTurn();
            isTurn = false;
        }
        oldPosition += new Vector3(0.75f, 0.5f, 0); // 한 칸 움직임
    }
    else if (Input.GetKeyDown(KeyCode.LeftArrow))
    {
        if (isTurn == false) // 만약에 반대쪽으로 있다면 방향을 바꿔라
        {
            CharTurn();
            isTurn = true;
        }
        oldPosition += new Vector3(-0.75f, 0.5f, 0); // 한 칸 움직임
    }

    transform.position = oldPosition;
    anim.SetTrigger("Move");
}

    //private void MoveDirection()
    //{
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        if (isTurn == true) //만약에 반대쪽으로 있다면 방향을 바꿔라
    //        {
    //            CharTurn();
    //            //방향이 전환된 상태를 true로 바꿈
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
