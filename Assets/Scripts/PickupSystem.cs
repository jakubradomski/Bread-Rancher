using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public float MaxDistance = 5.0f;
    public float MaxWeight = 25.0f;
    public float HeldDistance = 1.5f;
    public bool IgnoreWeight = false;
    public float TransitionTime = 5.0f;

    public Camera HeadCamera;

    private Rigidbody _heldObjectBody;
    private PickupableObject _heldObject;
    private bool _isHolding = false;
    private bool _doneTransition = true;

    public void PickUp()
    {
        if (_isHolding)
        {
            DropObject();
        }
        else
        {
            Ray front = new Ray(HeadCamera.transform.position, HeadCamera.transform.forward);

            if (Physics.Raycast(front, out RaycastHit hit, MaxDistance))
            {
                if (hit.rigidbody == null)
                    return;
                if (hit.rigidbody.mass <= MaxWeight || IgnoreWeight)
                {
                    var obj = hit.rigidbody.GetComponent<PickupableObject>();
                    if (obj == null)
                        return;

                    _heldObject = obj;
                    _heldObjectBody = hit.rigidbody;
                    _isHolding = true;
                    StartCoroutine(MoveToHand());

                    _heldObjectBody.isKinematic = true;
                    _heldObject.gameObject.layer = LayerMask.NameToLayer("Ignore Player");
                }
            }
        }
    }

    void DropObject()
    {
        if (!_doneTransition)
        {
            _doneTransition = true;
            StopCoroutine(MoveToHand());
        }

        _heldObject.gameObject.layer = LayerMask.NameToLayer("Default");
        _heldObjectBody.isKinematic = false;

        _heldObjectBody = null;
        _heldObject = null;
        _isHolding = false;
    }

    private Vector3 GetHeldPosition()
    {
        float dist = (float)_heldObject.HeldDistance / 10;

        Vector3 front = HeadCamera.transform.forward * HeldDistance * dist;
        Vector3 headPos = HeadCamera.transform.position;

        return front + headPos;
    }

    IEnumerator MoveToHand()
    {
        _doneTransition = false;
        float time = 0;
        Vector3 startPos = _heldObject.transform.position;

        while(time < TransitionTime) 
        {
            Vector3 pos = Vector3.Lerp(startPos, GetHeldPosition(), time / TransitionTime);
            _heldObjectBody.MovePosition(pos);
            time += Time.deltaTime;
            yield return null;
        }

        _doneTransition = true;
    }


    public void LateUpdate()
    {
        if (_isHolding && _doneTransition)
        {
            _heldObjectBody.transform.position = GetHeldPosition();
        }
    }
}
