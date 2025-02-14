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

        // PlayerControls 가져오기
        PlayerControls PlayerControls;    

        // 움직임을 입력받을 변수 만들기
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput; 
        public float moveAmount; 

 
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
            }

            PlayerControls.Enable();
        }
        
        // OnDestroy 메서드는 객체가 파괴될 때 호출한다.
        // 만약 오브젝트가 파괴되면 이전씬으로 되돌아갈테니 -=을 통해서 다시 인게임으로 넘어오기전까지 비활성화한다.
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
            HandleMovementIuput();
        }


        private void HandleMovementIuput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;  

            // RETURN THE ABSOULTE NUMBER , (Meaning number without the negative sign , so its always positive)
            // 절대값을 반환합니다 (즉, 음수 부호를 제거한 숫자로 항상 양수로 반환합니다).
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        
            // WE CLAMP THE VALUES , SO THEY ARE 0 , 0.5 OR 1 (OPTIONAL)
            // 값을 0, 0.5 또는 1로 제한합니다 (선택 사항)
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }
        
        }




    }
}

