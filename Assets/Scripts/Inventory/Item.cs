using UnityEditor;
using UnityEngine;
using System.Text;

[CreateAssetMenu( fileName = "New Item",menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string id;

    public string ItemName;
    public Sprite Icon;
    [Range(1,100)]
    public int MaximumStackValue =1;

    protected readonly StringBuilder sb = new StringBuilder();

    public string ID => id;

    //private void OnValidate()
    //{
    //    string path = AssetDatabase.GetAssetPath(this);
    //    id = AssetDatabase.AssetPathToGUID(path);
    //}

    public virtual Item GetCopy()
    {
        return this;
    }

    public virtual void Destroy()
    {

    }

    public virtual string GetItemType()
    {
        return "";
    }
    public virtual string GetDescription()
    {
        return "";
    }
}
