namespace RWS
{
    public class GraphicsTabState : BaseState<SettingsPanel>
    {
        public override void OnEnableState()
        {
            owner.graphicsPanel.OnBackButtonClicked += OnBackButtonClicked;
            owner.graphicsPanel.Show();
        }

        public override void OnDisableState()
        {
            owner.graphicsPanel.Hide();
            owner.graphicsPanel.OnBackButtonClicked -= OnBackButtonClicked;
        }
        
        
        void OnBackButtonClicked()
        {
            owner.ChangeState( new NavigationTabState() );
        }
    }
}

