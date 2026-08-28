using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class UIManagement : MonoBehaviour
{
    [SerializeField]
    Image P_HPImage;

    [SerializeField]
    TextMeshProUGUI P_HPtext;

    [SerializeField]
    Image P_MPImage;

    [SerializeField]
    TextMeshProUGUI P_MPtext;

    [SerializeField]
    GameObject[] E_HPImage;

    [SerializeField]
    Image E_Gauge;

    [SerializeField]
    float ActiveTime;

    RectTransform E_GaugePos;

    bool Image_active = false;
    public void UpdateHP(float current,float max)
    {
        P_HPImage.fillAmount = current / max;
        P_HPtext.text = current + "/" + max;
    }

    public void UpdateMP(float current,float max)
    {
        P_MPImage.fillAmount = current / max;
        P_MPtext.text = current + "/" + max;
    }
    public void E_UpdateHP(float current, float max,Enemy enemy)
    {
        StartCoroutine(ImageActive(enemy));
        E_Gauge.fillAmount = current / max;
    }

    IEnumerator ImageActive(Enemy enemy)
    {
        if (Image_active) yield break;
        Image_active = true;

        E_GaugePos = ActiveImage();

        while (ActiveTime > 0)
        {
            ActiveTime -= Time.deltaTime;

            Vector3 UIPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

            E_GaugePos.position = UIPos;
        }
        ActiveTime = 5.0f;
        E_GaugePos.gameObject.SetActive(false);
        Image_active = false;
    }

    RectTransform ActiveImage()
    {
        RectTransform activeimage = null;
        for (int i = 0; i < E_HPImage.Length; i++)
        {
            if (!E_HPImage[i].activeSelf)
            {
                E_HPImage[i].SetActive(true);
                activeimage = E_HPImage[i].GetComponent<RectTransform>();
                break;
            }
        }
        return activeimage;
    }
}
