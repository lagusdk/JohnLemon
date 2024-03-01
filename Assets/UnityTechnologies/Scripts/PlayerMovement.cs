using System.Collections; // using ���ù�; �ٸ� ���Ͽ��� ������ �ڵ带 ���
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour // Ŭ���� ���� ����
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    // Start�� ù ������ ������Ʈ ������ ȣ��
    void Start() // �޼��� ����
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update�� �����Ӹ��� �� ���� ȣ��
    void FixedUpdate() // Unity���� �ڵ����� ȣ��Ǵ� Ư�� �޼���. ������ ���߾� ���ÿ� ȣ��. ���� �ý����� �浹 �� ��ȣ �ۿ��� ����ϱ� �� ȣ��.
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
        m_Rotation = Quaternion.LookRotation(desiredForward); // LookRotation �޼��带 ȣ���Ͽ� �ش� �Ķ���� �������� �ٶ󺸴� ȸ�� ����
    }

    void OnAnimatorMove() // ���ϴ� ��� ��Ʈ ����� �����Ͽ� �̵��� ȸ���� ������ ����
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude); // deltaPosition : ��Ʈ ������� ���� �����Ӵ� ��ġ�� �̵���
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}

