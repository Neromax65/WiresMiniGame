using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Класс, отвечающий за соединение проводов
/// </summary>
public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    /// <summary>
    /// Индекс задается при инициализации объекта, два провода с одинаковым индексом считаются парными
    /// </summary>
    public int Index { get; private set; }
    
    /// <summary>
    /// Цвет гнезда и провода
    /// </summary>
    public Color Color { get; private set; }
    
    /// <summary>
    /// Соединён ли данный провод со своей парой
    /// </summary>
    public bool Connected { get; private set; }
    
    [SerializeField] private Image node;
    [SerializeField] private Image line;

    /// <summary>
    /// Метод для инициалиации провода
    /// </summary>
    /// <param name="index">Индекс провода для определения пары</param>
    /// <param name="color">Цвет провода</param>
    public void Init(int index, Color color)
    {
        Index = index;
        Color = color;
        node.color = Color;
        line.color = new Color(Color.r, Color.g, Color.b, 0.5f);;
        node.raycastTarget = true;
        line.gameObject.SetActive(false);
        Connected = false;
    }
    
    /// <summary>
    /// Задать позицию линии 
    /// </summary>
    /// <param name="startPosition">Начальная точка</param>
    /// <param name="endPosition">Конечная точка</param>
    private void SetLinePosition(Vector2 startPosition, Vector2 endPosition)
    {
        float lineLossyScale = line.rectTransform.lossyScale.x;
        float lineLength = Vector2.Distance(startPosition, endPosition);
        float lineWidth = line.rectTransform.sizeDelta.y;
        float lineAngle = Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg;
        line.rectTransform.localPosition = (endPosition - startPosition) / (2 * lineLossyScale);
        line.rectTransform.sizeDelta = new Vector2(lineLength * (1/lineLossyScale), lineWidth);
        line.rectTransform.rotation = Quaternion.Euler(0, 0, lineAngle);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        line.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetLinePosition(transform.position, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Connected) return;
        SetLinePosition(transform.position, transform.position);
        line.gameObject.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var droppedWire = eventData.pointerDrag.GetComponent<Wire>();
        if (!droppedWire || droppedWire == this) return;
        if (droppedWire.Index == Index)
        {
            Connect(droppedWire);
        }
    }

    /// <summary>
    /// Соединить текущий провод с дроугим парным
    /// </summary>
    /// <param name="otherWire">Другой провод</param>
    private void Connect(Wire otherWire)
    {
        Connected = true;
        node.raycastTarget = false;
        line.color = new Color(Color.r, Color.g, Color.b, 1);

        otherWire.Connected = true;
        otherWire.node.raycastTarget = false;
        otherWire.line.color = new Color(Color.r, Color.g, Color.b, 1);
    }
}
