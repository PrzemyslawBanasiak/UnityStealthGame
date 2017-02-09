using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour {
    RaycastHit hitInfo = new RaycastHit();
    IClickable target;
    NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void handleClickAble(Collider collider){
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
                    handleClickAble(hitInfo.collider);
                } else {
                    agent.destination = hitInfo.point;
                }
            }
        }
    }
}