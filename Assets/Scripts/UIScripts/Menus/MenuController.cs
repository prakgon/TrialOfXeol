using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [SerializeField] private GameObject title;

    [SerializeField] private GameObject start;
    [SerializeField] private GameObject startImage;
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject mainMenu;

    private Button[] buttons;

    [SerializeField] private InputAction anyKey;

    [SerializeField] private Animator fadeOutAnimator;
    [SerializeField] private Animator startAnimator;
    [SerializeField] private Animator startImageAnimator;
    private static readonly int FadeOut = Animator.StringToHash("FadeOut");

    [SerializeField] private AudioClip startAudioClip;

    [SerializeField] private bool goNextPhase;

    public bool GoNextPhase
    {
        set => goNextPhase = value;
    }

    private void Awake()
    {
        Instance = this;
        buttons = gameObject.GetComponentsInChildren<Button>();
        //buttons.ToList().ForEach(b => b.onClick.AddListener(() => OnButtonClick(b)));
        buttons.ToList().ForEach(b => b.interactable = false);
    }

    private void Start()
    {
        //anyKey.AddBinding("<Gamepad>/*");
        anyKey.AddBinding("<Mouse>/leftButton");
        anyKey.AddBinding("<Mouse>/rightButton");
        anyKey.AddBinding("<Keyboard>/anyKey");
        anyKey.AddBinding("<Gamepad>/start");
        anyKey.AddBinding("<Gamepad>/buttonNorth");
        anyKey.AddBinding("<Gamepad>/buttonSouth");
        anyKey.AddBinding("<Gamepad>/buttonEast");
        anyKey.AddBinding("<Gamepad>/buttonWest");

        anyKey.performed += AnyKeyPerformed;

        anyKey.Enable();

        StartCoroutine(StartMenu());
    }


    private void AnyKeyPerformed(InputAction.CallbackContext context)
    {
        if (goNextPhase && start.activeSelf)
        {
            startAnimator.SetTrigger(FadeOut);
            fadeOutAnimator.SetTrigger(FadeOut);
            startImageAnimator.SetTrigger(FadeOut);
            AudioMenuManager.Instance.PlayOneShot(startAudioClip);
            anyKey.performed -= AnyKeyPerformed;
            PointerController.Instance.pointerPosition.performed += PointerController.Instance.PointerPerformed;
        }
    }

    private IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(3.3f);
        title.SetActive(true);
        yield return new WaitForSeconds(2f);
        start.SetActive(true);
    }

    public void ShowMenu()
    {
        mainMenu.SetActive(true);
        buttons.ToList().ForEach(b => b.interactable = true);
    }
    
    public void DestroyStart()
    {
        Destroy(start);
    }

    public void CloseDialog(GameObject dialog) => dialog.SetActive(false);
    public void OpenDialog(GameObject dialog) => dialog.SetActive(true);

    public void SetSelected(GameObject selected) => EventSystem.current.SetSelectedGameObject(selected);

    public void Exit() => StartCoroutine(QuitGame());

    public void DestroyCanvas() => Destroy(startCanvas);
    public void DestroyInitialText() => Destroy(start);
    public void DestroyInitialImage() => Destroy(startImage);

    private IEnumerator QuitGame()
    {
        PointerController.Instance.DisableAll();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }
}