using UnityEngine;
using System.Collections;

public class FoosballTable : MonoBehaviour {

    public BallSpawner BallSpawner;
    public GoalTrigger RedSideGoalTrigger;
    public GoalTrigger BlueSideGoalTrigger;

    public enum Sides { Red, Blue };

    public delegate void TeamScored(Sides scoringSide);
    public TeamScored OnTeamScored;

    private void Start()
    {
        RedSideGoalTrigger.SetActionOnGoal(RedTeamScored);
        BlueSideGoalTrigger.SetActionOnGoal(BlueTeamScored);
    }

    private void RedTeamScored()
    {
        if(OnTeamScored != null)
        {
            OnTeamScored(Sides.Red);
        }
    }

    private void BlueTeamScored()
    {
        if (OnTeamScored != null)
        {
            OnTeamScored(Sides.Blue);
        }
    }

    public void LaunchBall()
    {
        BallSpawner.LaunchNewBall();
    }
}
