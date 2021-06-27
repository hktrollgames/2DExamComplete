using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    public float speed = 5;

    public GameObject arrow;
    public float arrowDelayTime = 0.15f;

    public Transform cameraTr;
    Vector3 cameraOffset;
    public float cameraSmoothLerp = 0.1f;


    public enum Direction
    {
        NotMove,
        Up,
        Down,
        Right,
        Left,
    }
    public Direction direction = Direction.Right;


    private void Awake()
    {
        cameraOffset = transform.position - cameraTr.position;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 문제1 (20점) : Player 이동 로직을 작성하시오.
        // 이동키를 누르면 해당 방향으로 이동하는 로직 구현(방향키와 WASD 중 선택해서 구현)
        // 대각선이동은 할 수 없다.
        // 문제6 (15점) :  마지막에 누른 방향키 방향으로 이동시키시오
        var move = Move();


        // 문제2) (15점) : 캐릭터가 움직일때 카메라가 따라가는 로직을 구현하시오.
        // 로직으로 구현해야함. 플레이어의 차일드로 설정하거나 시네머신 사용금지
        // 부드럽게 이동할경우 만점
        MoveCamera();

        // 문제4) (20점) : Space키로 화살(Arrow)을 날리는 공격을 구현 하시오
        //  (화살은 플레이어의 이동방향(transform.forward)으로 날아가야 합니다)
        // 문제3) (15점) : 화살 공격시 공격 애니메이션 재생하시오.
        FireArrow();


        // 문제5) (15점) : Player 이동 방향을 바라보도록 (상하좌우)스프라이트를 설정하시오.
        UpdateSprite(move);
    }

    private void MoveCamera()
    {
        cameraTr.position = Vector3.Lerp(cameraTr.position, transform.position - cameraOffset, cameraSmoothLerp);
    }

    // 마지막에 눌렀던 방향으로 이동 시킴
    List<KeyCode> pressedKeys = new List<KeyCode>();
    private Vector2 Move()
    {
        if (Input.GetKeyDown(KeyCode.W)) pressedKeys.Add(KeyCode.W);
        if (Input.GetKeyDown(KeyCode.S)) pressedKeys.Add(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.D)) pressedKeys.Add(KeyCode.D);
        if (Input.GetKeyDown(KeyCode.A)) pressedKeys.Add(KeyCode.A);

        if (Input.GetKeyUp(KeyCode.W)) pressedKeys.RemoveAll(x => x == KeyCode.W);
        if (Input.GetKeyUp(KeyCode.S)) pressedKeys.RemoveAll(x => x == KeyCode.S);
        if (Input.GetKeyUp(KeyCode.D)) pressedKeys.RemoveAll(x => x == KeyCode.D);
        if (Input.GetKeyUp(KeyCode.A)) pressedKeys.RemoveAll(x => x == KeyCode.A);


        Direction currentDirection = Direction.NotMove;
        if (pressedKeys.Count > 0)
        {
            KeyCode lastKey = pressedKeys[pressedKeys.Count -1];
            if (lastKey == KeyCode.W) currentDirection = Direction.Up;
            if (lastKey == KeyCode.S) currentDirection = Direction.Down;
            if (lastKey == KeyCode.D) currentDirection = Direction.Right;
            if (lastKey == KeyCode.A) currentDirection = Direction.Left;
        }

        if (currentDirection != Direction.NotMove)
        {
            direction = currentDirection;
            Vector2 move = GetDirectionVector(direction);
            transform.Translate(move * speed * Time.deltaTime);
            return move;
        }

        return Vector2.zero;
    }

    private Vector2 GetDirectionVector(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.Up: return Vector2.up;
            case Direction.Down: return Vector2.down;
            case Direction.Right: return Vector2.right;
            case Direction.Left: return Vector2.left;
            default: return Vector2.zero;
        }
    }

    private void UpdateSprite(Vector2 move)
    {
        string clipName = string.Empty;
        // move의 크기가 0보다 크면 forward방향으로 이동하는 모션을 보여주자.
        // 이동하지 않으면 Idle모션을 보여주자.
        if ( move.sqrMagnitude > 0) ////이동이 있다(Walk)
        {
            clipName = "Walk";
        }
        else // 이동이 없다. Idle
        {
            clipName = "Idle";
        }
        clipName = clipName + direction;

        animator.Play(clipName);
    }

    private void FireArrow()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 화살 생성.
            StartCoroutine(FireArrowCo());
        }
    }

    private IEnumerator FireArrowCo()
    {
        var dir = GetCurrentDirection();

        // 공격 애니메이션 재생.
        animator.Play("Attack" + direction, 1, 0);
        yield return new WaitForSeconds(arrowDelayTime);
        Instantiate(arrow, transform.position, dir);
    }

    private Quaternion GetCurrentDirection()
    {
        switch(direction)
        {
            case Direction.Up: return Quaternion.Euler(new Vector3(0, 0, 0));
            case Direction.Down: return Quaternion.Euler(new Vector3(0, 0, 180));
            case Direction.Right: return Quaternion.Euler(new Vector3(0, 0, 270));
            case Direction.Left: 
            default: return Quaternion.Euler(new Vector3(0, 0, 90));
        }
    }
}
