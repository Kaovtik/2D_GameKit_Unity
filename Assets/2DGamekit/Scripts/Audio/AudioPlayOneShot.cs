using UnityEngine;

public class AudioPlayOneShot : MonoBehaviour
{
    public void PlayDestructableWall()
    {
        AudioManager.Instance.PlayDestroy(this.gameObject);
    }
    
    
}
