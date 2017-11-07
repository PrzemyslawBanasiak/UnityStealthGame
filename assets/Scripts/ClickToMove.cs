using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour {
    RaycastHit hitInfo = new RaycastHit();
    IClickable target;
    UnityEngine.AI.NavMeshAgent agent;

    void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void HandleClickAble(Collider collider){
        target = collider.gameObject.GetComponent<IClickable>();
        if(target == null) {
            Debug.Log("Object Clickable but doesn't implement IClickable");
        } else {
            target.OnClick();
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if(target) {
                target.Reset();
                target = null;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) {
                if(hitInfo.collider.tag == "ClickAble") {
                    HandleClickAble(hitInfo.collider);
                } else {
                    if(!agent.isStopped)
                        agent.destination = hitInfo.point;
                }
            }
        }
    }

    public void Die()
    {
        GetComponent<Animator>().SetTrigger("die");
        agent.isStopped = true;
    }
}