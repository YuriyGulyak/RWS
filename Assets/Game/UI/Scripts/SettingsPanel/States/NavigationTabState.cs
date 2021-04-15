namespace RWS
{
    public class NavigationTabState : BaseState<SettingsPanel>
    {
        public override void OnEnableState()
        {
            owner.navigationPanel.OnGraphicsButtonClicked += OnGraphicsButtonClicked;
            owner.navigationPanel.OnSoundButtonClicked += OnSoundButtonClicked;
            owner.navigationPanel.OnControlsButtonClicked += OnControlsButtonClicked;
            owner.navigationPanel.OnSensitivityButtonClicked += OnSensitivityButtonClicked;
            owner.navigationPanel.OnEscapeButtonClicked += OnEscapeButtonClicked;
            
            owner.navigationPanel.Show();
        }

        public override void OnDisableState()
        {
            owner.navigationPanel.Hide();
            
            owner.navigationPanel.OnGraphicsButtonClicked -= OnGraphicsButtonClicked;
            owner.navigationPanel.OnSoundButtonClicked -= OnSoundButtonClicked;
            owner.navigationPanel.OnControlsButtonClicked -= OnControlsButtonClicked;
            owner.navigationPanel.OnSensitivityButtonClicked -= OnSensitivityButtonClicked;
            owner.navigationPanel.OnEscapeButtonClicked -= OnEscapeButtonClicked;
        }


        void OnGraphicsButtonClicked()
        {
            owner.ChangeState( new GraphicsTabState() );
        }

        void OnSoundButtonClicked()
        {
            owner.ChangeState( new SoundTabState() );
        }
        
        void OnControlsButtonClicked()
        {
            owner.ChangeState( new ControlsTabState() );
        }
        
        void OnSensitivityButtonClicked()
        {
            owner.ChangeState( new SensitivityTabState() );
        }

        void OnEscapeButtonClicked()
        {
            owner.Hide();
        }
    }
}

