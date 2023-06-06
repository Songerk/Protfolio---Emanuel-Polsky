using GarmentButton.AI;
using System.Collections;
using UnityEngine;

namespace GarmentButton.AI.SpaceShip
{
    public class LiftObjects : MonoBehaviour
    {

        [SerializeField] float radiusSphere;
        [SerializeField] float lenghtRay;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] MoveShip shipScript;
        [SerializeField] LayerMask ObstacleLayer;
        string dragItemName = "No Name";
        Itaken take;
        public RaycastHit hit;
        public bool CatchSomeone;
        float timeCheck;
        float howMuchTimeToWait = 20f;
        Vector3 position;
        [SerializeField] Transform box;

        [SerializeField] bool testWay;
        private void FixedUpdate()
        {

            if (Physics.SphereCast(transform.position, radiusSphere, -transform.up, out hit, lenghtRay, playerLayer, QueryTriggerInteraction.Ignore))
            {
                var direction = (hit.point - transform.position).normalized;
                RaycastHit ray;

                if (Physics.Raycast(transform.position, direction, out ray, lenghtRay * 100f, ObstacleLayer, QueryTriggerInteraction.Ignore))
                {
                    Debug.Log(ray.collider.gameObject.name);
                    Debug.DrawLine(transform.position, ray.point, Color.red);

                    if (ray.distance < hit.distance)
                    {
                        return;
                    }
                }




                CheckIfNeedToGetCommepont(hit);
                if (take != null)
                    ActiveDrag(hit);
                else
                    CatchSomeone = false;

            }
            else if (take != null)
            {
                Restart();
            }
        }

        private void CheckIfNeedToGetCommepont(RaycastHit hitObject)
        {
            var TempName = hitObject.collider.gameObject.name;
            if (take == null || !TempName.Equals(dragItemName))
            {
                take = hitObject.collider.GetComponent<Itaken>();
                if (take == null && !shipScript.afterPlayer)
                    take = hitObject.collider.GetComponentInParent<Itaken>();
                dragItemName = TempName;

            }
        }
        void ActiveDrag(RaycastHit hitObject)
        {
            if (take == null)
            {
                Debug.Log("CouldntFindDrag");
                return;

            }
            take.Taken(true, transform.position);
            CatchSomeone = true;
            CheckIfAbleToDragIncaseOfBug(hitObject);
        }

        private void CheckIfAbleToDragIncaseOfBug(RaycastHit hitObject)
        {
            if (position == hitObject.collider.transform.position)
                timeCheck += Time.deltaTime;
            else
                timeCheck = 0f;
            if (timeCheck > howMuchTimeToWait)
            {
                Restart();
            }
            position = hitObject.collider.transform.position;
        }

        void Restart()
        {
            timeCheck = 0f;
            CatchSomeone = false;
            if (take != null)
            {
                take.Taken(false, transform.position);
                take = null;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position + (-transform.up * lenghtRay), radiusSphere);
            Gizmos.DrawLine(transform.position, transform.position + (-transform.up * lenghtRay));

        }
    }
}