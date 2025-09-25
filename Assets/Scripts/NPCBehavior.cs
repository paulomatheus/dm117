using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [Header("Configura��o do NPC")]
    public bool isThief = false;

    [Header("Mensagens UI")]
    public GameObject thiefMessageUI; // Painel/texto para quando pegar o ladr�o
    public GameObject innocentMessageUI; // Painel/texto para quando pegar um inocente

    private void Start()
    {
        // Adiciona um Collider2D se n�o tiver
        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.isTrigger = true; // Para detectar colis�o sem f�sica
        }
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
        Debug.Log("Parab�ns! Voc� pegou o ladr�o!");

        // Opcoes para por depois se der tempo:
        // - Mostrar mensagem na tela
        // - Tocar som de vit�ria
        // - Parar o jogo
        // - Mostrar tela de vit�ria

        ShowMessage("PARAB�NS! VOC� PEGOU O LADR�O!", Color.green);

        // Tentar depois: Pausar o jogo
        // Time.timeScale = 0f;

        // Tentar depois pra ver se da certo: Carregar pr�xima fase ou reiniciar
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnInnocentTouched()
    {
        Debug.Log("Este personagem � inocente!");

        ShowMessage("ESTE PERSONAGEM � INOCENTE!", Color.yellow);

        // Sera que vale a pena?: Diminuir pontos ou vidas
        // GameManager.instance.LoseLife();
    }

    void ShowMessage(string message, Color color)
    {        
        Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");

        // Preciso testar pra ver se funciona:
        /*
        if (isThief && thiefMessageUI != null)
        {
            thiefMessageUI.SetActive(true);
            // Esconde a mensagem ap�s 2 segundos
            Invoke("HideThiefMessage", 2f);
        }
        else if (!isThief && innocentMessageUI != null)
        {
            innocentMessageUI.SetActive(true);
            // Esconde a mensagem ap�s 2 segundos
            Invoke("HideInnocentMessage", 2f);
        }
        */
    }

    // M�todos para esconder mensagens para UI
    /*
    void HideThiefMessage()
    {
        if (thiefMessageUI != null)
            thiefMessageUI.SetActive(false);
    }
    
    void HideInnocentMessage()
    {
        if (innocentMessageUI != null)
            innocentMessageUI.SetActive(false);
    }
    */
}