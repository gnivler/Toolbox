using System.Collections;
using TMPro;
using UnityEngine;

namespace Toolbox.Features
{
    public class FloatieBehaviour : MonoBehaviour
    {
        private const float Speed = 40f;

        private void Update()
        {
            GameObject o;
            (o = gameObject).transform.Translate(Vector3.up * (Time.deltaTime * Speed));
            o.transform.localPosition += new Vector3(0, 1, 0);
        }
    }

    public class FadeText : MonoBehaviour
    {
        private const float FadeSpeed = 1f;
        private TextMeshProUGUI text;

        private void Awake()
        {
            text = gameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            StartCoroutine(FadeOutText());

            IEnumerator FadeOutText()
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
                while (text.color.a > 0)
                {
                    text.color = new Color(
                        text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * FadeSpeed);
                    yield return null;
                }

                Destroy(gameObject);
            }
        }
    }
}
