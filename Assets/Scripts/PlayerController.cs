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
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

            // Log hitting directly
            //bool isHit = Physics.Raycast(ray, out hit, 100);
            //Debug.Log("Raycast Hit: " + isHit + " Collider: " + hit.collider);
            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {            
                //Debug.Log(hit.transform.position);
                motor.MoveToPoint(hit.point);
                anim.SetBool("isWalking", true);
                agent.updateRotation = true;
            }
        }

    }

}