using UnityEngine;

namespace UI
{
    public interface ITabControl 
    {
        
        Tab AddTab();
        void CloseTab(Tab tab);
        void CloseTab(int index);
        void SelectTab(int index);
        void SelectTab(Tab tab);
        void MoveTab(Tab tab, int position);
        void MoveTabTransform(Tab tab, int index);
        void DeselectTab(int index);
        void DeselectTab(Tab tab);
    }
}