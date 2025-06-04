using UnityEngine.Rendering.Universal; // Add this at the top of the file if not already present
using UnityEngine;
using System.Collections;

public class SkillShooter : MonoBehaviour
{
    public GameObject bulletPrefab;     // 발사할 탄환 프리팹
    public float bulletSpeed = 5f;      // 탄환 속도
    public Transform firePoint;         // 탄환이 생성될 위치

    public ParticleSystem skillEffect;      // 파티클 효과
    public AudioClip skillSound;            // 스킬 사운드
    private AudioSource audioSource;        // 오디오 소스

    private Animator animator;

    public int maxActiveBullets = 3;
    private int currentActiveBullets = 0;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();

        if (firePoint == null)
        {
            Debug.LogWarning("[SkillShooter] firePoint가 비어있습니다. 플레이어 자식에 빈 GameObject를 지정하세요.");
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootSkill();
        }
    }

    void ShootSkill()
    {
        if (currentActiveBullets >= maxActiveBullets) return;

        if (bulletPrefab == null || firePoint == null)
            return;

        Vector2 direction = GetLookDirection();
        Quaternion rot = Quaternion.LookRotation(Vector3.forward, direction);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);
        currentActiveBullets++;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Light2D 추가
        Light2D bulletLight = bullet.AddComponent<Light2D>();
        bulletLight.lightType = Light2D.LightType.Point;
        bulletLight.intensity = 0.8f;
        bulletLight.pointLightInnerRadius = 0.3f;
        bulletLight.pointLightOuterRadius = 1.0f;
        bulletLight.color = Color.red; // 필요에 따라 Color.blue 등으로 변경

        // 사운드 재생
        if (skillSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(skillSound);
        }

        // 파티클 효과 재생
        if (skillEffect != null)
        {
            ParticleSystem effect = Instantiate(skillEffect, firePoint.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration);
        }

        Destroy(bullet, 3f); // 일정 시간 후 탄환 제거
        StartCoroutine(DecreaseBulletCountAfterDelay(3f));
    }

    Vector2 GetLookDirection()
    {
        if (animator == null) return Vector2.right; // 기본값

        float x = animator.GetFloat("lastmoveX");
        float y = animator.GetFloat("lastmoveY");

        if (x == 0 && y == 0)
            return Vector2.right;

        return new Vector2(x, y).normalized;
    }

    private IEnumerator DecreaseBulletCountAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentActiveBullets = Mathf.Max(0, currentActiveBullets - 1);
    }
}
