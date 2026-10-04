using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VaultManager : MonoBehaviour
{
    [Header("Vault UI")]
    [SerializeField] protected Text codeText;
    [SerializeField] protected Text winUI;
    [SerializeField] protected Text codeTextUI;

    [Header("Vault Door")]
    [SerializeField] protected GameObject vaultDoor;

    protected Animator vaultDoorAnimator;

    protected int[] combination = new int[3];
    protected bool[] codeFound = new bool[3];

    protected int foundCount = 0;

    protected int[] playerInput = new int[3];
    protected int inputIndex = 0;

    protected bool playerInRange = false;
    protected bool vaultOpened = false;

    void Awake()
    {
        vaultDoorAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        if (codeText != null)
        {
            codeText.gameObject.SetActive(false);
            codeText.text = "- - -";
        }

        if (codeTextUI != null)
        {
            codeTextUI.gameObject.SetActive(false);
            codeTextUI.text = "Code: - - -";
        }

        if (winUI != null)
        {
            winUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerInRange || vaultOpened)
        {
            return;
        }

        CheckNumberInput();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        if (vaultOpened)
        {
            return;
        }

        playerInRange = true;

        if (codeText != null)
        {
            codeText.gameObject.SetActive(true);
            UpdateCodeDisplay();
        }

        Debug.Log("Player entered vault range.");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        playerInRange = false;

        if (codeText != null)
        {
            codeText.gameObject.SetActive(false);
        }

        ResetInput();
    }

    private void CheckNumberInput()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            Debug.Log("5 pressed");
            EnterNumber(5);
        }
        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            Debug.Log("6 pressed");
            EnterNumber(6);
        }
        else if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            Debug.Log("7 pressed");
            EnterNumber(7);
        }
    }

    public void SetCodeNumber(int index, int number)
    {
        if (index < 0 || index >= combination.Length)
        {
            Debug.LogError("Invalid code index: " + index);
            return;
        }

        if (codeFound[index])
        {
            Debug.Log("Code at index " + index + " was already found.");
            return;
        }

        combination[index] = number;
        codeFound[index] = true;
        foundCount++;

        Debug.Log(
            "Code stored! Index: " + index +
            " Number: " + number +
            " | Codes found: " + foundCount + "/3"
        );

        DisplayCodeNumber();
    }

    protected void DisplayCodeNumber()
    {
        if (codeTextUI == null)
        {
            return;
        }

        string display = "Code: ";

        for (int i = 0; i < combination.Length; i++)
        {
            if (codeFound[i])
            {
                display += combination[i];
            }
            else
            {
                display += "-";
            }

            if (i < combination.Length - 1)
            {
                display += " - ";
            }
        }

        codeTextUI.gameObject.SetActive(true);
        codeTextUI.text = display;
    }

    public bool HasAllCodes()
    {
        return foundCount == combination.Length;
    }

    public void EnterNumber(int number)
    {
        if (inputIndex >= playerInput.Length || vaultOpened)
        {
            return;
        }

        playerInput[inputIndex] = number;
        inputIndex++;

        Debug.Log("Player input: " + GetInputDisplay());

        UpdateCodeDisplay();

        if (inputIndex == playerInput.Length)
        {
            if (!HasAllCodes())
            {
                Debug.Log(
                    "Cannot check combination. Codes found: " +
                    foundCount + "/" + combination.Length
                );

                ResetInput();
                return;
            }

            CheckCombination();
        }
    }

    protected void CheckCombination()
    {
        Debug.Log(
            "Checking combination: " +
            playerInput[0] + " " +
            playerInput[1] + " " +
            playerInput[2]
        );

        Debug.Log(
            "Correct combination: " +
            combination[0] + " " +
            combination[1] + " " +
            combination[2]
        );

        for (int i = 0; i < combination.Length; i++)
        {
            if (playerInput[i] != combination[i])
            {
                Debug.Log("Wrong combination!");

                ResetInput();

                return;
            }
        }

        Debug.Log("VAULT OPENED!");

        if (winUI != null)
        {
            winUI.gameObject.SetActive(true);
        }

        GameSceneManager.Instance.PlayerWon();

        OpenVault();
    }

    protected void OpenVault()
    {
        vaultOpened = true;
        playerInRange = false;

        if (vaultDoorAnimator != null)
        {
            vaultDoorAnimator.SetTrigger("Open");
        }

        if (codeText != null)
        {
            codeText.gameObject.SetActive(false);
        }

        if (vaultDoor != null)
        {
            vaultDoor.SetActive(false);
        }
    }

    public void ResetInput()
    {
        inputIndex = 0;

        for (int i = 0; i < playerInput.Length; i++)
        {
            playerInput[i] = 0;
        }

        UpdateCodeDisplay();
    }

    protected void UpdateCodeDisplay()
    {
        if (codeText != null)
        {
            codeText.text = GetInputDisplay();
        }
    }

    public string GetInputDisplay()
    {
        string display = "";

        for (int i = 0; i < playerInput.Length; i++)
        {
            if (i < inputIndex)
            {
                display += playerInput[i];
            }
            else
            {
                display += "-";
            }

            if (i < playerInput.Length - 1)
            {
                display += " ";
            }
        }

        return display;
    }
}