using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireHazard : MonoBehaviour
{
    [field: SerializeField] public float BurnMultiplier { get; private set; }= 3f;
    [field: SerializeField] public Color BurnerInactiveColor { get; private set; } = Color.black;
    [field: SerializeField] public Color BurnerActiveColor { get; private set; } = Color.red;
    [SerializeField] Burner[] burners;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    
    float timer;
    float nextBurnTime;

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= nextBurnTime)
        {
            foreach (var burner in burners)
            {
                burner.DisableBurn();
            }
            
            var selectedBurner = burners[Random.Range(0, burners.Length)];
            selectedBurner.EnableBurn();
            
            nextBurnTime = Random.Range(minWaitTime, maxWaitTime);
            timer = 0f;
        }
    }
}
