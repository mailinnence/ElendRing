using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{

    public class PlayerIputManager : MonoBehaviour
    {

        public static PlayerIputManager instance;

        // THINK ABOUT GOALS IN STEPS
        // 1. FIND A WAY TO READ THE VALUES OF A JOY STICK
        // 2. MOVE CHARCTER BASED ON THOSE VALUES

        PlayerControls PlayerControls;    

        [SerializeField] Vector2 MovementInput;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }


        private void Start()
        {
            DontDestroyOnLoad(gameObject); 


            // WHEN THE SCENE CHANGES , RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false; 
        }


        private void OnSceneChange(Scene oldScene , Scene newScene)
        {
            // IF WE ARE LOADING INTO OUR WORLD SCENE , ENABLE OUR PLAYERS CONTROLS
            if(newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex() )
            {
                instance.enabled = true;
            }
            // OTHERWISE WE MUST BE AT THE MAIN MENU , DISABLE OUR PLAYERS CONTROLS
            // THIS IS SO OUR PLAYER CAN'T MOVE AROUND IF WE ENTER THINGS LIKE A CHARACTER CREATION MENU ECT
            else
            {
                instance.enabled = false;
            }
        }


        private void OnEnable()
        {
            if (PlayerControls == null)
            {
                PlayerControls = new PlayerControls();
                
                PlayerControls.PlayerMovement.Movement.performed += i => MovementInput = i.ReadValue<Vector2>();
            }

            PlayerControls.Enable();
        }

        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT , UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }


    }
}
