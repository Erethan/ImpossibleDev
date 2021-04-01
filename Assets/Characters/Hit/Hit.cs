
using UnityEngine;

[System.Serializable]
public class Hit
{
    public virtual HitType Type { get; set; }
    public virtual float Damage { get; set; }

    public virtual GameObject SourceGameObject{ get; set; }
    public virtual GameObject HitGameObject { get; set; }

    public virtual Vector2 Direction
    {
        get
        {
            return HitGameObject.transform.position - SourceGameObject.transform.position;
        }
    }


}
