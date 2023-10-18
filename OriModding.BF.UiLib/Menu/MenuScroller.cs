using UnityEngine;

namespace OriModding.BF.UiLib.Menu;

public class MenuScroller : MonoBehaviour
{
    CleverMenuItemSelectionManager selectionManager;
    Transform pivot;

    Transform tooltip;
    Vector3 tooltipPos;

    public float lerpSpeed = 0.5f;

    void Awake()
    {
        selectionManager = GetComponent<CleverMenuItemSelectionManager>();
        pivot = transform.Find("highlightFade/pivot");

        tooltip = pivot.Find("tooltip(Clone)");
        if (tooltip)
            tooltipPos = tooltip.position;
    }

    float MaximumY(int index)
    {
        return 2.9441f + 0.45f * index;
    }

    float MinimumY(int index)
    {
        return 2.4941f + 0.45f * (index - 9);
    }

    void Update()
    {
        float max = MaximumY(selectionManager.Index);
        float min = MinimumY(selectionManager.Index);

        float y = Mathf.Lerp(pivot.position.y, Mathf.Clamp(pivot.position.y, min, max), lerpSpeed);

        pivot.position = new Vector3(pivot.position.x, y, pivot.position.z);

        if (tooltip)
            tooltip.position = tooltipPos;
    }
}
