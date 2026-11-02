using UnityEngine;
using UnityEngine.UI;
using static UIManager;

// This controls the logic and animation of the resource buttons

// By OS

public class ResourceButton : MonoBehaviour
{
    [SerializeField]
    private RectTransform targetObject;
    [SerializeField]
    private Button button;
    [SerializeField]
    private RectTransform contentParent;
    [SerializeField]
    private GameObject targetContent;
    [Space(5)]

    [SerializeField]
    private Vector2 hoverTargetPos;
    [SerializeField]
    private Vector2 clickTargetPos;
    [SerializeField]
    private Vector2 contentParentTargetPos;
    [Space(10)]

    [SerializeField]
    private float buttonLerpTime;
    [SerializeField]
    private float contentLerpTime;

    private Vector2 buttonStartPos;
    private Vector2 parentStartPos;
    public bool isSelected { get; private set; } = false;


    //Set the starting position of the UI
    private void Awake()
    {
        buttonStartPos = targetObject.anchoredPosition;
        parentStartPos = contentParent.anchoredPosition;
    }

    private void Start()
    {
        targetContent.SetActive(false);
    }

    // Event to move the UI out slightly
    public void OnButtonPointerEnter()
    {
        if (!isSelected)
            StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, hoverTargetPos, buttonLerpTime));
    }

    // Event to move the UI back to original position
    public void OnButtonPointerExit()
    {
        if (!isSelected)
            StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, buttonStartPos, buttonLerpTime));
    }

    // Event to fully move out button when clicked
    public void OnButtonSelect()
    {
        if (!isSelected)
        {
            isSelected = true;
            StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, clickTargetPos, buttonLerpTime));

            targetContent.SetActive(true);

            StartCoroutine(um.Lerp(contentParent, contentParent.anchoredPosition, contentParentTargetPos, contentLerpTime));
        }
        else
        {
            CloseMenu();
        }
    }

    // Event to close menu and close button
    public void CloseMenu()
    {
        OnButtonDeselect();
        StartCoroutine(um.Lerp(contentParent, contentParent.anchoredPosition, parentStartPos, contentLerpTime));
    }

    // Event to close button
    public void OnButtonDeselect()
    {
        isSelected = false;
        StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, buttonStartPos, buttonLerpTime));
    }

    // Event to hide content
    public void HideContent()
    {
        targetContent.SetActive(false);
    }
}
