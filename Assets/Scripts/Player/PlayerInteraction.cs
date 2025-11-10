using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Transform cameraTransform;  // your main camera
    public float pickupRange = 5f;
    public float holdDistance = 3f;
    public LayerMask pickupLayer;

    private Rigidbody heldObject;
    private bool isHolding = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHolding)
            {
                DropObject();
            }
            else
            {
                TryPickup();
            }
        }

        if (isHolding && heldObject != null)
        {
            MoveHeldObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = rb;
                heldObject.useGravity = false;
                heldObject.linearDamping = 10f;
                heldObject.constraints = RigidbodyConstraints.FreezeRotation;
                isHolding = true;
            }
        }
    }

    void MoveHeldObject()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * holdDistance;
        Vector3 moveDirection = targetPosition - heldObject.position;
        heldObject.linearVelocity = moveDirection * 10f; // smooth movement
    }

    void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject.constraints = RigidbodyConstraints.None;
        heldObject = null;
        isHolding = false;
    }
}
