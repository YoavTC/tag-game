using UnityEngine;

public class TaggerDisplay : Singleton<TaggerDisplay>
{
    [SerializeField] private Transform currentTagger;
    
    public void SetNewTagger(Transform newTaggerTransform)
    {
        currentTagger = newTaggerTransform;
        transform.SetParent(currentTagger);
        transform.localPosition = new Vector3(0, 0, -5);
    }
}
