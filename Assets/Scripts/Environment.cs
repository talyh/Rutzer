using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField]
    private Sprite _dayBackground;
    [SerializeField]
    private Sprite _nightBackground;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        RunInitialChecks();

        GameController.speedIncreased += SwapBackground;
    }

    public void SwapBackground()
    {
        if (_renderer.sprite == _dayBackground)
        {
            _renderer.sprite = _nightBackground;
        }
        else if (_renderer.sprite == _nightBackground)
        {
            _renderer.sprite = _dayBackground;
        }
    }

    private void RunInitialChecks()
    {
        _renderer = GetComponent<SpriteRenderer>();

        Supporting.CheckRequiredProperty(gameObject, _renderer, "Spirte Renderer");
    }

    private void OnDisable()
    {
        GameController.speedIncreased -= SwapBackground;
    }
}
