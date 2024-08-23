using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private GameObject endGameMenu;
    private IInputManager inputManager;
    private Attack attack;
    private CharacterMove move;

    private void Awake()
    {
        Time.timeScale = 1f;
        attack = GetComponent<Attack>();
        move = GetComponent<CharacterMove>();
        inputManager = GetComponent<IInputManager>();
        Health h = GetComponent<Health>();
        h.unitHited += UpdateHp;
        h.unitDied += PlayerDied;
        hpSlider.maxValue = h.MaxHealth;
        hpSlider.value = h.MaxHealth;
    }

    private void UpdateHp(float hp)
    {
        hpSlider.value = hp;
    }

    private void Update()
    {
        inputManager.GaverInput();
        move.SelfUpdate();
        attack.SelfUpdate();
    }

    private void PlayerDied()
    {
        Time.timeScale = 0f;
        endGameMenu.SetActive(true);
    }
}
