using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionCamera : MonoBehaviour
{

    [SerializeField, Tooltip("Cameras disponibles")]
    private List<CinemachineCamera> camerasDisponibles;
    private CinemachineCamera camActive;
    [SerializeField]
    private PlayerInput controles;

    private void Awake()
    {
        //Activer la première caméra et laisser les autres désactivées
        camActive = camerasDisponibles[0];
        camActive.gameObject.SetActive(true);
        for (int i = 1; i < camerasDisponibles.Count; i++) {
            camerasDisponibles[i].gameObject.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InputAction actionChanger = controles.actions.FindAction("player/ChangerCamera");
        actionChanger.performed += ChangerCamera;
    }

    private void ChangerCamera(InputAction.CallbackContext contexte)
    {
        //Si la caméra qui était active était la dernière sur la liste
        if (camActive == camerasDisponibles[camerasDisponibles.Count - 1])
        {
            //On désactive la caméra présente
            camActive.gameObject.SetActive(false);
            //On change pour la caméra au début de la liste
            camActive = camerasDisponibles[0];
            //Et on mets le gameObject actif
            camActive.gameObject.SetActive(true);
            for (int i = 1; i < camerasDisponibles.Count; i++)
            {
                camerasDisponibles[i].gameObject.SetActive(false);
            }
        }
        else
        {
            //On désactive la caméra actuelle
            camActive.gameObject.SetActive(false);
            //On trouve le prochain index et on l'assigne en actif
            camActive = camerasDisponibles[camerasDisponibles.IndexOf(camActive) + 1];
            //On l'active
            camActive.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        InputAction actionChanger = controles.actions.FindAction("player/ChangerCamera");
        actionChanger.performed -= ChangerCamera;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
