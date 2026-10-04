using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [SerializeField] private float laserDistance = 10f;
    [SerializeField] private LayerMask laserLayers;
    [SerializeField] protected AudioSource laserSound;


    protected LineRenderer lineRenderer;

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

        if (hit.collider != null)
        {
            laserEnd = hit.point;        

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {   
                laserSound.Play();
                GameSceneManager.Instance.PlayerHit();
            }
        }
        else
        {
            laserEnd = (Vector2)transform.position +
                       (Vector2)transform.right * laserDistance;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserEnd);
    }
}