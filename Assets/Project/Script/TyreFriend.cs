using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TyreFriend : Tyre
{
    public float updatePathDelay = 1;
    public float distToStop = 6;
    LineRenderer line;
    Vector3[] path = new Vector3[0];


    protected override void Awake()
    {
        base.Awake();
        line = GetComponent<LineRenderer>();
        StartCoroutine(UpdatePath());
    }



    protected override void UpdateController()
    {
        if ((Player.inst.transform.position - transform.position).magnitude < distToStop)
        {
            if (rb.velocity.magnitude < 0.3f)
                controller.x = Vector3.Angle(Forward(), rb.velocity) > 90 ? 1 : -1;
            return;
        }

        if (path.Length < 2)
            return;

        Vector3 target = path[1];
        target.y = transform.position.y;
        Vector3 targetDir = target - transform.position;
        float angle = Vector3.SignedAngle(Forward(), targetDir, Vector3.up);

        controller.y = angle > 0 ? 1 : -1;
        controller.x = angle < 90 ? 1 : -1;
    }



    IEnumerator UpdatePath()
    {
        while (true)
        {
            yield return new WaitForSeconds(updatePathDelay);


            TyreAgent.inst.transform.position = transform.position;

            TyreAgent.inst.agent.SetDestination(Player.inst.transform.position);
            path = TyreAgent.inst.agent.path.corners;

            // draw path
            line.positionCount = TyreAgent.inst.agent.path.corners.Length;
            line.SetPositions(TyreAgent.inst.agent.path.corners);
        }
    }
}
