using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [Header("Marker Settings")]
    [Tooltip("The tip of the marker that makes contact with the whiteboard surface.")]
    [SerializeField] private Transform tip;

    [Tooltip("The diameter of the marker tip in pixels when drawing on the whiteboard.")]
    [SerializeField] private int penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;
    private Rigidbody markerRb;

    private void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        var baseColor = _renderer.material.color;
        var colorCount = penSize * penSize;
        _colors = Enumerable.Repeat(baseColor, colorCount).ToArray();
        _tipHeight = tip.localScale.y;
        markerRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Draw();
    }

    private void Draw()
    {
        var tipPosition = tip.position;
        var tipDirection = transform.up;
        var rayDistance = _tipHeight * 2f;

        if (!Physics.Raycast(tipPosition - tipDirection * _tipHeight, tipDirection, out _touch, rayDistance))
        {
            _whiteboard = null;
            _touchedLastFrame = false;

            markerRb.constraints = RigidbodyConstraints.None;

            return;
        }

        var touchedTransform = _touch.transform;

        if (!touchedTransform.GetComponent<Whiteboard>())
        {
            _whiteboard = null;
            _touchedLastFrame = false;
            return;
        }

        _whiteboard ??= touchedTransform.GetComponent<Whiteboard>();

        markerRb.constraints = RigidbodyConstraints.FreezeRotation;

        _touchPos.Set(_touch.textureCoord.x, _touch.textureCoord.y);

        var textureSize = _whiteboard.textureSize;
        var x = (int)(_touchPos.x * textureSize.x - penSize * 0.5f);
        var y = (int)(_touchPos.y * textureSize.y - penSize * 0.5f);

        if (x < 0 || y < 0 || x > textureSize.x || y > textureSize.y)
        {
            _touchedLastFrame = false;
            return;
        }

        if (_touchedLastFrame)
        {
            var texture = _whiteboard.texture;
            texture.SetPixels(x, y, penSize, penSize, _colors);

            for (float f = 0.01f; f < 1f; f += 0.04f)
            {
                var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                texture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
            }

            texture.Apply();
        }

        _lastTouchPos.Set(x, y);
        _touchedLastFrame = true;

    }
  
}
