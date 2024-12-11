using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    Animator anim;
    public LayerMask movementMask;  
    NavMeshAgent agent;
    Camera cam;        
    PlayerMotor motor;  

    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<PlayerMotor>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        movementMask = LayerMask.GetMask("ground");
    }

    void Update()
    {

       

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                motor.MoveToPoint(hit.point);
                anim.SetBool("isWalking", true);
                agent.updateRotation = true;
            }
        }

    }

}