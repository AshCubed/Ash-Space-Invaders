using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    [SerializeField] private float _activeTime;
    private float _currentTime;
    private bool _isActive;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        _isActive = true;
        _currentTime = 0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!_isActive) return;
        PowerUpTimerCheck();
    }

    private void PowerUpTimerCheck()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _activeTime)
        {
            EndPowerUp();
            _isActive = false;
        }
    }

    protected virtual void EndPowerUp()
    {
        
    }
}
