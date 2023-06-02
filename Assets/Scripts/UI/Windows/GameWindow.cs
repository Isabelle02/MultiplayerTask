using UnityEngine;
using UnityEngine.UI;

public class GameWindow : Window
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Image _hpFillImage;
    [SerializeField] private Button _shootButton;
    [SerializeField] private Text _coinAmountText;

    private JoystickController _handler;
    
    protected override void OnOpen(ViewParam viewParam)
    {
        _hpFillImage.fillAmount = 1f;
        _coinAmountText.text = CurrencyManager.Coins.ToString();
        
        _pauseButton.onClick.AddListener(OnPauseButtonClick);
        _shootButton.onClick.AddListener(SendShoot);

        _handler = Pool.Get<JoystickController>();

        PlayerView.Inited += OnPlayerInited;
        PlayerView.HpChanged += OnHpChanged;
        PopupManager.Opened += OnPopupOpened;
        PopupManager.Closed += OnPopupClosed;
        CurrencyManager.CoinCollect += OnCoinCollected;
    }

    private void OnPlayerInited()
    {
        _hpFillImage.color = PlayerView.Color;
    }

    private void Update()
    {
        if (NetworkController.PlayerView && _handler) 
            NetworkController.PlayerView.SendMove(_handler.MovingOffset);
    }

    private void OnCoinCollected(int amount)
    {
        _coinAmountText.text = amount.ToString();
    }

    private void SendShoot()
    {
        if (NetworkController.PlayerView) 
            NetworkController.PlayerView.SendShoot();
    }

    private void OnHpChanged(int value)
    {
        _hpFillImage.fillAmount = value / 100f;
    }

    private void OnPopupOpened(Popup popup)
    {
        _pauseButton.gameObject.SetActive(false);
        _shootButton.gameObject.SetActive(false);
        _handler.gameObject.SetActive(false);
    }

    private void OnPopupClosed(Popup popup)
    {
        _pauseButton.gameObject.SetActive(true);
        _shootButton.gameObject.SetActive(true);
        _handler.gameObject.SetActive(true);
    }
    
    private void OnPauseButtonClick()
    {
        PopupManager.Open<PausePopup>();
    }

    protected override void OnClose()
    {
        _pauseButton.onClick.RemoveListener(OnPauseButtonClick);
        _shootButton.onClick.RemoveListener(SendShoot);
        
        Pool.Release(_handler);
        _handler = null;

        PlayerView.Inited -= OnPlayerInited;
        PlayerView.HpChanged -= OnHpChanged;
        PopupManager.Opened -= OnPopupOpened;
        PopupManager.Closed -= OnPopupClosed;
        CurrencyManager.CoinCollect -= OnCoinCollected;
    }
}