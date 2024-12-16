using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipObject; 
    [SerializeField] private Vector2 offset = new Vector2(10, -10); 

    private void Update()
    {
        if (tooltipObject.gameObject.activeSelf)
        {
            Vector3 mousePosition = Input.mousePosition;
            tooltipObject.position = mousePosition + (Vector3)offset;
        }
    }

    public void ShowTooltip(string tooltipText)
    {
        tooltipObject.gameObject.SetActive(true);
        tooltipObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tooltipText;
    }

    public void HideTooltip()
    {
        tooltipObject.gameObject.SetActive(false);
    }
}
