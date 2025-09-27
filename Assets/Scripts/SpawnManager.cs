using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Configuração dos Personagens")]
    public GameObject[] characterPrefabs;
    public Transform[] spawnPoints;

    [Header("Identificação do Ladrão")]
    public int thiefCharacterIndex = 0; // Índice do personagem que será o ladrão (0-5)

    private List<GameObject> spawnedCharacters = new List<GameObject>();
    private int currentThiefSpawnIndex;

    void Start()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        ClearPreviousCharacters();
        // Cria uma lista com os índices dos spawn points (0, 1, 2, 3, 4, 5)
        List<int> availableSpawnIndices = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnIndices.Add(i);
        }

        for (int i = 0; i < availableSpawnIndices.Count; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnIndices.Count);
            int temp = availableSpawnIndices[i];
            availableSpawnIndices[i] = availableSpawnIndices[randomIndex];
            availableSpawnIndices[randomIndex] = temp;
        }

        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            int spawnIndex = availableSpawnIndices[i];
            Vector3 spawnPosition = spawnPoints[spawnIndex].position;

            GameObject spawnedChar = Instantiate(characterPrefabs[i], spawnPosition, Quaternion.identity);
            spawnedCharacters.Add(spawnedChar);

            // Adiciona o componente NPC a cada personagem
            NPCBehavior npcBehavior = spawnedChar.AddComponent<NPCBehavior>();

            // Define se este é o ladrão ou não
            if (i == thiefCharacterIndex)
            {
                npcBehavior.isThief = true;
                currentThiefSpawnIndex = spawnIndex;
                Debug.Log($"Ladrão spawnou no ponto {spawnIndex}");
            }
            else
            {
                npcBehavior.isThief = false;
            }
        }
    }

    void ClearPreviousCharacters()
    {
        foreach (GameObject character in spawnedCharacters)
        {
            if (character != null)
            {
                DestroyImmediate(character);
            }
        }
        spawnedCharacters.Clear();
    }
    public void RestartGame()
    {
        SpawnCharacters();
    }
}