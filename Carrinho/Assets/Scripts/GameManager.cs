using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviourPun
{
    #region Singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }
    #endregion

  
    public GameObject playerPrefab;
    [SerializeField]Transform TransformPosition;
   

    private bool gameEnded = false;

    private Dictionary<string, int> playerScores = new Dictionary<string, int>();

    void Start()
    {
        instance = this;


        // Cria um novo jogador na rede
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate("prefab/carrinho",TransformPosition.position , Quaternion.identity);
        }

    }



    // Método chamado quando o jogo termina
    void EndGame()
    {
        gameEnded = true;
        string winner = GetWinner();
        Debug.Log("Vencedor: " + winner);
        // Aqui você pode adicionar lógica para mostrar o vencedor na UI
    }

    // Determina quem ganhou
    string GetWinner()
    {
        string winner = "";
        int highestScore = 0;

        foreach (var playerScore in playerScores)
        {
            if (playerScore.Value > highestScore)
            {
                highestScore = playerScore.Value;
                winner = playerScore.Key;
            }
        }

        return winner;
    }


}

