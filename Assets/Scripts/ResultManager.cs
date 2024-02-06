using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text resultText;

    // Start is called before the first frame update
    void Start()
    {
        resultText.text = GameManager.result;
        Debug.Log(GameManager.result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }
}
