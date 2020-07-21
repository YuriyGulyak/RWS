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
        sensitivityPanel.Hide();
        
        gameObject.SetActive( false );
        panelRect.anchoredPosition = Vector2.zero;
    }
    
    //----------------------------------------------------------------------------------------------------

    void Awake()
    {
        closeButton.onClick.AddListener( Hide );
        
        sensitivityButton.onClick.AddListener( OnSensitivityButton );
    }


    void OnSensitivityButton()
    {
        navigationPanelGameObject.SetActive( false );
        
        sensitivityPanel.Show( () =>
        {
            sensitivityPanel.Hide();
            navigationPanelGameObject.SetActive( true );
        } );
    }
}
