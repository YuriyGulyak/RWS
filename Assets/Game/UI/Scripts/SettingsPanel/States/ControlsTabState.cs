namespace RWS
{
    public class ControlsTabState : BaseState<SettingsPanel>
    {
        public override void OnEnableState()
        {
            owner.controlsPanel.OnBackButtonClicked += OnBackButtonClicked;
            owner.controlsPanel.Show();
        }

        public override void OnDisableState()
        {
            owner.controlsPanel.Hide();
            owner.controlsPanel.OnBackButtonClicked -= OnBackButtonClicked;
        }
        
        
        void OnBackButtonClicked()
        {
            owner.ChangeState( new NavigationTabState() );
        }
    }
}
