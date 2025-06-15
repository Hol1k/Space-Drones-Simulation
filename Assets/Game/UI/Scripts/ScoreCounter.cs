using Game.Drones.Scripts;
using TMPro;
using UnityEngine;

namespace Game.UI.Scripts
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI blueTeamScoreLabel;
        [SerializeField] private TextMeshProUGUI redTeamScoreLabel;
        
        private int _blueTeamScore;
        private int _redTeamScore;

        public void AddScore(DroneTeam team, int score = 1)
        {
            switch (team)
            {
                case DroneTeam.Blue:
                    _blueTeamScore += score;
                    blueTeamScoreLabel.text = _blueTeamScore.ToString();
                    break;
                case DroneTeam.Red:
                    _redTeamScore += score;
                    redTeamScoreLabel.text = _redTeamScore.ToString();
                    break;
            }
        }
    }
}
