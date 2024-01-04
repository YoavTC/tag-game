using DG.Tweening;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private bool followPlayer;
    
    public Transform playerTransform;
    [SerializeField] private float cameraSpeed;

    private Vector3 cameraPos;
    private Vector3 playerPos;

    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;

    private void Start()
    {
        followPlayer = false;
        Destroy(GameObject.FindWithTag("TempCamera"));
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        targetPosition.z = transform.position.z;
        transform.DOMove(targetPosition, cameraSpeed).SetEase(Ease.InOutSine);
    }

    public void SetFollowPlayerState(bool state)
    {
        followPlayer = state;
    }
    
    public void InitiateCameraSettings(Transform targetTransform)
    {
        if (playerTransform == null) playerTransform = targetTransform;
        GameManager.Instance.clientCamera = this;
    }
    
    void FixedUpdate()
    {
        if (playerTransform == null || !followPlayer) return;
        
        cameraPos = transform.position;
        
        playerPos = playerTransform.transform.position;
        playerPos.z = cameraPos.z;
        playerPos.y = Mathf.Clamp(playerPos.y, minHeight, maxHeight);
        
        transform.position = Vector3.Lerp(cameraPos, playerPos, cameraSpeed * Time.deltaTime);
    }

    public void SetValues(CameraMovement cameraMovement)
    {
        cameraSpeed = cameraMovement.cameraSpeed;
        cameraPos = cameraMovement.cameraPos;
        playerPos = cameraMovement.playerPos;
        maxHeight = cameraMovement.maxHeight;
        minHeight = cameraMovement.minHeight;
    }
}
