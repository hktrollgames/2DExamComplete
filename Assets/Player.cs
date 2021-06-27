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
        // ����1 (20��) : Player �̵� ������ �ۼ��Ͻÿ�.
        // �̵�Ű�� ������ �ش� �������� �̵��ϴ� ���� ����(����Ű�� WASD �� �����ؼ� ����)
        // �밢���̵��� �� �� ����. (�������� ���� ����Ű �������� �̵��Ұ�� ����)
        var move = Move();

        // ����2) (20��) : ĳ���Ͱ� �����϶� ī�޶� ���󰡴� ������ �����Ͻÿ�.
        // �������� �����ؾ���. �÷��̾��� ���ϵ�� �����ϰų� �ó׸ӽ� ������
        // �ε巴�� �̵��Ұ�� ����
        MoveCamera();

        // SpaceŰ�� ȭ��(Arrow)�� ������ ������ ���� �Ͻÿ�
        // ����3) (20��) : ȭ�� ���ݽ� ���� �ִϸ��̼� ����Ͻÿ�.
        // ����4) (20��) : (ȭ���� �÷��̾��� �̵�����(transform.forward)���� ���ư��� �մϴ�)
        FireArrow();


        // ����5) (20��) : Player �̵� ������ �ٶ󺸵��� (�����¿�)��������Ʈ�� �����Ͻÿ�.
        // ������ ������ �̹����� Flip�ؼ� �����Ͻÿ�.
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
        // move �� �̵��� ������ forward�������� �̵��ϴ� ����� ��������.
        // ������ Idle����� ��������.
        if ( move.sqrMagnitude > 0) ////�̵��� �ִ�(Walk)
        {
            clipName = "Walk";
        }
        else // �̵��� ����. Idle
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
            // ȭ�� ����.
            var dir = GetCurrentDirection();
            Instantiate(arrow, transform.position, dir);

            // ���� �ִϸ��̼� ���.
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
