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
            textObject.SetActive( true );
            victoryText.text = "Victory";
        }

        void OnPlayerDefeat ( TurnSystem turnSystem )
        {
            textObject.SetActive( true );
            victoryText.text = "Defeat";
        }

    }
}
