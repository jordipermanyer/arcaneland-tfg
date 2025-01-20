using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialogueMark;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private float typingTime = 0.05f;
    [SerializeField] private PlayerController playerMovement; // Asegúrate de asignar esto en el Inspector.

    private int currentLine = 0;
    private bool isPlayerInRange = false;
    private bool isDialogueRunning = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isDialogueRunning)
            {
                StartDialogue();
            }
            else if (dialogueText.text != dialogueLines[currentLine])
            {
                // Si la corutina está corriendo y la línea no se ha mostrado completamente, mostrar toda la línea
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    private void StartDialogue()
    {
        dialogueMark.SetActive(false);
        dialoguePanel.SetActive(true);
        currentLine = 0;
        isDialogueRunning = true;
        playerMovement.enabled = false; // Deshabilitar movimiento del jugador
        StartCoroutine(ShowLine());
    }

    private void DisplayNextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[currentLine])
        {
            dialogueText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueMark.SetActive(true);
        isDialogueRunning = false;
        playerMovement.enabled = true; // Reanudar movimiento del jugador
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);
            dialoguePanel.SetActive(false); // También cierra el diálogo si el jugador sale de la zona
            if (isDialogueRunning)
            {
                EndDialogue();
            }
        }
    }
}
