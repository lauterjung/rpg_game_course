using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CooldownExample : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] float redColorDuration = 1f;
    float timer;
    float currentTimeInGame;
    float lastTimeWasDamaged;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        ChangeColorIfNeeded();
    }

    private void ChangeColorIfNeeded()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0 && sr.color != Color.white)
        {
            TurnWhite();
        }
    }

    void UpdateCooldown2()
    {
        currentTimeInGame = Time.time;

        if (currentTimeInGame > lastTimeWasDamaged + redColorDuration && sr.color != Color.white)
        {
            TurnWhite();
        }
    }

    public void TakeDamage()
    {
        sr.color = Color.red;
        timer = redColorDuration;
        lastTimeWasDamaged = Time.time;
    }

    private void UpdateTimer()
    {
        timer = redColorDuration;
    }

    private void TurnWhite()
    {
        sr.color = Color.white;
    }
}
