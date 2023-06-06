using System.Collections;
using UnityEngine;

namespace GarmentButton.AI.SpaceShip
{
    public class MoveShip : MonoBehaviour
    {

        [SerializeField] Transform position;
        [SerializeField] Transform position2;
        Vector3 targetPosition = Vector3.zero;
        [SerializeField] float speedMove;
        [SerializeField] float speedAfterPlayerMulti;
        [SerializeField] LiftObjects rayLift;
        public bool afterPlayer;
        bool RechedPoint;
        public float HowMuchTimeAfterPlayer = 30f;
        float timerToPlayer = 0f;


        Vector3 GetRandomPositon()
        {
            float _xPos = Random.Range(position.position.x, position2.position.x);
            float _zPos = Random.Range(position.position.z, position2.position.z);
            Vector3 newSpot = new Vector3(_xPos, transform.position.y, _zPos);
            return newSpot;
        }
        private void Start()
        {
            StartCoroutine(Move());
        }
        void MoveToDestinetion(Vector3 wantedTarget, float speedMulti)
        {
            var target = new Vector3(wantedTarget.x, transform.position.y, wantedTarget.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speedMove * speedMulti);
        }
        IEnumerator Move()
        {
            while (true)
            {
                while (!rayLift.CatchSomeone)
                {
                    if (targetPosition == Vector3.zero)
                        targetPosition = GetRandomPositon();
                    MoveToDestinetion(targetPosition, afterPlayer ? speedAfterPlayerMulti : 1f);
                    while (!ReachPoint(targetPosition))
                    {
                        MoveToDestinetion(targetPosition, afterPlayer ? speedAfterPlayerMulti : 1f);
                        yield return new WaitForEndOfFrame();
                        if (rayLift.CatchSomeone)
                        {
                            MoveToDestinetion(rayLift.hit.point, afterPlayer ? speedAfterPlayerMulti : 1f);
                            RechedPoint = true;
                            yield return new WaitForEndOfFrame();
                            break;
                        }
                    }
                    RechedPoint = true;
                    targetPosition = Vector3.zero;
                }
                yield return new WaitForSeconds(1f);
            }
        }
        bool ReachPoint(Vector3 wantedTarget)
        {
            var Distance = Vector3.Distance(transform.position, wantedTarget);
            if (Distance < 0.1)
                return true;
            return false;
        }
        public void PlayerSeen(Vector3 playerPosition)
        {
            RechedPoint = false;
            timerToPlayer = 0f;
            targetPosition = playerPosition;
            if (!afterPlayer)
                StartCoroutine(AfterPlayer());
        }
        IEnumerator AfterPlayer()
        {
            afterPlayer = true;
            while (timerToPlayer < HowMuchTimeAfterPlayer)
            {
                if (RechedPoint)
                    timerToPlayer += Time.deltaTime;
                yield return new WaitForSeconds(1f);
            }
            afterPlayer = false;
        }
    }
}