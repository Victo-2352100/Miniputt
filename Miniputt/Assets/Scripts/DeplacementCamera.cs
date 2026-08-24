using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementCamera : MonoBehaviour
{
    private Vector3 deplacement;
    [SerializeField]
    private float vitesseDeplacement = 5.0f;
    [SerializeField]
    private PlayerInput controles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {

    }
    private void ArreterDeplacement(InputAction.CallbackContext contexte)
    {

    }

    private void GererDeplacement(InputAction.CallbackContext contexte) {
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
