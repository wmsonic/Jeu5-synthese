using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{

    private List<Transform> _patrolPoints = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform point in transform)
        {
            _patrolPoints.Add(point);
        }
    }

    public List<Transform> GetPatrolPoints(){
        return _patrolPoints;
    }
}
