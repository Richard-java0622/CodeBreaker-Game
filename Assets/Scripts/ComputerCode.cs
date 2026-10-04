using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComputerCode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected VaultManager vaultManager;
    [SerializeField] protected GameObject button;
    [SerializeField] protected Text textUI;
    [SerializeField] protected Player player;
    [SerializeField] protected GameObject laser;
    protected AudioSource audioSource;

    [Header("Code Information")]
    [SerializeField] protected int codeIndex;
    [SerializeField] protected int codeNumber;

    protected bool hasBeenHacked = false;
    protected Button buttonComponent;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected void Start()
    {   
        textUI.text = "Hack Computer";
        buttonComponent = button.GetComponent<Button>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBeenHacked)
            return;

        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(OnButtonPressed);
            
        button.SetActive(true);
        textUI.gameObject.SetActive(true);
        textUI.text = "Hack Computer";
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (hasBeenHacked)
            return; 

        buttonComponent.onClick.RemoveListener(OnButtonPressed);

        button.SetActive(false);
        textUI.gameObject.SetActive(false);

    }

    public void OnButtonPressed()
    {
        StartCoroutine(HackingRoutine());
    }

    protected IEnumerator HackingRoutine()
{
    if (player != null)
    {
        player.SetMovementEnabled(false);
    }

    audioSource.Play();

    textUI.gameObject.SetActive(true);
    textUI.text = "Hacking.....";

    yield return new WaitForSeconds(3f);

    textUI.text = "Hacking Complete!";

    laser.SetActive(false);

    if (vaultManager != null)
    {
        vaultManager.SetCodeNumber(codeIndex, codeNumber);
    }

    yield return new WaitForSeconds(1f);

    buttonComponent.onClick.RemoveListener(OnButtonPressed);

    button.SetActive(false);
    textUI.gameObject.SetActive(false);

    if (player != null)
    {
        player.SetMovementEnabled(true);
    }

    hasBeenHacked = true;
}
}