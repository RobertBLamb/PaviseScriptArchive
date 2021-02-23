using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform target;
    public float speed = 20;
    float speedDefault;
    Vector3[] path;
    int targetIndex;
    int numberOfFailures;
    public GameObject superStar;

    void Start()
    {
        StartCoroutine(UpdatePath());
        speedDefault = speed;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            //numberOfFailures = 0;
        }
        /*else
        {
            numberOfFailures++;
            Debug.Log(numberOfFailures);
            if (numberOfFailures >= 4)
            {
                //superStar.transform.position = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);
                //superStar.GetComponent<Grid>().CreateGrid();
            }
            
        }*/

    }
    public IEnumerator UpdatePath()
    {
        if (!GetComponent<StalkerDrone>().aStarDisabled)
        {

            //target = GetComponent<StalkerDrone>().desiredLocation;
            if (Time.timeSinceLevelLoad < .3f)
            {
                yield return new WaitForSeconds(.3f);
            }
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);

            float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
            Vector3 targetPosOld = target.position;

            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
                {
                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                    targetPosOld = target.position;
                }
            }
        }
    }
    public IEnumerator FollowPath()
    {
        if (!GetComponent<StalkerDrone>().aStarDisabled)
        {
            Vector3 currentWaypoint = path[0];
            while (true)
            {
                if (transform.position == currentWaypoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWaypoint = path[targetIndex];
                }
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                yield return null;

            }
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}