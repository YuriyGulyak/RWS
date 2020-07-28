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
    Button soundsButton = null;
    
    [SerializeField]
    Button controlsButton = null;
    
    [SerializeField]
    Button sensitivityButton = null;

    [SerializeField]
    GraphicsPanel graphicsPanel = null;

    [SerializeField]
    ControlsPanel controlsPanel = null;
    
    [SerializeField]
    SensitivityPanel sensitivityPanel = null; 
    
    //----------------------------------------------------------------------------------------------------
    
    public void Show()
    {
        if( gameObject.activeSelf )
        {
            return;
        }
        gameObject.SetActive( true );
    }

    public void Hide()
    {
        if( !gameObject.activeSelf )
        {
            return;
        }
        
        navigationPanelGameObject.SetActive( true );
        
        graphicsPanel.Hide();
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
        controlsButton.onClick.AddListener( OnControlsButton );
        sensitivityButton.onClick.AddListener( OnSensitivityButton );
        
        navigationPanelGameObject.SetActive( true );
        
        graphicsPanel.Hide();    
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
}
