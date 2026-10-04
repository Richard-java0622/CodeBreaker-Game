using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] protected int moneyVal = 20;
    protected AudioClip collectSound;

    protected void Awake()
    {
        collectSound = GetComponent<AudioSource>().clip;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        GameSceneManager.Instance.AddMoney(moneyVal);

        Destroy(gameObject);
    }
}