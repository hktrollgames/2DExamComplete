using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject arrow;
    public float speed = 5;
    public Transform cameraTr;
    public Animator animator;

    Vector3 cameraOffset;
    private void Awake()
    {
        cameraOffset = transform.position - cameraTr.position;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 문제1 (20점) : Player 이동 로직을 작성하시오.
        // 이동키를 누르면 해당 방향으로 이동하는 로직 구현(방향키와 WASD 중 선택해서 구현)
        // 대각선이동은 할 수 없다. (마지막에 누른 방향키 방향으로 이동할경우 만점)
        var move = Move();

        // 문제2) (20점) : 캐릭터가 움직일때 카메라가 따라가는 로직을 구현하시오.
        // 로직으로 구현해야함. 플레이어의 차일드로 설정하거나 시네머신 사용금지
        // 부드럽게 이동할경우 만점
        MoveCamera();

        // Space키로 화살(Arrow)을 날리는 공격을 구현 하시오
        // 문제3) (20점) : 화살 공격시 공격 애니메이션 재생하시오.
        // 문제4) (20점) : (화살은 플레이어의 이동방향(transform.forward)으로 날아가야 합니다)
        FireArrow();


        // 문제5) (20점) : Player 이동 방향을 바라보도록 (상하좌우)스프라이트를 설정하시오.
        // 왼쪽은 오른쪽 이미지를 Flip해서 구현하시오.
        UpdateSprite(move);
    }

    private void MoveCamera()
    {
        cameraTr.position = transform.position - cameraOffset;
    }


    Direction direction = Direction.Down;
    private Vector2 Move()
    {
        Direction currentDirection = Direction.NotMove;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) currentDirection = Direction.Up;
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) currentDirection = Direction.Down;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) currentDirection = Direction.Right;
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) currentDirection = Direction.Left;
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

    public enum Direction
    {
        NotMove,
        Up, 
        Down,
        Right,
        Left,
    }
    public string clipName;
    private void UpdateSprite(Vector2 move)
    {
        clipName = string.Empty;
        // move 가 이동이 있으면 forward방향으로 이동하는 모션을 보여주자.
        // 없으면 Idle모션을 보여주자.
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
            var dir = GetCurrentDirection();
            Instantiate(arrow, transform.position, dir);

            // 공격 애니메이션 재생.
            animator.Play("Attack" + direction, 1, 0);
        }
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
