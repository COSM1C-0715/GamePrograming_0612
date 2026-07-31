using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class UIManagement : MonoBehaviour
{
    [SerializeField]
    Enemy enemy;

    [SerializeField]
    Player player;

    [SerializeField]
    Image P_HPImage;

    [SerializeField]
    TextMeshProUGUI P_HPtext;

    [SerializeField]
    Image P_MPImage;

    [SerializeField]
    TextMeshProUGUI P_MPtext;

    [SerializeField]
    GameObject E_HPImage;

    [SerializeField]
    Image E_Gauge;

    [SerializeField]
    float ActiveTime;

    RectTransform E_GaugePos;

    bool Image_active = false;
    void Awake()
    {
        E_GaugePos = E_HPImage.GetComponent<RectTransform>();
        player.OnHPMethod(UpdateHP);
        player.OnMPMethod(UpdateMP);
        enemy.OnHPMethod(E_UpdateHP);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        P_HPImage.fillAmount = 1;
        P_HPtext.text = player.MaxHp + "/" + player.MaxHp;
        P_MPImage.fillAmount = 1;
        P_MPtext.text = player.MaxMp + "/" + player.MaxMp;
    }

    void FixedUpdate()
    {
        
    }
    void UpdateHP(float current,float max)
    {
        P_HPImage.fillAmount = current / max;
        P_HPtext.text = current + "/" + max;
    }

    void UpdateMP(float current,float max)
    {
        P_MPImage.fillAmount = current / max;
        P_MPtext.text = current + "/" + max;
    }
    void E_UpdateHP(float current, float max)
    {
        StartCoroutine(ImageAvtive());
        E_Gauge.fillAmount = current / max;
    }

    IEnumerator ImageAvtive()
    {
        if (Image_active) yield break;
        Image_active = true;
        E_HPImage.SetActive(true);
        while(ActiveTime > 0)
        {
            ActiveTime -= Time.deltaTime;

            Vector3 UIPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

            E_GaugePos.position = UIPos;
        }
        ActiveTime = 5.0f;
        Image_active = false;
    }
}
