using UnityEngine;
using UnityEngine.UIElements;

public class ResizeManipulator : PointerManipulator
{
    private Vector3 m_Start;
    protected bool m_Active;
    private int m_PointerId;
    private Vector2 m_StartSize;
    private VisualElement m_WindowContainer;

    public ResizeManipulator(VisualElement windowContainer)
    {
        m_PointerId = -1;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
        m_Active = false; 
        m_WindowContainer = windowContainer;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected virtual void OnPointerDown(PointerDownEvent e)
    {
        if (m_Active)
        {
            e.StopImmediatePropagation();
            return;
        }

        if (CanStartManipulation(e))
        {
            m_Start = e.position;
            m_PointerId = e.pointerId;

            // Store the initial size of the window container
            m_StartSize = new Vector2(
                m_WindowContainer.resolvedStyle.width,
                m_WindowContainer.resolvedStyle.height
            );

            m_Active = true;
            target.CapturePointer(m_PointerId);
            e.StopPropagation();
        }
    }

    protected virtual void OnPointerMove(PointerMoveEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId))
            return;

        Vector2 diff = e.position - m_Start;

        // Update the size of the window container
        m_WindowContainer.style.width = m_StartSize.x + diff.x;
        m_WindowContainer.style.height = m_StartSize.y + diff.y;

        e.StopPropagation();
    }

    protected virtual void OnPointerUp(PointerUpEvent e)
    {
        if (!m_Active || !target.HasPointerCapture(m_PointerId) || !CanStopManipulation(e))
            return;

        m_Active = false;
        target.ReleasePointer(m_PointerId);
        e.StopPropagation();
    }
}