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
        // ����1 (20��) : Player �̵� ������ �ۼ��Ͻÿ�.
        // �̵�Ű�� ������ �ش� �������� �̵��ϴ� ���� ����(����Ű�� WASD �� �����ؼ� ����)
        // �밢���̵��� �� �� ����.
        // ����6 (15��) :  �������� ���� ����Ű �������� �̵���Ű�ÿ�
        var move = Move();


        // ����2) (15��) : ĳ���Ͱ� �����϶� ī�޶� ���󰡴� ������ �����Ͻÿ�.
        // �������� �����ؾ���. �÷��̾��� ���ϵ�� �����ϰų� �ó׸ӽ� ������
        // �ε巴�� �̵��Ұ�� ����
        MoveCamera();

        // ����4) (20��) : SpaceŰ�� ȭ��(Arrow)�� ������ ������ ���� �Ͻÿ�
        //  (ȭ���� �÷��̾��� �̵�����(transform.forward)���� ���ư��� �մϴ�)
        // ����3) (15��) : ȭ�� ���ݽ� ���� �ִϸ��̼� ����Ͻÿ�.
        FireArrow();


        // ����5) (15��) : Player �̵� ������ �ٶ󺸵��� (�����¿�)��������Ʈ�� �����Ͻÿ�.
        UpdateSprite(move);
    }

    private void MoveCamera()
    {
        cameraTr.position = Vector3.Lerp(cameraTr.position, transform.position - cameraOffset, cameraSmoothLerp);
    }

    // �������� ������ �������� �̵� ��Ŵ
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
        // move�� ũ�Ⱑ 0���� ũ�� forward�������� �̵��ϴ� ����� ��������.
        // �̵����� ������ Idle����� ��������.
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
            StartCoroutine(FireArrowCo());
        }
    }

    private IEnumerator FireArrowCo()
    {
        var dir = GetCurrentDirection();

        // ���� �ִϸ��̼� ���.
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
