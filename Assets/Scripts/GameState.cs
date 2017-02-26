using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

    public int GoalsToWin = 7;

    public Text HeaderLabel;
    public Text pressButtonToContinueLabel;
    public Text BlueScoreLabel;
    public Text RedScoreLabel;

    private int BlueScore = 0;
    private int RedScore = 0;

    private FoosballTable TableManager;

    private void Start()
    {
        TableManager = GameObject.FindWithTag("Table").GetComponent<FoosballTable>();
        TableManager.OnTeamScored += OnTeamScored;

        UpdateScoreLabels();

        StartNewRound();
    }

    private void UpdateScoreLabels()
    {
        BlueScoreLabel.text = "Blue: " + BlueScore;
        RedScoreLabel.text = "Red: " + RedScore;
    }

    private void StartNewRound()
    {
        HeaderLabel.text = "First to " + GoalsToWin;
        pressButtonToContinueLabel.text = "Press 'A' to start the round";

        StartCoroutine(WaitForBallLaunchButton());
    }

    private IEnumerator WaitForBallLaunchButton()
    {
        while(!Input.GetButtonDown("LaunchBall"))
        {
            yield return null;
        }

        pressButtonToContinueLabel.text = "";

        TableManager.LaunchBall();
    }

    private void OnTeamScored(FoosballTable.Sides scoringSide)
    {
        switch (scoringSide)
        {
            case FoosballTable.Sides.Red:
                RedScore++;
                break;
            case FoosballTable.Sides.Blue:
                BlueScore++;
                break;
            default:
                break;
        }

        UpdateScoreLabels();
        HeaderLabel.text = scoringSide.ToString() + " team scores!";

        Invoke("CheckScoresForWinner", 3f);
    }

    private void CheckScoresForWinner()
    {
        if(BlueScore >= GoalsToWin)
        {
            OnGameWon(FoosballTable.Sides.Blue);
        }
        else if (RedScore >= GoalsToWin)
        {
            OnGameWon(FoosballTable.Sides.Red);
        }
        else
        {
            StartNewRound();
        }
    }

    private void OnGameWon(FoosballTable.Sides winningSide)
    {
        HeaderLabel.text = winningSide.ToString() + " team wins!";
        pressButtonToContinueLabel.text = "Press 'A' to start a new game";

        StartCoroutine(WaitForStartNewGame());
    }

    private IEnumerator WaitForStartNewGame()
    {
        while (!Input.GetButtonDown("LaunchBall"))
        {
            yield return null;
        }

        ResetPoints();
        StartNewRound();
    }

    private void ResetPoints()
    {
        BlueScore = 0;
        RedScore = 0;
    }
}
