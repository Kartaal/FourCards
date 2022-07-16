using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class GroupLayoutController : MonoBehaviour
{
    HorizontalLayoutGroup LayoutGroup;

    private void Awake() {
        LayoutGroup = GetComponent<HorizontalLayoutGroup>();
    }

    public void UpdateSpacing(int childCount) {
        // Debug.Log($"Updating spacing with {childCount} children");

        // QQQ DO SOME SMART MATHS HERE FOR SPACING!!
        if (childCount > 8)
        {
            // Debug.Log("Spacing more than 8 children");
            LayoutGroup.spacing = -10f;
        } else if(childCount > 4) {
            // Debug.Log("Spacing more than 4 children");
            LayoutGroup.spacing = 10f;
        } else if(childCount > 2) {
            // Debug.Log("Spacing more than 2 children");
            LayoutGroup.spacing = 30f;
        }
    }
}
