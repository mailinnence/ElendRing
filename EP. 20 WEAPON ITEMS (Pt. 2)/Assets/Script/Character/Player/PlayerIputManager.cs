using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SG
{

    public class PlayerIputManager : MonoBehaviour
    {

        // THINK ABOUT GOALS IN STEPS
        // 1. FIND A WAY TO READ THE VALUES OF A JOY STICK
        // 2. MOVE CHARCTER BASED ON THOSE VALUES


        // 싱글톤 패턴 인스턴스
        public static PlayerIputManager instance;
        public PlayerManager player;

        // PlayerControls 가져오기
        PlayerControls PlayerControls;    

        // 움직임을 입력받을 변수 만들기
        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput; // OnEnable의 performed 함수에 의해서 값이 초기화 된다.
        public float verticalInput;             // 수직축 이동
        public float horizontalInput;           // 평행축 이동
        public float moveAmount;  

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;   // OnEnable의 performed 함수에 의해서 값이 초기화 된다.
        public float cameraVerticalInput;       // 수직축 이동
        public float cameraHorizontalInput;     // 평행축 이동

     
        [Header("PLAYER ACTION INPUT")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;   
 
        // 싱글톤 패턴
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

            // 시작하면 객체가 씬 이동시 파괴되지 않게 설정한다
            DontDestroyOnLoad(gameObject); 

            // WHEN THE SCENE CHANGES , RUN THIS LOGIC
            // 씬이 바뀌면 이전 씬과 현재씬을 입력한다.
            SceneManager.activeSceneChanged += OnSceneChange;

            // 시작하면 인스턴스를 비활성화 시킨다.
            instance.enabled = false; 
        }

        // 현재 씬이 인게임 씬일때만 활성화한다.
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

        // OnEnable() 메서드는 객체가 활성화될 때 호출되며, 입력 시스템을 설정하고 활성화 한다.
        private void OnEnable()
        {
            // PlayerControls가 없을때 
            if (PlayerControls == null)
            {
                // 생성 및 초기화   
                PlayerControls = new PlayerControls();
                
                // i => MovementInput = i.ReadValue<Vector2>()는 람다 식으로, 
                // 입력 이벤트가 발생하면 MovementInput 변수에 현재 입력 값을 Vector2 형식으로 저장합니다. 
                // 여기서 Vector2는 2D 공간에서의 방향과 크기를 나타내는 구조체
                PlayerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                PlayerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                PlayerControls.PlayerAction.Dodge.performed += i => dodgeInput = true;
                PlayerControls.PlayerAction.Jump.performed += i => jumpInput = true;


                // HOLDING THE INPUT, SETS THE BOOL TO TRUE
                PlayerControls.PlayerAction.Sprint.performed += i => sprintInput = true; 
                // RELEASING THE INPUT, SETS THE BOOL TO FALSE
                PlayerControls.PlayerAction.Sprint.canceled += i => sprintInput = false;        

                  
            }

            PlayerControls.Enable();
        }
        
        private void OnDestroy()
        {
            // IF WE DESTROY THIS OBJECT , UNSUBSCRIBE FROM THIS EVENT
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // IF WE MINIMIZE OR LOWER THE WINDOW, STOP ADJUSTING INPUTS
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    PlayerControls.Enable();
                }
                else
                {
                    PlayerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInput();
        }


        private void HandleAllInput()
        {
            HandlePlayerMovementIuput();
            HandleCameraMovementIuput();
            HandleDodgeInput();
            HandelSpringting();
            HandelJumpting();
        }


        // MOVEMENT
        private void HandlePlayerMovementIuput()
        {
            verticalInput = movementInput.y;    // movementInput.y는 플레이어가 세로 방향(상하)으로 입력한 값 >> 위쪽으로 이동하면 1, 아래쪽으로 이동하면 -1
            horizontalInput = movementInput.x;  // movementInput.x는 플레이어가 가로 방향(좌우)으로 입력한 값 >> 오른쪽으로 이동하면 1, 왼쪽으로 이동하면 -1

            // RETURN THE ABSOULTE NUMBER , (Meaning number without the negative sign , so its always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // 최소한의 이동 강도로 설정 
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            // 최대한의 이동 강도로 설정
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
          
            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON-STRAFING MOVEMENT
            // WE USE THE HORIZONTAL WHEN WE ARE STRAFING OR LOCKED ON

            if(player == null)
                return;
            // IF WE ARE NOT LOCKED ON, ONLY USE THE MOVE AMOUNT
            player.playerAnimationManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
        
            // IF WE ARE LOCKED ON PASS THE HORIZONTAL MOVEMENT AS WELL
                    
        }

        private void HandleCameraMovementIuput()
        {
            cameraVerticalInput = cameraInput.y;    // movementInput.y는 플레이어가 세로 방향(상하)으로 입력한 값 >> 위쪽으로 이동하면 1, 아래쪽으로 이동하면 -1
            cameraHorizontalInput = cameraInput.x;  // movementInput.x는 플레이어가 가로 방향(좌우)으로 입력한 값 >> 오른쪽으로 이동하면 1, 왼쪽으로 이동하면 -1
        }

        // ACTION
        private void HandleDodgeInput()
        {

            if(dodgeInput)
            {
                dodgeInput = false;

                // FUTURE NOTE : RETURN (DO NOTHING) IF MENU OR UI WINDOW IS OPEN

                player.playerLocomotionManager.AttempToPerformDodge();
            }

        }

        private void HandelSpringting()
        {
            sprintInput = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if(sprintInput)
            {
                player.playerLocomotionManager.HandelSpringting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }

        private void HandelJumpting()
        {
            if(jumpInput)
            {
                jumpInput = false;

                // IF WE HAVE A AUI WINDOW OPEN , SIMPLY RETURN WITHOUT DOING ANYTHING
            
                // ATTEMPT TO PERFORM JUMP
                player.playerLocomotionManager.AttempToPerformJump();

            }
        }



    }
}

