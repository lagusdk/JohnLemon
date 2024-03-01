using System.Collections; // using 지시문; 다른 파일에서 구현된 코드를 사용
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour // 클래스 선언 시작
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start는 첫 프레임 업데이트 이전에 호출
    void Start() // 메서드 정의
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update는 프레임마다 한 번씩 호출
    void FixedUpdate() // Unity에서 자동으로 호출되는 특수 메서드. 물리에 맞추어 적시에 호출. 물리 시스템이 충돌 및 상호 작용을 계산하기 전 호출.
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward); // LookRotation 메서드를 호출하여 해당 파라미터 방향으로 바라보는 회전 생성
    }

    void OnAnimatorMove() // 원하는 대로 루트 모션을 적용하여 이동과 회전을 개별적 적용
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude); // deltaPosition : 루트 모션으로 인한 프레임당 위치의 이동량
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}


