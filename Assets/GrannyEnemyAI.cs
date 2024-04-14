using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class GrannyEnemyAI : MonoBehaviour
{
    //public
    [SerializeField] private GameObject GrannyJumpscare;
    [SerializeField] private GameObject chaseMusik;

    [SerializeField] private AudioClip[] GrannySounds;
    
    [SerializeField] private Animator GrannyAnim;

    [SerializeField] private NavMeshAgent Granny;

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private Transform granny;
    [SerializeField] private Transform grannyHead;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerHead;

    public bool SeePlayer;

    [SerializeField] PlayerController playerController;
    [SerializeField] CameraController playerCam;

    //private
    public bool GrannyisSearching => status == GrannyStatus.GrannyisSearching;
    public bool GrannySearchedWaiting => status == GrannyStatus.GrannySearchedWaiting;
    public bool GrannyisChasing => status == GrannyStatus.GrannyisChasing;
    public bool GrannyChasedWaiting => status == GrannyStatus.GrannyChasedWaiting;
    public bool playerGetCaught => status == GrannyStatus.GrannyCaughtPlayer;

    GrannyStatus status = GrannyStatus.Start;

    float statusChangeTime = 0f;
    float waypointTime = 7f;
    float lookAroundTime = 6.32f;
    float walkSpeed = 1.4f;
    float followSpeed = 3f;
    float stopSpeed = 0f;
    float playerSafeTime = 7f;

    bool canFadeMusik;

    public virtual void FixedUpdate()
    {
        OnStatusTick(status, Time.time - statusChangeTime);
        FadeChaseMusik();
    }

    public virtual void ChangeStatus(GrannyStatus nextStatus)
    {
        //If cannot change status, threw an issue.
        if(nextStatus == status)
        {
            Debug.LogError($"{nameof(ChangeStatus)} fails as {nextStatus} is a current state already.");
            return;
        }

        OnStatusExit(status);
        status = nextStatus;
        statusChangeTime = Time.time;
        OnStatusEnter(status);
    }

    public virtual void OnStatusEnter(GrannyStatus status)
    {
        //Beginning
        if(status == GrannyStatus.Start)
        {

        }
        else if(status == GrannyStatus.GrannyisSearching)
        {
            Granny.stoppingDistance = 2f;
            DefindGrannyDestination();

            GrannyAnim.Play("Walk_1");
            walkSpeed = Granny.speed;
        }
        else if(status == GrannyStatus.GrannySearchedWaiting)
        {
            GrannyAnim.Play("idle_4");
            stopSpeed = Granny.speed;
        }
        else if (status == GrannyStatus.GrannyisChasing)
        {
            Granny.stoppingDistance = 3f;
            Granny.SetDestination(player.position);
            canFadeMusik = true;

            GrannyAnim.Play("EasyGranny");
            followSpeed = Granny.speed;
        }
        else if(status == GrannyStatus.GrannyChasedWaiting)
        {
            GrannyAnim.Play("Look");
            stopSpeed = Granny.speed;

            canFadeMusik = false;
        }
        else if(status == GrannyStatus.GrannyCaughtPlayer)
        {
            GrannyAnim.Play("Hit_0");
            stopSpeed = Granny.speed;

            player.LookAt(granny);
            playerHead.LookAt(grannyHead);

            GrannyJumpscare.GetComponent<AudioSource>().Play();

            canFadeMusik = false;
            playerController.canMove = false;
            playerCam.canRotate = false;
        }
        else throw new System.NotImplementedException($"{status} not implemented");
    }

    public virtual void OnStatusExit(GrannyStatus status)
    {
        //Ending
        if (status == GrannyStatus.Start)
        {

        }
        else if (status == GrannyStatus.GrannyisSearching)
        {

        }
        else if (status == GrannyStatus.GrannySearchedWaiting)
        {

        }
        else if (status == GrannyStatus.GrannyisChasing)
        {
            Granny.stoppingDistance = 2f;
            canFadeMusik = false;
        }
        else if (status == GrannyStatus.GrannyChasedWaiting)
        {
            DefindGrannyDestination();
            UpdateGrannySounds();
        }
        else if (status == GrannyStatus.GrannyCaughtPlayer)
        {
            playerController.canMove = true;
            playerCam.canRotate = true;
            canFadeMusik = false;
        }
    }

    public virtual void OnStatusTick(GrannyStatus status, float duration)
    {
        //Updating
        if(status == GrannyStatus.Start)
        {
            ChangeStatus(GrannyStatus.GrannyisSearching);
        }
        else if (status == GrannyStatus.GrannyisSearching)
        {
            if (SeePlayer == true)
            {
                ChangeStatus(GrannyStatus.GrannyisChasing);
            }

            if (Granny.remainingDistance < Granny.stoppingDistance)
            {
                ChangeStatus(GrannyStatus.GrannySearchedWaiting);
            }
        }
        else if (status == GrannyStatus.GrannySearchedWaiting)
        {
            if (SeePlayer == true)
            {
                ChangeStatus(GrannyStatus.GrannyisChasing);
            }

            if (duration >= waypointTime)
            {
                ChangeStatus(GrannyStatus.GrannyisSearching);
            }
        }
        else if (status == GrannyStatus.GrannyisChasing)
        {
            if (duration >= playerSafeTime)
            {
                if (SeePlayer == false)
                {
                    ChangeStatus(GrannyStatus.GrannyChasedWaiting);
                }
            }

            if (Granny.remainingDistance < Granny.stoppingDistance)
            {
                ChangeStatus(GrannyStatus.GrannyCaughtPlayer);
            }
        }
        else if (status == GrannyStatus.GrannyChasedWaiting)
        {
            if (duration >= lookAroundTime)
            {
                ChangeStatus(GrannyStatus.GrannyisSearching);
            }

            if (SeePlayer == true)
            {
                ChangeStatus(GrannyStatus.GrannyisChasing);
            }
        }
        else if(status == GrannyStatus.GrannyCaughtPlayer)
        {

        }
        else throw new System.NotImplementedException($"{status} not implemented");
    }

    public virtual void FadeChaseMusik()
    {
        var audioComp = chaseMusik.GetComponent<AudioSource>();
        if (canFadeMusik == true)
        {
            chaseMusik.SetActive(true);
            audioComp.volume = audioComp.volume + 0.3f * Time.deltaTime;
        }
        else
        {
            audioComp.volume = audioComp.volume - 0.3f * Time.deltaTime;
            if (audioComp.volume <= 0f)
            {
                chaseMusik.SetActive(false);
            }
        }
    }

    public virtual void DefindGrannyDestination()
    {
        int randomIndex = Random.Range(0, patrolPoints.Length);
        Granny.SetDestination(patrolPoints[randomIndex].position);
    }


    public virtual void UpdateGrannySounds()
    {
        GetComponent<AudioSource>().clip = GrannySounds[Random.Range(0, GrannySounds.Length)];
        GetComponent<AudioSource>().Play();
    }

    public enum GrannyStatus
    {
        Start = 0,
        GrannyisSearching,
        GrannySearchedWaiting,
        GrannyisChasing,
        GrannyChasedWaiting,
        GrannyCaughtPlayer,
    }
}


public class PlayerController : MonoBehaviour
{
    public bool canMove;
}
public class CameraController : MonoBehaviour
{
    public bool canRotate;
}

public class PlayerComponentTypeName : MonoBehaviour
{
    
}




// using UnityEngine;

public class GrannyEyes : MonoBehaviour
{
	[SerializeField] Transform _playerCenter;
    [SerializeField] Transform _raycastOrigin;
    [SerializeField] LayerMask _raycastLayerMask = ~0;
	[SerializeField] float _detectionRadius = 5f;
    [SerializeField] float _coneHalfAngle = 45f;
	[SerializeField] GrannyEnemyAI _granny;

	void FixedUpdate()
	{
        Vector3 myPosition = _raycastOrigin.position;
        Vector3 myForward = _raycastOrigin.forward;
        Vector3 targetPosition = _playerCenter.position;
        Vector3 targetDir = targetPosition - myPosition;

        float angle = Vector3.Angle( targetDir , myForward );
        float distance = Vector3.Distance( myPosition , targetPosition );

        // 45 degrees half angle - means it's a detection cone of 90 degrees
        bool isInsideACone = angle<_coneHalfAngle && distance<_detectionRadius;

        bool canSeePlayer = false;
        if( isInsideACone && Physics.Raycast(_raycastOrigin.position,targetDir, out RaycastHit hit, _detectionRadius, _raycastLayerMask ) )
        {
            canSeePlayer = hit.collider.GetComponentInParent<PlayerComponentTypeName>()!=null;
        }

        _granny.SeePlayer = canSeePlayer;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos ()
    {
        // if( !Application.isPlaying ) FixedUpdate();

        // UnityEditor.Handles.arc
    }
    #endif
}
