using RWS;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    RectTransform panelRect = null;
    
    [SerializeField]
    Button closeButton = null;

    [SerializeField]
    GameObject navigationPanelGameObject = null;
    
    [SerializeField]
    Button graphicsButton = null;
    
    [SerializeField]
    Button soundButton = null;
    
    [SerializeField]
    Button controlsButton = null;
    
    [SerializeField]
    Button sensitivityButton = null;

    [SerializeField]
    GraphicsPanel graphicsPanel = null;

    [SerializeField]
    SoundPanel soundPanel = null;
    
    [SerializeField]
    ControlsPanel controlsPanel = null;
    
    [SerializeField]
    SensitivityPanel sensitivityPanel = null; 
    
    //----------------------------------------------------------------------------------------------------

    public bool IsOpen => gameObject.activeSelf;

    public void Show()
    {
        if( IsOpen )
        {
            return;
        }
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        if( !IsOpen)
        {
            return;
        }
        
        navigationPanelGameObject.SetActive( true );
        
        graphicsPanel.Hide();
        soundPanel.Hide();
        controlsPanel.Hide();
        sensitivityPanel.Hide();
        
        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;
    }
    
    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        closeButton.onClick.AddListener( Hide );

        graphicsButton.onClick.AddListener( OnGraphicsButton );
        soundButton.onClick.AddListener( OnSoundButton );
        controlsButton.onClick.AddListener( OnControlsButton );
        sensitivityButton.onClick.AddListener( OnSensitivityButton );
        
        InputManager.Instance.OnEscapeButton += OnEscapeButton;
        
        navigationPanelGameObject.SetActive( true );
        
        graphicsPanel.Hide();    
        soundPanel.Hide();
        controlsPanel.Hide();
        sensitivityPanel.Hide();
    }

    void OnGraphicsButton()
    {
        navigationPanelGameObject.SetActive( false );
        
        graphicsPanel.Show( () =>
        {
            navigationPanelGameObject.SetActive( true );
        } );
    }

    void OnSoundButton()
    {
        navigationPanelGameObject.SetActive( false );
        
        soundPanel.Show( () =>
        {
            navigationPanelGameObject.SetActive( true );
        } );
    }

    void OnControlsButton()
    {
        navigationPanelGameObject.SetActive( false );
        
        controlsPanel.Show( () =>
        {
            navigationPanelGameObject.SetActive( true );
        } );
    }

    void OnSensitivityButton()
    {
        navigationPanelGameObject.SetActive( false );
        
        sensitivityPanel.Show( () =>
        {
            navigationPanelGameObject.SetActive( true );
        } );
    }
    
    void OnEscapeButton()
    {
        if( navigationPanelGameObject.activeSelf )
        {
            Hide();
        }
    }
}
