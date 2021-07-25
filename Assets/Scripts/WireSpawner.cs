using System.Collections.Generic;
using Extensions;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WireSpawner : Singleton<WireSpawner>
{
    [SerializeField] private Wire wirePrefab;
    [SerializeField] private Transform wireParent;


    public List<Wire> CurrentWires { get; } = new List<Wire>();

    public void SpawnWires(int count)
    {
        List<Wire> tempWires = new List<Wire>();
        for (int i = 0; i < count; i++)
        {
            var wirePosition = new Vector2(0,  Screen.height / (count + 1) * (i + 1));
            var wire = SpawnWire(wirePosition, i, GetRandomColor());
            var rectTransform = wire.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.position = wirePosition;
            tempWires.Add(wire);
        }
            
        tempWires.Shuffle();
        for (var i = 0; i < tempWires.Count; i++)
        {
            var wirePosition = new Vector2(Screen.width, Screen.height / (count + 1) * (i + 1));
            var wire = SpawnWire(wirePosition, tempWires[i].Index, tempWires[i].Color);
            var rectTransform = wire.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.pivot = new Vector2(1, 0.5f);
            rectTransform.position = wirePosition;
        }
    }

    private Wire SpawnWire(Vector2 position, int index, Color color)
    {
        var wire = Instantiate(wirePrefab, position, quaternion.identity, wireParent);
        wire.gameObject.name = $"Wire {index}";
        wire.Init(index, color);
        CurrentWires.Add(wire);
        return wire;
    }

    private Color GetRandomColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }

}