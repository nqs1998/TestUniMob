using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputSystem : MonoBehaviour
{
    SelectionSystem selectionSystem = new();
    [SerializeField] private LayerMask selectableMask;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, selectableMask))
            {
                if (hit.collider.TryGetComponent(out ISelectable selectable))
                {
                    selectionSystem.Select(selectable);
                }
                else
                {
                    selectionSystem.Clear();
                }
            }
            else 
            {
                selectionSystem.Clear();
            }
        }
    }
}