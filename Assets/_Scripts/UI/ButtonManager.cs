using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class ButtonManager : MonoBehaviour
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


    private void Awake()
    {
        buttonStartPos = targetObject.anchoredPosition;
        parentStartPos = contentParent.anchoredPosition;
    }

    private void Start()
    {
        targetContent.SetActive(false);
    }

    public void OnButtonPointerEnter()
    {
        if (!isSelected)
            StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, hoverTargetPos, buttonLerpTime));
    }

    public void OnButtonPointerExit()
    {
        if (!isSelected)
            StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, buttonStartPos, buttonLerpTime));
    }

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

    public void CloseMenu()
    {
        OnButtonDeselect();
        StartCoroutine(um.Lerp(contentParent, contentParent.anchoredPosition, parentStartPos, contentLerpTime));
    }

    public void OnButtonDeselect()
    {
        isSelected = false;
        StartCoroutine(um.Lerp(targetObject, targetObject.anchoredPosition, buttonStartPos, buttonLerpTime));
    }

    public void HideContent()
    {
        targetContent.SetActive(false);
    }
}
