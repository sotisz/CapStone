using UnityEngine;
<<<<<<< Updated upstream

public class GameManager : MonoBehaviour
{
    public static string gameState = "playing";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
=======
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static string gameState = "playing";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;

>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
