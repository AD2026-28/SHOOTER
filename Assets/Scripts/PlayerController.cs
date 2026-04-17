using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public int lives;
    private float speed;
    private int weaponType;

    private GameManager gameManager;

    private float horizontalInput;
    private float verticalInput;

    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject thrusterPrefab;
    public GameObject shieldPrefab;

    public bool shieldActive;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lives = 3;
        speed = 5.0f;
        weaponType = 1;
        gameManager.ChangeLivesText(lives);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting();
    }

    public void LoseALife()
    {
        if (!shieldActive)
        {
            lives--;
        }
        if (shieldActive)
        {
            shieldPrefab.SetActive(false);
            shieldActive = false;

        }
        //lives = lives - 1;
        //lives -= 1;
        gameManager.ChangeLivesText(lives);
        if (lives == 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            gameManager.GameOver();


            Destroy(this.gameObject);
        }
    }


    IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(5);
        shieldPrefab.SetActive(false);
        shieldActive = false;
        gameManager.PlaySound(2);
        gameManager.ManagePowerupText(5);
    }
    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5);
        speed = 5f;
        thrusterPrefab.SetActive(false);
        gameManager.PlaySound(2);
        gameManager.ManagePowerupText(5);
    }
    IEnumerator WeaponPowerDown()
    {
        yield return new WaitForSeconds(5);
        weaponType = 1;
        gameManager.PlaySound(2);
        gameManager.ManagePowerupText(5);
    }

    private void OnTriggerEnter2D(Collider2D whatDitIHit)
    {
        if (whatDitIHit.tag == "PowerUp")
        {
            Destroy(whatDitIHit.gameObject);
            int whichPowerup = Random.Range(1, 5);
            gameManager.PlaySound(1);
            switch (whichPowerup)
            {
                case 1:
                    speed = 10f;
                    // start speed corotuine down 
                    // thruster active 
                    thrusterPrefab.SetActive(true);
                    gameManager.ManagePowerupText(1);
                    StartCoroutine(SpeedPowerDown());
                    break;
                case 2:
                    // set weapon type to 2
                    // weapon power down coroutine 
                    weaponType = 2;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(2);
                    break;
                case 3:
                    // set weapon type to 3
                    // weapon power dwon coroutine 
                    weaponType = 3;
                    StartCoroutine(WeaponPowerDown());
                    gameManager.ManagePowerupText(3);
                    break;
                case 4:
                    //set the sheild active - turn on a bool and a game object
                    // set the sheild power down coroutine 
                    shieldPrefab.SetActive(true);
                    shieldActive = true;
                    gameManager.ManagePowerupText(4);
                    StartCoroutine(ShieldPowerDown());
                    break;
            }

        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (weaponType)
            {
                case 1:
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(bulletPrefab, transform.position + new Vector3(-0.5f, 0.5f, 0), Quaternion.Euler(0, 0, 45));
                    Instantiate(bulletPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    Instantiate(bulletPrefab, transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.Euler(0, 0, -45));
                    break;
            }
        }
    }

    void Movement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * speed);

        float horizontalScreenLimit = gameManager.horizontalScreenSize;
        float verticalScreenLimit = 0;

        if (transform.position.x <= -horizontalScreenLimit || transform.position.x > horizontalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }

        //if (transform.position.y <= -verticalScreenSize || transform.position.y > verticalScreenSize)
        //{
        //    transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        //}
        if (transform.position.y > verticalScreenLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0);
        }
        if (transform.position.y <= -3.25)
        {
            transform.position = new Vector3(transform.position.x, -3.25f, 0);
        }
    }
}
