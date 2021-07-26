using System.Collections.Generic;
using Helpers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// Класс, управляющий жизненным циклом проводов
    /// </summary>
    public class WireManager : Singleton<WireManager>
    {
        /// <summary>
        /// Текущие активные провода на сцене
        /// </summary>
        public List<Wire> CurrentWires { get; } = new List<Wire>();

        /// <summary>
        /// Проверка, все ли провода соединены с парой
        /// </summary>
        public bool AllWiresConnected => CurrentWires.TrueForAll(w => w.Connected);
        
        /// <summary>
        /// Создать определенное количество проводов
        /// </summary>
        /// <param name="count">Количество</param>
        public void SpawnWires(int count)
        {
            ClearWires();
            List<Wire> tempWires = new List<Wire>();
            for (int i = 0; i < count; i++)
            {
                var wirePosition = new Vector2(0,  Screen.height / (count + 1) * (i + 1));
                var wire = SpawnWire(wirePosition, i, GetRandomColor(), 0);
                tempWires.Add(wire);
            }
            
            tempWires.Shuffle();
            for (var i = 0; i < tempWires.Count; i++)
            {
                var wirePosition = new Vector2(Screen.width, Screen.height / (count + 1) * (i + 1));
                var wire = SpawnWire(wirePosition, tempWires[i].Index, tempWires[i].Color, 1);
            }
        }

        /// <summary>
        /// Убрать провода со сцены
        /// </summary>
        private void ClearWires()
        {
            foreach (var currentWire in CurrentWires)
            {
                WirePool.Instance.Destroy(currentWire);
            }
            CurrentWires.Clear();
        }

        /// <summary>
        /// Создать провод
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="index">Индекс</param>
        /// <param name="color">Цвет</param>
        /// <param name="anchor">Сторона крепления</param>
        /// <returns>Компонент провода</returns>
        private Wire SpawnWire(Vector2 position, int index, Color color, float anchor)
        {
            var wire = WirePool.Instance.GetFromPool();
            var rectTransform = wire.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(anchor, 0.5f);
            rectTransform.anchorMax = new Vector2(anchor, 0.5f);
            rectTransform.pivot = new Vector2(anchor, 0.5f);
            rectTransform.position = position;
            wire.gameObject.name = $"Wire {(anchor == 0 ? "Left" : "Right")} {index}";
            wire.Init(index, color);
            CurrentWires.Add(wire);
            return wire;
        }

        /// <summary>
        /// Получение случайного цвета
        /// </summary>
        /// <returns>Цвет 0-1</returns>
        private Color GetRandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }

    }
}