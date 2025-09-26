using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Importante para TextMeshPro
using System.Collections;

public class NPCBehavior : MonoBehaviour
{
    [Header("Configuração do NPC")]
    public bool isThief = false;

    [Header("Configuração de UI")]
    public static GameObject messagePanel; // Painel da mensagem
    public static TextMeshProUGUI messageText; // Texto da mensagem (TextMeshPro)
    public static Button continueButton; // Botão para continuar

    // Referências estáticas para encontrar a UI automaticamente
    private static bool uiReferencesSet = false;

    private void Start()
    {
        // Configura as referências da UI se ainda não foram configuradas
        if (!uiReferencesSet)
        {
            SetupUIReferences();
        }

        // Adiciona um Collider2D se não tiver
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true; // Para detectar colisão sem física
        }
    }

    void SetupUIReferences()
    {
        // Procura pelos componentes de UI na cena
        messagePanel = GameObject.Find("MessagePanel");

        if (messagePanel != null)
        {
            Debug.Log("MessagePanel encontrado!");

            // Tenta encontrar TextMeshPro primeiro, depois Text legacy
            messageText = messagePanel.GetComponentInChildren<TextMeshProUGUI>();
            if (messageText == null)
            {
                // Se não achou TextMeshPro, tenta Text legacy
                Text legacyText = messagePanel.GetComponentInChildren<Text>();
                if (legacyText != null)
                {
                    Debug.Log("Usando Text Legacy - considere usar TextMeshPro");
                    // Vamos criar um método alternativo para Text legacy
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
                Debug.Log("Botão encontrado!");
                // Configura o botão para esconder o painel quando clicado
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(HideMessage);
            }
            else
            {
                Debug.LogError("Botão não encontrado no MessagePanel!");
            }

            // Garante que o painel comece escondido
            messagePanel.SetActive(false);
            Debug.Log("UI configurada com sucesso!");
        }
        else
        {
            Debug.LogError("MessagePanel não encontrado! Verificando nomes na hierarquia...");

            // Debug extra para ver todos os GameObjects da cena
            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            Debug.Log("GameObjects encontrados na cena:");
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Panel") || obj.name.Contains("Message"))
                {
                    Debug.Log($"- Objeto encontrado: {obj.name}");
                }
            }
        }

        uiReferencesSet = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se quem tocou foi o detetive (player)
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
        Debug.Log("Parabéns! Você pegou o ladrão!");

        ShowMessage("PARABÉNS! VOCÊ PEGOU O LADRÃO!", Color.green);

        // Após 3 segundos, volta para o menu principal
        StartCoroutine(ReturnToMainMenuAfterDelay(3f));
    }

    void OnInnocentTouched()
    {
        Debug.Log("Este personagem é inocente!");

        ShowMessage("ESTE PERSONAGEM É INOCENTE!", Color.red);
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
            // Fallback para Debug.Log se a UI não estiver configurada
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
            Debug.LogWarning("UI não encontrada! Certifique-se de ter um 'MessagePanel' na cena.");
        }
    }

    void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }
}