using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Color customColor; // Custom color for the wire, used to match wires.
    [SerializeField] private Image Image; // The image component of the wire.
    [SerializeField] private LineRenderer LineRenderer; // LineRenderer to visually represent the wire.

    [SerializeField] private Canvas canvas; // The canvas component, necessary for coordinate calculations.
    [SerializeField] private bool isDragStarted = false; // Flag to indicate if a drag operation is started.
    public bool isLeftWire = false; // Flag to determine if the wire is on the left side.
    public bool isSucces = false; // Flag to indicate if the wire connection is successful.
    public bool isFried = false; // Flag to indicate if the wrong connection has been made.
    public Vector3 initialpozition; // Initial position of the wire (unused).
    public Vector3 startpozWire; // Starting position of the wire.

    private WireTask wireTask; // Reference to the parent component managing the wire tasks.

    private void Awake()
    {
        // Initialize references to components.
        Image = GetComponent<Image>();
        LineRenderer = GetComponent<LineRenderer>();
        canvas = GetComponentInParent<Canvas>();
        wireTask = GetComponentInParent<WireTask>();
    }

    private void Update()
    {
        if (isDragStarted)
        {
            // Handle the dragging of the wire.
            Vector2 movePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out movePos
            );
            Vector2 linePos = movePos;
            linePos.x -= 60f; // Offset to adjust the wire's visual endpoint.
            transform.position = canvas.transform.TransformPoint(movePos);
            LineRenderer.SetPosition(0, startpozWire);
            LineRenderer.SetPosition(1, canvas.transform.TransformPoint(linePos));
        }
        else if (!isSucces)
        {
            // Reset line renderer positions when not successful.
            LineRenderer.SetPosition(0, Vector3.zero);
            LineRenderer.SetPosition(1, Vector3.zero);
        }

        // Check if the wire is currently hovered over.
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
        // Reset the wire state.
        isSucces = false;
        isDragStarted = false;
        isFried = false;
        transform.position = startpozWire;
        LineRenderer.SetPosition(0, Vector3.zero);
        LineRenderer.SetPosition(1, Vector3.zero);
    }

    public void wireLocation()
    {
        // Set the initial and starting positions of the wire.
        initialpozition = GetComponent<RectTransform>().anchoredPosition;
        startpozWire = transform.position;
    }

    public void SetColor(Color color)
    {
        // Set the color of the wire image and the line renderer.
        Image.color = color;
        LineRenderer.startColor = color;
        LineRenderer.endColor = color;
        customColor = color;
    }

    // Unity interface methods for drag operations.
    public void OnDrag(PointerEventData eventData)
    {
        // Method required by the interface, implemented in OnBeginDrag and OnEndDrag.
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Begin the drag operation if the conditions are met.
        if (!isLeftWire || isSucces)
        {
            return;
        }
        isDragStarted = true;
        wireTask.currentDraggedWire = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // End the drag operation and determine the outcome of the connection.
        if (wireTask.currentHoveredWire != null)
        {
            if (wireTask.currentHoveredWire.customColor == customColor && !wireTask.currentHoveredWire.isLeftWire)
            {
                isSucces = true; // Correct connection.
            }
            else if (wireTask.currentHoveredWire.customColor != customColor && !wireTask.currentHoveredWire.isLeftWire)
            {
                isFried = true; // Incorrect connection.
            }
        }
        isDragStarted = false;
        wireTask.currentDraggedWire = null;
    }
}
