using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player2dScript : MonoBehaviour
{
    [SerializeField]
    private InputActionReference  attack, pointerPosition;
    public Vector2 pointerInput;
    private WeaponParent weaponParent;

    private void OnEnable()
    {
        attack.action.performed += PerformAttack;
    }

    private void OnDisable()
    {
        attack.action.performed -= PerformAttack;
    }

    private void PerformAttack(InputAction.CallbackContext context)
    {
        weaponParent.Attack();
    }

    void Start()
    {
        
    }

    private void Awake()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void Update()
    {

        pointerInput = GetPointerInput();
        weaponParent.Pointerposition = pointerInput;

    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
