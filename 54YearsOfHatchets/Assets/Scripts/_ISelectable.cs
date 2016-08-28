using UnityEngine;
using System.Collections;

public interface ISelectable
{

    string Name { get; set; }
    string Description { get; set; }
    string Tooltip { get; set; }
    Vector3 location { get; }
    GameObject IGameObject { get; }

    bool Selected { get; set; }

    void OnSelected();
    void OnDeselected();

}
