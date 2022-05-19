using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CloseDialog(GameObject dialog) => dialog.SetActive(false);
    public void OpenDialog(GameObject dialog) => dialog.SetActive(true);

    public void SetSelected(GameObject selected) => EventSystem.current.SetSelectedGameObject(selected);

    public void Exit() => StartCoroutine(QuitGame());
    
    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}