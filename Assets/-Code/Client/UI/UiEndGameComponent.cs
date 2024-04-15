using UnityEngine;
using TMPro;

using Game.Shared;
using Game.Server;// @TODO: Client should  never access Server

namespace Game.Client
{
    /// <summary>
    /// What is the sole responsibility of this class?
    /// </summary>
    public class UiEndGameComponent : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI victoryText;
        [SerializeField] GameObject textObject;
        [SerializeField] TurnSystem _turnSystem;

        void Awake ()
        {
            textObject.SetActive( false );
            _turnSystem.OnGameStarted += OnGameStarted;
            _turnSystem.OnPlayerWin += OnPlayerWin;
            _turnSystem.OnPlayerDefeat += OnPlayerDefeat;
        }

        void OnDestroy ()
        {
            _turnSystem.OnGameStarted -= OnGameStarted;
            _turnSystem.OnPlayerWin -= OnPlayerWin;
            _turnSystem.OnPlayerDefeat -= OnPlayerDefeat;
        }

        void OnGameStarted ( TurnSystem turnSystem )
        {
            textObject.SetActive( false );
        }

        void OnPlayerWin ( TurnSystem turnSystem )
        {
            victoryText.text = "Victory";
            textObject.SetActive( true );
        }

        void OnPlayerDefeat ( TurnSystem turnSystem )
        {
            if( PlayerAsset.Player.CardsInDeck.Length==0 ) victoryText.text = "Deck Out, You Lose";
            else victoryText.text = "Defeat";
            textObject.SetActive( true );
        }

    }
}
