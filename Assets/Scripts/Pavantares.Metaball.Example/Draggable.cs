using UnityEngine;

namespace Pavantares.Metaball.Example
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Draggable : MonoBehaviour
    {
        private Vector3 offset;

        private void OnMouseDown()
        {
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }
    }
}