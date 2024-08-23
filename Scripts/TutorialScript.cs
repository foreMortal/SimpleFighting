using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private TouchPadInput input;
    [SerializeField] private Image move, attack;
    private Collider[] t = new Collider[1];
    private int step;
    private float timeOut;

    private void Update()
    {
        if(step == 0)
        {
            if(input.GetMoveInput() != Vector3.zero)
            {
                step = 1;
                text.text = "Атакуйте удерживая или нажимая внутри выделенной области";
                move.enabled = false;
                attack.enabled = true;
            }
        }
        else if(step == 1)
        {
            if (input.GetAttackInput() != Vector3.zero)
            {
                step = 2;
                move.enabled = false;
                attack.enabled = false;
                text.text = "Победите первого противника!";
                spawner.Spawn();
                t = Physics.OverlapBox(Vector3.zero, new Vector3(25f, 25f, 25f), Quaternion.identity, 1 << 6);
                t[0].GetComponent<Health>().unitDied += TutorialEnded;
                timeOut = 1f;
            }
        }

        if(timeOut > 0f && step == 2)
        {
            timeOut -= Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, timeOut);
        }
        else if(timeOut < 0 && step == 3)
        {
            SceneManager.LoadScene(1);
        }
        else if(step == 3)
        {
            timeOut -= Time.deltaTime;
        }
    }

    private void TutorialEnded()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.text = "Поздравляем обучение пройдено!";
        step = 3;
        timeOut = 1f;
    }
}
