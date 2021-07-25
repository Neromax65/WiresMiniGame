using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image node;
    [SerializeField] private Image line;

    public int Index { get; private set; }
    public Color Color { get; private set; }

    public bool Connected { get; private set; }

    private float _canvasScaleFactor;


    private Vector2 _startPosition;
    private void Awake()
    {
        _startPosition = transform.position;
        _canvasScaleFactor = FindObjectOfType<CanvasScaler>().transform.lossyScale.x;
        Debug.Log($"CanvasScaleFactor: {_canvasScaleFactor}");
        Debug.Log($"Line Lossy Scale X{line.rectTransform.lossyScale.x}");
    }

    public void Init(int index, Color color)
    {
        Index = index;
        Color = color;
        node.color = Color;
        line.color = Color;
        
        
        // TODO: Testing
        // line.rectTransform.anchorMin = new Vector2(0, 0.5f);
        // line.rectTransform.anchorMax = new Vector2(0, 0.5f);
        // line.rectTransform.pivot = new Vector2(0, 0.5f);
    }

    public Vector2 startPos;
    public Vector2 transformPos;
    public Vector2 cursorPos;
    public Vector2 lineLocalPos;
    public Vector2 linePosition;
    public Vector2 lineAnchoredPosition;
    public float lineLen;
    
    void SetLinePosition(Vector2 cursorPosition)
    {
        // var _startPosition = GetComponent<RectTransform>().position;
        // var _startPosition = _startPosition;
        float totalScaleFactor = 1/_canvasScaleFactor;
        // Debug.Log($"Total Scale Factor: {totalScaleFactor}");
        // _startPosition.x = 25;
        float lineLength = Vector2.Distance(cursorPosition, _startPosition);
        // float lineLength = cursorPosition.x - _startPosition.x;
        float lineWidth = line.rectTransform.sizeDelta.y;
        float lineAngle = Mathf.Atan2(cursorPosition.y - _startPosition.y, cursorPosition.x - _startPosition.x) * Mathf.Rad2Deg;
        line.rectTransform.localPosition = new Vector2((cursorPosition.x - _startPosition.x)/(2 * _canvasScaleFactor), (cursorPosition.y - _startPosition.y)/ (2 * _canvasScaleFactor));
        // line.rectTransform.localPosition = new Vector2((cursorPosition.x - _startPosition.x)/2, 0);
        line.rectTransform.sizeDelta = new Vector2(lineLength * totalScaleFactor, lineWidth);
        // line.rectTransform.localScale = new Vector2(lineLength / 2, 1);
        line.rectTransform.rotation = Quaternion.Euler(0, 0, lineAngle);
        // line.rectTransform.localPosition = (Vector2)(cursorPosition - (Vector2)_startPosition) / 2;
        // line.rectTransform.sizeDelta = new Vector2(lineLength * (1 + (1-_canvasScaleFactor)), lineWidth);
        // line.rectTransform.localScale = Vector3.one * totalScaleFactor;
        
        // TODO: Testing
        startPos = _startPosition;
        transformPos = _startPosition;
        cursorPos = cursorPosition;
        lineLocalPos = line.rectTransform.localPosition;
        linePosition = line.rectTransform.position;
        lineAnchoredPosition = line.rectTransform.anchoredPosition;
        lineLen = lineLength;
    }

    void SetLinePosition2(Vector2 cursorPosition)
    {
        line.rectTransform.position = ((Vector3)cursorPosition + transform.position) / 2;
        Vector3 dif = (Vector3)cursorPosition - transform.position;
        line.rectTransform.sizeDelta = new Vector3(dif.magnitude, 5);
        line.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Connected) return;
        // Debug.Log($"Vector3 Node Position {transform.position}");
        // Debug.Log($"Vector2 Node Position {(Vector2)transform.position}");
        // Debug.Log("OnBeginDrag");
        // _lineRenderer.SetPosition(0, transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Connected) return;
        SetLinePosition(eventData.position);
        // node.raycastTarget = false;
        // transform.position = eventData.position;
        // _lineRenderer.SetPosition(1, eventData.position);
        // Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Connected) return;
        // node.raycastTarget = true;
        transform.position = _startPosition;
        // line.rectTransform.localPosition = Vector3.zero;
        SetLinePosition(transform.position);
        // _lineRenderer.SetPosition(1, transform.position);
        // Debug.Log("OnEndDrag");
    }



    public void OnDrop(PointerEventData eventData)
    {
        var droppedWire = eventData.pointerDrag.GetComponent<Wire>();
        if (!droppedWire || droppedWire == this) return;
        if (droppedWire.Index == Index)
        {
            Connected = true;
            droppedWire.Connected = true;
        }
    }
}
