using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour {

    private System.Action OnGoalCallback;

    public void SetActionOnGoal(System.Action actionOnGoal)
    {
        OnGoalCallback = actionOnGoal;
    }

	private void OnTriggerEnter(Collider col)
    {
        if (col.tag.Equals("Ball"))
        {
            if(OnGoalCallback != null)
            {
                OnGoalCallback();
            }

            Destroy(col.gameObject);
        }
    }
}
