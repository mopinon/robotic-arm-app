    using UnityEngine;

namespace UI
{
    public class Page : MonoBehaviour
    { 
        [SerializeField] private TabControl tabControl;
        public void Show()
        {
            tabControl.selectedPage = this;
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        public void Remove() => Destroy(gameObject);

        public void SetContent(RectTransform content)
        {
            content.SetParent(transform);
            content.offsetMin = Vector2.zero;
            content.offsetMax = Vector2.zero;
        }
    }
}