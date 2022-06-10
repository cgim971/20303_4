using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController _characterController;

    [Header("Movement Property")]
    Vector3 vecMoveDirection = Vector3.zero;
    public float _currentSpeed = 5f;
    public float _walkSpeed = 5f;
    public float _rush = 45f;
    bool _isRush = false;
    public float _jumpPower = 8f;
    public float _gravity = 20f;
    float _vecGravity = 0;
    float h;
    float v;
    Vector3 vecNowVelocity;
    public float rotateBodySpd = 2.0f;
    public float rotateMoveSpd = 2.0f;
    // ĳ���� �̵� �ӵ� ������
    public float moveChageSpd = 0.1f;

    [Header("Animation Property")]
    AnimationType _animationType = AnimationType.NONE;
    Animator _animator;
    bool _isAttacking = false;
    Vector3 forward;

    SkinnedMeshRenderer _skinnedMeshRenderer;

    enum AnimationType
    {
        NONE,
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DEATH
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _skinnedMeshRenderer = transform.Find("DummyMesh").GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (_characterController == null) return;

        Move();
        SetGravity();

        if (Input.GetMouseButtonDown(0))
        {
            _animationType = AnimationType.ATTACK;
        }

        PlayAnimation();
        CheckMonster();
        vecDirectionChangeBody();
    }

    void PlayAnimation()
    {
        if (_isAttacking) return;

        switch (_animationType)
        {
            case AnimationType.IDLE:
                _animator.SetBool("isMoving", false);
                break;
            case AnimationType.WALK:
            case AnimationType.RUN:
                _animator.SetFloat("H", h);
                _animator.SetFloat("V", v);
                _animator.SetBool("isMoving", true);
                break;
            case AnimationType.ATTACK:
                _animator.SetTrigger("Attack");
                _isAttacking = true;
                break;
            case AnimationType.DEATH:
                _animator.SetTrigger("Death");
                break;
        }
    }

    void Move()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        // animation Set
        _animationType = (h == 0 && v == 0) ? AnimationType.IDLE : AnimationType.WALK;

        Vector3 targetDirection = h * right + v * forward;
        targetDirection.Normalize();

        vecMoveDirection = Vector3.RotateTowards(vecMoveDirection, targetDirection, rotateMoveSpd * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        vecMoveDirection = vecMoveDirection.normalized;

        Vector3 vecGravity = new Vector3(0, _vecGravity, 0);
        _currentSpeed = _walkSpeed;
        if (_isRush) _currentSpeed = _rush;

        Vector3 moveAmount = targetDirection * _currentSpeed;
        moveAmount += vecGravity;

        _characterController.Move(moveAmount * Time.deltaTime);
    }

    void vecDirectionChangeBody()
    {
        //ĳ���� �̵� ��
        if (getNowVelocityVal() > 0.0f)
        {
            //�� ����  �ٶ���� �ϴ� ���� ���?
            Vector3 newForward = _characterController.velocity;
            newForward.y = 0.0f;

            //�� ĳ���� ���� ���� 
            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpd * Time.deltaTime);

        }
    }
    void SetGravity()
    {
        if (_characterController.isGrounded) _vecGravity = 0;
        else _vecGravity -= _gravity * Time.deltaTime;
    }

    public void FinishAttack()
    {
        _isAttacking = false;
    }

    /// <summary>
    /// ���� �� �ɸ��� �̵� �ӵ� �������� ��  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //���� ĳ���Ͱ� ���� �ִٸ� 
        if (_characterController.velocity == Vector3.zero)
        {
            //��ȯ �ӵ� ���� 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //��ȯ �ӵ� ���� ���� /
            Vector3 retVelocity = _characterController.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //�Ÿ� ũ��
        return vecNowVelocity.magnitude;
    }

    private void OnGUI()
    {
        if (_characterController != null && _characterController.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            //ĳ���� ���� �ӵ�
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("����ӵ� : " + _getVelocitySpd.ToString(), labelStyle);

            //���� ĳ���� ���� + ũ��
            GUILayout.Label("���纤�� : " + _characterController.velocity.ToString(), labelStyle);

            //������� ũ�� �ӵ�
            GUILayout.Label("������� ũ�� �ӵ� : " + vecNowVelocity.magnitude.ToString(), labelStyle);
        }
    }

    void CheckMonster()
    {
        Ray ray = new Ray(transform.position + Vector3.up, forward);

        RaycastHit raycastHit = new RaycastHit();

        if (Physics.Raycast(ray, out raycastHit, 10f))
        {
            if (raycastHit.transform.GetComponent<JombieMovement>() != null)
            {
                Debug.Log("���Ͱ� ���� || ���Ϳ��� �Ÿ� : " + (int)(transform.position - raycastHit.transform.position).magnitude);

                if ((transform.position - raycastHit.transform.position).magnitude < 5f)
                {
                    Debug.Log("���� ���� | LeftShift�� ��������");
                    _skinnedMeshRenderer.material.color = Color.green;
                    if (Input.GetKeyDown(KeyCode.LeftShift)) _isRush = true;
                }
                else
                {
                    _skinnedMeshRenderer.material.color = Color.white;
                    _isRush = false;
                }
            }
        }
        else
        {
            _skinnedMeshRenderer.material.color = Color.white;
            _isRush = false;
        }
    }
}
