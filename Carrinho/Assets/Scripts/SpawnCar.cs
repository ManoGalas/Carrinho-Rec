using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Referência ao prefab do carrinho
    public GameObject carPrefab;

    // Posição onde o carrinho vai aparecer
    public Transform spawnPoint;

    // Tempo de delay entre cada spawn (se for mais de um carrinho)
    public float spawnInterval = 2f;

    // Start é chamado antes do primeiro frame update
    void Start()
    {
        // Chama a função SpawnCar repetidamente a cada spawnInterval segundos
        InvokeRepeating("SpawnCar", 0f, spawnInterval);
    }

    // Função para spawnar o carrinho
    void SpawnCar()
    {
        // Instancia o carrinho no ponto de spawn com rotação padrão
        Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
