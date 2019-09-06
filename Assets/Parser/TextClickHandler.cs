using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextClickHandler : MonoBehaviour, IPointerClickHandler {

    [SerializeField]
    private GameObject clickPrefab;

    public void OnPointerClick(PointerEventData eventData) {

        //Debug.Log($"Click Location: ${eventData.position}");

        var point = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, -1));
        point.z = 9;
        GameObject.Instantiate(clickPrefab,
            point, 
            Quaternion.identity);
    }
}
