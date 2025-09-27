using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class NPCBehavior : MonoBehaviour
{
    [Header("Configura��o do NPC")]
    public bool isThief = false;

    [Header("Configura��o de UI")]
    public static GameObject messagePanel; 
    public static TextMeshProUGUI messageText;
    public static Button continueButton;

    // Refer�ncias est�ticas para encontrar a UI automaticamente
    private static bool uiReferencesSet = false;

    private void Awake()
    {
        ResetUIReferences();
    }

    private void Start()
    {
        if (!uiReferencesSet || messagePanel == null)
        {
            SetupUIReferences();
        }

        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
        }
    }

    public static void ResetUIReferences()
    {
        messagePanel = null;
        messageText = null;
        continueButton = null;
        uiReferencesSet = false;
    }

    void SetupUIReferences()
    {
        messagePanel = GameObject.Find("MessagePanel");

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
            messageText = messagePanel.GetComponentInChildren<TextMeshProUGUI>();
            if (messageText == null)
            {
                // Se n�o achou TextMeshPro, tenta Text legacy
                Text legacyText = messagePanel.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    Debug.Log("Usando Text Legacy - considere usar TextMeshPro");                    
                }
                else
                {
                    Debug.LogError("Nenhum componente de texto encontrado no MessagePanel!");
                }
            }
            else
            {
                Debug.Log("TextMeshPro encontrado!");
            }

            continueButton = messagePanel.GetComponentInChildren<Button>();
            if (continueButton != null)
            {
                Debug.Log("Bot�o encontrado e configurado!");
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(() => {
                    Debug.Log("Bot�o clicado - escondendo mensagem");
                    HideMessage();
                });
            }
            else
            {
                Debug.LogError("Bot�o n�o encontrado no MessagePanel!");
            }

            Debug.Log("UI configurada com sucesso!");
            uiReferencesSet = true;
        }
        else
        {
            Debug.LogError("MessagePanel n�o encontrado! Verificando nomes na hierarquia...");
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            Debug.Log("GameObjects encontrados na cena:");
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Panel") || obj.name.Contains("Message") || obj.name.Contains("Canvas"))
                {
                    Debug.Log($"- Objeto encontrado: {obj.name} (ativo: {obj.activeInHierarchy})");
                }
            }
            uiReferencesSet = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isThief)
            {
                OnThiefCaught();
            }
            else
            {
                OnInnocentTouched();
            }
        }
    }

    void OnThiefCaught()
    {
        Debug.Log("Parab�ns! Voc� pegou o ladr�o!");
        ShowMessage("PARAB�NS! VOC� PEGOU O LADR�O!", Color.green);
        // Ap�s 3 segundos, volta para o menu principal
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

    void OnInnocentTouched()
    {
        Debug.Log("Este personagem � inocente!");
        ShowMessage("ESTE PERSONAGEM � INOCENTE!", Color.red);
    }

    IEnumerator ReturnToMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }

    void ShowMessage(string message, Color color)
    {
        if (messagePanel != null && messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
            messagePanel.SetActive(true);
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
        }
        else
        {
            // Fallback para Debug.Log se a UI n�o estiver configurada
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
            Debug.LogWarning("UI n�o encontrada! Certifique-se de ter um 'MessagePanel' na cena.");
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
            Debug.Log("Mensagem escondida!");
        }
        else
        {
            Debug.LogWarning("Tentou esconder mensagem, mas messagePanel � null!");
        }
    }
}