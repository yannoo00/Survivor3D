using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트



public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도


    //마우스 바라보기
    //Camera viewCamera;


    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기

        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();


        //Look At Camera
    //     viewCamera = Camera.main;
    // }
    
    // public void LookAt(Vector3 lookPoint)
    // {
    //     Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
    //     transform.LookAt(heightCorrectedPoint);
        }
    


   

    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행


        // //Look At Camera
        // Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        // Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        // float rayDistance;

        // if(groundPlane.Raycast(ray, out rayDistance))
        // {
        //     Vector3 point = ray.GetPoint(rayDistance);
        //     LookAt(point);
        // }
        // //~Look At Camera




        Rotate();

        Move();

        
        if(playerInput.move != 0 && playerInput.move + playerInput.rotate == 0) //대각으로 움직이는 상황
        {
            playerAnimator.SetFloat("Move", playerInput.move);
        }

        else
        {
            playerAnimator.SetFloat("Move", playerInput.move + playerInput.rotate);
        }
        
    }




    private void Move() {

        Vector3 moveDistance = 
        playerInput.move * Vector3.forward * moveSpeed * Time.deltaTime; 
        //playerInput.move * Vector3.right * moveSpeed * Time.deltaTime; 

        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance); 
    }




   
    private void Rotate() {

        Vector3 turn = 
        
        //playerInput.rotate * transform.right * moveSpeed * Time.deltaTime; =>  바라보는 방향 기준 움직임
        playerInput.rotate * Vector3.right * moveSpeed * Time.deltaTime;
        

        playerRigidbody.MovePosition(playerRigidbody.position + turn);


        //playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }
}