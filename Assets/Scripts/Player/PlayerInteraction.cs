using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform cameraTransform;
    public float pickupRange = 7f;
    public float holdDistance = 3f;
    public LayerMask pickupLayer;

    private Rigidbody heldObject;
    private MonsterManager heldMonster;
    private bool isHolding;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding) DropObject();
            else TryPickup();
        }

        if (isHolding && heldObject != null)
            MoveHeldObject();
    }

    void TryPickup()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb == null) return;

            heldObject = rb;
            heldMonster = rb.GetComponent<MonsterManager>();

            // Turn off physics while held
            heldObject.useGravity = false;
            heldObject.linearDamping = 10f;
            heldObject.constraints = RigidbodyConstraints.FreezeRotation;

            if (heldMonster != null)
                heldMonster.OnPickedUp();

            isHolding = true;
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        Vector3 moveDirection = targetPosition - heldObject.position;

        float moveSpeed = 10f;
        heldObject.linearVelocity = moveDirection * moveSpeed;
    }

    void DropObject()
    {
        if (heldObject == null) return;

        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject.constraints = RigidbodyConstraints.None;

        if (heldMonster != null)
        {
            heldMonster.OnDropped();
            heldMonster = null;
        }

        heldObject = null;
        isHolding = false;
    }
}
