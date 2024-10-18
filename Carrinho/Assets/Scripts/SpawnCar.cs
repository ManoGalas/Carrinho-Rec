using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    // Refer�ncia ao prefab do carrinho
    public GameObject carPrefab;

    // Posi��o onde o carrinho vai aparecer
    public Transform spawnPoint;

    // Tempo de delay entre cada spawn (se for mais de um carrinho)
    public float spawnInterval = 2f;

    // Start � chamado antes do primeiro frame update
    void Start()
    {
        // Chama a fun��o SpawnCar repetidamente a cada spawnInterval segundos
        InvokeRepeating("SpawnCar", 0f, spawnInterval);
    }

    // Fun��o para spawnar o carrinho
    void SpawnCar()
    {
        // Instancia o carrinho no ponto de spawn com rota��o padr�o
        Instantiate(carPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
