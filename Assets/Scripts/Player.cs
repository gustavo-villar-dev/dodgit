using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float xMin = -2.1f;
    private float xMax = 2.1f;
    private float speed = 0.08f;
    private float stamina = 500f;
    private float stopCostPerSecond = 100f;
    private float recoverStaminaPerSecond = 100f;
    public Image staminaBar;

    public GameObject gameController;

    private int isGrowing;
    private float xPos;
    private float originalPos;
    private bool isMoving = true;
    private float timeSinceStopped = 0f;

    void Start()
    {
        originalPos = transform.position.x;
        isGrowing = 1;
    }

    void Update()
    {
        HandleKeyInput();

        UpdateStamina();

        if(isMoving){
            MoveBackAndForth();
        }        
    }

    private void MoveBackAndForth()
    {
        xPos = transform.position.x - originalPos;
        
        if(isGrowing == 1){

            if(xPos >= xMax){
                isGrowing = 0;
                xPos -= speed;
            }

            else{
                xPos +=speed;
            }
        }

        else {

            if(xPos <= xMin){
                isGrowing = 1;
                xPos += speed;
            }
        
            else{
                xPos -= speed;
            }
        }

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    private void HandleTouchInput()
    {
        // Stop movement
        if ((Input.touchCount > 0))
        {
        
            // Stop player if touching and there is enough stamina
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (stamina >= stopCostPerSecond)
                {
                    Time.timeScale = 0.8f;
                    isMoving = false;
                    timeSinceStopped = 0f;
                }

                else {
                    Time.timeScale = 1f;
                    isMoving = true;
                }
            }

            // Start moving again if touch ends
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isMoving = true;
                Time.timeScale = 1f;
            }
        }
    }

    private void HandleKeyInput()
    {
        if (Input.GetKey(KeyCode.Space) && (stamina >= stopCostPerSecond)) 
        {
            Time.timeScale = 0.7f;
            isMoving = false;
            timeSinceStopped = 0f;
        }           

        else if(Input.GetKeyUp(KeyCode.Space) || (stamina <= stopCostPerSecond)) 
        {
           Time.timeScale = 1f;
           isMoving = true;
        }

        else 
        {
            Time.timeScale = 1f;
            isMoving = true;
        }
    }
    
    private void UpdateStamina()
    {
        // Reduce stamina if player is stopped
        if (!isMoving)
        {
            timeSinceStopped += Time.deltaTime;
            stamina -= stopCostPerSecond * timeSinceStopped;
        }

        // Increase stamina if player is moving
        else
        {
            stamina += recoverStaminaPerSecond * Time.deltaTime;
        }

        // Clamp stamina value
        stamina = Mathf.Clamp(stamina, 0, 500f);
        staminaBar.fillAmount = stamina/500;
    }

    //call die function if meteor hits player
    void OnTriggerEnter2D (Collider2D col) {
        if (col.CompareTag("Meteor") == true) {
            gameController.GetComponent<LevelControl>().Die();
        }
    }
}
