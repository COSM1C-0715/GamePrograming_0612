using R3;
using R3.Triggers;
using UnityEngine;

public class UpdateParameter : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    Enemy[] enemy;
    [SerializeField]
    UIManagement uimanager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uimanager.UpdateHP(player.hp.CurrentValue,player.MaxHP);

        uimanager.UpdateMP(player.mp.CurrentValue,player.MaxMP);

        player.hp.Subscribe(
            Currenthp =>
            {
                uimanager.UpdateHP(Currenthp, player.MaxHP);
            });
        player.mp.Subscribe(
            Currentmp =>
            {
                uimanager.UpdateMP(Currentmp, player.MaxMP);
            });
        player.MpChargeTime.Subscribe(
            CurrentTime =>
            {
                if (CurrentTime <= 0)
                {
                    CurrentTime = 5;
                    player.mp.Value++;
                }
            });
        for (int i = 0;i < enemy.Length;i++)
        {
            Enemy l_enemy = enemy[i];
            enemy[i].hp.Subscribe(
            CurrentHP =>
            {
                uimanager.E_UpdateHP(CurrentHP, l_enemy.MaxHP, l_enemy);
            });
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
