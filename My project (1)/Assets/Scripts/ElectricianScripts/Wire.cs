using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Color customColor;
    [SerializeField] private Image Image;
    [SerializeField] private LineRenderer LineRenderer;

    [SerializeField] private Canvas canvas;
    [SerializeField] private bool isDragStarted = false;
    public bool isLeftWire = false;
    public bool isSucces = false;
    public bool isFried = false;
    public Vector3 initialpozition;
    public Vector3 startpozWire;

    

    private WireTask wireTask;
    private void Awake()
    {
        
        Image = GetComponent<Image>();
        LineRenderer = GetComponent<LineRenderer>();
        canvas = GetComponentInParent<Canvas>();
        wireTask = GetComponentInParent<WireTask>();
        
    }
    private void Update()
    {
       
        if (isDragStarted)
        {
            

            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out movePos
                );
            Vector2 linePos = movePos;
            linePos.x -= 60f;
            transform.position = canvas.transform.TransformPoint(movePos);
            LineRenderer.SetPosition(0, startpozWire);
            LineRenderer.SetPosition(1, canvas.transform.TransformPoint(linePos));
        }
        else 
        {
            if (!isSucces)
            {
               // transform.position = startpozWire;
                LineRenderer.SetPosition(0, Vector3.zero);
                LineRenderer.SetPosition(1, Vector3.zero);
            }
           
        }
        float offsetX = 60f;
        Vector3 mouse = Input.mousePosition;
        mouse.x += offsetX;
        bool isHovered = RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, mouse, canvas.worldCamera);
        if (isHovered)
        {
            wireTask.currentHoveredWire = this;
        }
        
    }

    public void ResetWire()
    {
        isSucces = false;
        isDragStarted = false;
        isFried = false;
        transform.position = startpozWire;
        LineRenderer.SetPosition(0, Vector3.zero);
        LineRenderer.SetPosition(1, Vector3.zero);
    }

    public void wireLocation()
    {
        initialpozition = GetComponent<RectTransform>().anchoredPosition;
        startpozWire = transform.position;
    }
    public void SetColor(Color color)
    {
        Image.color = color;
        LineRenderer.startColor = color;
        LineRenderer.endColor = color;
        customColor = color;
    }

   

    public void OnDrag(PointerEventData eventData)
    {
       
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isLeftWire || isSucces)
        { return; }
        isDragStarted = true;
        wireTask.currentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(wireTask.currentHoveredWire != null)
        {
            if(wireTask.currentHoveredWire.customColor == customColor && !wireTask.currentHoveredWire.isLeftWire)
            {
                isSucces = true;
            }
            else if(wireTask.currentHoveredWire.customColor != customColor && !wireTask.currentHoveredWire.isLeftWire)
            {
                isFried = true;
            }
        }
        isDragStarted = false;
        wireTask.currentDraggedWire = null;
    }
}
