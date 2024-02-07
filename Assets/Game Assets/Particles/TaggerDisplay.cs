using UnityEngine;

public class TaggerDisplay : Singleton<TaggerDisplay>
{
    [SerializeField] private Transform currentTagger;
    
    public void SetNewTagger(Transform newTaggerTransform)
    {
        currentTagger = newTaggerTransform;
        transform.SetParent(currentTagger);
        transform.localPosition = new Vector3(0, 0, 0);
        
        SetColor();
    }

    private void SetColor()
    {
        Color color = transform.parent.GetComponent<Renderer>().material.GetColor("_Color");
        color.a = 1f;
        GetComponent<Renderer>().material.color = color;
    }
}
