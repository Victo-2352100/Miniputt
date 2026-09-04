using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementCamera : MonoBehaviour
{
    [SerializeField]
    private float vitesseDeplacement = 5.0f;
    [SerializeField]
    private float vitesseRotationX = 10.0f;
    [SerializeField]
    private float vitesseRotationY = 10.0f;
    [SerializeField]
    private PlayerInput controles;
    [SerializeField, Tooltip("Volume qui confine la camera")]
    private BoxCollider collider;

    [SerializeField, Tooltip("Limites de rotation selon les angles (x, y, z)")]
    private Vector2 LimitesRotation;
    private Vector2 deplacement;

    //Angle de la rotation
    private float rotationX;
    private float rotationY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed += CommencerDeplacement;
        actionDeplacement.canceled += ArreterDeplacement;

        InputAction actionRotationX = controles.actions.FindAction("player/RotationX");
        actionRotationX.performed += CommencerRotationX;
        actionRotationX.canceled += ArreterRotationX;

        InputAction actionRotationY = controles.actions.FindAction("player/RotationY");
        actionRotationY.performed += CommencerRotationY;
        actionRotationY.canceled += ArreterRotationY;
    }
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = contexte.ReadValue<Vector2>().normalized;
    }
    private void ArreterDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = Vector2.zero;
    }


    private void CommencerRotationX(InputAction.CallbackContext contexte)
    {
        rotationX = vitesseRotationX * contexte.ReadValue<float>();
    }
    private void ArreterRotationX(InputAction.CallbackContext contexte)
    {
        rotationX = 0.0f;
    }

    private void CommencerRotationY(InputAction.CallbackContext contexte)
    {
        rotationY = vitesseRotationY * contexte.ReadValue<float>();
    }
    private void ArreterRotationY(InputAction.CallbackContext contexte)
    {
        rotationY = 0.0f;
    }
    private void GererDeplacement() {
        if (deplacement.sqrMagnitude > 0.0f) //Si on se déplace réellement
        {
            Vector3 positionDeplacement = transform.forward * deplacement.y * Time.deltaTime
            + transform.right * deplacement.x * Time.deltaTime;
            deplacement.Normalize();
            deplacement *= vitesseDeplacement;
            Vector3 nouvellePosition = transform.position + positionDeplacement;
            if (collider.bounds.Contains(nouvellePosition))
            {
                Debug.Log("Voila");
                transform.position += positionDeplacement;
            }
            
        }
    }

    private void EffectuerRotationX()
    {
        transform.Rotate(new Vector3(0.0f, rotationX * Time.deltaTime, 0.0f), Space.World); //La rotation sur X se basera sur l'axe de l'environnement
        //Ainsi, si l'objet est incliné (/), la rotation à 180° le transformera en (\)
    }

    private void EffectuerRotationY()
    {
        float angleRotationY = (transform.localEulerAngles.x + rotationY * Time.deltaTime) % 360; //(%360 pour par que la valeur de rotation dépasse 360°

        if (angleRotationY < LimitesRotation.x || angleRotationY > LimitesRotation.y)
        {
            transform.Rotate(new Vector3(rotationY * Time.deltaTime, 0.0f, 0.0f), Space.Self);
        }
    }

    private void OnDestroy()
    {
        if (controles == null) return;
        InputAction actiondeplacement = controles.actions.FindAction("player/DeplacerCamera");
        InputAction actionRotationX = controles.actions.FindAction("player/RotationX");
        InputAction actionRotationY = controles.actions.FindAction("player/RotationY");
        actiondeplacement.performed -= CommencerDeplacement;
        actiondeplacement.canceled -= ArreterDeplacement;

        actionRotationX.performed -= CommencerRotationX;
        actionRotationX.canceled -= ArreterRotationX;

        actionRotationY.performed -= CommencerRotationY;
        actionRotationY.canceled -= ArreterRotationY;

    }
    // Update is called once per frame
    void Update()
    {
        GererDeplacement();
        EffectuerRotationX();
        EffectuerRotationY();
    }
}
