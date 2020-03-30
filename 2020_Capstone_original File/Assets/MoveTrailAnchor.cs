using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrailAnchor : MonoBehaviour
{
    public AnswerChoosing_level3 answerChoosing_Level3;
    public Transform originalTrailAnchorTransform;
    private Transform targetTransformForTrails;
    // Start is called before the first frame update
    void Start()
    {
        targetTransformForTrails = originalTrailAnchorTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if (answerChoosing_Level3.answerIsTarget1 && !answerChoosing_Level3.answerIsTarget2)
        {
            targetTransformForTrails = GameObject.FindGameObjectWithTag("rightAnswerPosition_1").transform;
            transform.position = targetTransformForTrails.position;
        }
        else if (answerChoosing_Level3.answerIsTarget2 && !answerChoosing_Level3.answerIsTarget1)
        {
            targetTransformForTrails = GameObject.FindGameObjectWithTag("rightAnswerPosition_2").transform;
            transform.position = targetTransformForTrails.position;
        }
        else if (!answerChoosing_Level3.answerIsTarget1 && !answerChoosing_Level3.answerIsTarget2)
        {
            targetTransformForTrails = originalTrailAnchorTransform;
            transform.position = targetTransformForTrails.position;
        }
    }
}
