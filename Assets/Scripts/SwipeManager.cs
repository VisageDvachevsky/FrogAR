using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectorMenu;

    [SerializeField]
    Camera _camera;
    private Vector3 _startPosition;
    private float _minSwipeSlideY = 1.5f;

    void SwipeHandler()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            Vector3 _worldPosition = _camera.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPosition = _camera.ScreenToWorldPoint(touch.position);
                    break;

                case TouchPhase.Ended:
                    float _swipeVerticalValue = (new Vector3(0, _worldPosition.y, 0) - new Vector3(0, _startPosition.y, 0)).magnitude;

                    if (_swipeVerticalValue > _minSwipeSlideY)
                    {
                        float _swipeValue = Mathf.Sign(_worldPosition.y - _startPosition.y);

                        if (_swipeValue > 0)
                        {
                            Debug.Log("Registered swipe to bottom");
                            SelectorMenu.transform.position = SelectorMenu.transform.position + Vector3.down;
                        }

                        else if (_swipeValue < 0)
                        {
                            Debug.Log("Registered swipe to top");
                            SelectorMenu.transform.position = SelectorMenu.transform.position + Vector3.up;
                        }
                    }

                    break;
            }
        }
    }

    void Update()
    {
        SwipeHandler();
    }
}
