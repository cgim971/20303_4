using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    public float atkPower = 2f;

    public Transform camerats;

    #region Movement 加己
    [Header("Movement")]
    public float _moveSpeed = 6f;
    float movementMultiplier = 10f;
    [SerializeField] float _airMultiplier = 0.4f;
    float _horizontalMovement;
    float _verticalMovement;
    Vector3 _moveDirection;

    [Header("Drag")]
    float _groundDrag = 6f;
    float _airDrag = 2f;
    #endregion

    #region Jumping 加己
    [Header("Jumping")]
    public float _jumpForce = 5f;
    bool _isGrounded;
    #endregion

    #region Keybinds 加己
    [Header("Keybinds")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    #endregion

    #region Anim 加己
    private Animator _anim;

    enum AnimType
    {
        NONE,
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DEATH
    }

    AnimType _animType = AnimType.NONE;
    #endregion


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        _rb.freezeRotation = true;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        MyInput();
        ControlDrag();

        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            Jump();
        }
        if (Input.GetMouseButtonDown(0))
        {
            _animType = AnimType.ATTACK;
            PlayAnim();
        }


        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit = new RaycastHit();
        if (Physics.Raycast(ray, out raycastHit, 3f))
        {
            if (Input.GetMouseButtonDown(1))
                if (raycastHit.transform.CompareTag("Box"))
                    Destroy(raycastHit.transform.gameObject);
        }
    }

    void MyInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");

        _anim.SetFloat("H", _horizontalMovement);
        _anim.SetFloat("V", _verticalMovement);

        if (_horizontalMovement == 0 && _verticalMovement == 0)
        {
            _animType = AnimType.WALK;
            PlayAnim();
        }
        else
        {
            _animType = AnimType.IDLE;
            PlayAnim();
        }

        _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;
        _moveDirection.y = 0;
    }

    void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    void PlayAnim()
    {
        switch (_animType)
        {
            case AnimType.IDLE:
                break;
            case AnimType.WALK:
                break;
            case AnimType.RUN:
                break;
            case AnimType.ATTACK:
                Attack();
                break;
            case AnimType.DEATH:
                Death();
                break;
            default:
                break;
        }
    }

    void Attack()
    {
        _anim.SetTrigger("Attack");
    }

    void Death()
    {
        _anim.SetTrigger("Death");
    }

    void ControlDrag()
    {
        if (_isGrounded)
        {
            _rb.drag = _groundDrag;
        }
        else
        {
            _rb.drag = _airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * movementMultiplier, ForceMode.Acceleration);
        else
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * movementMultiplier * _airMultiplier, ForceMode.Acceleration);
    }

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;
        labelStyle.normal.textColor = Color.white;
        //某腐磐 泅犁 加档
        GUILayout.Label("泅犁加档 : " + _rb.velocity.magnitude, labelStyle);
    }

    public void Attacking(GameObject obj)
    {
        obj.GetComponent<Enemy>().TakeDamage(atkPower);
    }

}
