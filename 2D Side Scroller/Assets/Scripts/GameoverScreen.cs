using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverScreen : MonoBehaviour
{
    private Button buttonRetry;
    // Start is called before the first frame update
    void Start()
    {
        buttonRetry = GetComponentInChildren<Button>();
        buttonRetry.onClick.AddListener(GameManager.current.PlayerRespawned);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
