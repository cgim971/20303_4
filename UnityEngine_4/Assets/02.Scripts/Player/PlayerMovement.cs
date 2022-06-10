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
    // 캐릭터 이동 속도 증가값
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
        //캐릭터 이동 시
        if (getNowVelocityVal() > 0.0f)
        {
            //내 몸통  바라봐야 하는 곳은 어디?
            Vector3 newForward = _characterController.velocity;
            newForward.y = 0.0f;

            //내 캐릭터 전면 설정 
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
    /// 현재 내 케릭터 이동 속도 가져오는 함  
    /// </summary>
    /// <returns>float</returns>
    float getNowVelocityVal()
    {
        //현재 캐릭터가 멈춰 있다면 
        if (_characterController.velocity == Vector3.zero)
        {
            //반환 속도 값은 0
            vecNowVelocity = Vector3.zero;
        }
        else
        {

            //반환 속도 값은 현재 /
            Vector3 retVelocity = _characterController.velocity;
            retVelocity.y = 0.0f;

            vecNowVelocity = Vector3.Lerp(vecNowVelocity, retVelocity, moveChageSpd * Time.fixedDeltaTime);

        }
        //거리 크기
        return vecNowVelocity.magnitude;
    }

    private void OnGUI()
    {
        if (_characterController != null && _characterController.velocity != Vector3.zero)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;
            //캐릭터 현재 속도
            float _getVelocitySpd = getNowVelocityVal();
            GUILayout.Label("현재속도 : " + _getVelocitySpd.ToString(), labelStyle);

            //현재 캐릭터 방향 + 크기
            GUILayout.Label("현재벡터 : " + _characterController.velocity.ToString(), labelStyle);

            //현재백터 크기 속도
            GUILayout.Label("현재백터 크기 속도 : " + vecNowVelocity.magnitude.ToString(), labelStyle);
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
                Debug.Log("몬스터가 존재 || 몬스터와의 거리 : " + (int)(transform.position - raycastHit.transform.position).magnitude);

                if ((transform.position - raycastHit.transform.position).magnitude < 5f)
                {
                    Debug.Log("돌진 가능 | LeftShift를 누르세용");
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
