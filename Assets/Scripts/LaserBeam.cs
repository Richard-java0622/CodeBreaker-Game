
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private float laserDistance = 10f;
    [SerializeField] private LayerMask laserLayers;

    protected AudioSource laserSound;
    protected LineRenderer lineRenderer;

    private bool playerDetected;

    void Start()
    {
        laserSound = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.right,
            laserDistance,
            laserLayers
        );

        Vector2 laserEnd;

        bool playerHit = hit.collider != null &&
            hit.collider.gameObject.layer == LayerMask.NameToLayer("Player");

        if (hit.collider != null)
        {
            laserEnd = hit.point;
        }
        else
        {
            laserEnd = (Vector2)transform.position +
                       (Vector2)transform.right * laserDistance;
        }

        if (playerHit && !playerDetected)
        {
            playerDetected = true;

            if (laserSound != null)
            {
                laserSound.PlayOneShot(laserSound.clip);
            }

            GameSceneManager.Instance.PlayerHit();
        }

        if (!playerHit)
        {
            playerDetected = false;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserEnd);
    }
}