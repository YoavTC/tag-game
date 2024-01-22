using UnityEngine;

public class TaggerDisplay : Singleton<TaggerDisplay>
{
    [SerializeField] private Transform currentTagger;
    
    public void SetNewTagger(Transform newTaggerTransform)
    {
        currentTagger = newTaggerTransform;
        transform.SetParent(currentTagger);
        transform.localPosition = Vector3.zero;
    }
}
