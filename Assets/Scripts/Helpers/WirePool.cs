using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    /// <summary>
    /// Пул проводов
    /// </summary>
    public class WirePool : Singleton<WirePool>
    {
        [SerializeField] private Wire wirePrefab;
        [SerializeField] private Transform wireParent;
        [SerializeField] private int preSpawnCount = 10;
        
        private readonly List<Wire> _poolWires = new List<Wire>();

        private void Start()
        {
            PreSpawn();
        }

        /// <summary>
        /// Предсоздание некоторого количества объектов пула
        /// </summary>
        private void PreSpawn()
        {
            for (int i = 0; i < preSpawnCount; i++)
            {
                Wire poolWire = Instantiate(wirePrefab, wireParent);
                poolWire.gameObject.SetActive(false);
                _poolWires.Add(poolWire);
            }
        }

        /// <summary>
        /// Получить провод из пула
        /// </summary>
        /// <returns>Провод</returns>
        public Wire GetFromPool()
        {
            foreach (var poolWire in _poolWires)
            {
                if (!poolWire.gameObject.activeInHierarchy)
                {
                    poolWire.gameObject.SetActive(true);
                    return poolWire;
                }
            }

            var wire = Wire.Instantiate(wirePrefab, wireParent);
            _poolWires.Add(wire);
            return wire;
        }

        /// <summary>
        /// Скрыть объект из пула
        /// </summary>
        /// <param name="poolWire">Провод для удаления</param>
        public void Destroy(Wire poolWire)
        {
            if (_poolWires.Contains(poolWire))
                poolWire.gameObject.SetActive(false);
        }
    }
}